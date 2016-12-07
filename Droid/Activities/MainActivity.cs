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

namespace MyIssues.Droid
{
    [Activity(Label = "MyIssues",
        MainLauncher = true,
        Theme = "@style/MyTheme",
        NoHistory = true,
        Icon = "@drawable/icon")]
    public class MainActivity : AppCompatActivity
    {
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

            // Get our button from the layout resource,
            // and attach an event to it
            _learnMoreButton = FindViewById<ImageButton>(Resource.Id.LearnMoreButton);
            _accessTokenEditText = FindViewById<EditText>(Resource.Id.AccessTokenEditText);
            _setAccessTokenButton = FindViewById<Button>(Resource.Id.SetAccessTokenButton);

			string accessToken = await _storage.GetToken() ?? "081821f76f6d5b53cfbebc63aaa80a78993be3f9";
            _accessTokenEditText.Text = accessToken;
            if (accessToken != null && await Authenticate(accessToken))
            {
                NavigateToRepoSelection();
            }
            _setAccessTokenButton.Click += AuthenticateButton_Click;

			_learnMoreButton.Click += (sender, e) =>
			{
				var browserIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse("http://thatcsharpguy.com"));
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
            progress = ProgressDialog.Show(this, "Authenticating",
                "Please wait", true);
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

