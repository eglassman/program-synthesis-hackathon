using System;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.AST;
using Microsoft.ProgramSynthesis.Features;
using static SortKeySynthesis.Semantics.Semantics;

namespace SortKeySynthesis.Learning
{
    public class NumberOfEquals : Feature<int>
    {
        private readonly State _input;

        public NumberOfEquals(Grammar grammar, State input) : base(grammar, nameof(NumberOfEquals))
        {
            _input = input;
        }

        [FeatureCalculator(nameof(Sort), Method = CalculationMethod.FromProgramNode)]
        public int NumberOfEquals_Sort(ProgramNode p)
        {
            return 0;
        }
    }
}
