// This file has been autogenerated from a class added in the UI designer.

using System;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using MyIssues.DataAccess;
using UIKit;

namespace MyIssues2.iOS
{
	public partial class ReplyToIssueViewController : UIViewController
	{

		struct StoryboardId
		{
			public const string PreviewReplySegueIdentifier = "Preview Reply";
		}
		public ReplyToIssueViewController(IntPtr handle) : base(handle)
		{
		}

		int _issueNumber;
		public void SetIssueNumber(int number)
		{
			_issueNumber = number;
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			NavBar.Title = $"Reply to #{_issueNumber}";
			NavBar.LeftBarButtonItem.Clicked += (sender, e) => Cancel();

			ReplyTextView.SelectAll(this);

			ReplyTextView.BecomeFirstResponder();
			var previewButton = NavBar.RightBarButtonItems
			                          .FirstOrDefault(b => "Preview".Equals(b.Title));
			var doneButton = NavBar.RightBarButtonItems
								   .FirstOrDefault(b => !"Preview".Equals(b.Title));

			doneButton.Clicked += ReplyButtonClicked;

			previewButton.Clicked += (sender, e) =>
			{
				PerformSegue(StoryboardId.PreviewReplySegueIdentifier, this);
			};


			var notificationCenter = NSNotificationCenter.DefaultCenter;
			notificationCenter.AddObserver(UIKeyboard.WillChangeFrameNotification, AdjustForKeyboard);
			notificationCenter.AddObserver(UIKeyboard.WillHideNotification, AdjustForKeyboard);
		}

		async void ReplyButtonClicked (object sender, EventArgs e)
		{
			await Reply();
			Cancel();
		}

		void AdjustForKeyboard(NSNotification notification)
		{
			var userInfo = notification.UserInfo;

			var keyboardScreenEndFrame = (userInfo[UIKeyboard.FrameEndUserInfoKey] as NSValue).CGRectValue;
			var keyboardViewEndFrame = View.ConvertRectFromView(keyboardScreenEndFrame, View.Window);

			if (notification.Name == UIKeyboard.WillHideNotification)
			{
				ReplyTextView.ContentInset = UIEdgeInsets.Zero;
			}
			else 
			{
				ReplyTextView.ContentInset = new UIEdgeInsets(0, 0, keyboardViewEndFrame.Height, 0);
			}

			ReplyTextView.ScrollIndicatorInsets = ReplyTextView.ContentInset;

			var selectedRange = ReplyTextView.SelectedRange;
			ReplyTextView.ScrollRangeToVisible(selectedRange);
		}

		void Cancel()
		{
			PresentingViewController.DismissModalViewController(true);
		}

		async Task<bool> Reply()
		{
			return await Storage.GetInstance().SendComment(_issueNumber, ReplyTextView.Text);
		}

		public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
		{
			if (segue.Identifier.Equals(StoryboardId.PreviewReplySegueIdentifier))
			{
				var destination = segue.DestinationViewController as PreviewReplyViewController;
				destination.Content = ReplyTextView.Text;
			}
			else
			{
				base.PrepareForSegue(segue, sender);
			}
		}
	}
}
