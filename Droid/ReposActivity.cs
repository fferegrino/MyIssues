
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
	public class ReposActivity : AppCompatActivity
	{

		ListView _reposListView;
		GitHubClient _client;
       List< Octokit.Repository> repos;

		protected override async void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
            _client = GitHubClient.Client();

			SetContentView(Resource.Layout.Repos);
			var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
			SetSupportActionBar(toolbar);


			_reposListView = FindViewById<ListView>(Resource.Id.reposListView);

			var repos1 = await _client.GetRepositoriesForUser();
			repos = repos1.ToList();

			var adapter = new ReposAdapter(this, repos);
			_reposListView.Adapter = (adapter);

            _reposListView.ItemClick += _reposListView_ItemClick;

		}

        private void _reposListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            
            Intent returnIntent = new Intent();
            returnIntent.PutExtra("repoName", repos[e.Position].Name);
            SetResult(Result.Ok, returnIntent);
            this.Finish();
        }
    }
}
