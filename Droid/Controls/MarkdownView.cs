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

namespace MyIssues.Droid.Controls
{
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
            throw new NotImplementedException();
            //new LoadMarkdownUrlTask().Execute(url, cssFileUrl);
        }

        private String ReadFileFromAsset(String fileName)
        {
            try
            {
                var input = Context.Assets.Open(fileName);
                try
                {
                    BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(input));
                    StringBuilder content = new StringBuilder((int)input.Length);
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

        private void LoadMarkdownToView(String txt, String cssFileUrl)
        {
            //MarkdownProcessor m = new MarkdownProcessor();
            String content = CommonMarkConverter.Convert(txt); // TODO: transform 

            var head = @"<head>
    <meta name='viewport' content='width=device-width, initial-scale=1.0, user-scalable=no'>
    <link id='_ss' rel='stylesheet' href='" +  (cssFileUrl ?? "#") + @"' >
</head>";
            var body = @"<body class='markdown-body'>" +
                content+
"</body>";

            base.LoadDataWithBaseURL("fake://", "<html>" + head + body + "</html>", "text/html", "UTF-8", null);
        }

        private class LoadMarkdownUrlTask : AsyncTask<string, int, string>
        {
            private string cssFileUrl;
            protected override string RunInBackground(params string[] @params)
            {
                try
                {
                    var markdown = "";
                    var url = @params[0];
				this.cssFileUrl = @params[1];
//				if(URLUtil.IsNetworkUrl(url)){
//					markdown = HttpHelper.get(url).getResponseMessage();
//              } else if (URLUtil.isAssetUrl(url)) {
//					markdown = readFileFromAsset(url.substring("file:///android_asset/".length() , url.length()));
//				} else{
//					throw new IllegalArgumentException("The URL string provided is not a network URL or Asset URL.");
//              }

				return markdown;
			} catch (Exception ex) {
				Log.Debug(TAG, "Error Loading Markdown File.", ex);
				return null;
			}
            }
        }

        /*
        

        private class LoadMarkdownUrlTask extends
                AsyncTask<String, Integer, String> {

        private String cssFileUrl;

        protected String doInBackground(String... params)
        {
            try
            {
                String markdown = "";
                String url = params[0];
				this.cssFileUrl = params[1];
				if(URLUtil.isNetworkUrl(url)){
					markdown = HttpHelper.get(url).getResponseMessage();
    } else if (URLUtil.isAssetUrl(url)) {
					markdown = readFileFromAsset(url.substring("file:///android_asset/".length() , url.length()));
				} else{
					throw new IllegalArgumentException("The URL string provided is not a network URL or Asset URL.");
				}

				return markdown;
			} catch (Exception ex) {
				Log.d(TAG, "Error Loading Markdown File.", ex);
				return null;
			}
		}

		protected void onProgressUpdate(Integer...progress)
{
    // no-op
}

protected void onPostExecute(String result)
{
    if (result != null)
    {
        loadMarkdownToView(result, cssFileUrl);
    }
    else
    {
        loadUrl("about:blank");
    }
}
	}

	private void loadMarkdownToView(String txt, String cssFileUrl)
{
    MarkdownProcessor m = new MarkdownProcessor();
    String html = m.markdown(txt);
    if (cssFileUrl != null)
    {
        html = "<link rel='stylesheet' type='text/css' href='" + cssFileUrl + "' />" + html;
    }
    loadDataWithBaseURL("fake://", html, "text/html", "UTF-8", null);
}
         */
    }
}