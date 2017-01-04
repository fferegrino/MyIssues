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
            var h = holder as IssueViewHolder;

            var issue = _dataSource[position];
            h.IssueListTitle.Text = issue.Title;
            var milestone = issue.Milestone;
            if (System.String.IsNullOrWhiteSpace(milestone))
            {
                h.IssueListMilestone.Visibility = ViewStates.Gone;
            }
            else
            {
                h.IssueListMilestone.Visibility = ViewStates.Visible;
                h.IssueListMilestone.Text = $"{issue.MilestoneDueDate.Humanize(DateTimeOffset.Now)}: {milestone}";
            }

            SetLabelColors(h, issue.Labels);

            h.Bind(issue, OnIssueSelected);
        }


        readonly Color[] DefaultColors = new Color[]
        {
            Color.Argb(255,160,160,160),
            Color.Argb(255,180,180,180),
            Color.Argb(255,200,200,200),
            Color.Argb(255,220,220,220),
            Color.Argb(255,240,240,240)
        };

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

            if (constraint == null) return returnObj;

            if (_parent._originalData != null && _parent._originalData.Any())
            {
                // Compare constraint to all names lowercased. 
                // It they are contained they are added to results.
                results.AddRange(
                    _parent._originalData.Where(
                        chemical => chemical.Labels.Where(l => l.Name.Equals(constraint.ToString())).Any()));
            }

            // Nasty piece of .NET to Java wrapping, be careful with this!
            returnObj.Values = FromArray(results.Select(r => r.ToJavaObject()).ToArray());
            returnObj.Count = results.Count;

            constraint.Dispose();

            return returnObj;
        }

        protected override void PublishResults(ICharSequence constraint, FilterResults results)
        {
            using (var values = results.Values)
                _parent._dataSource = values.ToArray<Object>()
                    .Select(r => r.ToNetObject<Models.Issue>()).ToList();

            _parent.NotifyDataSetChanged();

            // Don't do this and see GREF counts rising
            constraint.Dispose();
            results.Dispose();

        }
    }

}

