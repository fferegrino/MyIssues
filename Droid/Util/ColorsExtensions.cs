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
    public static class ColorsExtensions
    {
        public static Android.Graphics.Color ContrastingColor(this Android.Graphics.Color bgColor)
        {
            const float gamma = 2.2f;
            var R = (float)(bgColor.R) / 255;
            var G = (float)(bgColor.G) / 255;
            var B = (float)(bgColor.B) / 255;
            float L = 0.2126f * R * R + 0.7152f * G * G + 0.0722f * B * B;

            return (L > System.Math.Pow(0.5, gamma)) ? Android.Graphics.Color.Black : Android.Graphics.Color.White;
        }
    }
}