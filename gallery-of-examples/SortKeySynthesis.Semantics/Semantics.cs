using System;
using System.Collections.Generic;

namespace SortKeySynthesis.Semantics
{

    public delegate int Key(string s);

    public static class Semantics
    {
        public static string[] Sort(string[] input, Comparator comparator) {
            List<String> inputList = new List<String>();
            inputList.AddRange(input);
            inputList.Sort(comparator.Comparison);
            return inputList.ToArray();
        }

        private static Comparator cmp(Key key) {
            return new Comparator((x, y) => Math.Sign(key(x) - key(y)));

        }

        public static Comparator  Length() {
            return cmp((string item) => item.Length);
        }

        
        public static Comparator First(string str) {
            return cmp((string item) => item.IndexOf(str));
        }
        
        public static Comparator Last(string str) {
            return cmp((string item) => item.LastIndexOf(str));
        }

        
        public static Comparator  Count(string str) {
            return cmp((string item) => {
                int count = 0;
                for (int i = item.IndexOf(str, StringComparison.Ordinal);
                    i >= 0;
                i = item.IndexOf(str, i + 1, StringComparison.Ordinal)) {
                    count++;
                }
                return count;
            });
        }
    }
}
