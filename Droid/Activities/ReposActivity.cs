
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
using SearchView = Android.Support.V7.Widget.SearchView;
using MyIssues.DataAccess;

namespace MyIssues.Droid
{
	[Activity(Label = "@string/repositories_activity_name",
		Theme = "@style/MyTheme")]
	//[IntentFilter( actions: new string[] { Intent.ActionSearch })]
	//[MetaData("android.app.searchable", Resource = "@xml/searchable")]
	public class ReposActivity : ActionBarActivity, View.IOnFocusChangeListener
	{

		ListView _reposListView;
		SearchView _searchView;
		Storage _storage;
		Spinner _repoKindSelector;
		List<Models.Repository> repos;
		TextView _instrucciones;
		ReposAdapter _adapter;


		protected override async void OnCreate(Bundle savedInstanceState)
		{
            base.OnCreate(savedInstanceState);
            _storage = Storage.GetInstance();

			SetContentView(Resource.Layout.Repos);
			var toolbar = FindViewById<Toolbar>(Resource.Id.Toolbar);
			SetSupportActionBar(toolbar);


			_reposListView = FindViewById<ListView>(Resource.Id.ReposListView);
			var repos1 = await _storage.GetRepositoriesForUser();
			repos = repos1.ToList();

			 _adapter = new ReposAdapter(this, repos);
			_reposListView.Adapter = (_adapter);
			_reposListView.ItemClick += _reposListView_ItemClick;

			_instrucciones = FindViewById<TextView>(Resource.Id.InstruccionesText);
			_instrucciones.Visibility = ViewStates.Gone;
			//_repoKindSelector = FindViewById<Spinner>(Resource.Id.RepoKindSelector);
			//var repoKindAdapter = ArrayAdapter.CreateFromResource(this,
			//                                                      Resource.Array.repo_kind, Android.Resource.Layout.SimpleSpinnerItem);
			//// Specify the layout to use when the list of choices appears
			//repoKindAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerItem);
			//// Apply the adapter to the spinner
			//_repoKindSelector.Adapter = (repoKindAdapter);


		}

        private void _reposListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            
            Intent returnIntent = new Intent();
			var repo = _adapter[e.Position];
			returnIntent.PutExtra("repoName", repo.Name);
            returnIntent.PutExtra("repoId", repo.Id);
            SetResult(Result.Ok, returnIntent);
            this.Finish();
        }

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.SearchMenu, menu);

			//// Associate searchable configuration with the SearchView
			var item = menu.FindItem(Resource.Id.ActionSearch);
			//var searchView = MenuItemCompat.GetActionView(item);
			//var a = item.ActionView;
							   _searchView = new SearchView(this);
			MenuItemCompat.SetActionView(item, _searchView);
			item.SetShowAsAction(ShowAsAction.CollapseActionView | ShowAsAction.IfRoom);
			//item.SetActionView(_searchView);

			_searchView.FocusChange += SearchView_FocusChange;
			_searchView.SetOnQueryTextFocusChangeListener(this);
			//item.SetOnMenuItemClickListener(this);

			_searchView.QueryTextChange += (sender, e) => {
				_adapter.Filter.InvokeFilter(e.NewText);
				System.Diagnostics.Debug.WriteLine("Shown elements :" + _adapter.Count);
			};

			_searchView.QueryTextSubmit += async (sender, e) =>
			{
				var searchResult = await _storage.SearchRepositories(e.Query);
				_adapter.Replace(searchResult);

				e.Handled = true;
			};

						return true;
		}

		void SearchView_FocusChange(object sender, View.FocusChangeEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine($"Has focus: {e.HasFocus}");
		}

		public bool OnMenuItemClick(IMenuItem item)
		{
			item.ExpandActionView();
			return true;
		}

		public void OnFocusChange(View v, bool hasFocus)
		{

			_instrucciones.Visibility = hasFocus ? ViewStates.Visible : ViewStates.Gone;
		}
	}
}
