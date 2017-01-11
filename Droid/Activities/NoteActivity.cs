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
using Android.Support.V7.App;
using Java.IO;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using MyIssues.Droid.Controls.Editor;
using Android.Preferences;
using MyIssues.Droid.Util;
using Android.Graphics;
using Android.Widget;
using ICharSequence = Java.Lang.ICharSequence;
using MyIssues.Droid.Controls;
using Android.Util;
using Android.Views.InputMethods;
using MyIssues.DataAccess;
using System.Threading.Tasks;
using MyIssues.Util;

namespace MyIssues.Droid.Activities
{
    [Activity(Label = "@string/reply_activity_label",
        Theme = "@style/MyTheme")]
    public class NoteActivity : ActionBarActivity
    {
        public const string Tag = nameof(NoteActivity);

        const string EMPTY_STRING = "";
        private Context context;

        private EditText noteTitle;
        internal HighlightingEditor Content { get; private set; }
        private ScrollView scrollView;

        private ViewGroup keyboardBarView;
        private int issueNumber;
        private bool isPreviewIncoming = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Note);

            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.Toolbar);
            if (toolbar != null)
            {
                SetSupportActionBar(toolbar);
                SupportActionBar.SetDefaultDisplayHomeAsUpEnabled(true);
            }

            context = ApplicationContext;
            Content = FindViewById<HighlightingEditor>(Resource.Id.NoteContent);
            noteTitle = (EditText)FindViewById(Resource.Id.EditNoteTitle);
            scrollView = (ScrollView)FindViewById(Resource.Id.NoteScrollview);
            keyboardBarView = (ViewGroup)FindViewById(Resource.Id.KeyboardBar);

            Intent receivingIntent = Intent;
            issueNumber = receivingIntent.GetIntExtra(Constants.IssueNumber, 0);

            if (issueNumber != 0)
            {
                noteTitle.Enabled = false;
               var replyToIssue = Resources.GetString(Resource.String.reply_to_issue_number);
                noteTitle.Text = String.Format(replyToIssue, issueNumber);
            }
            Content.RequestFocus();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.NoteMenu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    base.OnBackPressed();
                    OverridePendingTransition(Resource.Animation.AnimSlideOutRight, Resource.Animation.AnimSlideInRight);
                    return true;
                case Resource.Id.ActionComment:
                    var published = AsyncHelpers.RunSync<bool>(() => Publish(Content.Text, noteTitle.Text)); ;
                    if (published)
                    {
                        Intent returnIntent = new Intent();
                        returnIntent.PutExtra(Constants.CommentSuccessful, true);
                        SetResult(Result.Ok, returnIntent);
                        Finish();
                    }

                    return true;
                case Resource.Id.ActionPreview:
                    InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
                    imm.HideSoftInputFromWindow(Content.WindowToken, 0);
                    ShowPopup(Content.Text, noteTitle.Text);
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private async Task<bool> Publish(string text1, string text2)
        {
            return await Storage.GetInstance().SendComment(issueNumber, text1);
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            OverridePendingTransition(Resource.Animation.AnimSlideOutRight, Resource.Animation.AnimSlideInRight);
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
        }

        protected override void OnResume()
        {
            // Set up the font and background activity_preferences
            SetupKeyboardBar();
            SetupAppearancePreferences();

            //IntentFilter ifilter = new IntentFilter();
            //ifilter.AddAction(Constants.SHARE_BROADCAST_TAG);
            base.OnResume();
        }

        protected override void OnPause()
        {
            SaveNote();

            if (isPreviewIncoming)
            {
                Finish();
            }

            base.OnPause();
        }


        private void SetupKeyboardBar()
        {
            var showShortcuts = PreferenceManager.GetDefaultSharedPreferences(this).GetBoolean(GetString(Resource.String.pref_show_markdown_shortcuts_key), true);

            if (showShortcuts && keyboardBarView.ChildCount == 0)
            {
                AppendRegularShortcuts();
                if (IsSmartShortcutsActivated())
                {
                    AppendSmartBracketShortcuts();
                }
                else
                {
                    AppendRegularBracketShortcuts();
                }
            }
            else if (!showShortcuts)
            {
                FindViewById(Resource.Id.KeyboardBarScroll).Visibility = ViewStates.Gone;
            }
        }

        private void AppendRegularShortcuts()
        {

            foreach (String shortcut in Constants.KEYBOARD_SHORTCUTS)
            {
                AppendButton(shortcut, new KeyboardBarListener(this));
            }
        }

        private void AppendRegularBracketShortcuts()
        {
            foreach (String shortcut in Constants.KEYBOARD_SHORTCUTS_BRACKETS)
            {
                AppendButton(shortcut, new KeyboardBarListener(this));
            }

        }

        private void AppendSmartBracketShortcuts()
        {
            foreach (String shortcut in Constants.KEYBOARD_SMART_SHORTCUTS)
            {
                AppendButton(shortcut, new KeyboardBarSmartShortCutListener(this));
            }
        }

        private void AppendButton(string shortcut, View.IOnClickListener l)
        {
            TextView shortcutButton = (TextView)LayoutInflater.Inflate(Resource.Layout.KeyboardShortcut, null);
            shortcutButton.Text = shortcut;
            shortcutButton.SetOnClickListener(l);

            String theme = PreferenceManager.GetDefaultSharedPreferences(this).GetString(GetString(Resource.String.pref_theme_key), EMPTY_STRING);

            if (theme.Equals(GetString(Resource.String.theme_dark)))
            {
                shortcutButton.SetTextColor(Resources.GetColor(Android.Resource.Color.White));
            }
            else
            {
                shortcutButton.SetTextColor(Resources.GetColor(Android.Resource.Color.DarkerGray));
            }

            keyboardBarView.AddView(shortcutButton);
        }


        private bool IsSmartShortcutsActivated()
        {
            return PreferenceManager.GetDefaultSharedPreferences(context).GetBoolean(GetString(Resource.String.pref_smart_shortcuts_key), true);
        }

        private void SetupAppearancePreferences()
        {
            String theme = PreferenceManager.GetDefaultSharedPreferences(this).GetString(GetString(Resource.String.pref_theme_key), EMPTY_STRING);
            String fontType = PreferenceManager.GetDefaultSharedPreferences(this).GetString(GetString(Resource.String.pref_font_choice_key), EMPTY_STRING);
            String fontSize = PreferenceManager.GetDefaultSharedPreferences(this).GetString(GetString(Resource.String.pref_font_size_key), EMPTY_STRING);

            if (!fontSize.Equals(EMPTY_STRING))
            {
                Content.SetTextSize(Android.Util.ComplexUnitType.Sp, Single.Parse(fontSize));
            }

            if (!fontType.Equals(EMPTY_STRING))
            {
                Content.SetTypeface(Typeface.Create(fontType, TypefaceStyle.Normal), TypefaceStyle.Normal);
            }

            if (theme.Equals(GetString(Resource.String.theme_dark)))
            {
                Content.SetBackgroundColor(Resources.GetColor(Resource.Color.dark_grey));
                Content.SetTextColor(Resources.GetColor(Android.Resource.Color.White));
                keyboardBarView.SetBackgroundColor(Resources.GetColor(Resource.Color.grey));
            }
            else
            {
                Content.SetBackgroundColor(Resources.GetColor(Android.Resource.Color.White));
                Content.SetTextColor(Resources.GetColor(Resource.Color.dark_grey));
                keyboardBarView.SetBackgroundColor(Resources.GetColor(Resource.Color.lighter_grey));
            }


        }

        private void PreviewNote()
        {
            //        SaveNote();
            //        Intent intent = new Intent(this,typeof( PreviewActivity));

            //    // .replace is a workaround for Markdown lists requiring two \n characters
            //    if (note != null) {
            //        Uri uriBase = WriteilySingleton.GetInstance().GetUriFromFile(note.GetParentFile());
            //    intent.putExtra(Constants.MD_PREVIEW_BASE, uriBase.toString());
            //    }

            //intent.putExtra(Constants.NOTE_KEY, note);
            //    intent.putExtra(Constants.MD_PREVIEW_KEY, content.GetText().toString().replace("\n-", "\n\n-"));

            //    isPreviewIncoming = true;
            //    startActivity(intent);

            throw new NotImplementedException();
        }

        private void SaveNote()
        {
            //try
            //{
            //    String content = this.content.GetText().toString();
            //    String filename = normalizeFilename(content, noteTitle.GetText().toString());
            //    if (filename == null) return;

            //    String parent = tarGetDirectory != null ? tarGetDirectory : note.GetParent();
            //    File newNote = new File(parent, filename + Constants.MD_EXT);
            //    FileOutputStream fos = new FileOutputStream(newNote);
            //    OutputStreamWriter writer = new OutputStreamWriter(fos);

            //    writer.write(content);
            //    writer.flush();

            //    writer.close();
            //    fos.close();
            //    // If we have created a new note due to renaming, delete the old copy
            //    if (note != null && !newNote.GetName().equals(note.GetName()) && newNote.exists())
            //    {
            //        note.delete();
            //    }
            //    updateWidgets();
            //}
            //catch (IOException e)
            //{
            //    e.printStackTrace();
            //}
            //throw new NotImplementedException();
        }

        public void ShowPopup(string content, string title)
        {
            View popupView = LayoutInflater.Inflate(Resource.Layout.MarkdownExtendedCardView, null);

            PopupWindow popupWindow = new PopupWindow(popupView, ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);

            // If the PopupWindow should be focusable
            popupWindow.Focusable = (true);
            popupWindow.SetBackgroundDrawable(new Android.Graphics.Drawables.ColorDrawable(Android.Graphics.Color.Transparent));

            var fullMarkdownTextView = popupView.FindViewById<MarkdownView>(Resource.Id.FullMarkdownView);
            fullMarkdownTextView.LoadMarkdown(content, "file:///android_asset/github-markdown.css");

            // Using location, the PopupWindow will be displayed right under anchorView
            popupWindow.ShowAtLocation(Window.DecorView, GravityFlags.NoGravity,
                                            0, 0);

        }

    }

    class KeyboardBarListener : Java.Lang.Object, View.IOnClickListener
    {

        NoteActivity _editor;
        public KeyboardBarListener(NoteActivity editor)
        {
            _editor = editor;
        }

        public void OnClick(View v)
        {
            var shortcut = ((TextView)v).Text;
            var selectionStart = _editor.Content.SelectionStart;
            _editor.Content.Text = _editor.Content.Text.Insert(_editor.Content.SelectionStart, shortcut);
            try
            {
                _editor.Content.SetSelection(selectionStart + shortcut.Length);
            }
            catch (Java.Lang.IndexOutOfBoundsException ex)
            {
                Log.Error(NoteActivity.Tag, $"Error trying to reset selection");
            }
        }
    }

    class KeyboardBarSmartShortCutListener : Java.Lang.Object, View.IOnClickListener
    {

        NoteActivity _editor;
        public KeyboardBarSmartShortCutListener(NoteActivity editor)
        {
            _editor = editor;
        }

        public void OnClick(View v)
        {
            var shortcut = ((TextView)v).Text;
            if (_editor.Content.HasSelection)
            {
                var selected = _editor.Content.Text.Substring(_editor.Content.SelectionStart,
                        _editor.Content.SelectionEnd - _editor.Content.SelectionStart);
                var before = _editor.Content.Text.Substring(0, _editor.Content.SelectionStart);
                var after = _editor.Content.Text.Substring(_editor.Content.SelectionEnd);

                _editor.Content.Text = before + shortcut[0] + selected + shortcut[1] + after;
                try
                {
                    _editor.Content.SetSelection(before.Length + shortcut.Length + selected.Length);
                }
                catch (Java.Lang.IndexOutOfBoundsException ex)
                {
                    Log.Error(NoteActivity.Tag, $"Error trying to reset selection");
                }
            }
            else
            {
                var prevSelectionStart = _editor.Content.SelectionStart;
                _editor.Content.Text = _editor.Content.Text.Insert(_editor.Content.SelectionStart, shortcut);
                try
                {
                    _editor.Content.SetSelection(prevSelectionStart + shortcut.Length - 1);
                }
                catch (Java.Lang.IndexOutOfBoundsException ex)
                {
                    Log.Error(NoteActivity.Tag, $"Error trying to reset selection");
                }
            }
        }
    }
}