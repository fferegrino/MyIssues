
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
using MyIssues.Droid.Controls;

namespace MyIssues.Droid.Adapters
{
    public delegate void IssueCommentSelected(Models.IssueComment selected);

    public class IssueCommentsAdapter : RecyclerView.Adapter
    {
        private List<Models.IssueComment> _items;
        public event IssueCommentSelected OnIssueCommentSelected;

        public IssueCommentsAdapter(List<Models.IssueComment> items)
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

            var issueComment = _items[position];
            h.IssueCommentAuthor.Text = issueComment.Author;
            h.IssueCommentBody.LoadMarkdown(issueComment.Body, "file:///android_asset/github-markdown.css");
            h.IssueCommentDate.Text = issueComment.CreatedAt.ToString();

            h.Bind(issueComment, OnIssueCommentSelected);
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
        public MarkdownView IssueCommentBody { get; private set; }
        public TextView IssueCommentDate { get; private set; }

        public IssueCommentViewHolder(View itemView) : base(itemView)
        {
            IssueCommentAuthor = itemView.FindViewById<TextView>(Resource.Id.IssueCommentAuthor);
            IssueCommentBody = itemView.FindViewById<MarkdownView>(Resource.Id.IssueCommentBody);
            IssueCommentDate = itemView.FindViewById<TextView>(Resource.Id.IssueCommentDate);

            itemView.Click += ItemView_Click;
        }

        Models.IssueComment _boundIssue;
        IssueCommentSelected _l;

        void ItemView_Click(object sender, EventArgs e)
        {
            _l(_boundIssue);
        }

        public void Bind(Models.IssueComment item, IssueCommentSelected listener)
        {
            _boundIssue = item;
            _l = listener;
        }
    }

}

