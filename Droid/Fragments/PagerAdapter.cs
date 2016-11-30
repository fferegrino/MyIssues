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
using Android.Support.V4.App;

namespace MyIssues.Droid.Fragments
{
    public class IssuePagerAdapter : FragmentStatePagerAdapter
    {
    int _tabCount;
        int _issueNumber;

        Android.Support.V4.App.Fragment[] Tabs;

        public IssuePagerAdapter(Android.Support.V4.App.FragmentManager fm, int numOfTabs, int issueNumber) 
            : base(fm)
        {
            Tabs = new Android.Support.V4.App.Fragment[ numOfTabs];
            _issueNumber = issueNumber;

            Tabs[0] = new IssueDetailFragment(_issueNumber);
            Tabs[1] = new IssueCommentsFragment(_issueNumber);
        }

        public override int Count => Tabs.Count();

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            return Tabs[position];
        }


        
    }
}

