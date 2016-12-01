using Java.Util.Regex;

namespace MyIssues.Droid.Controls.Editor
{
    internal class HighlighterPattern
    {
        public static HighlighterPattern LIST = new HighlighterPattern(Pattern.Compile("(\\n|^)\\s*(\\*|\\d+\\.)"));
        public static HighlighterPattern QUOTATION = new HighlighterPattern(Pattern.Compile("(\\n|^)>"));
        public static HighlighterPattern HEADER = new HighlighterPattern(Pattern.Compile("(((\\n|^)#+.*?\\n)|((\\n|^).*?\\n(-|= new HighlighterPattern()+))"));
        public static HighlighterPattern LINK = new HighlighterPattern(Pattern.Compile("\\[([^\\[]+)\\]\\(([^\\)]+)\\)"));
        public static HighlighterPattern STRIKETHROUGH = new HighlighterPattern(Pattern.Compile("\\~\\~(.*?)\\~\\~"));
        public static HighlighterPattern MONOSPACED = new HighlighterPattern(Pattern.Compile("`(.*?)`"));
        public static HighlighterPattern BOLD = new HighlighterPattern(Pattern.Compile("(\\*\\*|__)(.*?)\\1"));
        public static HighlighterPattern ITALICS = new HighlighterPattern(Pattern.Compile("(\\*|_)(.*?)\\1"));

        private Pattern pattern;

        HighlighterPattern(Pattern pattern)
        {
            this.pattern = pattern;
        }

        public Pattern Pattern
        {
            get { return pattern; }
        }
    }
}
