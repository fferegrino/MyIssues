using Android.Graphics;
using Android.Text;
using Android.Text.Style;
using Java.Util.Regex;
using System;

namespace MyIssues.Droid.Controls.Editor
{
    internal class Highlighter
    {
        readonly IHighlighterColors colors;

        public int FontSize { get; internal set; }
        public string FontType { get; internal set; }

        public Highlighter(IHighlighterColors colors, string fontType, string fontSize)
        {
            this.colors = colors;
            this.FontType = fontType;
            this.FontSize = Int32.Parse(fontSize);
        }

        public IEditable Run(IEditable e)
        {
            try
            {
                ClearSpans(e);

                if (e.Length() == 0)
                {
                    return e;
                }

                CreateHeaderSpanForMatches(e, HighlighterPattern.HEADER.Pattern, colors.HeaderColor);
                CreateColorSpanForMatches(e, HighlighterPattern.LINK.Pattern, colors.LinkColor);
                CreateColorSpanForMatches(e, HighlighterPattern.LIST.Pattern, colors.ListColor);
                CreateStyleSpanForMatches(e, HighlighterPattern.BOLD.Pattern, TypefaceStyle.Bold);
                CreateStyleSpanForMatches(e, HighlighterPattern.ITALICS.Pattern, TypefaceStyle.Italic);
                CreateColorSpanForMatches(e, HighlighterPattern.QUOTATION.Pattern, colors.ListColor);
                CreateSpanWithStrikeThroughForMatches(e, HighlighterPattern.STRIKETHROUGH.Pattern);
                CreateMonospaceSpanForMatches(e, HighlighterPattern.MONOSPACED.Pattern);

            }
            catch (Exception ex)
            {
                // Ignoring errors
            }

            return e;
        }

        private void CreateHeaderSpanForMatches(IEditable e, Pattern pattern, Android.Graphics.Color headerColor)
        {
            CreateSpanForMatches(e, pattern, new HeaderSpanCreator(this, e, headerColor));
        }

        private void CreateMonospaceSpanForMatches(IEditable e, Pattern pattern)
        {
            Func<IParcelableSpan> creator = () => new TypefaceSpan("monospace");
            CreateSpanForMatches(e, pattern, creator);
        }

        private void CreateSpanWithStrikeThroughForMatches(IEditable e, Pattern pattern)
        {

            Func<IParcelableSpan> creator = () => new StrikethroughSpan();
            CreateSpanForMatches(e, pattern, creator);
        }

        private void CreateStyleSpanForMatches(IEditable e, Pattern pattern,
                                   TypefaceStyle style)
        {
            Func<IParcelableSpan> creator = () => new StyleSpan(style);
            CreateSpanForMatches(e, pattern, creator);
        }
        private void CreateColorSpanForMatches(IEditable e, Pattern pattern,
                                                                       Android.Graphics.Color color)
        {
            Func<IParcelableSpan> creator = () => new ForegroundColorSpan(color);
            CreateSpanForMatches(e, pattern, creator);
        }
        private void CreateSpanForMatches(IEditable e, Pattern pattern,
                                              Func<IParcelableSpan> creator)
        {
            for (Matcher m = pattern.Matcher(e); m.Find();)
            {
                e.SetSpan((Java.Lang.Object)creator(), m.Start(), m.End(), SpanTypes.ExclusiveExclusive);
            }
        }

        private void CreateSpanForMatches(IEditable e, Pattern pattern,
                                              ISpanCreator creator)
        {
            for (Matcher m = pattern.Matcher(e); m.Find();)
            {
                e.SetSpan((Java.Lang.Object)creator.Create(m), m.Start(), m.End(), SpanTypes.ExclusiveExclusive);
            }
        }
        private void ClearSpans(IEditable e)
        {

            ClearSpanType(e, Java.Lang.Class.FromType(typeof(TextAppearanceSpan)));
            ClearSpanType(e, Java.Lang.Class.FromType(typeof(ForegroundColorSpan)));
            ClearSpanType(e, Java.Lang.Class.FromType(typeof(BackgroundColorSpan)));
            ClearSpanType(e, Java.Lang.Class.FromType(typeof(StrikethroughSpan)));
            ClearSpanType(e, Java.Lang.Class.FromType(typeof(StyleSpan)));
        }
        private void ClearSpanType(IEditable e, Java.Lang.Class spanType)
        {

            CharacterStyle[] spans = (CharacterStyle[])e.GetSpans(0, e.Length(), spanType);

            for (int n = spans.Length; n-- > 0;)
            {
                e.RemoveSpan(spans[n]);
            }
        }
    }
}