﻿using Android.App;
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
        Icon = "@mipmap/icon")]
	public class MainActivity : AppCompatActivity
    {
        int count = 1;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
			var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
			SetSupportActionBar(toolbar);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.myButton);

            var accountStore = AccountStore.Create();
            string accessToken = null;
            var account = accountStore.FindAccountsForService("github").FirstOrDefault();
            if (account != null)
            {
                accessToken = account.Properties["access_token"];
            }

            if (accessToken == null)
            {
                button.Click += AuthenticateButton_Click;
            }
            else
            {
				NavigateToRepoSelection(accessToken);
            }
        }

        void AuthenticateButton_Click(object sender, System.EventArgs e)
        {
            var authenticator = new OAuth2Authenticator(
                clientId: "537972d3ca6fe7d5d6b9",
                clientSecret: "fd8a9776604e79b4e8234e63c51f6fee7b09360c",
                scope: "repo",
                authorizeUrl: new Uri("https://github.com/login/oauth/authorize"),
                redirectUrl: new Uri("https://github.com"),
                accessTokenUrl: new Uri("https://github.com/login/oauth/access_token"));

            authenticator.Completed += Authenticator_Completed;

            this.StartActivity(authenticator.GetUI(this));
        }

        void Authenticator_Completed(object sender, Xamarin.Auth.AuthenticatorCompletedEventArgs e)
        {
			if (e.IsAuthenticated)
			{
				AccountStore.Create().Save(e.Account, "github");
				var aToken = e.Account.Properties["access_token"];
				NavigateToRepoSelection(aToken);
			}
        }


		void NavigateToRepoSelection(string accessToken)
		{

            GitHubClient.Client(accessToken);
			Intent intent = new Intent(this, typeof(ViewReposActivity));
			StartActivity(intent);
		}
    }
}

