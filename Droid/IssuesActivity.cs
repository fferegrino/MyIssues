
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

namespace MyIssues.Droid
{

    [Activity(Label = "Issues",
        Theme = "@style/MyTheme")]
    public class IssuesActivity : AppCompatActivity
    {
        const int SelectRepoRequestCode = 1;
        RecyclerView _issuesListView;
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


            _issuesListView = FindViewById<RecyclerView>(Resource.Id.issuesListView);
            _layoutManager = new LinearLayoutManager(this);
            _issuesListView.SetLayoutManager(_layoutManager);


            string repo = "that-c-sharp-guy";
            if (repo == null)
            {
                var i = new Intent(this, typeof(ReposActivity));
                StartActivityForResult(i, SelectRepoRequestCode);
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
    }
}
