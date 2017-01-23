using System;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Learning;
using Microsoft.ProgramSynthesis.Specifications;
using System.Collections.Generic;
using Microsoft.ProgramSynthesis.Utils;
using SortKeySynthesis.Semantics;

namespace SortKeySynthesis.Learning
{
    public class Learners : DomainLearningLogic
    {
        public Learners(Grammar grammar) : base(grammar) {}

        [WitnessFunction(nameof(Semantics.Semantics.Sort), 1)]
        public static FunctionalDisjunctiveOutputSpec WitnessSortKey(GrammarRule rule, ExampleSpec spec) {
            var fExamples = new Dictionary<State, MultiValueDictionary<object, object>>();
            foreach (State input in spec.ProvidedInputs) {
                var inputArray = (string[]) input[rule.Body[0]];
                var outArray = (string[]) spec.Examples[input];
                int len = inputArray.Length;

                

                // Get positions for each item in sorted list
                var indexes = new int[len];
                for (var i = 0; i < len; i++) {
                    var item = inputArray[i];
                    var idx = Array.IndexOf(outArray, item);
                    if (idx < 0) {
                        throw new Exception("Cannot find index in output array");
                    }
                    indexes[i] = idx;
                }

                // Create a dictionary for comparisons spec
                var fMap = new MultiValueDictionary<object, object>();


                // Loop through all possible pairs
                for (var i = 0; i < len-1; i++) {
                    for (var j = i+1; j < len; j++) {
                        var item1 = inputArray[i];
                        var item2 = inputArray[j];
                        var idx1 = indexes[i];
                        var idx2 = indexes[j];
                        var itemTuple = (Tuple<string, string>) Tuple.Create(item1, item2);
                        // Two cases are interesting, swaps and splices:
                        // (1) idx1 > idx2 ==> item1 > item2
                        // (2) (idx2 - idx1) > (j - i) ==> item2 > item1
                        
                        if (idx1 > idx2) {
                            fMap.Add(itemTuple, 1);
                        } else if ((idx2 - idx1) > (j - i)) {
                            fMap.Add(itemTuple, -1);
                        } else {
                            // If strict ordering is not found, then
                            // idx1 < idx2 ==> (item1 <= item2)
                            fMap.Add(itemTuple, -1);
                            fMap.Add(itemTuple, 0);
                        }


                    }
                }
                
                // Add comparisons for this list in the functional spec
                fExamples[input] = fMap;
                

            }
            return new FunctionalDisjunctiveOutputSpec(fExamples);
        }




        
        [WitnessFunction(nameof(Semantics.Semantics.Count), 0)]
        public static DisjunctiveExamplesSpec WitnessCountStr(GrammarRule rule, FunctionalDisjunctiveOutputSpec spec) {
            return deduceSubstrBasedKey(spec, s => Semantics.Semantics.Count(s));
        }

        [WitnessFunction(nameof(Semantics.Semantics.First), 0)]
        public static DisjunctiveExamplesSpec WitnessFirstStr(GrammarRule rule, FunctionalDisjunctiveOutputSpec spec) {
            return deduceSubstrBasedKey(spec, s => Semantics.Semantics.First(s));
        }
        
        [WitnessFunction(nameof(Semantics.Semantics.Last), 0)]
        public static DisjunctiveExamplesSpec WitnessLastStr(GrammarRule rule, FunctionalDisjunctiveOutputSpec spec) {
            return deduceSubstrBasedKey(spec, s => Semantics.Semantics.Last(s));
        }

        private delegate Comparator ComparatorCreator(string str);

        private static DisjunctiveExamplesSpec deduceSubstrBasedKey(FunctionalDisjunctiveOutputSpec spec,  ComparatorCreator cmpCreator) {
            var strExamples = new Dictionary<State, IEnumerable<object>>();

            foreach(State input in spec.ProvidedInputs) {
                var validStrings = new List<string>();
                var relation = spec.Relation[input];

                var superString = "";

                // To find a super-string, find a mapping that contains exactly
                // one value (i.e. a strict ordering)
                foreach (var mapping in relation) {
                    var key = (Tuple<string, string>) mapping.Key;
                    var possibleResults = mapping.Value;
                    if (possibleResults.Count == 1) {
                        var resultsEnumerator = possibleResults.GetEnumerator();
                        resultsEnumerator.MoveNext();
                        var onlyResult = (int) resultsEnumerator.Current;
                        if (onlyResult == 0) {
                            throw new Exception("strict comparisons cannot be zero");
                        }

                        // If we have found a strict ordering, the super-string is
                        // the item corresponding to the larger value
                        superString = (string) ((onlyResult > 0) ? key.Item1 : key.Item2);
                        break;

                    }

                }

                if (superString.Length == 0) throw new Exception("No strict ordering found");

                // Generate all possible sub-strings from superString
                for (var i = 0; i < superString.Length-1; i++) {
                    for (var len = 1; i + len <= superString.Length; len++) {
                        // Extract sub-string
                        var subStr = superString.Substring(i, len);
                        // Create comparator from provided lambda
                        var cmp = cmpCreator(subStr);
                        // Check if this subStr is consistent for all mappings
                        if (validateComparator(relation, cmp)) {
                            validStrings.Add(subStr);
                        }
                    }
                }
                strExamples[input] = validStrings;
            }

            return DisjunctiveExamplesSpec.From(strExamples);
        }

        private static bool validateComparator(MultiValueDictionary<object, object> relation, Comparator cmp) {
            foreach (var mapping in relation) {
                var tuple = (Tuple<string, string>) mapping.Key;
                var actualResult = cmp.Evaluate(tuple);
                var allowedResults = mapping.Value;
                if (matchesAny(actualResult, allowedResults) == false) {
                    return false;
                }
            }
            return true;
        }

        private static bool matchesAny(object x, IReadOnlyCollection<object> collection) {
            foreach(var obj in collection) {
                if (obj.Equals(x)) {
                    return true;
                }
            }
            return false;
        }
    }
}
