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
using Android.Preferences;
using MyIssues.Droid.Fragments;
using Android.Support.V7.App;

namespace MyIssues.Droid.Activities
{
    [Activity(Label = "SettingsActivity",
        Theme = "@style/MyTheme")]
    public class SettingsActivity : AppCompatActivity
    {
        SettingsFragment _settingsFragment;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _settingsFragment = new SettingsFragment();

            _settingsFragment.DidChangedRepoEventHandler += (s, repoId) =>
            {
                Intent returnIntent = new Intent();
                returnIntent.PutExtra("didChangeRepo", repoId != 0);
                returnIntent.PutExtra("repoId", repoId);
                SetResult(Result.Ok, returnIntent);
                this.Finish();

            };

            SupportFragmentManager.BeginTransaction()
                .Replace(Android.Resource.Id.Content, _settingsFragment)
                    .Commit();

        }
    }
}