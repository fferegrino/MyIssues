// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MyIssues.iOS
{
	[Register ("SetAccessTokenViewController")]
	partial class SetAccessTokenViewController
	{
		[Outlet]
		UIKit.UITextView tokenTextView { get; set; }

		[Action ("RequestInfo:")]
		partial void RequestInfo (Foundation.NSObject sender);

		[Action ("SetToken:")]
		partial void SetToken (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (tokenTextView != null) {
				tokenTextView.Dispose ();
				tokenTextView = null;
			}
		}
	}
}
