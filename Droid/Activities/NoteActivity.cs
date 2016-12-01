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

namespace MyIssues.Droid.Activities
{
    [Activity(Label = "NoteActivity",
        Theme = "@style/MyTheme")]
    public class NoteActivity : ActionBarActivity
    {

        const string EMPTY_STRING = "";
        private File note;
        private Context context;

        private EditText noteTitle;
        internal HighlightingEditor Content { get; private set; }
        private ScrollView scrollView;

        private ViewGroup keyboardBarView;
        private string tarGetDirectory;
        private bool isPreviewIncoming = false;

        public NoteActivity()
        {
        }

        public NoteActivity(Context context)
        {
            this.context = context;
            this.note = null;
        }

        public NoteActivity(Context context, File note)
        {
            this.context = context;
            this.note = note;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Note);

            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            if (toolbar != null)
            {
                SetSupportActionBar(toolbar);
                SupportActionBar.SetDefaultDisplayHomeAsUpEnabled(true);
            }

            context = ApplicationContext;
            Content = FindViewById<HighlightingEditor>(Resource.Id.note_content);
            noteTitle = (EditText)FindViewById(Resource.Id.edit_note_title);
            scrollView = (ScrollView)FindViewById(Resource.Id.note_scrollview);
            keyboardBarView = (ViewGroup)FindViewById(Resource.Id.keyboard_bar);

            Intent receivingIntent = Intent;
            //tarGetDirectory = receivingIntent.GetStringExtra(Constants.TARGET_DIR);

            var intentAction = receivingIntent.Action;
            var type = receivingIntent.Type;

            if (Intent.ActionSend.Equals(intentAction) && type != null)
            {
                //OpenFromSendAction(receivingIntent);
            }
            else if (Intent.ActionEdit.Equals(intentAction) && type != null)
            {
                //OpenFromEditAction(receivingIntent);
            }
            else
            {
                //note = (File)GetIntent().GetSerializableExtra(Constants.NOTE_KEY);

            }

            if (note != null)
            {
                //content.Text = ReadNote();
                //noteTitle.SetText(note.Name.Replace("((?i)\\.md$)", ""));
            }
        }


        private string ReadNote()
        {
            //Java.Net.URI oldUri = note.ToURI();
            //return WriteilySingleton.GetInstance().readFileUri(Uri.parse(oldUri.ToString()), this);
            throw new NotImplementedException();
        }
        private void openFromSendAction(Intent receivingIntent)
        {
            //Uri fileUri = receivingIntent.GetParcelableExtra(Intent.EXTRA_STREAM);
            //readFileUriFromIntent(fileUri);
            throw new NotImplementedException();
        }

        private void openFromEditAction(Intent receivingIntent)
        {
            //Uri fileUri = receivingIntent.GetData();
            //readFileUriFromIntent(fileUri);
            throw new NotImplementedException();
        }

        private void readFileUriFromIntent(Uri fileUri)
        {
            //if (fileUri != null)
            //{
            //    note = WriteilySingleton.GetInstance().GetFileFromUri(fileUri);
            //    content.SetText(WriteilySingleton.GetInstance().readFileUri(fileUri, this));
            //}
            throw new NotImplementedException();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.note_menu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    base.OnBackPressed();
                    OverridePendingTransition(Resource.Animation.anim_slide_out_right, Resource.Animation.anim_slide_in_right);
                    return true;
                //case R.id.action_share:
                //    shareNote();
                //    return true;
                case Resource.Id.action_preview:
                    PreviewNote();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            OverridePendingTransition(Resource.Animation.anim_slide_out_right, Resource.Animation.anim_slide_in_right);
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
                FindViewById(Resource.Id.keyboard_bar_scroll).Visibility = ViewStates.Gone;
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
            TextView shortcutButton = (TextView)LayoutInflater.Inflate(Resource.Layout.keyboard_shortcut, null);
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

        private void ShareNote()
        {
            //saveNote();

            //String shareContent = content.GetText().toString();

            //Intent shareIntent = new Intent();
            //shareIntent.SetAction(Intent.ACTION_SEND);
            //shareIntent.putExtra(Intent.EXTRA_TEXT, shareContent);
            //shareIntent.SetType("text/plain");
            //startActivity(Intent.createChooser(shareIntent, Resources.GetText(Resource.String.share_string)));
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
            throw new NotImplementedException();
        }

        private void UpdateWidgets()
        {
            //AppWidGetManager appWidGetManager = AppWidGetManager.GetInstance(context);
            //int appWidGetIds[] = appWidGetManager.GetAppWidGetIds(
            //        new ComponentName(context, WriteilyWidGetProvider.class));
            //appWidGetManager.notifyAppWidGetViewDataChanged(appWidGetIds, R.id.widGet_notes_list);
            throw new NotImplementedException();
        }

        private string normalizeFilename(String content, String title)
        {
            //String filename = title;
            //if (filename.length() == 0)
            //{
            //    if (content.length() == 0)
            //    {
            //        return null;
            //    }
            //    else
            //    {
            //        if (content.length() < Constants.MAX_TITLE_LENGTH)
            //        {
            //            filename = content.substring(0, content.length());
            //        }
            //        else
            //        {
            //            filename = content.substring(0, Constants.MAX_TITLE_LENGTH);
            //        }
            //    }
            //}
            //filename = filename.replaceAll("[\\\\/:\"*?<>|]+", "").trim();

            //if (filename.isEmpty())
            //{
            //    filename = "Writeily - " + String.valueOf(UUID.randomUUID().GetMostSignificantBits()).substring(0, 6);
            //}
            //return filename;
            throw new NotImplementedException();
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
            _editor.Content.Text = _editor.Content.Text.Insert(_editor.Content.SelectionStart, shortcut);
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

                _editor.Content.Text = before+ shortcut[0] + selected + shortcut[1] + after;
            }
            else
            {
                var prevSelectionStart = _editor.Content.SelectionStart;
                _editor.Content.Text = _editor.Content.Text.Insert(_editor.Content.SelectionStart, shortcut);
                _editor.Content.SetSelection(prevSelectionStart - 1);
            }
        }
    }
}