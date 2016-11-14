
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
		RecyclerView _issuesListView;
		RecyclerView.LayoutManager _layoutManager;

		protected override async void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Issues);


			_issuesListView = FindViewById<RecyclerView>(Resource.Id.issuesListView);
			_layoutManager = new LinearLayoutManager(this);
			_issuesListView.SetLayoutManager(_layoutManager);


			await ShowLabels();
		}

		private async Task ShowLabels()
		{
			var issues = await GitHubClient.Client().GetIssues();
			var adapter = new IssuesAdapter(issues.ToList());

			adapter.OnIssueSelected += (selected) => {
				Intent intent = new Intent(this, typeof(IssueActivity));
				StartActivity(intent);
			};
			_issuesListView.SetAdapter(adapter);
		}
	}
}
