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
    [Activity(Label = "Issue",
        Theme = "@style/MyTheme")]
    public class IssueMainActivity : AppCompatActivity
    {
        ViewPager _viewPager;
        IssuePagerAdapter _pagerAdapter;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.IssueMain);
            var toolbar = (Android.Support.V7.Widget.Toolbar)FindViewById(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            var title = Intent.GetStringExtra("title");
            var id = Intent.GetIntExtra("id", -1);
            var number = Intent.GetIntExtra("number", -1);

            Title = $"Issue #{number}";

            TabLayout tabLayout = (TabLayout)FindViewById(Resource.Id.TabLayout);
            tabLayout.AddTab(tabLayout.NewTab().SetText("Detail"));
            tabLayout.AddTab(tabLayout.NewTab().SetText("Comments"));
            tabLayout.TabGravity = TabLayout.GravityFill;

             _pagerAdapter = new IssuePagerAdapter(SupportFragmentManager, tabLayout.TabCount, number);
            _viewPager = FindViewById<ViewPager>(Resource.Id.Pager);
            _viewPager.Adapter = (_pagerAdapter);
            _viewPager.AddOnPageChangeListener(new TabLayout.TabLayoutOnPageChangeListener(tabLayout));
            tabLayout.TabSelected += TabLayout_TabSelected;
        }


        
        private async void TabLayout_TabSelected(object sender, TabLayout.TabSelectedEventArgs e)
        {
            switch(e.Tab.Position)
            {
                case 0: // Details
                    break;
                case 1: // Comments
                    var a = _pagerAdapter.GetItem(e.Tab.Position) as IssueCommentsFragment;
                    a.LoadIssueComments();
                    break;
                default:
                    throw new Exception("Unknown tab");
            }
        }


    }
}
