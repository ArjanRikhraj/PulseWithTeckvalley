using Android.Graphics.Drawables;
using Pulse;
using Pulse.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
[assembly: ExportRenderer(typeof(ExtendedFrame), typeof(ExtendedFrameRenderer))]
namespace Pulse.Droid
{
	public class ExtendedFrameRenderer : FrameRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
		{
			base.OnElementChanged(e);
            ExtendedFrame extFrame = e.NewElement as ExtendedFrame;

            GradientDrawable gd = new Android.Graphics.Drawables.GradientDrawable();
            gd.SetColor(extFrame.BackgroundColor.ToAndroid());
            gd.SetStroke(extFrame.BorderWidth, extFrame.OutlineColor.ToAndroid());
            gd.SetCornerRadius((float)extFrame.BorderRadius);
            SetBackgroundDrawable(gd);
        }


	}
}