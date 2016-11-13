
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace MyIssues.Droid.Adapters
{
	public class IssuesAdapter : BaseAdapter
	{
		private Context _context;
		private LayoutInflater _inflater;
		private List<Octokit.Issue> _dataSource;

		public IssuesAdapter(Context context, List<Octokit.Issue> items)
		{
			_context = context;
			_dataSource = items;
			_inflater =  (LayoutInflater)_context.GetSystemService(Context.LayoutInflaterService);
		}

		public override int Count
		{
			get
			{
				return _dataSource.Count();
			}
		}

		public override Java.Lang.Object GetItem(int position)
		{
			//return _dataSource[position];
			return  null ;
			}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View rowView = _inflater.Inflate(Resource.Layout.ListItemIssue, parent, false);
			var issue = _dataSource[position];
			var issueListTitle = rowView.FindViewById<TextView>(Resource.Id.issueListTitle);
			issueListTitle.Text = issue.Title;

			var labelColors = new View[]
			{
				rowView.FindViewById(Resource.Id.issueColorLabel1),
				rowView.FindViewById(Resource.Id.issueColorLabel2),
				rowView.FindViewById(Resource.Id.issueColorLabel3),
				rowView.FindViewById(Resource.Id.issueColorLabel4),
				rowView.FindViewById(Resource.Id.issueColorLabel5)
			};

			for (int i = 0; i < issue.Labels.Count && i < labelColors.Length; i++)
			{
				var c = GitHubClient.LabelColors[issue.Labels[i].Name];
				labelColors[i].SetBackgroundColor(Color.Argb(255, c[0], c[1], c[2]));
			}
			//var titleTextView = rowView.FindViewById<TextView>(Resource.Id.RepoListTitle);
			//var descriptionTextView = rowView.FindViewById<TextView>(Resource.Id.RepoListDescription);
			//var languageTextView = rowView.FindViewById<TextView>(Resource.Id.RepoListLanguage);
			//var repoColor = rowView.FindViewById(Resource.Id.RepoListColor);

			//titleTextView.Text = _dataSource[position].Name;
			//descriptionTextView.Text = _dataSource[position].Description;
			//languageTextView.Text = _dataSource[position].Language;
			//var color = Helpers.GetLanguageColor(_dataSource[position].Language);
			//repoColor.SetBackgroundColor(Color.Argb(255, color[0], color[1], color[2]));

			return rowView;
		}
	}
}
