using System;
using UIKit;

namespace Pulse.iOS
{
	public static class FontHelper
	{
		public static UIFont GetFont(FontFace fontFace, double fontSize)
		{
			UIFont returnVal = null;
			switch (fontFace)
			{
				case FontFace.PoppinsLight:
					returnVal = GetCustomFont("Poppins-Light", nfloat.Parse(Convert.ToString(fontSize)));
					break;
				case FontFace.PoppinsRegular:
					returnVal = GetCustomFont("Poppins-Regular", nfloat.Parse(Convert.ToString(fontSize)));
					break;
				case FontFace.PoppinsSemiBold:
					returnVal = GetCustomFont("Poppins-SemiBold", nfloat.Parse(Convert.ToString(fontSize)));
					break;
				case FontFace.PoppinsExtraLight:
					returnVal = GetCustomFont("Poppins-ExtraLight", nfloat.Parse(Convert.ToString(fontSize)));
					break;
				default:
					returnVal = GetCustomFont("Poppins-Medium", nfloat.Parse(Convert.ToString(fontSize)));
					break;
			}

			return returnVal;
		}

		private static UIFont GetCustomFont(string fontName, nfloat fontSize)
		{
			return UIFont.FromName(fontName, fontSize);
		}
	}
}


