// This file has been autogenerated from a class added in the UI designer.

using System;
using System.Threading.Tasks;
using Foundation;
using Humanizer;
using MyIssues.DataAccess;
using UIKit;

namespace MyIssues2.iOS
{
	public partial class IssueDetailsTableViewController : UITableViewController
	{
		public IssueDetailsTableViewController (IntPtr handle) : base (handle)
		{
		}


		Storage _storage;
		int _issueNumber;

		public override async void ViewDidLoad()
		{
			base.ViewDidLoad();

			TableView.RowHeight = UITableView.AutomaticDimension;
			TableView.EstimatedRowHeight = 140;
			TableView.SeparatorColor = UIColor.Clear;
			var controller = TabBarController as IssueTabBarViewController;
			_issueNumber = controller.IssueNumber;
			_storage = Storage.GetInstance();
			await LoadIssue();

			TableView.ReloadData();
		}

		Octokit.Issue _issue;
		public async Task LoadIssue()
		{
			_issue = await _storage.GetIssue(_issueNumber);
			IssueNameLabel.Text = _issue.Title;
			IssueStatusLabel.Text = _issue.State.ToString().ApplyCase(LetterCasing.Title);
			IssueCreationDateLabel.Text = _issue.CreatedAt.Humanize();

			//LabelsView.Delegate = this;
			LabelsView.ShowsVerticalScrollIndicator = false;
			int x = 0;
			foreach (var label in _issue.Labels)
			{
				var lbl = new UILabel { Text = label.Name, TextAlignment = UITextAlignment.Center }; //new UIView(new CoreGraphics.CGRect(x,0, 100,50));
				lbl.SizeToFit();
				lbl.Frame = new CoreGraphics.CGRect(x, lbl.Frame.Y, lbl.Frame.Width + 10, lbl.Frame.Height +10);

				var c = Storage.LabelColors[label.Name];
				var color = UIColor.FromRGB(c[0], c[1], c[2]);

				lbl.BackgroundColor = color;
				lbl.TextColor = color.ContrastingColor();
				LabelsView.AddSubview(lbl);
				System.Diagnostics.Debug.WriteLine($"{label.Name} : {x}");
				x += (int)lbl.Frame.Size.Width;
			}

			IssueBodyLabel.AttributedText = _issue.Body.FromMarkdown();
			System.Diagnostics.Debug.WriteLine(_issue.Body);

			LabelsView.ContentSize = new CoreGraphics.CGSize(x, LabelsView.Frame.Size.Height);
		}

		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
			return UITableView.AutomaticDimension;
		}

		public override nfloat EstimatedHeight(UITableView tableView, NSIndexPath indexPath)
		{
			return UITableView.AutomaticDimension;
			//returnreturn base.EstimatedHeight(tableView, indexPath);
		}
	}
}
