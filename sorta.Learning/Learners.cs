using System;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Learning;
using Microsoft.ProgramSynthesis.Specifications;
using System.Collections.Generic;

namespace sorta.Learning
{
    public class Learners : DomainLearningLogic
    {
        public Learners(Grammar grammar) : base(grammar) {}

        // Your custom learning logic here (for example, witness functions)


        [WitnessFunction("FirstIndexOf", 1)]
	    DisjunctiveExamplesSpec WitnessPositionPair(GrammarRule rule, ExampleSpec spec){
                Console.WriteLine("witness called");
                return null;
        }
    }
}
