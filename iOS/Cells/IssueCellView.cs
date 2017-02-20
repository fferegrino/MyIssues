// This file has been autogenerated from a class added in the UI designer.

using System;
using System.Collections.Generic;
using Foundation;
using MyIssues.Models;
using UIKit;

namespace MyIssues2.iOS.Cells
{
	public partial class IssueCellView : UITableViewCell
	{
		public IssueCellView (IntPtr handle) : base (handle)
		{
		}

		public void SetIssue(Issue issue)
		{
			if (issue.MilestoneDueDate.HasValue)
			{
				var milestoneDate = issue.MilestoneDueDate.GetValueOrDefault().LocalDateTime;
				MilestoneLabel.Text = $"{milestoneDate:MMMM}";
				MilestoneDayLabel.Text = $"{milestoneDate:dd}";
			}
			else
			{
				MilestoneLabel.Text = "--";
				MilestoneDayLabel.Text = "--";
			}
			IssueTitleLabel.Text = issue.Title;
			SetColors(issue.Labels);
		}

		readonly UIColor[] DefaultColors = new UIColor[]
		{
			UIColor.FromRGB(160,160,160),
			UIColor.FromRGB(180,180,180),
			UIColor.FromRGB(200,200,200),
			UIColor.FromRGB(220,220,220),
			UIColor.FromRGB(240,240,240)
		};

		internal void SetColors(IReadOnlyList<Label> labels)
		{
			if (labels.Count > 0)
				LabelColorView1.BackgroundColor = UIColor.FromRGB(labels[0].R, labels[0].G, labels[0].B);
			else
				LabelColorView1.BackgroundColor = DefaultColors[0];

			if (labels.Count > 1)
				LabelColorView2.BackgroundColor = UIColor.FromRGB(labels[1].R, labels[1].G, labels[1].B);
			else
				LabelColorView2.BackgroundColor = DefaultColors[1];

			if (labels.Count > 2)
				LabelColorView3.BackgroundColor = UIColor.FromRGB(labels[2].R, labels[2].G, labels[2].B);
			else
				LabelColorView3.BackgroundColor = DefaultColors[2];

			if (labels.Count > 3)
				LabelColorView4.BackgroundColor = UIColor.FromRGB(labels[3].R, labels[3].G, labels[3].B);
			else
				LabelColorView4.BackgroundColor = DefaultColors[3];

			if (labels.Count > 4)
				LabelColorView5.BackgroundColor = UIColor.FromRGB(labels[4].R, labels[4].G, labels[4].B);
			else
				LabelColorView5.BackgroundColor = DefaultColors[4];

		}

	}
}
