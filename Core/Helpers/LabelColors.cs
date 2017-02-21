using System;
using System.Collections.Generic;

namespace MyIssues
{
    public static partial class Helpers
    {
        public static Dictionary<string, int[]> LabelColors;

        private static int[] GetColor(string hex)
        {
            var colors = new int[3];
            colors[0] = Int32.Parse(hex.Substring(0, 2));
            colors[1] = Int32.Parse(hex.Substring(2, 2));
            colors[2] = Int32.Parse(hex.Substring(4, 2));
            return colors;
        }
    }
}