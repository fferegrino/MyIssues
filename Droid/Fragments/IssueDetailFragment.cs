using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using MyIssues.DataAccess;
using System.Threading.Tasks;
using Android.Support.V7.Widget;
using Humanizer;
using MyIssues.Droid.Adapters;
using MyIssues.Droid.Controls;

namespace MyIssues.Droid.Fragments
{
    public class IssueDetailFragment : Android.Support.V4.App.Fragment
    {
        Storage _storage;
        Octokit.Issue _issue;
        MarkdownView _issueBodyTextView;
		TextView _issueTitleTextView,
			_issueStatusTextView,
			_issueCreatedAtTextView,
			_issueMilestoneInfo,
			_issueMilestoneDate;
        RecyclerView _labelsRecyclerView;

        int _issueNumber;


        public IssueDetailFragment(int issueNumber)
        {
            _issueNumber = issueNumber;
        }
        public override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _storage = Storage.GetInstance();
            await LoadIssue();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.IssueDetailView, container, false);

            _issueBodyTextView = view.FindViewById<MarkdownView>(Resource.Id.IssueBodyTextView);
			_issueMilestoneDate = view.FindViewById<TextView>(Resource.Id.IssueMilestoneDate);
            _issueTitleTextView = view.FindViewById<TextView>(Resource.Id.IssueTitle);
            _issueStatusTextView = view.FindViewById<TextView>(Resource.Id.IssueStatusTextView);
            _issueCreatedAtTextView = view.FindViewById<TextView>(Resource.Id.IssueCreatedAtTextView);
            _issueMilestoneInfo = view.FindViewById<TextView>(Resource.Id.IssueMilestoneInfo);
            _labelsRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.LabelsRecyclerView);

            return view;
        }


        public async Task LoadIssue()
        {
            _issue = await _storage.GetIssue(_issueNumber);

            if (String.IsNullOrEmpty(_issue.Body))
            {
                _issueBodyTextView.Visibility = ViewStates.Gone;
            }
            else
            {
                _issueBodyTextView.Visibility = ViewStates.Visible;
                _issueBodyTextView.LoadMarkdown(_issue.Body, "file:///android_asset/github-markdown.css");
            }

            _issueTitleTextView.Text = _issue.Title;

            _issueStatusTextView.Text = _issue.State.ToString();
            if (_issue.Milestone != null)
            {
				if (_issue.Milestone.DueOn.HasValue)
				{
					_issueMilestoneDate.Text = $"{_issue.Milestone.DueOn.GetValueOrDefault().DateTime:dd MMM}";
				}
				else 
				{
					_issueMilestoneDate.Text = "--";
				}
				_issueMilestoneDate.Visibility = ViewStates.Visible;
                _issueMilestoneInfo.Visibility = ViewStates.Visible;
				_issueMilestoneInfo.Text = $"{_issue.Milestone.Title}";
            }
            else
            {
				_issueMilestoneDate.Visibility = ViewStates.Gone;
                _issueMilestoneInfo.Visibility = ViewStates.Gone;
            }

            _issueStatusTextView.SetBackgroundColor(_issue.State == Octokit.ItemState.Closed ?
                                                    Resources.GetColor(Resource.Color.closed_issue) :
                                                    Resources.GetColor(Resource.Color.open_issue));

            _issueCreatedAtTextView.Text = _issue.CreatedAt.Humanize(DateTimeOffset.Now);

            var _layoutManager = new LinearLayoutManager(this.Context, LinearLayoutManager.Horizontal, false);

            _labelsRecyclerView.SetLayoutManager(_layoutManager);

            var adapter = new LabelsAdapter(_issue.Labels.ToList());
            _labelsRecyclerView.SetAdapter(adapter);

            System.Diagnostics.Debug.WriteLine("Comments : " + _issue.CommentsUrl);
        }
    }
}

