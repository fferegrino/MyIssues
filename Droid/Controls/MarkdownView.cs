using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Webkit;
using Android.Util;
using Java.IO;
using CommonMark;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyIssues.Droid.Controls
{
    [Register("myissues.droid.controls.MarkdownView")]
    public class MarkdownView : WebView
	{
		const string TAG = nameof(MarkdownView);

		public MarkdownView(Context context, IAttributeSet attrs) : base(context, attrs)
		{

		}

		public MarkdownView(Context context) : base(context)
		{

		}

		/// <summary>
		/// Loads the given Markdown text to the view as rich formatted HTML. The
		/// HTML output will be styled based on the given CSS file.
		/// </summary>
		/// <param name="txt">input in markdown format</param>
		/// <param name="cssFileUrl">a URL to css File. If the file located in the project assets folder 
		/// then the URL should start with "file:///android_asset/"</param>
		public void LoadMarkdown(String txt, String cssFileUrl = null)
		{
			LoadMarkdownToView(txt, cssFileUrl);
		}

		/// <summary>
		/// Loads the given Markdown file to the view as rich formatted HTML. 
		/// The HTML output will be styled based on the given CSS file.
		/// </summary>
		/// <param name="url">a URL to the Markdown file. 
		/// If the file located in the project assets folder then 
		/// the URL should start with "file:///android_asset/"</param>
		/// <param name="cssFileUrl">a URL to css File. If the file located in the project assets
		/// folder then the URL should start with "file:///android_asset/"</param>
		public void LoadMarkdownFile(String url, String cssFileUrl = null)
		{
			//throw new NotImplementedException();
			new LoadMarkdownUrlTask(this).Execute(url, cssFileUrl);
		}



		private void LoadMarkdownToView(String txt, String cssFileUrl)
		{
			//MarkdownProcessor m = new MarkdownProcessor();
			String content = CommonMarkConverter.Convert(txt); // TODO: transform 

			var head = @"<head>
    <meta name='viewport' content='width=device-width, initial-scale=1.0, user-scalable=no'>
    <link id='_ss' rel='stylesheet' href='" + (cssFileUrl ?? "#") + @"' >
</head>";
			var body = @"<body class='markdown-body'>" +
				content +
"</body>";

			base.LoadDataWithBaseURL("fake://", "<html>" + head + body + "</html>", "text/html", "UTF-8", null);
		}


		private String ReadFileFromAsset(String fileName)
		{
			try
			{
				var input = Context.Assets.Open(fileName);
				try
				{
					BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(input));
					StringBuilder content = new StringBuilder();
					String line;
					while ((line = bufferedReader.ReadLine()) != null)
					{
						content.Append(line);
						content.Append(Java.Lang.JavaSystem.GetProperty("line.separator"));
					}
					return content.ToString();
				}
				finally { input.Close(); }
			}
			catch (Exception ex)
			{
				Log.Debug(TAG, "Error while reading file from assets", ex);
				return null;
			}
		}

		private class LoadMarkdownUrlTask : AsyncTask<string, int, string>
		{
			MarkdownView _parent;
			public LoadMarkdownUrlTask(MarkdownView parent)
			{
				_parent = parent;
			}

			private string cssFileUrl;
			protected override string RunInBackground(params string[] @params)
			{
				try
				{
					var markdown = "";
					var url = @params[0];
					this.cssFileUrl = @params[1];
					if (URLUtil.IsNetworkUrl(url))
					{
						HttpClient c = new HttpClient();
						markdown = MyIssues.Util.AsyncHelpers.RunSync(() => c.GetStringAsync(new Uri(url)));
					}
					else if (URLUtil.IsAssetUrl(url))
					{
						markdown = _parent.ReadFileFromAsset(url.Substring("file:///android_asset/".Length ));
						//throw new NotImplementedException();
					}
					else {
						throw new ArgumentException("The URL string provided is not a network URL or Asset URL.", nameof(url));
					}

					return markdown;
				}
				catch (Exception ex)
				{
					Log.Debug(TAG, "Error Loading Markdown File.", ex);
					return null;
				}
			}



			protected override void OnPostExecute(String result)
			{
				if (result != null)
				{
					_parent.LoadMarkdownToView(result, cssFileUrl);
				}
				else
				{
					_parent.LoadUrl("about:blank");
				}
			}
		}
	}
}