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
        
    protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SupportFragmentManager.BeginTransaction()
                .Replace(Android.Resource.Id.Content, new SettingsFragment())
                    .Commit();

        }
    }
}