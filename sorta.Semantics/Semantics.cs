using System;
namespace sorta.Semantics
{
    public enum Order { Equal, Less, Greater };

    public static class Semantics
    {
        // Your semantics implementations here

        // helper function
        private static Order cmp(int a, int b) {
            return (a > b)? Order.Greater : (a < b)? Order.Less : Order.Equal;
        }

        public static Order Length(Tuple<string, string> inp) {
            return cmp(inp.Item1.Length, inp.Item2.Length);
        }

        public static Order FirstIndexOf(Tuple<string, string> inp, char c) {
            return cmp(inp.Item1.IndexOf(c), inp.Item2.IndexOf(c));
        }

        public static Order Invert(Order order) {
            return (order == Order.Less)? Order.Greater :
                   (order == Order.Greater)? Order.Less : Order.Equal;
        }
        
    }
}
