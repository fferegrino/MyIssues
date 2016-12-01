using System;
using Java.Lang;
using Java.Util.Regex;
using Android.Text;

namespace MyIssues.Droid.Controls.Editor
{
    internal interface ISpanCreator
    {
        IParcelableSpan Create(Matcher m);
    }
}