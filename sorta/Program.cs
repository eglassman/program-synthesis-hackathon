using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.AST;
using Microsoft.ProgramSynthesis.Compiler;
using Microsoft.ProgramSynthesis.Learning;
using Microsoft.ProgramSynthesis.Learning.Logging;
using Microsoft.ProgramSynthesis.Learning.Strategies;
using Microsoft.ProgramSynthesis.Specifications;
using Microsoft.ProgramSynthesis.Utils;
using Microsoft.ProgramSynthesis.VersionSpace;
using sorta.Learning;
using sorta.Semantics;

namespace sorta
{
    class Program
    {
        public static void Main(string[] args)
        {
            
            var parseResult = DSLCompiler.ParseGrammarFromFile("sorta.grammar");
            foreach (var d in parseResult.Diagnostics) {
                System.Console.WriteLine(d);
            }
            if(parseResult.Value == null) {
                Console.WriteLine("Failed to load grammar (see errors above)");
                return;
            }

            //---------------------------------------------------------------------------------
            var grammar = parseResult.Value;
            Console.WriteLine("sucessfuly loaded: " + grammar.Name + "\n");

            //---------------------------------------------------------------------------------
            {
            ProgramNode p = ProgramNode.Parse("Length(x)", grammar, ASTSerializationFormat.HumanReadable);
            var inp = new Tuple<string, string>("aaa", "aa");
            State input = State.Create(grammar.InputSymbol, inp);
            Console.WriteLine(p.Invoke(input));
            }

            {
            /*
            ProgramNode p = ProgramNode.Parse("FirstIndexOf(x, 'a')", grammar, ASTSerializationFormat.HumanReadable);
            var inp = new Tuple<string, string>("aaa", "aa");
            State input = State.Create(grammar.InputSymbol, inp);
            Console.WriteLine(p.Invoke(input));
            */
            }
            
            //---------------------------------------------------------------------------------
            var g = grammar;
            var l = new sorta.Learning.Learners(g);
            // simple length comparisons
            Print(Learn(g,l, ex("aab", "bb", Order.Greater)));
            Print(Learn(g,l, ex("aab", "bb", Order.Less)));
            Print(Learn(g,l, ex("bb", "aab", Order.Greater)));
            //
            Print(Learn(g,l, ex("aab", "bb", Order.Less), ex("bb", "aab", Order.Greater)));
            // cannot be solved by length alone
            Print(Learn(g,l, ex("aab", "bb", Order.Greater), ex("bb", "aab", Order.Greater)));
            


        }

        private static Tuple<string, string, Order> ex(string a, string b, Order o) {
            return new Tuple<string, string, Order>(a,b,o);
        }

        private static ProgramSet Learn(Grammar grammar, DomainLearningLogic logic,  params Tuple<string, string, Order>[] examples) {
            Console.WriteLine();
            var dd = new Dictionary<State,object>();
            foreach(var example in examples) {
                var a = example.Item1;
                var b = example.Item2;
                var o = example.Item3;
                Console.WriteLine(String.Format("{0} {2} {1}", a, b, o));
                var inp = new Tuple<string, string>(a, b);
                Order output = o;
                var input = State.Create(grammar.InputSymbol, inp);
                dd[input] = output;
            }
            var spec = new ExampleSpec(dd);
            var engine = new SynthesisEngine(grammar, new SynthesisEngine.Config{
                UseThreads = false,
                Strategies = new ISynthesisStrategy[] {
                    new EnumerativeSynthesis(),
                    new DeductiveSynthesis(logic),
                },
                LogListener = new LogListener(),
            });
            return engine.LearnGrammar(spec);
        }

        private static void Print(ProgramSet progs) {
           if(progs.IsEmpty) {
                Console.WriteLine("wasn't able to find any matching program");
            } else {
                foreach(var prog in progs.AllElements) {
                    Console.WriteLine(String.Format("* {0}", prog.PrintAST(ASTSerializationFormat.HumanReadable)));
                }
            } 
        }
    }
}
