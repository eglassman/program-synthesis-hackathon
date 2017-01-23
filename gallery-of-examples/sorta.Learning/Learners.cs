using System;
using System.Text.RegularExpressions;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Learning;
using Microsoft.ProgramSynthesis.Specifications;
using System.Collections.Generic;
using System.Linq;  
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

        [WitnessFunction(nameof(Semantics.Semantics.Invert), 0)]
        ExampleSpec WitnessInvert(GrammarRule rule, ExampleSpec spec) {
            var nExamples = new Dictionary<State, object>();
            foreach(State st in spec.ProvidedInputs) {
                var output = (Order) spec.Examples[st];
                nExamples[st] = Semantics.Semantics.Invert(output);
            }
            return new ExampleSpec(nExamples);
        }

        delegate Order SearchCompareFun(Tuple<string, string> input, string needle);

        DisjunctiveExamplesSpec WitnessSearchCompareFun(SearchCompareFun fun, GrammarRule rule, ExampleSpec spec) {
            var nExamples = new Dictionary<State, IEnumerable<object>>();
                foreach(State st in spec.ProvidedInputs) {
                    var input = (Tuple<string, string>) st[rule.Body[0]];
                    var output = (Order) spec.Examples[st];
                    var needles = new List<string>();
                    foreach(var needle in FindCharacters(input.Item1 + input.Item2)) {
                        if(fun(input, needle) == output) {
                            needles.Add(needle);
                            //Console.WriteLine("possible needle: " + needle);
                        }
                    }
                    nExamples[st] = needles;
                }
                return DisjunctiveExamplesSpec.From(nExamples);
        }

        [WitnessFunction(nameof(Semantics.Semantics.FirstIndexOf), 1)]
	    DisjunctiveExamplesSpec WitnessFirstIndexOf(GrammarRule rule, ExampleSpec spec){
                return WitnessSearchCompareFun(Semantics.Semantics.FirstIndexOf, rule, spec); 
        }

        [WitnessFunction(nameof(Semantics.Semantics.LastIndexOf), 1)]
	    DisjunctiveExamplesSpec WitnessLastIndexOf(GrammarRule rule, ExampleSpec spec){
                return WitnessSearchCompareFun(Semantics.Semantics.LastIndexOf, rule, spec); 
        }

        [WitnessFunction(nameof(Semantics.Semantics.CountOf), 1)]
	    DisjunctiveExamplesSpec WitnessCountOf(GrammarRule rule, ExampleSpec spec){
                return WitnessSearchCompareFun(Semantics.Semantics.CountOf, rule, spec); 
        }

        [WitnessFunction(nameof(Semantics.Semantics.FirstLetterOfWord), 1)]
	    DisjunctiveExamplesSpec WitnessFirstLetterOfWord(GrammarRule rule, ExampleSpec spec){
                var nExamples = new Dictionary<State, IEnumerable<object>>();
                foreach(State st in spec.ProvidedInputs) {
                    var input = (Tuple<string, string>) st[rule.Body[0]];
                    var output = (Order) spec.Examples[st];
                    var positions = new List<object>();
                    var words1 = Regex.Split(input.Item1, @"\s+").Length;
                    var words2 = Regex.Split(input.Item2, @"\s+").Length;
                    foreach(var n in Enumerable.Range(0, Math.Max(words1, words2))) {
                        if(Semantics.Semantics.FirstLetterOfWord(input, n) == output) {
                            positions.Add(n);
                        }
                    }
                    nExamples[st] = positions;
                }
                return DisjunctiveExamplesSpec.From(nExamples);
        }
    }
}
