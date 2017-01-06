using System;
using System.Linq;
using System.Collections.Generic;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MyIssues.DataAccess;
using Humanizer;
using Java.Lang;
using Object = Java.Lang.Object;

namespace MyIssues.Droid.Adapters
{

	public delegate void IssueSelected(Models.Issue selected);

	public class IssuesAdapter : RecyclerView.Adapter, IFilterable
	{
		internal List<Models.Issue> _dataSource;
		internal List<Models.Issue> _originalData;

		public event IssueSelected OnIssueSelected;

		public IssuesAdapter(List<Models.Issue> items)
		{
			_dataSource = items;
			Filter = new ByContainingLabel(this);
		}

		public void Update(List<Models.Issue> issues)
		{
			_dataSource.Clear();
			_dataSource.AddRange(issues);
			NotifyDataSetChanged();
		}

		public override int ItemCount
		{
			get
			{
				return _dataSource.Count;
			}
		}

		public Filter Filter
		{
			get; private set;
		}

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
		{

			var issue = _dataSource[position];
			var milestone = issue.Milestone;
			var title = issue.Title;
			var labels = issue.Labels;
			var milestoneDate = issue.MilestoneDueDate?.DateTime;

			if (!milestoneDate.HasValue)
			{
				var h = holder as IssueNoMilestoneViewHolder;

				h.IssueListTitle.Text = issue.Title;
				h.IssueListMilestone.Visibility = ViewStates.Gone;
				SetLabelColors(h, issue.Labels);

				h.Bind(issue, OnIssueSelected);
			}
			else
			{
				var h = holder as IssueViewHolder;

				h.IssueListTitle.Text = issue.Title;
				h.IssueListMilestone.Visibility = ViewStates.Gone;
				SetLabelColors(h, issue.Labels);
				h.IssueMilestoneDay.Text = milestoneDate.Value.Day.ToString("00");
				h.IssueMilestoneMonth.Text = milestoneDate.Value.ToString("MMMM");

				h.Bind(issue, OnIssueSelected);
			}

		}


		readonly Color[] DefaultColors = new Color[]
		{
			Color.Argb(255,160,160,160),
			Color.Argb(255,180,180,180),
			Color.Argb(255,200,200,200),
			Color.Argb(255,220,220,220),
			Color.Argb(255,240,240,240)
		};

		void SetLabelColors(IssueNoMilestoneViewHolder h, IReadOnlyList<Models.Label> labels)
		{
			for (int i = 0; i < h.LabelColorViews.Length; i++)
			{
				int[] c;
				if (labels.Count > i && Storage.LabelColors.TryGetValue(labels[i].Name, out c))
					h.LabelColorViews[i].SetBackgroundColor(Color.Argb(255, labels[i].R, labels[i].G, labels[i].B));
				else
					h.LabelColorViews[i].SetBackgroundColor(DefaultColors[i]);
			}
		}

		void SetLabelColors(IssueViewHolder h, IReadOnlyList<Models.Label> labels)
		{
			for (int i = 0; i < h.LabelColorViews.Length; i++)
			{
				int[] c;
				if (labels.Count > i && Storage.LabelColors.TryGetValue(labels[i].Name, out c))
					h.LabelColorViews[i].SetBackgroundColor(Color.Argb(255, labels[i].R, labels[i].G, labels[i].B));
				else
					h.LabelColorViews[i].SetBackgroundColor(DefaultColors[i]);
			}
		}

		struct ViewTypes
		{
			public const int WithMilestone = 0;
			public const int WithoutMilestone = 1;
		}

		public override int GetItemViewType(int position)
		{
			var issue = _dataSource[position];

			if (issue.MilestoneDueDate == null)
				return ViewTypes.WithoutMilestone;
			return ViewTypes.WithMilestone;
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
		{
			// Inflate the CardView for the photo:
			View itemView;

			switch (viewType)
			{
				case ViewTypes.WithMilestone:

					itemView = LayoutInflater.From(parent.Context).
										Inflate(Resource.Layout.IssueCardView, parent, false);
					return new IssueViewHolder(itemView);
					break;
				case ViewTypes.WithoutMilestone:

					itemView = LayoutInflater.From(parent.Context).
								Inflate(Resource.Layout.IssueCardNoMilestoneView, parent, false);
					return new IssueNoMilestoneViewHolder(itemView);
					break;
				default:
					throw new System.Exception("No type");
			}
		}
	}



	public class IssueNoMilestoneViewHolder : RecyclerView.ViewHolder
	{
		public View[] LabelColorViews { get; private set; }
		public TextView IssueListTitle { get; private set; }
		public TextView IssueListMilestone { get; private set; }

		public IssueNoMilestoneViewHolder(View itemView) : base(itemView)
		{
			IssueListTitle = itemView.FindViewById<TextView>(Resource.Id.IssueListTitle);
			IssueListMilestone = itemView.FindViewById<TextView>(Resource.Id.IssueListMilestone);
			LabelColorViews = new View[]
			{
				itemView.FindViewById(Resource.Id.IssueColorLabel1),
				itemView.FindViewById(Resource.Id.IssueColorLabel2),
				itemView.FindViewById(Resource.Id.IssueColorLabel3),
				itemView.FindViewById(Resource.Id.IssueColorLabel4),
				itemView.FindViewById(Resource.Id.IssueColorLabel5)
			};
			itemView.Click += ItemView_Click;
		}

		Models.Issue _boundItem;
		IssueSelected _boundEvent;
		public void Bind(Models.Issue item, IssueSelected listener)
		{
			_boundItem = item;
			_boundEvent = listener;
		}

		void ItemView_Click(object sender, EventArgs e)
		{
			_boundEvent(_boundItem);
		}
	}

	public class IssueViewHolder : RecyclerView.ViewHolder
	{
		public View[] LabelColorViews { get; private set; }
		public TextView IssueListTitle { get; private set; }
		public TextView IssueListMilestone { get; private set; }
		public TextView IssueMilestoneMonth { get; private set; }
		public TextView IssueMilestoneDay { get; private set; }

		public IssueViewHolder(View itemView) : base(itemView)
		{
			IssueListTitle = itemView.FindViewById<TextView>(Resource.Id.IssueListTitle);
			IssueListMilestone = itemView.FindViewById<TextView>(Resource.Id.IssueListMilestone);
			IssueMilestoneMonth = itemView.FindViewById<TextView>(Resource.Id.MilestoneMonth);
			IssueMilestoneDay = itemView.FindViewById<TextView>(Resource.Id.MilestoneDay);
			LabelColorViews = new View[]
			{
				itemView.FindViewById(Resource.Id.IssueColorLabel1),
				itemView.FindViewById(Resource.Id.IssueColorLabel2),
				itemView.FindViewById(Resource.Id.IssueColorLabel3),
				itemView.FindViewById(Resource.Id.IssueColorLabel4),
				itemView.FindViewById(Resource.Id.IssueColorLabel5)
			};
			itemView.Click += ItemView_Click;
		}

		Models.Issue _boundItem;
		IssueSelected _boundEvent;
		public void Bind(Models.Issue item, IssueSelected listener)
		{
			_boundItem = item;
			_boundEvent = listener;
		}

		void ItemView_Click(object sender, EventArgs e)
		{
			_boundEvent(_boundItem);
		}
	}

	public class ByContainingLabel : Filter
	{

		IssuesAdapter _parent;
		public ByContainingLabel(IssuesAdapter parent)
		{
			_parent = parent;
		}

		protected override FilterResults PerformFiltering(ICharSequence constraint)
		{

			var returnObj = new FilterResults();
			var results = new List<Models.Issue>();
			if (_parent._originalData == null)
				_parent._originalData = _parent._dataSource;


			if (_parent._originalData != null && _parent._originalData.Any())
			{

				if (constraint == null)
				{
					results.AddRange(_parent._originalData);
				}
				else
				{
					results.AddRange(
						_parent._originalData.Where(
							chemical => chemical.Labels.Where(l => l.Name.Equals(constraint.ToString())).Any()));
				}
			}

			// Nasty piece of .NET to Java wrapping, be careful with this!
			returnObj.Values = FromArray(results.Select(r => r.ToJavaObject()).ToArray());
			returnObj.Count = results.Count;

			if (constraint != null)
			{
				constraint.Dispose();
			}
			return returnObj;
		}

		protected override void PublishResults(ICharSequence constraint, FilterResults results)
		{
			using (var values = results.Values)
				_parent._dataSource = values.ToArray<Object>()
					.Select(r => r.ToNetObject<Models.Issue>()).ToList();

			_parent.NotifyDataSetChanged();

			// Don't do this and see GREF counts rising
			if (constraint != null)
			{
				constraint.Dispose();
			}
			results.Dispose();

		}
	}

}

