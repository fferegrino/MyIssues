using Android.OS;
using Android.Views;
using System.Threading.Tasks;
using MyIssues.DataAccess;
using Android.Support.V7.Widget;
using MyIssues.Droid.Adapters;
using System.Linq;
using Android.Support.Design.Widget;
using MyIssues.Droid.Activities;
using System.Reactive.Linq;
using System;
using Android.Content;
using MyIssues.Droid.Util;
using Android.Widget;
using Android.App;
using MyIssues.Models;
using System.Collections.Generic;
using MyIssues.Droid.Controls;

namespace MyIssues.Droid.Fragments
{
    public class IssueCommentsFragment : Android.Support.V4.App.Fragment
    {
        Storage _storage;
        RecyclerViewEmptySupport commentsView;
        FloatingActionButton _replyButton;

        int _issueNumber;


        public IssueCommentsFragment()
        {
        }

        public IssueCommentsFragment(int issueNumber)
        {
            _issueNumber = issueNumber;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _storage = Storage.GetInstance();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            var view = inflater.Inflate(Resource.Layout.IssueComments, container, false);

            _replyButton = view.FindViewById<FloatingActionButton>(Resource.Id.ReplyButton);
			//_replyButton.BackgroundTintList =   BackgroundTintList(ColorStateList.valueOf(Color .parseColor("#33691E")))
            _replyButton.Click += ReplyButton_Click;

            commentsView = view.FindViewById<RecyclerViewEmptySupport>(Resource.Id.IssueCommentsListView);

            var emptyView = view.FindViewById(Resource.Id.EmptyListView);
            commentsView.EmptyView = emptyView;

            var _layoutManager = new LinearLayoutManager(this.Context);
            commentsView.SetLayoutManager(_layoutManager);

            return view;
        }

        private void ReplyButton_Click(object sender, System.EventArgs e)
        {
            Intent intent = new Intent(Context, typeof(NoteActivity));
            intent.PutExtra(Constants.IssueNumber, _issueNumber);
            StartActivityForResult(intent, Constants.PublishCommentRequestCode);
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            if (requestCode == Constants.PublishCommentRequestCode)
            {
                if (resultCode == (int)Result.Ok)
                    LoadIssueComments();
            }
            else
            {
                base.OnActivityResult(requestCode, resultCode, data);
            }
        }



        //bool cachedComments = true;
        void ResetComments(IReadOnlyList<IssueComment> comments)
        {
            //System.Diagnostics.Debug.WriteLine($"LoadIssueComments {cachedComments}");
            //if (cachedComments)
            //{
            //    cachedComments = false;
            //    return;
            //}

            var adapter = new IssueCommentsAdapter(comments.ToList());
            using (var h = new Handler(Looper.MainLooper))
                h.Post(() =>
                {
                    UiExtensions.RunOnUi(() =>
                    {
                        commentsView.SetAdapter(adapter);
                    });
                });

        }

        public void LoadIssueComments()
        {
            var loadComments = _storage.GetIssueComments(_issueNumber);

#if DEBUG

            Action onCompleted = () =>
            {
                UiExtensions.RunOnUi(() =>
                {
                    Toast.MakeText(this.Context, "Comentarios cargados", ToastLength.Long).Show();
                });
            };
            loadComments.Subscribe(ResetComments, onCompleted: onCompleted);
			#else
            loadComments.Subscribe(ResetComments);
#endif
        }
    }
}