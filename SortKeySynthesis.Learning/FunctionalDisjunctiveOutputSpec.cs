using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using Microsoft.ProgramSynthesis.Utils;

namespace SortKeySynthesis.Learning {
    /// <summary>
    ///     Defines a specification on a program output that has type <see cref="Func{T,TResult}"/>.
    ///     For each input state in <see cref="Relation"/>, it holds a set of disjunctive input-output examples
    ///     that constrain the behavior of the program output (which is a function).
    ///     This function, given an input value stored as a key of <see cref="MultiValueDictionary{TKey,TValue}"/>,
    ///     must return one of the output values stored as the values of <see cref="MultiValueDictionary{TKey,TValue}"/>.
    /// </summary>
    /// <example>
    ///     The following <see cref="FunctionalDisjunctiveOutputSpec"/> defines a function that compares a given input
    ///     with the value of the variable <c>v</c> stored in the input <see cref="State"/>, and returns
    ///     <c>-1</c>, <c>0</c>, or <c>1</c> if the input is less that, equal, or greater than <c>v</c>, respectively.
    ///     <code>
    ///         var v = grammar.Symbol("v");
    ///         var relation = new Dictionary&lt;State, MultiValueDictionary&lt;object, object&gt; {
    ///             [State.Create(v, 7)] = new MultiValueDictionary&lt;object, object&gt; {
    ///                 [1] = -1, [6] = -1, [7] = 0, [8] = 1, [42] = 1
    ///             },
    ///             [State.Create(v, -2)] = new MultiValueDictionary&lt;object, object&gt; {
    ///                 [-2] = 0, [0] = 1
    ///             }
    ///         };
    ///         var spec = new FunctionalDisjunctiveOutputSpec(relation);
    ///         spec.CorrectOnProvided(State.Create(v, 7), x => x.CompareTo(7));  // true
    ///         spec.CorrectOnProvided(State.Create(v, 7), x => x + 1));          // false
    ///     </code>
    /// </example>
    public class FunctionalDisjunctiveOutputSpec : Spec {
        public IDictionary<State, MultiValueDictionary<object, object>> Relation { get; }
        public FunctionalDisjunctiveOutputSpec(IDictionary<State, MultiValueDictionary<object, object>> relation)
            : base(relation.Keys) {
            Relation = relation;
        }
        /// <summary>
        ///     Is the <paramref name="output"/> correct on a given input <paramref name="state"/>?
        /// </summary>
        protected override bool CorrectOnProvided(State state, object output) {
            var func = (IFunctionalSymbol1) output;
            MultiValueDictionary<object, object> relation;
            if (!Relation.TryGetValue(state, out relation)) return false;
            return relation.All(entry => entry.Value.Contains(func.Evaluate(entry.Key), ValueEquality.Comparer));
        }
        /// <summary>
        ///     For a given input <paramref name="state"/>, is the content of this spec the same
        ///     as in the <paramref name="other"/> given spec?
        /// </summary>
        protected override bool EqualsOnInput(State state, Spec other) {
            var otherSpec = other as FunctionalDisjunctiveOutputSpec;
            if (otherSpec == null) return false;
            MultiValueDictionary<object, object> relation;
            if (!Relation.TryGetValue(state, out relation)) return false;
            MultiValueDictionary<object, object> otherRelation;
            if (!otherSpec.Relation.TryGetValue(state, out otherRelation)) return false;
            if (relation.Count != otherRelation.Count) return false;
            foreach (var entry in relation) {
                IReadOnlyCollection<object> otherOutputs;
                if (!otherRelation.TryGetValue(entry.Key, out otherOutputs)) return false;
                if (!entry.Value.SequenceEqual(otherOutputs)) return false;
            }
            return true;
        }
        /// <summary>
        ///     Compute a hash code of the content of this spec associated with a given <paramref name="state"/>.
        /// </summary>
        protected override int GetHashCodeOnInput(State state)
            => Relation[state].OrderIndependentHashCode(KeyValueComparer<object, IReadOnlyCollection<object>>.Instance);
        /// <summary>
        ///     Return an XML representation of the content of this spec
        ///     associated with a given <paramref name="input"/> state.
        /// </summary>
        protected override XElement InputToXML(State input, Dictionary<object, int> identityCache)
            => new XElement("Relation",
                            Relation[input].Select(
                                kvp => new XElement("Return",
                                                    new XAttribute("on", kvp.Key.InternedFormat(identityCache)),
                                                    kvp.Value.CollectionToXML("AnyOf"))));
        /// <summary>
        ///     Produces a copy of this spec
        ///     where the provided inputs are transformed with a given transformer function <paramref name="transformer" />,
        ///     and the associated constrains on the inputs remain the same.
        /// </summary>
        protected override Spec TransformInputs(Func<State, State> transformer)
            => new FunctionalDisjunctiveOutputSpec(Relation.ToDictionary(kvp => transformer(kvp.Key), kvp => kvp.Value));
    }
}
