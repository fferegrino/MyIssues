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
using Humanizer;
using MyIssues.DataAccess;
using Android.Support.Design.Widget;

namespace MyIssues.Droid
{
	[Activity(Label = "Issue",
		Theme = "@style/MyTheme")]
	public class IssueActivity : AppCompatActivity
	{

        Storage _storage;
		Octokit.Issue _issue;

		protected override async void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.Issue);
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            var title = Intent.GetStringExtra("title");
			var id = Intent.GetIntExtra("id", -1); 
			var number = Intent.GetIntExtra("number", -1);

			Title = $"Issue #{number}";


            //CollapsingToolbarLayout collapsingToolbar =
            //        (CollapsingToolbarLayout)FindViewById(Resource.Id.collapsing_toolbar);


            _storage = Storage.GetInstance();
			await LoadIssue(number);

            await LoadIssueComments(number);

			// Create your application here
		}

		async Task LoadIssue(int number)
		{
            _issue = await _storage.GetIssue(number);
            var issueBodyTextView = FindViewById<TextView>(Resource.Id.IssueBodyTextView);
            if (String.IsNullOrEmpty(_issue.Body))
            {
                issueBodyTextView.Visibility = ViewStates.Gone;
            }
            else
            {
                issueBodyTextView.Visibility = ViewStates.Visible;
                issueBodyTextView.Click += ViewMoreIssueButton_Click;
                const int bodyLength = 50;
                issueBodyTextView.Text = _issue.Body.Truncate(bodyLength, Truncator.FixedNumberOfWords);
            }

            var issueTitleTextView = FindViewById<TextView>(Resource.Id.IssueTitle);
            issueTitleTextView.Text = _issue.Title;

            var issueStatusTextView = FindViewById<TextView>(Resource.Id.IssueStatusTextView);
            issueStatusTextView.Text = _issue.State.ToString();

            issueStatusTextView.SetBackgroundColor(_issue.State == Octokit.ItemState.Closed ?
                                                   Resources.GetColor(Resource.Color.closedIssue) :
                                                   Resources.GetColor(Resource.Color.openIssue));

            var issueCreatedAtTextView = FindViewById<TextView>(Resource.Id.IssueCreatedAtTextView);
            issueCreatedAtTextView.Text = _issue.CreatedAt.Humanize(DateTimeOffset.Now);

            var _layoutManager = new LinearLayoutManager(this, LinearLayoutManager.Horizontal, false);

            var labelsRecyclerView = FindViewById<RecyclerView>(Resource.Id.LabelsRecyclerView);
            labelsRecyclerView.SetLayoutManager(_layoutManager);

            var adapter = new LabelsAdapter(_issue.Labels.ToList());
            labelsRecyclerView.SetAdapter(adapter);

            //System.Diagnostics.Debug.WriteLine("Comments : " + _issue.CommentsUrl);
        }

        private void ViewMoreIssueButton_Click(object sender, EventArgs e)
        {

            View popupView = LayoutInflater.Inflate(Resource.Layout.MarkdownExtendedCardView, null);

            PopupWindow popupWindow = new PopupWindow(popupView, ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);


    // If the PopupWindow should be focusable
    popupWindow.Focusable = (true);
            popupWindow.SetBackgroundDrawable(new Android.Graphics.Drawables.ColorDrawable(Android.Graphics.Color.Transparent));

            var fullMarkdownTextView = popupView.FindViewById<TextView>(Resource.Id.FullMarkdownTextView);
            fullMarkdownTextView.Text = _issue.Body;
            

            int[] location = new int[2];

            // Get the View's(the one that was clicked in the Fragment) location
            var anchorView = sender as TextView;
            anchorView.GetLocationOnScreen(location);

            // Using location, the PopupWindow will be displayed right under anchorView
            popupWindow.ShowAtLocation(anchorView,GravityFlags.NoGravity,
                                            0, 0);

        }

        async Task LoadIssueComments(int number)
        {
            var comments = (await _storage.GetIssueComments(number)).ToList();



            var _layoutManager = new LinearLayoutManager(this);
            var adapter = new IssueCommentsAdapter(comments);

            var issueCommentsListView = FindViewById<RecyclerView>(Resource.Id.IssueCommentsListView);


            issueCommentsListView.SetLayoutManager(_layoutManager);
            issueCommentsListView.SetAdapter(adapter);

            //System.Diagnostics.Debug.WriteLine("Comments : " + _issue.CommentsUrl);
        }
    }
}
