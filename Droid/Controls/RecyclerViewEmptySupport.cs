using System;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using MyIssues.Droid.Util;

namespace MyIssues.Droid.Controls
{
    public class RecyclerViewEmptySupport : RecyclerView
    {
        private View emptyView;

        private EmptyDataObserver _emptyObserver;
        private EmptyDataObserver EmptyObserver
        {
			get { return _emptyObserver ?? (_emptyObserver = new EmptyDataObserver(this)); }
        }

        public RecyclerViewEmptySupport(Context context) : base(context)
        {
        }

        public RecyclerViewEmptySupport(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public RecyclerViewEmptySupport(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
        }

		private bool _isLoading;
		public bool IsLoading
		{
			get { return _isLoading; }
			set { _isLoading = value; 
            EmptyObserver.OnChanged(); }
		}

		public override void SetAdapter(Adapter adapter)
        {
            base.SetAdapter(adapter);

            if (adapter != null)
            {
				adapter.RegisterAdapterDataObserver(EmptyObserver);
            }

            EmptyObserver.OnChanged();
        }


        public View EmptyView { get; set; }
    }

    public class EmptyDataObserver : RecyclerView.AdapterDataObserver
    {
        RecyclerViewEmptySupport _parent;
        public EmptyDataObserver(RecyclerViewEmptySupport parent) : base()
        {
            _parent = parent;
        }


        public override void OnChanged()
        {
            var adapter = _parent.GetAdapter();
            if (adapter != null && _parent.EmptyView != null)
            {
				if (adapter.ItemCount == 0 && !_parent.IsLoading)
                {
                    _parent.EmptyView.Visibility = ViewStates.Visible;
                    _parent.Visibility = ViewStates.Gone;
                }
                else
                {
                    _parent.EmptyView.Visibility = ViewStates.Gone;
                    _parent.Visibility = ViewStates.Visible;
                }
            }
        }
    }
}
