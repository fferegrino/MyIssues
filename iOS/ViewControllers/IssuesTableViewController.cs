using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Foundation;
using MyIssues.DataAccess;
using MyIssues.Models;
using MyIssues2.iOS.Cells;
using UIKit;
using Humanizer;

namespace MyIssues2.iOS
{
	public partial class IssuesTableViewController : UITableViewController
	{
		public IssuesTableViewController(IntPtr handle) : base(handle)
		{
		}

		struct StoryboardId
		{
			public const string IssueCellIdentifier = "Issue Cell";
			public const string IssueNoMilestoneCellIdentifier = "Issue No Milestone Cell";
			public const string ViewIssueDetailSegue = "View Issue Detail";
			public const string ShowSettingsSegue = "Show Settings";
			public const string SwitchRepoSegue = "Switch Repo";
			public const string ShowLabelsSegue = "Show Labels";
		}

		public long RepoId { get; set; }
		public string ViewingLabel { get; private set; }

		Storage _storage;
		Octokit.Repository _repo;
		List<Issue> _originalIssues;
		List<Issue> _shownIssues;

		public override async void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			if (RepoId == 0)
			{
				OpenRepoSelector();
			}
			else
			{
				await LoadRepo();
			}

			if (String.IsNullOrEmpty(ViewingLabel))
			{
				SetNavigateToLabelsEvent();
			}
			else
			{
				SetRemoveFilterEvent();
			}
		}

		public override async void ViewDidLoad()
		{
			base.ViewDidLoad();
			TableView.RowHeight = UITableView.AutomaticDimension;
			TableView.EstimatedRowHeight = 70;
			_storage = Storage.GetInstance();
			if (RepoId == 0)
				RepoId = await _storage.GetWorkingRepo();



		}

		async Task LoadRepo()
		{
			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
			try
			{
				await _storage.SetWorkingRepo(RepoId);
			}
			catch (Exception e)
			{
				UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
				OpenRepoSelector();
				return;
			}
			_repo = await _storage.GetRepo(RepoId);

			Title = _repo.Name;
			NavigationItem.BackBarButtonItem = new UIBarButtonItem(Title.Truncate(7), UIBarButtonItemStyle.Plain, null);

			var observableIssues = _storage.GetIssues();
			observableIssues.Subscribe(UpdateIssues, onCompleted:  DoneLoadingIssues);
		}

		void OpenRepoSelector()
		{
			PerformSegue(StoryboardId.SwitchRepoSegue, this);
		}

		void UpdateIssues(List<Issue> issues)
		{
			_originalIssues = issues;
		}

		void DoneLoadingIssues()
		{
			InvokeOnMainThread(() =>
			{
				if (String.IsNullOrEmpty(ViewingLabel))
				{
					_shownIssues = _originalIssues;
				}
				else
				{
					_shownIssues = _originalIssues.Where(i => i.Labels.Where(l => l.Equals(ViewingLabel)).Any()).ToList();
				}
				TableView.ReloadData();
			});
			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
		}

		public void SetNavigateToLabelsEvent()
		{
			NavigationItem.RightBarButtonItem.Image = UIImage.FromFile("toolbar_labels.png");
			NavigationItem.RightBarButtonItem.Clicked -= RemoveFilter;
			NavigationItem.RightBarButtonItem.Clicked -= NavigateToLabels;
			NavigationItem.RightBarButtonItem.Clicked += NavigateToLabels;
		}

		public void SetRemoveFilterEvent()
		{

			NavigationItem.RightBarButtonItem.Image = UIImage.FromFile("toolbar_labels_remove.png");
			NavigationItem.RightBarButtonItem.Clicked -= NavigateToLabels;
			NavigationItem.RightBarButtonItem.Clicked -= RemoveFilter;
			NavigationItem.RightBarButtonItem.Clicked += RemoveFilter;
		}

		void RemoveFilter(object sender, EventArgs e)
		{
			ViewingLabel = null;
			SetNavigateToLabelsEvent();
			DoneLoadingIssues();
		}

		void NavigateToLabels(object sender, EventArgs e)
		{
			SetRemoveFilterEvent();
			PerformSegue(StoryboardId.ShowLabelsSegue, this);
		}

		#region Table stuff
		public override nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}

		public override nint RowsInSection(UITableView tableView, nint section)
		{
			return _shownIssues?.Count ?? 0;
		}


		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var issue = _shownIssues[indexPath.Row];
			if (!String.IsNullOrEmpty(issue.Milestone))
			{
				var cell = TableView.DequeueReusableCell(StoryboardId.IssueCellIdentifier, indexPath) as IssueCellView;
				if (cell != null)
				{
					cell.SetIssue(issue);
				}
				return cell;
			}
			else 
			{
				var cell = TableView.DequeueReusableCell(StoryboardId.IssueNoMilestoneCellIdentifier, indexPath) as IssueNoMilestoneCellView;
				if (cell != null)
				{
					cell.SetIssue(issue);
				}
				return cell;
			}
		}

		int _issueNumber;
		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			_issueNumber = _shownIssues[indexPath.Row].Number;
			PerformSegue(StoryboardId.ViewIssueDetailSegue, tableView);

		}

		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{	
			return UITableView.AutomaticDimension;
		}

		public override nfloat EstimatedHeight(UITableView tableView, NSIndexPath indexPath)
		{
			return UITableView.AutomaticDimension;
		}
		#endregion


		#region Segues

		public override bool ShouldPerformSegue(string segueIdentifier, NSObject sender)
		{
			if (StoryboardId.ViewIssueDetailSegue.Equals(segueIdentifier))
			{
				return _issueNumber != 0;
			}
			return base.ShouldPerformSegue(segueIdentifier, sender);
		}

		public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
		{
			
			if (StoryboardId.ViewIssueDetailSegue.Equals(segue.Identifier))
			{
				var destination = segue.DestinationViewController.ContentViewController() as IssueTabBarViewController;
				destination.IssueNumber = _issueNumber;
			}
			else if (StoryboardId.ShowLabelsSegue.Equals(segue.Identifier))
			{
				var destination = segue.DestinationViewController.ContentViewController() as LabelsTableViewController;
				destination.LabelSelected += (s, selectedLabel) =>
				{
					ViewingLabel = selectedLabel;
				};
			}
			else
			{
				base.PrepareForSegue(segue, sender);
			}
		}


		#endregion
	}
}
