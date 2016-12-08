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
using Android.Preferences;
using Android.Util;
using Android.Text;

namespace MyIssues.Droid.Controls.Editor
{
    [Register("myissues.droid.controls.editor.HighlightingEditor")]
    public class HighlightingEditor : EditText
    {
        const int DEFAULT_DELAY = 500;
        private Highlighter highlighter;
        private ISharedPreferences prefs;

        public interface IOnTextChangedListener
        {
            void OnTextChanged(string text);
        }

        internal IOnTextChangedListener onTextChangedListener = null;

        internal readonly Handler UpdateHandler;
        internal readonly UpdateRunnable UpdateRunnable;


        public bool Modified { get; private set; }
        public int HighlightingDelay { get; private set; }

        public HighlightingEditor(Context context) : base(context)
        {
            UpdateHandler = new Handler();
            UpdateRunnable = new UpdateRunnable(this);
            prefs = PreferenceManager.GetDefaultSharedPreferences(this.Context);
            if (prefs.GetBoolean(GetStringFromStringTable(Resource.String.pref_highlighting_activated_key), false))
            {
                Init();
            }
        }

        public HighlightingEditor(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            UpdateHandler = new Handler();
            UpdateRunnable = new UpdateRunnable(this);
            prefs = PreferenceManager.GetDefaultSharedPreferences(this.Context);
            if (prefs.GetBoolean(GetStringFromStringTable(Resource.String.pref_highlighting_activated_key), false))
            {
                Init();
            }
        }

        private void Init()
        {

            SetFilters(new IInputFilter[] { new IndentationFilter(this) });

            HighlightingDelay = GetHighlightingDelayFromPrefs();

            highlighter = new Highlighter(new MyHighlighterColorsNeutral(),
                prefs.GetString(GetStringFromStringTable(Resource.String.pref_font_choice_key), ""),
                prefs.GetString(GetStringFromStringTable(Resource.String.pref_font_size_key), ""));
            AfterTextChanged += HighlightingEditor_AfterTextChanged;
        }

        private void HighlightingEditor_AfterTextChanged(object sender, AfterTextChangedEventArgs e)
        {
            if (!Modified)
                return;

            UpdateHandler.PostDelayed(
                UpdateRunnable,
                HighlightingDelay);
        }

   private void CancelUpdate() {
            UpdateHandler.RemoveCallbacks(UpdateRunnable);
   }


        internal void HighlightWithoutChange(IEditable e)
        {
            Modified = false;
            highlighter.Run(e);
            Modified = true;
        }

        private string GetStringFromStringTable(int preferenceKey)
        {
            return this.Context.GetString(preferenceKey);
        }

        private int GetHighlightingDelayFromPrefs()
        {
            var value = prefs.GetString(GetStringFromStringTable(Resource.String.pref_highlighting_delay_key), "");
            return value == null || value.Equals("") ? DEFAULT_DELAY : Int32.Parse(value);
        }

        internal ICharSequence AutoIndent(ICharSequence source, ISpanned dest, int dstart, int dend)
        {

            int istart = FindLineBreakPosition(dest, dstart);

            // append white space of previous line and new indent
            return new Java.Lang.String(source + CreateIndentForNextLine(dest, dend, istart));
        }
        private int FindLineBreakPosition(ISpanned dest, int dstart)
        {
            int istart = dstart - 1;

            for (; istart > -1; --istart)
            {
                char c = dest.CharAt(istart);

                if (c == '\n')
                    break;
            }
            return istart;
        }

        private string CreateIndentForNextLine(ISpanned dest, int dend, int istart)
        {
            if (istart > -1)
            {
                int iend;

                for (iend = ++istart;
                     iend < dend;
                     ++iend)
                {
                    char c = dest.CharAt(iend);

                    if (c != ' ' &&
                            c != '\t')
                    {
                        break;
                    }
                }
                var subsequence = dest.SubSequence(istart, iend);
                return subsequence + AddBulletPointIfNeeded(dest.CharAt(iend));
            }
            else
            {
                return "";
            }
        }
        private string AddBulletPointIfNeeded(char character)
        {
            return (character == '*' ? Character.ToString(character) + " " : "");
        }
    }
}