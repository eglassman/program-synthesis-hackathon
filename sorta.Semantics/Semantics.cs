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

        public static Order FirstIndexOf(Tuple<string, string> inp, String s) {
            return cmp(inp.Item1.IndexOf(s), inp.Item2.IndexOf(s));
        }

        public static Order LastIndexOf(Tuple<string, string> inp, String s) {
            return cmp(inp.Item1.LastIndexOf(s), inp.Item2.LastIndexOf(s));
        }

        private static int SubstringCount(string haystack, string needle) {
            int count = 0;
            for (int i = haystack.IndexOf(needle, StringComparison.Ordinal);
                     i >= 0;
                     i = haystack.IndexOf(needle, i + 1, StringComparison.Ordinal)) {
                         count++;
                     }
            return count;
        }

        public static Order CountOf(Tuple<string, string> inp, String s) {
            return cmp(SubstringCount(inp.Item1, s) , SubstringCount(inp.Item2, s));
        }

        public static Order Invert(Order order) {
            return (order == Order.Less)? Order.Greater :
                   (order == Order.Greater)? Order.Less : Order.Equal;
        }
        
    }
}
