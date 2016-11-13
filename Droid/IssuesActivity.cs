
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MyIssues.Droid.Adapters;

namespace MyIssues.Droid
{

	[Activity(Label = "Issues",
		Theme = "@style/MyTheme")]
	public class IssuesActivity : Activity
	{
		ListViewCompat issuesListView;

		protected override async void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Issues);

			var issues = await GitHubClient.Client().GetIssues();

			issuesListView = FindViewById<ListViewCompat>(Resource.Id.issuesListView);

			var adapter = new IssuesAdapter(this, issues.ToList());
			issuesListView.Adapter = adapter;
		}
	}
}
