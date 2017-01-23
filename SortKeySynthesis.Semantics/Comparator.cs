using System;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;

namespace SortKeySynthesis.Semantics
{
    public class Comparator : IFunctionalSymbol1
    {
        public Comparison<string> Comparison { get; }

        public Comparator(Comparison<string> comparision)
        {
            Comparison = comparision;
        }

        public object Evaluate(object arg)
        {
            var input = (Tuple<string, string>)arg;
            var output = Comparison(input.Item1, input.Item2);
            Evaluated?.Invoke(this, new EvaluatedEventArgs {
                X = input.Item1,
                Y = input.Item2,
                Result = output
            });
            return output;
        }

        public event EventHandler<EvaluatedEventArgs> Evaluated;
    }

    public class EvaluatedEventArgs
    {
        public string X { get; set; }
        public string Y { get; set; }
        public int Result { get; set; }
    }
}
