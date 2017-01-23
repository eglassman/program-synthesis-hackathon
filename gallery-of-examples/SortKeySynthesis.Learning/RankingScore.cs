using System;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.AST;
using Microsoft.ProgramSynthesis.Features;
using static SortKeySynthesis.Semantics.Semantics;

namespace SortKeySynthesis.Learning
{
    public class RankingScore : Feature<double>
    {
        public RankingScore(Grammar grammar) : base(grammar, "Score") {}

        protected override double GetFeatureValueForVariable(VariableNode variable) => 0;

        // Your ranking functions here
        [FeatureCalculator(nameof(Sort))]
        public static double Score_Sort(double vs, double ks) {
            return ks;
        }

        [FeatureCalculator(nameof(Length))]
        public static double Score_Length() => 1.0;

        
        [FeatureCalculator(nameof(Count))]
        public static double Score_Count(double ss) => ss;

        
        [FeatureCalculator(nameof(First))]
        public static double Score_First(double ss) => ss;
        
        
        [FeatureCalculator(nameof(Last))]
        public static double Score_Last(double ss) => ss;
        
        [FeatureCalculator("s", Method = CalculationMethod.FromLiteral)]
        public static double Score_s(string s) => 1.0/s.Length;
        
    }
}
