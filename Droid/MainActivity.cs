﻿using Android.App;
using Android.Widget;
using Android.OS;
using System;

namespace MyIssues.Droid
{
	[Activity(Label = "MyIssues", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		int count = 1;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button>(Resource.Id.myButton);

			button.Click += Button_Click;
		}

		void Button_Click(object sender, System.EventArgs e)
		{
			var authenticator = new Xamarin.Auth.OAuth2Authenticator(
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

		}

	}
}

