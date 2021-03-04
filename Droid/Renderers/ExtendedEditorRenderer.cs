using Pulse;
using Pulse.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
[assembly: ExportRenderer(typeof(ExtendedEditor), typeof(ExtendedEditorRenderer))]

namespace Pulse.Droid
{
	public class ExtendedEditorRenderer : EditorRenderer
	{
		#region Private control
		#endregion

		/// <summary>
		/// On the element changed.
		/// </summary>
		/// <returns>The element changed.</returns>
		/// <param name="e">E.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
		{
			base.OnElementChanged(e);
			if (Control != null)
			{
				var element = Element as ExtendedEditor;
				Control.Background = null;
				Control?.SetBackgroundColor(Android.Graphics.Color.Transparent);
				Control.VerticalScrollBarEnabled = true;
				Control.Hint = element.Placeholder;
				Control.SetHintTextColor(Android.Graphics.Color.Rgb(86, 88, 92));
				Control.Typeface = FontHelper.GetFontFace(FontFace.PoppinsRegular);
			}
		}
		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (Control != null)
			{
				Control.Background = null;
				Control.Typeface = FontHelper.GetFontFace(FontFace.PoppinsRegular);
			}

		}
	}
}
