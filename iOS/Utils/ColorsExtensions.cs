using System;
using UIKit;

namespace MyIssues2.iOS
{
	public static class ColorsExtensions
	{

        public static UIColor ContrastingColor(this UIColor bgColor)
		{
			const float gamma = 2.2f;
			nfloat R;
			nfloat G;
			nfloat B;
			nfloat A;

			bgColor.GetRGBA(out R, out G, out B, out A);

			nfloat L = 0.2126f * R * R + 0.7152f * G * G + 0.0722f * B * B;

			return (L > System.Math.Pow(0.5, gamma)) ? UIColor.Black : UIColor.White;
		}
	}
}
