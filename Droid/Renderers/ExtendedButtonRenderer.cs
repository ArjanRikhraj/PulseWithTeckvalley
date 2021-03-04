using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Views;
using Pulse;
using Pulse.Droid;

[assembly: ExportRenderer(typeof(ExtendedButton), typeof(ExtendedButtonRenderer))]
namespace Pulse.Droid
{
	public class ExtendedButtonRenderer : ButtonRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
		{
			base.OnElementChanged(e);
			var view = e.NewElement as ExtendedButton;
			Android.Widget.Button btn = Control as Android.Widget.Button;
			Control.SetPadding(0, 0, 0, 0);
			btn.TransformationMethod = null;
			if (view != null)
			{
				SetFont(view);
				if (view.IsSortButton)
				{
					btn.Gravity = GravityFlags.Left;
					btn.SetPadding(100, 42, 0, 0);
				}
			}


		}
		private void SetFont(ExtendedButton view)
		{
			Control.Typeface = FontHelper.GetFontFace(view.FontFace);
		}

	}
}

