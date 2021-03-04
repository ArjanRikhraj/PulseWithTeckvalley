using System;
using Android.Graphics;
using Pulse;
using Pulse.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Xamarin.Forms.Color;

[assembly: ExportRenderer(typeof(GradientColorStack), typeof(GradientColorStackRenderer))]
namespace Pulse.Droid
{
	[Obsolete]
	public class GradientColorStackRenderer : VisualElementRenderer<StackLayout>
	{
		private Color StartColor { get; set; }
		private Color EndColor { get; set; }
		private StackOrientation stackOrientation { get; set; }

		protected override void DispatchDraw(global::Android.Graphics.Canvas canvas)
		{
			LinearGradient gradient = null;
			if (stackOrientation == StackOrientation.Vertical)
			{
				gradient = new Android.Graphics.LinearGradient(0, 0, 0, Height,
			   this.StartColor.ToAndroid(),
				  this.EndColor.ToAndroid(),
				  Android.Graphics.Shader.TileMode.Mirror);
			}
			else if (stackOrientation == StackOrientation.Horizontal)
			{
				gradient = new Android.Graphics.LinearGradient(0, 0, Width, 0,
			   this.StartColor.ToAndroid(),
				  this.EndColor.ToAndroid(),
				  Android.Graphics.Shader.TileMode.Mirror);
			}

			#region for Vertical Gradient
			//var gradient = new Android.Graphics.LinearGradient(0, 0, 0, Height,
			#endregion

			#region for Horizontal Gradient
			//gradient = new Android.Graphics.LinearGradient(0, 0, Width, 0,
			#endregion

			//	   this.StartColor.ToAndroid(),
			//	   this.EndColor.ToAndroid(),
			//	   Android.Graphics.Shader.TileMode.Mirror);

			var paint = new Android.Graphics.Paint()
			{
				Dither = true,
			};
			paint.SetShader(gradient);
			canvas.DrawPaint(paint);
			base.DispatchDraw(canvas);
		}

		protected override void OnElementChanged(ElementChangedEventArgs<StackLayout> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null || Element == null)
			{
				return;
			}
			try
			{
				var stack = e.NewElement as GradientColorStack;
				this.StartColor = stack.StartColor;
				this.EndColor = stack.EndColor;
				this.stackOrientation = stack.stackOrientation;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(@"ERROR:", ex.Message);
			}
		}
	}
}
