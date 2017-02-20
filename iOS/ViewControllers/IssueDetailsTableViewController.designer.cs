// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MyIssues2.iOS
{
	[Register ("IssueDetailsTableViewController")]
	partial class IssueDetailsTableViewController
	{
		[Outlet]
		UIKit.UILabel IssueBodyLabel { get; set; }

		[Outlet]
		UIKit.UILabel IssueCreationDateLabel { get; set; }

		[Outlet]
		UIKit.UITableViewCell IssueMilestoneCell { get; set; }

		[Outlet]
		UIKit.UILabel IssueMilestoneDateLabel { get; set; }

		[Outlet]
		UIKit.UILabel IssueMilestoneLabel { get; set; }

		[Outlet]
		UIKit.UILabel IssueNameLabel { get; set; }

		[Outlet]
		UIKit.UILabel IssueStatusLabel { get; set; }

		[Outlet]
		UIKit.UIView IssueStatusView { get; set; }

		[Outlet]
		UIKit.UIScrollView LabelsView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (IssueBodyLabel != null) {
				IssueBodyLabel.Dispose ();
				IssueBodyLabel = null;
			}

			if (IssueCreationDateLabel != null) {
				IssueCreationDateLabel.Dispose ();
				IssueCreationDateLabel = null;
			}

			if (IssueMilestoneCell != null) {
				IssueMilestoneCell.Dispose ();
				IssueMilestoneCell = null;
			}

			if (IssueMilestoneLabel != null) {
				IssueMilestoneLabel.Dispose ();
				IssueMilestoneLabel = null;
			}

			if (IssueMilestoneDateLabel != null) {
				IssueMilestoneDateLabel.Dispose ();
				IssueMilestoneDateLabel = null;
			}

			if (IssueNameLabel != null) {
				IssueNameLabel.Dispose ();
				IssueNameLabel = null;
			}

			if (IssueStatusLabel != null) {
				IssueStatusLabel.Dispose ();
				IssueStatusLabel = null;
			}

			if (IssueStatusView != null) {
				IssueStatusView.Dispose ();
				IssueStatusView = null;
			}

			if (LabelsView != null) {
				LabelsView.Dispose ();
				LabelsView = null;
			}
		}
	}
}
