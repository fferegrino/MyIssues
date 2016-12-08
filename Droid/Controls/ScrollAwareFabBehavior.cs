using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.Design.Widget;
using Java.Lang;
using Android.Support.V4.View;
using Android.Util;

namespace MyIssues.Droid.Controls
{
    [Register("myissues.droid.controls.ScrollAwareFabBehavior")]
    public class ScrollAwareFabBehavior : CoordinatorLayout.Behavior
    {

        public ScrollAwareFabBehavior(Context context, IAttributeSet attrs) : base(context, attrs)
		{
        }

        public override bool OnStartNestedScroll(CoordinatorLayout coordinatorLayout, 
            Java.Lang.Object child, View directTargetChild, View target, int nestedScrollAxes)
        {

            return nestedScrollAxes == (int)ScrollAxis.Vertical ||
                base.OnStartNestedScroll(coordinatorLayout, child, directTargetChild, target,
                nestedScrollAxes);
        }

        public override void OnNestedScroll(CoordinatorLayout coordinatorLayout, 
            Java.Lang.Object child, View target, 
            int dxConsumed, int dyConsumed, int dxUnconsumed, int dyUnconsumed)
        {
            base.OnNestedScroll(coordinatorLayout, child, target, 
                dxConsumed, dyConsumed, dxUnconsumed, dyUnconsumed);

            var fab = child as FloatingActionButton;

            if (dyConsumed > 0 && fab.Visibility == ViewStates.Visible)
            {
                fab.Hide();
            }
            else if (dyConsumed < 0 && fab.Visibility != ViewStates.Visible)
            {
                fab.Show();
            }
        }
    }
}