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
	[Register ("IssueDescriptionVIewController")]
	partial class IssueDescriptionVIewController
	{
		[Outlet]
		UIKit.UILabel IssueBodyLabel { get; set; }

		[Outlet]
		UIKit.UILabel IssueDateLabel { get; set; }

		[Outlet]
		UIKit.UILabel IssueStatusLabel { get; set; }

		[Outlet]
		UIKit.UIView IssueStatusView { get; set; }

		[Outlet]
		UIKit.UILabel IssueTitleLabel { get; set; }

		[Outlet]
		UIKit.UIScrollView LabelsView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (IssueStatusLabel != null) {
				IssueStatusLabel.Dispose ();
				IssueStatusLabel = null;
			}

			if (IssueDateLabel != null) {
				IssueDateLabel.Dispose ();
				IssueDateLabel = null;
			}

			if (IssueBodyLabel != null) {
				IssueBodyLabel.Dispose ();
				IssueBodyLabel = null;
			}

			if (IssueStatusView != null) {
				IssueStatusView.Dispose ();
				IssueStatusView = null;
			}

			if (IssueTitleLabel != null) {
				IssueTitleLabel.Dispose ();
				IssueTitleLabel = null;
			}

			if (LabelsView != null) {
				LabelsView.Dispose ();
				LabelsView = null;
			}
		}
	}
}
