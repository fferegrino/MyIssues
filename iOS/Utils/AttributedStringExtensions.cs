using System;
using Foundation;

namespace MyIssues2.iOS
{
	public static class AttributedStringExtensions
	{
		public static NSAttributedString FromMarkdown(this string markdown)
		{ 
			NSError error = null;
			var htmlString = new NSAttributedString(CommonMark.CommonMarkConverter.Convert(markdown),
			                                        new NSAttributedStringDocumentAttributes { DocumentType = NSDocumentType.HTML, StringEncoding= NSStringEncoding.UTF8 },
								 ref error);

			return htmlString;
		}
	}
}
