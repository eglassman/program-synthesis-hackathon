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
using sorta.Learning;

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
            Console.WriteLine("sucessfuly loaded: " + grammar.Name);

            //---------------------------------------------------------------------------------

            ProgramNode p = ProgramNode.Parse("Length(x)", grammar, ASTSerializationFormat.HumanReadable);
            var inp = new Tuple<string, string>("aaa", "aa");
            State input = State.Create(grammar.InputSymbol, inp);
            Console.WriteLine(p.Invoke(input));


            
        }
    }
}
