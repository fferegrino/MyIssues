
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Xamarin.Auth;

namespace MyIssues.Droid
{
	[Activity(Label = "Repositories")]
	public class ViewReposActivity : ActionBarActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
            var client = GitHubClient.Client();

			
		}
	}
}
