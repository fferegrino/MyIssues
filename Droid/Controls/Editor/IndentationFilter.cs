using System;
using Android.Text;
using Java.Lang;

namespace MyIssues.Droid.Controls.Editor
{
    internal class IndentationFilter : Java.Lang.Object, IInputFilter
    {

        HighlightingEditor _editor;
        public IndentationFilter(HighlightingEditor editor)
        {
            _editor = editor;
        }

        public ICharSequence FilterFormatted(ICharSequence source, int start, int end, 
            ISpanned dest, int dstart, int dend)
        {
            if (_editor.Modified &&
        end - start == 1 &&
        start < source.Length() &&
        dstart < dest.Length())
            {
                char c = source.CharAt(start);

                if (c == '\n')
                    return _editor.AutoIndent(
                            source,
                            dest,
                            dstart,
                            dend);
            }
            return source;
        }
    }
}