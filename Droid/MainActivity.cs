using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Xamarin.Auth;
using System.Linq;
using Android.Content;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace MyIssues.Droid
{
    [Activity(Label = "MyIssues",
        MainLauncher = true,
        Theme = "@style/MyTheme",
        NoHistory = true,
        Icon = "@mipmap/icon")]
    public class MainActivity : AppCompatActivity
    {
        int count = 1;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            Startup.Start();

            var storage = MyIssues.DataAccess.Storage.GetInstance();

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.SetAccessTokenButton);

            string accessToken =  "081821f76f6d5b53cfbebc63aaa80a78993be3f9";

            if (accessToken == null)
            {
                button.Click += AuthenticateButton_Click;
            }
            else
            {
                var cliente = GitHubClientFactory.CreateClient(accessToken);
                await storage.SetClient(cliente);
                NavigateToRepoSelection();
            }
        }

        void AuthenticateButton_Click(object sender, System.EventArgs e)
        {

        }


        void NavigateToRepoSelection()
        {

            Intent intent = new Intent(this, typeof(IssuesActivity));
            StartActivity(intent);
        }
    }
}

