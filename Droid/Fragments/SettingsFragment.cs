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

namespace MyIssues.Droid.Fragments
{
    [Register("MyIssues.Droid.Fragments.SettingsFragment")]
    public class SettingsFragment : PreferenceFragmentCompat
    {

        const int SelectRepoRequestCode = 1;

        public EventHandler<long> DidChangedRepoEventHandler;

        public override void OnCreatePreferences(Bundle savedInstanceState, string rootKey)
        {

            AddPreferencesFromResource(Resource.Xml.Preferences);

            var aboutPreference = FindPreference("about");
            aboutPreference.PreferenceClick += (sender, e) =>
            {
                var i = new Intent(this.Context, typeof(ThanksActivity));
                StartActivity(i);
            };


            var switchRepoPreference = FindPreference("switchRepo");
            switchRepoPreference.PreferenceClick += (sender, e) =>
            {
                var i = new Intent(this.Context, typeof(ReposActivity));
                StartActivityForResult(i, SelectRepoRequestCode);
            };
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