using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace MyIssues2.iOS
{
	public class LabelView : UIView
	{
		public LabelView(IntPtr handle) : base(handle) { }
		public LabelView(NSCoder coder) : base(coder) { }


		private string _labelName;
		public string LabelName
		{
			get { return _labelName; }
			set { _labelName = value; MeasureLabelSize(); }
		}

		private float _padding;
		public float Padding
		{
			get { return _padding; }
			set { _padding = value; MeasureLabelSize(); }
		}

		CGSize _size;
		void MeasureLabelSize()
		{
			var str = new NSString(LabelName);
			_size = str.StringSize(UIFont.PreferredHeadline);
		}

		public float Width => (float)(_size.Width + _padding * 2);

		public LabelView(int xPosition, string labelName)
		{
			
		}

		public override void Draw(CoreGraphics.CGRect rect)
		{
			System.Diagnostics.Debug.WriteLine($"Drawing {LabelName} at {Bounds}");
			var path = UIBezierPath.FromRoundedRect(Bounds, 10);
			UIColor.Red.SetFill();
			path.Fill();
		}
	}
}
