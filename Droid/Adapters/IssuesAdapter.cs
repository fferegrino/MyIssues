
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
using MyIssues.DataAccess;

namespace MyIssues.Droid.Adapters
{

	public delegate void IssueSelected(Models.Issue selected);

	public class IssuesAdapter : RecyclerView.Adapter
	{
		private List<Models.Issue> _items;
		public event IssueSelected OnIssueSelected;

		public IssuesAdapter(List<Models.Issue> items)
		{
			_items = items;
		}

        public void Update(List<Models.Issue> issues)
        {
            _items.Clear();
            _items.AddRange(issues);
            NotifyDataSetChanged();
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
			var h = holder as IssueViewHolder;

			var issue = _items[position];
			h.IssueListTitle.Text = issue.Title;
			var milestone = issue.Milestone;
			if (System.String.IsNullOrWhiteSpace(milestone))
			{
				h.IssueListMilestone.Visibility = ViewStates.Gone;
			}
			else
			{
				h.IssueListMilestone.Visibility = ViewStates.Visible;
				h.IssueListMilestone.Text = milestone;
			}

			for (int i = 0; i < issue.Labels?.Count && i < h.LabelColorViews.Length; i++)
			{
                int[] c;
                if(Storage.LabelColors.TryGetValue(issue.Labels[i].Name, out c))
                {
                    h.LabelColorViews[i].SetBackgroundColor(Color.Argb(255, c[0], c[1], c[2]));
                }
                else
                {
                    h.LabelColorViews[i].SetBackgroundColor(Color.LightGray);
                }
			}

			h.Bind(issue, OnIssueSelected);
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
		{
			// Inflate the CardView for the photo:
			View itemView = LayoutInflater.From(parent.Context).
						Inflate(Resource.Layout.IssueCardView, parent, false);

			// Create a ViewHolder to hold view references inside the CardView:
			IssueViewHolder vh = new IssueViewHolder(itemView);
			return vh;
		}
	}



	public class IssueViewHolder : RecyclerView.ViewHolder
	{
		public View[] LabelColorViews { get; private set; }
		public TextView IssueListTitle { get; private set; }
		public TextView IssueListMilestone { get; private set; }

		public IssueViewHolder(View itemView) : base(itemView)
		{
			IssueListTitle = itemView.FindViewById<TextView>(Resource.Id.issueListTitle);
			IssueListMilestone = itemView.FindViewById<TextView>(Resource.Id.issueListMilestone);
			LabelColorViews = new View[]
			{
				itemView.FindViewById(Resource.Id.issueColorLabel1),
				itemView.FindViewById(Resource.Id.issueColorLabel2),
				itemView.FindViewById(Resource.Id.issueColorLabel3),
				itemView.FindViewById(Resource.Id.issueColorLabel4),
				itemView.FindViewById(Resource.Id.issueColorLabel5)
			};

			itemView.Click += ItemView_Click;
		}

        Models.Issue _boundIssue;
		IssueSelected _l;
		public void Bind(Models.Issue item,  IssueSelected listener)
		{
			_boundIssue = item;
			_l = listener;
        }

		void ItemView_Click(object sender, EventArgs e)
		{
			_l(_boundIssue);
		}
	}

}

