using Android.OS;
using Android.Views;
using System.Threading.Tasks;
using MyIssues.DataAccess;
using Android.Support.V7.Widget;
using MyIssues.Droid.Adapters;
using System.Linq;

namespace MyIssues.Droid.Fragments
{
    public class IssueCommentsFragment : Android.Support.V4.App.Fragment
    {
        Storage _storage;
        RecyclerView commentsView;

        int _issueNumber;

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

            commentsView = view.FindViewById<RecyclerView>(Resource.Id.IssueCommentsListView);
            var _layoutManager = new LinearLayoutManager(this.Context);
            commentsView.SetLayoutManager(_layoutManager);

            return view;
        }

        bool _commentsLoaded = false;
        public async Task<bool> LoadIssueComments()
        {
            if (!_commentsLoaded)
            {
                var comments = (await _storage.GetIssueComments(_issueNumber)).ToList();
                var adapter = new IssueCommentsAdapter(comments);
                commentsView.SetAdapter(adapter);
                _commentsLoaded = true;
                return true;
            }
            return false;
        }

    }
}