
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
using MyIssues.Droid.Adapters;
using Xamarin.Auth;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace MyIssues.Droid
{
	[Activity(Label = "Repositories", 
        Theme = "@style/MyTheme")]
	public class ViewReposActivity : AppCompatActivity
	{

		ListView _reposListView;

		GitHubClient _client;

		protected override async void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
            _client = GitHubClient.Client();

			SetContentView(Resource.Layout.ReposView);
			var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
			SetSupportActionBar(toolbar);


			_reposListView = FindViewById<ListView>(Resource.Id.reposListView);

			var repos = await _client.GetRepositoriesForUser();
			var reposArray = repos.ToList();

			var adapter = new RepoAdapter(this, reposArray);
			_reposListView.Adapter = (adapter);


		}


	}
}
