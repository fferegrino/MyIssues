using System;
using UIKit;

namespace MyIssues2.iOS
{
	public class IssueLabel : UIView
	{
		private UIColor _labelColor;
		public UIColor LabelColor
		{
			get { return _labelColor; }
			set { _labelColor = value; }
		}
	}
}
