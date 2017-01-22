using System;
namespace sorta.Semantics
{
    public enum Order { LessOrEqual, Greater };

    public static class Semantics
    {
        // Your semantics implementations here
        public static Order Length(Tuple<string, string> inp) {
            return (inp.Item1.Length > inp.Item2.Length)? Order.Greater : Order.LessOrEqual;
        }

        public static Order Invert(Order order) {
            return (order == Order.LessOrEqual)? Order.Greater : Order.LessOrEqual;
        }
    }
}
