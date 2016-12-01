using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MyIssues.Droid.Controls.Editor
{
    internal class MyHighlighterColorsNeutral : IHighlighterColors {

    const uint COLOR_HEADER = 0xffef6C00;
    const uint COLOR_LINK = 0xff1ea3fd;
    const uint COLOR_LIST = COLOR_HEADER;

        public Color HeaderColor => Color.Argb(255, 239, 108, 0);

        public Color LinkColor => Color.Argb(255, 30, 163, 253);

        public Color ListColor
        {
            get
            {
                return HeaderColor;
            }
        }
    }
}