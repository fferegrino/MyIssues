using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Xamarin.Auth;
using System.Linq;
using Android.Content;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Text.Method;
using MyIssues.DataAccess;
using System.Threading.Tasks;
using MyIssues.Droid.Activities;
using Android.Util;

namespace MyIssues.Droid
{
    [Activity(Label = "MyIssues",
        MainLauncher = true,
        Theme = "@style/MyTheme",
        NoHistory = true,
        Icon = "@drawable/icon")]
    public class MainActivity : AppCompatActivity
    {

		const string Tag = nameof(MainActivity);

        ImageButton _learnMoreButton;
        Button _setAccessTokenButton;
        EditText _accessTokenEditText;
        Storage _storage;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            Startup.Start();

            _storage = MyIssues.DataAccess.Storage.GetInstance();

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            _learnMoreButton = FindViewById<ImageButton>(Resource.Id.LearnMoreButton);
            _accessTokenEditText = FindViewById<EditText>(Resource.Id.AccessTokenEditText);
            _setAccessTokenButton = FindViewById<Button>(Resource.Id.SetAccessTokenButton);

            string accessToken = "3282fb0f86f8063f8c8dfb1e3f0df2b839f1f298";// await _storage.GetToken() ?? "3282fb0f86f8063f8c8dfb1e3f0df2b839f1f298";
            _accessTokenEditText.Text = accessToken;
            if (accessToken != null && await Authenticate(accessToken))
            {
                NavigateToRepoSelection();
            }
            _setAccessTokenButton.Click += AuthenticateButton_Click;

			_learnMoreButton.Click += (sender, e) =>
			{
				var browserIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse("http://thatcsharpguy.com/apps/myissues#personal-access-token"));
				StartActivity(browserIntent);
			};
        }

        async void AuthenticateButton_Click(object sender, System.EventArgs e)
        {
            if(await Authenticate(_accessTokenEditText.Text))
            {
                NavigateToRepoSelection();
            }
        }

        async Task<bool> Authenticate(string accessToken)
        {
            ProgressDialog progress;
			progress = ProgressDialog.Show(this,Resources.GetString(Resource.String.Authenticating),
			                               Resources.GetString(Resource.String.PleaseWait), true);
            var cliente = GitHubClientFactory.CreateClient(accessToken);
            var authed = await _storage.SetClient(cliente);

			if (authed)
			{
				await _storage.SaveToken(accessToken);
			}

            progress.Dismiss();
            return authed;
        }

        void NavigateToRepoSelection()
        {
            Intent intent = new Intent(this, typeof(IssuesActivity));
            StartActivity(intent);
        }
    }
}

