using System;
using System.Collections.Generic;
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
            parseResult.TraceDiagnostics();
            var grammar = parseResult.Value;
            
            Console.WriteLine(grammar.Name);
        }
    }
}
