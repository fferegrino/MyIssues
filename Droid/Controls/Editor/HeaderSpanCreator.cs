using System;
using Android.Text;
using Java.Util.Regex;
using Android.Util;
using Android.Content.Res;
using Android.Text.Style;
using Android.Graphics;

namespace MyIssues.Droid.Controls.Editor
{
    internal class HeaderSpanCreator : ISpanCreator 
    {

        const char POUND_SIGN = '#';
        public static DisplayMetrics DISPLAY_METRICS = Resources.System.DisplayMetrics;
        public static float STANDARD_PROPORTION_MAX = 1.80f;
        public static float SIZE_STEP = 0.20f;

        private IEditable e;
        private Highlighter highlighter;
        private Android.Graphics.Color color;

        public HeaderSpanCreator(Highlighter highlighter, IEditable e, Android.Graphics.Color headerColor)
        {
            this.highlighter = highlighter;
            this.e = e;
            this.color = headerColor;
        }


        public IParcelableSpan Create(Matcher m)
        {
           var charSequence = ExtractMatchingRange(m);
           var proportion = CalculateProportionBasedOnHeaderType(charSequence);
           var size = CalculateAdjustedSize(proportion);
            return new TextAppearanceSpan(highlighter.FontType, TypefaceStyle.Bold, (int)size,
                ColorStateList.ValueOf(color), null);
        }

        private float CalculateAdjustedSize(float proportion)
        {
            return TypedValue.ApplyDimension(ComplexUnitType.Sp,
                    highlighter.FontSize * proportion,
                    DISPLAY_METRICS);
        }

        private char[] ExtractMatchingRange(Matcher m)
        {
            return e.SubSequenceFormatted(m.Start(), m.End()).ToString().Trim().ToCharArray();
        }

        private float CalculateProportionBasedOnHeaderType(char[] charSequence)
        {

            var proportion = CalculateProportionForHashesHeader(charSequence);
            if (proportion == STANDARD_PROPORTION_MAX)
            {
                return CalculateProportionForUnderlineHeader(charSequence);
            }
            return proportion;
        }

        private float CalculateProportionForUnderlineHeader(char[] charSequence)
        {
            var proportion = STANDARD_PROPORTION_MAX;
            if (Char.GetNumericValue('=').Equals(charSequence[charSequence.Length - 1]))
            {
                proportion -= SIZE_STEP;
            }
            else if (Char.GetNumericValue('=').Equals(charSequence[charSequence.Length - 1]))
            {
                proportion -= (SIZE_STEP * 2);
            }
            return proportion;
        }

        private float CalculateProportionForHashesHeader(char[] charSequence)
        {
            float proportion = STANDARD_PROPORTION_MAX;
            int i = 0;
            // Reduce by SIZE_STEP for each #
            while (POUND_SIGN.Equals(charSequence[i]))
            {
                proportion -= SIZE_STEP;
                i++;
            }
            return proportion;
        }
        
    }
}