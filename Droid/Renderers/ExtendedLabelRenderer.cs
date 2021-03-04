using Pulse;
using Pulse.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtendedLabel), typeof(ExtendedLabelRenderer))]

namespace Pulse.Droid
{
	public class ExtendedLabelRenderer : LabelRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
		{
			base.OnElementChanged(e);
			var view = e.NewElement as ExtendedLabel;
			if (view != null)
			{
				SetFont(view);
			}
		}

		private void SetFont(ExtendedLabel view)
		{
			Control.Typeface = FontHelper.GetFontFace(view.FontFace);
		}
		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			var view = this.Element as ExtendedLabel;
			if (view != null)
			{
				SetFont(view);
			}
		}
	}
}


