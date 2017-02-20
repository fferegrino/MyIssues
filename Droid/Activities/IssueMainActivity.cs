using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MyIssues.Droid.Adapters;
using Humanizer;
using MyIssues.DataAccess;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using MyIssues.Droid.Fragments;

namespace MyIssues.Droid
{
    [Activity(Label = "@string/issue_activity_label",
        Theme = "@style/MyTheme",
        ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class IssueMainActivity : AppCompatActivity
    {
        ViewPager _viewPager;
        IssuePagerAdapter _pagerAdapter;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.IssueMain);
            var toolbar = (Android.Support.V7.Widget.Toolbar)FindViewById(Resource.Id.Toolbar);
            SetSupportActionBar(toolbar);

            var title = Intent.GetStringExtra("title");
            var id = Intent.GetIntExtra("id", -1);
            var number = Intent.GetIntExtra("number", -1);

            Title =String.Format(Resources.GetString(Resource.String.issue_number),number);

            TabLayout tabLayout = (TabLayout)FindViewById(Resource.Id.TabLayout);
            tabLayout.AddTab(tabLayout.NewTab().SetText(Resources.GetString(Resource.String.issue_details)));
            tabLayout.AddTab(tabLayout.NewTab().SetText(Resources.GetString(Resource.String.issue_comments)));
            tabLayout.TabGravity = TabLayout.GravityFill;

             _pagerAdapter = new IssuePagerAdapter(SupportFragmentManager, tabLayout.TabCount, number);
            _viewPager = FindViewById<ViewPager>(Resource.Id.Pager);
            _viewPager.Adapter = (_pagerAdapter);
            _viewPager.AddOnPageChangeListener(new TabLayout.TabLayoutOnPageChangeListener(tabLayout));
            tabLayout.TabSelected += TabLayout_TabSelected;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.IssueMenu, menu);
			menu.TintIcons();
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.RefreshIssue:
                    var a = _pagerAdapter.GetItem(1) as IssueCommentsFragment;
                    a.LoadIssueComments();
                    break;
                default:
                    return base.OnOptionsItemSelected(item);
            }
            return true;
        }

        bool loadedInfo = false;

        private void TabLayout_TabSelected(object sender, TabLayout.TabSelectedEventArgs e)
        {
            switch(e.Tab.Position)
            {
                case 0: // Details
                    break;
                case 1: // Comments

                    if (loadedInfo == true) break;
                    loadedInfo = true;
                    var a = _pagerAdapter.GetItem(e.Tab.Position) as IssueCommentsFragment;

                    a.LoadIssueComments();

                    break;
                default:
                    throw new Exception("Unknown tab");
            }
            _viewPager.CurrentItem = e.Tab.Position;
        }


    }
}
