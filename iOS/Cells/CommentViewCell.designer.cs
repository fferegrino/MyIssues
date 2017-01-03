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
	[Register ("CommentViewCell")]
	partial class CommentViewCell
	{
		[Outlet]
		UIKit.UILabel BodyLabel { get; set; }

		[Outlet]
		UIKit.UILabel CreatedAtLabel { get; set; }

		[Outlet]
		UIKit.UILabel UsernameLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (UsernameLabel != null) {
				UsernameLabel.Dispose ();
				UsernameLabel = null;
			}

			if (CreatedAtLabel != null) {
				CreatedAtLabel.Dispose ();
				CreatedAtLabel = null;
			}

			if (BodyLabel != null) {
				BodyLabel.Dispose ();
				BodyLabel = null;
			}
		}
	}
}
