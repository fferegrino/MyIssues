
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MyIssues.Droid.Adapters;
using MyIssues.Droid.Controls;

namespace MyIssues.Droid
{

    [Activity(Label = "Issues",
        Theme = "@style/MyTheme")]
    public class IssuesActivity : AppCompatActivity
    {
        const int SelectRepoRequestCode = 1;
		MyIssues.Droid.Controls.RecyclerViewEmptySupport _issuesListView;
        RecyclerView.LayoutManager _layoutManager;
        GitHubClient _client;
        Octokit.Repository _repo;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Issues);
			var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
			SetSupportActionBar(toolbar);


            _client = GitHubClient.Client();


            _issuesListView = FindViewById<RecyclerViewEmptySupport>(Resource.Id.issuesListView);
			var view = FindViewById(Resource.Id.list_empty);
			_issuesListView.EmptyView = view;
            _layoutManager = new LinearLayoutManager(this);
            _issuesListView.SetLayoutManager(_layoutManager);


            string repo = null;
            if (repo == null)
            {
				OpenRepoSelector();
            }
            else
            {
                await LoadRepo(repo);
            }
        }

        private async Task LoadRepo(string repo)
        {
			
            _repo = await _client.GetRepo(repo);

			Title = _repo.Name;
            var issues = await _client.GetIssues(repo);

            var adapter = new IssuesAdapter(issues.ToList());

            adapter.OnIssueSelected += (selected) =>
            {
                Intent intent = new Intent(this, typeof(IssueActivity));
				intent.PutExtra("id", selected.Id);
				intent.PutExtra("number", selected.Number);
				intent.PutExtra("title", selected.Title);
                StartActivity(intent);
            };
            _issuesListView.SetAdapter(adapter);
        }

		void OpenRepoSelector()
		{

            var i = new Intent(this, typeof(ReposActivity));
			StartActivityForResult(i, SelectRepoRequestCode);
		}

        protected override async void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (requestCode == SelectRepoRequestCode)
            {
                var repoName = data.GetStringExtra("repoName");
                await LoadRepo(repoName);
            }
            else
            {
                base.OnActivityResult(requestCode, resultCode, data);
            }
        }

		public override bool OnCreateOptionsMenu(IMenu menu)
		{

			MenuInflater.Inflate(Resource.Menu.IssuesMenu, menu);
			return true;
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
				case Resource.Id.SwitchRepoMenu:
					OpenRepoSelector();
					break;
				default:
					return base.OnOptionsItemSelected(item);
			}
			return true;
		}
    }
}
