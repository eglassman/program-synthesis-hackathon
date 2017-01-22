using System;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Learning;
using Microsoft.ProgramSynthesis.Specifications;
using System.Collections.Generic;
using sorta.Semantics;

namespace sorta.Learning
{
    public class Learners : DomainLearningLogic
    {
        public Learners(Grammar grammar) : base(grammar) {}

        // Your custom learning logic here (for example, witness functions)

        ICollection<string> FindCharacters(string s) {
            var chars = new HashSet<string>();
            foreach(char cc in s) {
                chars.Add(cc.ToString());
            }
            return chars;
        }

        [WitnessFunction("FirstIndexOf", 1)]
	    DisjunctiveExamplesSpec WitnessPositionPair(GrammarRule rule, ExampleSpec spec){
                var nExamples = new Dictionary<State, IEnumerable<object>>();
                foreach(State st in spec.ProvidedInputs) {
                    var input = (Tuple<string, string>) st[rule.Body[0]];
                    var output = (Order) spec.Examples[st];
                    var needles = new List<string>();
                    foreach(var needle in FindCharacters(input.Item1 + input.Item2)) {
                        if(Semantics.Semantics.FirstIndexOf(input, needle) == output) {
                            needles.Add(needle);
                            Console.WriteLine("possible needle: " + needle);
                        }
                    }
                    nExamples[st] = needles;
                }
                return DisjunctiveExamplesSpec.From(nExamples);
        }
    }
}
