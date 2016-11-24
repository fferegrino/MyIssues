
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


    public class LabelsAdapter : RecyclerView.Adapter
    {
        private List<Octokit.Label> _items;
        public event IssueSelected OnIssueSelected;

        public LabelsAdapter(List<Octokit.Label> items)
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
            var h = holder as LabelViewHolder;

            var label = _items[position];
            h.LabelTitle.Text = label.Name;
            var c = Storage.LabelColors[label.Name];
            var color = Color.Argb(255, c[0], c[1], c[2]);
            h.LabelTitle.SetTextColor(TextColor(color));
            h.CardView.SetBackgroundColor(color);

        }

        Android.Graphics.Color TextColor(Android.Graphics.Color bgColor)
        {
            const float gamma = 2.2f;
            var R = (float)(bgColor.R) / 255;
            var G = (float)(bgColor.G) / 255;
            var B = (float)(bgColor.B) / 255;
            float L = 0.2126f * R * R + 0.7152f * G * G + 0.0722f * B * B;

            return (L > System.Math.Pow(0.5, gamma)) ? Android.Graphics.Color.Black : Android.Graphics.Color.White;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            // Inflate the CardView for the photo:
            View itemView = LayoutInflater.From(parent.Context).
                                          Inflate(Resource.Layout.LabelCardView, parent, false);

            // Create a ViewHolder to hold view references inside the CardView:
            LabelViewHolder vh = new LabelViewHolder(itemView);
            return vh;
        }
    }



    public class LabelViewHolder : RecyclerView.ViewHolder
    {
        public CardView CardView { get; private set; }
        public TextView LabelTitle { get; private set; }

        public LabelViewHolder(View itemView) : base(itemView)
        {
            CardView = itemView.FindViewById<CardView>(Resource.Id.LabelCardView);
            LabelTitle = itemView.FindViewById<TextView>(Resource.Id.LabelCardViewName);
        }
    }

}

