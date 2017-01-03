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
	[Register ("SettingsTableViewController")]
	partial class SettingsTableViewController
	{
		[Outlet]
		UIKit.UILabel AboutLabel { get; set; }

		[Outlet]
		UIKit.UILabel AccountLabel { get; set; }

		[Outlet]
		UIKit.UILabel ChangeRepoLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ChangeRepoLabel != null) {
				ChangeRepoLabel.Dispose ();
				ChangeRepoLabel = null;
			}

			if (AccountLabel != null) {
				AccountLabel.Dispose ();
				AccountLabel = null;
			}

			if (AboutLabel != null) {
				AboutLabel.Dispose ();
				AboutLabel = null;
			}
		}
	}
}
