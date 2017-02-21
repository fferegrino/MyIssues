using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyIssues.Util
{
    public static class StringColorExtensions
    {
        public static int[] GetInts(this string hex)
        {
            var colors = new int[3];
            colors[0] = Int32.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            colors[1] = Int32.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            colors[2] = Int32.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            return colors;
        }
    }
}