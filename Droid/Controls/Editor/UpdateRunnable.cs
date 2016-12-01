using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace MyIssues.Droid.Controls.Editor
{
    public class UpdateRunnable : Java.Lang.Object, IRunnable
    {
        HighlightingEditor _editor;
        public UpdateRunnable(HighlightingEditor editor)
        {
            _editor = editor;
        }
        public void Run()
        {
            var e = _editor.EditableText;

            if (_editor.onTextChangedListener != null)
                _editor.onTextChangedListener.OnTextChanged(e.ToString());

            _editor.HighlightWithoutChange(e);
        }
    }
}