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
	[Register ("PreviewReplyViewController")]
	partial class PreviewReplyViewController
	{
		[Outlet]
		UIKit.UILabel ContentLabel { get; set; }

		[Outlet]
		UIKit.UINavigationItem NavBar { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ContentLabel != null) {
				ContentLabel.Dispose ();
				ContentLabel = null;
			}

			if (NavBar != null) {
				NavBar.Dispose ();
				NavBar = null;
			}
		}
	}
}
