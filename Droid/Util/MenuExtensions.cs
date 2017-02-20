using System;
using Android.Graphics;
using Android.Views;

namespace MyIssues.Droid
{
	public static class MenuExtensions 
	{ 
		static Color TintColor = Color.Rgb(200, 201, 203);
		public static void TintIcons( this IMenu menu)
		{
			for (int i = 0; i < menu.Size(); ++i)
			{
				var item = menu.GetItem(i);
				TintMenuItemIcon(item, TintColor);
			}
		}

		private static void TintMenuItemIcon(this IMenuItem item, Color color)
		{
			var drawable = item.Icon;
			if (drawable != null)
			{
				drawable.SetColorFilter(color, PorterDuff.Mode.SrcIn);
			}
		}
	}
}

