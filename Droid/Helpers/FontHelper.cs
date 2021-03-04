using Android.Graphics;
using Xamarin.Forms;

namespace Pulse.Droid
{
	public static class FontHelper
	{
		static Typeface PoppinsLight = Typeface.CreateFromAsset(Forms.Context.Assets, "Poppins-Light.ttf");
		static Typeface PoppinsMedium = Typeface.CreateFromAsset(Forms.Context.Assets, "Poppins-Medium.ttf");
		static Typeface PoppinsSemiBold = Typeface.CreateFromAsset(Forms.Context.Assets, "Poppins-SemiBold.ttf");
		static Typeface PoppinsRegular = Typeface.CreateFromAsset(Forms.Context.Assets, "Poppins-Regular.ttf");
		static Typeface PoppinsExtraLight = Typeface.CreateFromAsset(Forms.Context.Assets, "Poppins-ExtraLight.ttf");

		public static Typeface GetFontFace(FontFace fontface)
		{
			Typeface returnVal;
			if (fontface == FontFace.PoppinsLight)
			{
				returnVal = PoppinsLight;
			}
			else if (fontface == FontFace.PoppinsSemiBold)
			{
				returnVal = PoppinsSemiBold;
			}
			else if (fontface == FontFace.PoppinsRegular)
			{
				returnVal = PoppinsRegular;
			}
			else if (fontface == FontFace.PoppinsExtraLight)
			{
				returnVal = PoppinsExtraLight;
			}
			else
			{
				returnVal = PoppinsMedium;
			}
			return returnVal;
		}

	}
}