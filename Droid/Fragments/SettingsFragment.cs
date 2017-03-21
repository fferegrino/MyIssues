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
using Android.Support.V7.Preferences;
using MyIssues.Droid.Activities;
using MyIssues.DataAccess;

namespace MyIssues.Droid.Fragments
{
    [Register("MyIssues.Droid.Fragments.SettingsFragment")]
    public class SettingsFragment : PreferenceFragmentCompat
    {

        const int SelectRepoRequestCode = 1;

        public EventHandler<long> DidChangedRepoEventHandler;

        public EventHandler DidEraseData;

        public override void OnCreatePreferences(Bundle savedInstanceState, string rootKey)
        {

            AddPreferencesFromResource(Resource.Xml.Preferences);

            var aboutPreference = FindPreference("about");
            aboutPreference.PreferenceClick += (sender, e) =>
            {
                var i = new Intent(this.Context, typeof(ThanksActivity));
                StartActivity(i);
            };

            var supportPreference = FindPreference("support");
            supportPreference.PreferenceClick += (sender, e) =>
            {
                Intent intent = new Intent(Intent.ActionSend);
                intent.SetType("text/plain");
                intent.PutExtra(Intent.ExtraEmail, new String[] { "apps@messier16.com" });
                intent.PutExtra(Intent.ExtraSubject, "SimpleIssues support");
                StartActivity(Intent.CreateChooser(intent, "Send Email"));
            };

            var switchRepoPreference = FindPreference("switchRepo");
            switchRepoPreference.PreferenceClick += (sender, e) =>
            {
                var i = new Intent(this.Context, typeof(ReposActivity));
                StartActivityForResult(i, SelectRepoRequestCode);
            };

            var accountPreference = FindPreference("account");
            accountPreference.PreferenceClick += (sender, e) =>
            {
                var alertDialog = new AlertDialog.Builder(this.Context)
                    .SetTitle(Resources.GetString(Resource.String.erase_account_confirmation_title))
                    .SetMessage(Resources.GetString(Resource.String.erase_account_confirmation_body))
                    .SetPositiveButton(Resources.GetString(Resource.String.dialog_yes), DeleteAccountClick)
                    .SetNegativeButton(Resources.GetString(Resource.String.dialog_no), listener: null);

                alertDialog.Show();
            };
        }

        async void DeleteAccountClick(object sender, DialogClickEventArgs e)
        {
            await Storage.GetInstance().EraseAll();
            DidEraseData?.Invoke(this, new EventArgs());
        }


        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            if (requestCode == SelectRepoRequestCode && resultCode == (int)Result.Ok)
            {
                var repoId = data.GetLongExtra("repoId", 0);
                DidChangedRepoEventHandler.Invoke(this, repoId);

            }
            else
            {
                base.OnActivityResult(requestCode, resultCode, data);
            }
        }
    }
}