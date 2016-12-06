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
using MyIssues.Droid.Controls;

namespace MyIssues.Droid.Activities
{
	
    [Activity(Label = "ThanksActivity",
        //Name = "myissues.droid.activities.ThanksActivity",
        Theme = "@style/MyTheme")]
    public class ThanksActivity : AppCompatActivity
    {
        
	MarkdownView _thanksMarkdown;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.Thanks);

			_thanksMarkdown = FindViewById<MarkdownView>(Resource.Id.ThanksMarkdown);
			_thanksMarkdown.LoadMarkdownFile(
				"file:///android_asset/thanks.md", 
				"file:///android_asset/github-markdown.css");
		}
    }
}