using System;
using System.Threading.Tasks;
using MyIssues;
using MyIssues.DataAccess;
using UIKit;

namespace MyIssues2.iOS
{
	public partial class ViewController : UIViewController
	{
		protected ViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}
		Storage _storage;
		public override async void ViewDidLoad()
		{
			base.ViewDidLoad();


			_storage = MyIssues.DataAccess.Storage.GetInstance();
			string accessToken = await _storage.GetToken() ?? "3282fb0f86f8063f8c8dfb1e3f0df2b839f1f298";
			if (accessToken != null && await Authenticate(accessToken))
			{
				System.Diagnostics.Debug.WriteLine("Authed");
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("Not authed");
			}
		}


        async Task<bool> Authenticate(string accessToken)
		{
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

			//progress.Dismiss();
			return authed;
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}
