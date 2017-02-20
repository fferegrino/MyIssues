using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace MyIssues2.iOS
{
	[Register("VerticalCenteredTextView"), System.ComponentModel.DesignTimeVisible(true)]
	public class VerticalCenteredTextView : UITextView
	{

		public VerticalCenteredTextView(IntPtr handle) : base (handle) {
			Text = "Set your token here";
			TextColor = UIColor.LightGray;
			AddContentSizeObserver();
		}
		public VerticalCenteredTextView(CGRect frame, NSTextContainer textContainer) :
			base(frame, textContainer)
		{
			Text = "Set your token here";
			TextColor = UIColor.LightGray;
			AddContentSizeObserver();

		}

		public VerticalCenteredTextView(CGRect frame) : base(frame)
		{
			AddContentSizeObserver();

		}

		void AddContentSizeObserver()
		{
			AddObserver(this, (Foundation.NSString)"contentSize", Foundation.NSKeyValueObservingOptions.New, IntPtr.Zero);
		}

		void RemoveContentSizeObserver()
		{
			RemoveObserver(this, new Foundation.NSString("contentSize"));
		}

		void HandleAction(Foundation.NSObservedChange obj)
		{

		}

		public override void ObserveValue(Foundation.NSString keyPath, 
		                                  Foundation.NSObject ofObject, 
		                                  Foundation.NSDictionary change, 
		                                  IntPtr context)
		{
			
			var top = (Bounds.Size.Height - ContentSize.Height * ZoomScale) / 2.0;
			top = top < 0.0 ? 0.0 : top;
			this.SetContentOffset(CGPoint.Empty, false);
		}

		protected override void Dispose(bool disposing)
		{
			RemoveContentSizeObserver();
			base.Dispose(disposing);
		}
	}
}
