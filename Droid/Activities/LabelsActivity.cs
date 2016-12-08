
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
using Android.Support.V7.Widget;

namespace MyIssues.Droid
{
	[Activity(Label = "Labels",
		Theme = "@style/MyTheme")]
	//[IntentFilter( actions: new string[] { Intent.ActionSearch })]
	//[MetaData("android.app.searchable", Resource = "@xml/searchable")]
	public class LabelsActivity : AppCompatActivity
	{

		ListView _reposListView;
		SearchView _searchView;
		Storage _storage;
		Spinner _repoKindSelector;
		List<Models.Repository> repos;
		TextView _instrucciones;
		ReposAdapter _adapter;

		RecyclerView _labelsRecyclerView;



		protected override async void OnCreate(Bundle savedInstanceState)
		{
            base.OnCreate(savedInstanceState);
            _storage = Storage.GetInstance();

			SetContentView(Resource.Layout.Labels);
			var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
			SetSupportActionBar(toolbar);



			_labelsRecyclerView = FindViewById<RecyclerView>(Resource.Id.LabelsRecyclerView);

			var _layoutManager = new LinearLayoutManager(this, LinearLayoutManager.Vertical, false);

			_labelsRecyclerView.SetLayoutManager(_layoutManager);
			var labels = await _storage.GetLabels();
			var adapter = new ModelLabelsAdapter(labels.ToList());
			_labelsRecyclerView.SetAdapter(adapter);

		}

	}
}
