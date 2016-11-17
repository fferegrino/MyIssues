
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Octokit;

namespace MyIssues.Droid.Adapters
{
	public class IssueCommentsAdapter : RecyclerView.Adapter
	{
		private List<Octokit.IssueComment> _items;
		public event SelectedIssue OnIssueSelected;

		public IssueCommentsAdapter(List<Octokit.IssueComment> items)
		{
			_items = items;
		}

		public override int ItemCount
		{
			get
			{
				return _items.Count;
			}
		}

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
		{
			var h = holder as IssueCommentViewHolder;

			var issue = _items[position];
            h.IssueCommentAuthor.Text = issue.User.Name;
            h.IssueCommentBody.Text = issue.Body;
            //h.Bind(issue, OnIssueSelected);
        }

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
		{
			// Inflate the CardView for the photo:
			View itemView = LayoutInflater.From(parent.Context).
						Inflate(Resource.Layout.IssueCommentView, parent, false);

            // Create a ViewHolder to hold view references inside the CardView:
            IssueCommentViewHolder vh = new IssueCommentViewHolder(itemView);
			return vh;
		}
	}



	public class IssueCommentViewHolder : RecyclerView.ViewHolder
	{
		public TextView IssueCommentAuthor { get; private set; }
		public TextView IssueCommentBody { get; private set; }

		public IssueCommentViewHolder(View itemView) : base(itemView)
		{
            IssueCommentAuthor = itemView.FindViewById<TextView>(Resource.Id.IssueCommentAuthor);
            IssueCommentBody = itemView.FindViewById<TextView>(Resource.Id.IssueCommentBody);
		}

		Issue _boundIssue;
		SelectedIssue _l;
		public void Bind( Issue item,  SelectedIssue listener)
		{
			_boundIssue = item;
			_l = listener;
        }

		void ItemView_Click(object sender, EventArgs e)
		{
		}
	}

}

