using System;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.AST;
using Microsoft.ProgramSynthesis.Compiler;
using Microsoft.ProgramSynthesis.Specifications;
using Microsoft.ProgramSynthesis.Utils;
using Microsoft.ProgramSynthesis.Specifications.Extensions;
using static SortKeySynthesis.Utils;

namespace SortKeySynthesis
{

    class Program
    {
        public static void Main(string[] args)
        {
            
            var parseResult = DSLCompiler.ParseGrammarFromFile("SortKeySynthesis.grammar");
            parseResult.TraceDiagnostics();
            var grammar = parseResult.Value;


            foreach (var d in parseResult.Diagnostics) {
                System.Console.WriteLine(d);
            }

            if (grammar == null) {
                Console.WriteLine("Grammar is null");
                return;
            }
            
            testForwardExecution(grammar);
            testLearning(grammar);
        }

        private static void testLearning(Grammar grammar) {
            var inputList = new[] {"Hello", "World!!!!!!", "Eureka!!", "oh!!"};

            var outputLists = new String[][] {
                new []{"oh!!", "Hello", "Eureka!!", "World!!!!!!"},
                new []{"Eureka!!", "oh!!", "Hello", "World!!!!!!"},
                new []{"Hello", "oh!!", "World!!!!!!", "Eureka!!"},
                new []{"Eureka!!", "oh!!", "World!!!!!!", "Hello"},
                new []{ "Hello", "oh!!", "Eureka!!", "World!!!!!!"},
                new []{ "Hello",  "Eureka!!", "oh!!", "World!!!!!!"},
                
            };

            foreach (var outputList in outputLists) {
                Spec spec = ShouldConvert.Given(grammar).To(inputList, outputList);
                
                Learn(grammar, spec, 
                    new SortKeySynthesis.Learning.RankingScore(grammar), 
                    new SortKeySynthesis.Learning.Learners(grammar));
            }
        }

        private static void testForwardExecution(Grammar grammar) {
           
            // Create input
            var inputList = new[] {"Hello", "World!!!!!!", "Eureka!", "oh?"};
            State input = State.Create(grammar.InputSymbol, inputList);

            // Try programs
            var programs = new[] {
                "Sort(v, Length())",
                "Sort(v, First(\"l\"))",
                "Sort(v, First(\"!\"))",
                "Sort(v, Count(\"l\"))",
                "Sort(v, Count(\"!\"))"
                };
            foreach (string program in programs) {
                ProgramNode p = ProgramNode.Parse(program, grammar, ASTSerializationFormat.HumanReadable);
                var output = p.Invoke(input);
                Console.WriteLine(((string[]) output).DumpCollection());
            }
            

        }
    }
}
