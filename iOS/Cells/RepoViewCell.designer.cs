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
	[Register ("RepoViewCell")]
	partial class RepoViewCell
	{
		[Outlet]
		UIKit.UILabel DescriptionLabel { get; set; }

		[Outlet]
		UIKit.UILabel LanguageLabel { get; set; }

		[Outlet]
		UIKit.UIView LanguageView { get; set; }

		[Outlet]
		UIKit.UILabel OwnerLabel { get; set; }

		[Outlet]
		UIKit.UILabel RepoNameLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (DescriptionLabel != null) {
				DescriptionLabel.Dispose ();
				DescriptionLabel = null;
			}

			if (LanguageLabel != null) {
				LanguageLabel.Dispose ();
				LanguageLabel = null;
			}

			if (OwnerLabel != null) {
				OwnerLabel.Dispose ();
				OwnerLabel = null;
			}

			if (RepoNameLabel != null) {
				RepoNameLabel.Dispose ();
				RepoNameLabel = null;
			}

			if (LanguageView != null) {
				LanguageView.Dispose ();
				LanguageView = null;
			}
		}
	}
}
