using System;
using Pulse;
using Pulse.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics.Drawables;
[assembly: ExportRenderer(typeof(GradientColorFrame), typeof(GradientColorFrameRenderer))]
namespace Pulse.Droid
{
	public class GradientColorFrameRenderer : VisualElementRenderer<Frame>
	{
		private Color StartColor { get; set; }
		private Color EndColor { get; set; }
		private int BorderRadius { get; set; }
        		protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null || Element == null)
			{
				return;
			}
			try
			{
				var stack = e.NewElement as GradientColorFrame;
				this.BorderRadius = stack.BorderRadius;
				this.StartColor = stack.StartColor;
				this.EndColor = stack.EndColor;

				var paint = new Android.Graphics.Paint()
				{
					Dither = true,
				};
				var gradientColors = new Color[] { this.StartColor, this.EndColor };
				//Need to convert the colors to Android Color objects
				int[] androidColors = new int[gradientColors.Length];

				for (int i = 0; i < gradientColors.Length; i++)
				{
					Xamarin.Forms.Color temp = gradientColors[i];
					androidColors[i] = temp.ToAndroid();
				}

				GradientDrawable gradient1 = new GradientDrawable(GradientDrawable.Orientation.LeftRight, androidColors);
				if (StartColor == EndColor)
				{
					gradient1.SetCornerRadii(new float[] {  0, 0, this.BorderRadius, this.BorderRadius, this.BorderRadius, this.BorderRadius, this.BorderRadius, this.BorderRadius });
				}
				else
				{
					gradient1.SetCornerRadii(new float[] { this.BorderRadius, this.BorderRadius, this.BorderRadius, this.BorderRadius, 0, 0, this.BorderRadius, this.BorderRadius });
				}
                if(stack.IsBottomLeftNotRounded && stack.IsBottomRightNotRounded)
                {
                    gradient1.SetCornerRadii(new float[] {  this.BorderRadius+30, this.BorderRadius+30, this.BorderRadius+30, this.BorderRadius+30,0,0,0,0 });
                }
				this.SetBackground(gradient1);

			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(@"ERROR:", ex.Message);
			}
		}
	}
}
