// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using MyIssues.DataAccess;
using UIKit;

namespace MyIssues2.iOS
{
	public partial class SettingsTableViewController : UITableViewController
	{

		struct StoryboardId 
		{
			public const string SwitchRepoSegue = "Switch Repo";
		}
		public SettingsTableViewController (IntPtr handle) : base (handle)
		{
		}


		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);


			UITapGestureRecognizer changeRepoTap = new UITapGestureRecognizer(ChangeRepoTap);
			ChangeRepoLabel.AddGestureRecognizer(changeRepoTap);


			UITapGestureRecognizer eraseDataTap = new UITapGestureRecognizer(EraseDataTap);
			AccountLabel.AddGestureRecognizer(eraseDataTap);

		}

		void ChangeRepoTap(UITapGestureRecognizer obj)
		{
			PerformSegue(StoryboardId.SwitchRepoSegue, this);
		}

		async void EraseDataTap(UITapGestureRecognizer obj)
		{
			var alert = UIAlertController.Create("Borrar datos", "Si decides borrar los datos la aplicación se cerrará y tendrás que abrirla manualmente para volver a usarla", UIAlertControllerStyle.Alert);
			alert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Default, (actiom) => { }));
			alert.AddAction(UIAlertAction.Create("Erase", UIAlertActionStyle.Destructive, async (actiom) => 
			{ 
				await Storage.GetInstance().EraseAll();
				System.Threading.Thread.CurrentThread.Abort();

			}));
			this.PresentViewController(alert, true, null);

			//await Storage.GetInstance().EraseAll();
		}
				
	}
}
