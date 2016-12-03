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

namespace MyIssues.Droid.Util
{
    public static class UiExtensions
    {

        public static void RunOnUi(this Action action)
        {
            using (var h = new Handler(Looper.MainLooper))
                h.Post(() =>
                {
                    action();
                });
        }
    }
}