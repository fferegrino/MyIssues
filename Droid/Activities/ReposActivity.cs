
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using MyIssues.Droid.Adapters;
using Xamarin.Auth;
using Toolbar = Android.Support.V7.Widget.Toolbar;
//using SearchView = Android.Support.V7.Widget.SearchView;
using MyIssues.DataAccess;

namespace MyIssues.Droid
{
	[Activity(Label = "Repositories",
		Theme = "@style/MyTheme")]
	//[IntentFilter( actions: new string[] { Intent.ActionSearch })]
	//[MetaData("android.app.searchable", Resource = "@xml/searchable")]
	public class ReposActivity : ActionBarActivity
	{

		ListView _reposListView;
		SearchView _searchView;
		Storage _storage;
       List< Models.Repository> repos;

		protected override async void OnCreate(Bundle savedInstanceState)
		{
            base.OnCreate(savedInstanceState);
            _storage = Storage.GetInstance();

			SetContentView(Resource.Layout.Repos);
			var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
			SetSupportActionBar(toolbar);


			_reposListView = FindViewById<ListView>(Resource.Id.reposListView);

			var repos1 = await _storage.GetRepositoriesForUser();
			repos = repos1.ToList();

			var adapter = new ReposAdapter(this, repos);
			_reposListView.Adapter = (adapter);

            //_reposListView.ItemClick += _reposListView_ItemClick;

			HandleIntent(Intent);

		}

        private void _reposListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            
            Intent returnIntent = new Intent();
            returnIntent.PutExtra("repoName", repos[e.Position].Name);
            returnIntent.PutExtra("repoId", repos[e.Position].Id);
            SetResult(Result.Ok, returnIntent);
            this.Finish();
        }

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.SearchMenu, menu);

			//// Associate searchable configuration with the SearchView
			var item = menu.FindItem(Resource.Id.action_search);
			//var searchView = MenuItemCompat.GetActionView(item);
			//var a = item.ActionView;
							   _searchView = new SearchView(this);
			MenuItemCompat.SetActionView(item, _searchView);
			//item.SetActionView(_searchView);

			_searchView.QueryTextChange += (sender, e) => {
				System.Diagnostics.Debug.WriteLine(e.NewText);;
			};

			_searchView.QueryTextSubmit += (sender, e) =>
			{
				System.Diagnostics.Debug.WriteLine(e.Query);
				e.Handled = true;
			};

						return true;
		}

		protected override void OnNewIntent(Intent intent)
		{
			HandleIntent(intent);
		}

		private void HandleIntent(Intent intent)
		{
			if (Intent.ActionSearch.Equals(intent.Action))
			{
				String query = intent.GetStringExtra(SearchManager.Query);
				//use the query to search your data somehow
				System.Diagnostics.Debug.WriteLine("Search " + query);
			}
		}
    }
}
