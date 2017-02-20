// This file has been autogenerated from a class added in the UI designer.

using System;
using System.Threading.Tasks;
using Foundation;
using MyIssues;
using MyIssues.DataAccess;
using UIKit;

namespace MyIssues2.iOS
{
	public partial class SetTokenViewController : UIViewController, IUITextViewDelegate
	{
		public SetTokenViewController (IntPtr handle) : base (handle)
		{
		}

		struct StoryboardId
		{
			public const string ViewIssuesSegue = "View Issues";
		}

		Storage _storage;
		public override async void ViewDidLoad()
		{
			base.ViewDidLoad();


			_storage = MyIssues.DataAccess.Storage.GetInstance();
			string accessToken = await _storage.GetToken();
			TokenTextView.ContentInset = new UIEdgeInsets(-10, 0, 0, 0);
			TokenTextView.TextAlignment = UITextAlignment.Center;
			TokenTextView.Delegate = this;
			if (!String.IsNullOrEmpty(accessToken))
			{
				await TryAuth(accessToken);
				TokenTextView.Text = accessToken;
			}
			else 
			{
				TokenTextView.Text = PlaceholderText;
				TokenTextView.TextColor = PlaceholderColor;
			}

		}

		[Export("textView:shouldChangeTextInRange:replacementText:")]
		public bool ShouldChangeText(UITextView textView, NSRange range, string text)
		{
			TokenTextView.ContentInset = new UIEdgeInsets(-10, 0, 0, 0);
			if (text == "\n")
			{
				TryAuth();
				return false;
			}
			return true;
		}

		partial void LearnMoreClick(NSObject sender)
		{
			UIApplication.SharedApplication.OpenUrl(new Uri("https://help.github.com/articles/creating-an-access-token-for-command-line-use/"));
		}


		async partial void ContinueButton(NSObject sender)
		{
			await TryAuth();
		}

		//public override void ViewDidLayoutSubviews()
		//{
		//	base.ViewDidLayoutSubviews();
		//	TokenTextView.SetContentOffset(CoreGraphics.CGPoint.Empty, false);
		//}

		async Task TryAuth(string token = null)
		{
			if(String.IsNullOrEmpty(token))
				token = TokenTextView.Text;
			if (!String.IsNullOrEmpty(token) && await Authenticate(token))
			{
				PerformSegue(StoryboardId.ViewIssuesSegue, this);
			}
			else
			{
				var authError = NSBundle.MainBundle.LocalizedString("Auth error", "optional");

				new UIAlertView("Error", authError, null, "OK", null).Show();
				System.Diagnostics.Debug.WriteLine("Not authed");
			}
		}

		async Task<bool> Authenticate(string accessToken)
		{
			ActivityIndicator?.StartAnimating();
			//ProgressDialog progress;
			//progress = ProgressDialog.Show(this, Resources.GetString(Resource.String.Authenticating),
			//							   Resources.GetString(Resource.String.PleaseWait), true);
			var cliente = GitHubClientFactory.CreateClient(accessToken);
			var authed = await _storage.SetClient(cliente);

			System.Diagnostics.Debug.WriteLine("Authenticate");
			if (authed)
			{
				await _storage.SaveToken(accessToken);
			}

			ActivityIndicator?.StopAnimating();
			return authed;
		}

		static UIColor PlaceholderColor = UIColor.Gray;
		const string PlaceholderText = "Set your token here";

		[Export("textViewDidBeginEditing:")]
		public void EditingStarted(UITextView textView)
		{
			if (TokenTextView.TextColor == PlaceholderColor)
			{
				TokenTextView.Text = null;
				TokenTextView.TextColor = UIColor.Black;
			}
		}

		[Export("textViewDidEndEditing:")]
		public void EditingEnded(UITextView textView)
		{
			if (String.IsNullOrEmpty(TokenTextView.Text))
			{
				TokenTextView.Text = PlaceholderText;
				TokenTextView.TextColor = PlaceholderColor;
			}
		}
	}
}
