// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MyIssues2.iOS.Cells
{
	[Register ("IssueCellView")]
	partial class IssueCellView
	{
		[Outlet]
		UIKit.UILabel IssueTitleLabel { get; set; }

		[Outlet]
		UIKit.UIView LabelColorView1 { get; set; }

		[Outlet]
		UIKit.UIView LabelColorView2 { get; set; }

		[Outlet]
		UIKit.UIView LabelColorView3 { get; set; }

		[Outlet]
		UIKit.UIView LabelColorView4 { get; set; }

		[Outlet]
		UIKit.UIView LabelColorView5 { get; set; }

		[Outlet]
		UIKit.UILabel MilestoneDayLabel { get; set; }

		[Outlet]
		UIKit.UILabel MilestoneLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (MilestoneLabel != null) {
				MilestoneLabel.Dispose ();
				MilestoneLabel = null;
			}

			if (MilestoneDayLabel != null) {
				MilestoneDayLabel.Dispose ();
				MilestoneDayLabel = null;
			}

			if (IssueTitleLabel != null) {
				IssueTitleLabel.Dispose ();
				IssueTitleLabel = null;
			}

			if (LabelColorView1 != null) {
				LabelColorView1.Dispose ();
				LabelColorView1 = null;
			}

			if (LabelColorView2 != null) {
				LabelColorView2.Dispose ();
				LabelColorView2 = null;
			}

			if (LabelColorView3 != null) {
				LabelColorView3.Dispose ();
				LabelColorView3 = null;
			}

			if (LabelColorView4 != null) {
				LabelColorView4.Dispose ();
				LabelColorView4 = null;
			}

			if (LabelColorView5 != null) {
				LabelColorView5.Dispose ();
				LabelColorView5 = null;
			}
		}
	}
}
