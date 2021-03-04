using Android.Runtime;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;
using Pulse;
using Pulse.Droid;
using System;
using System.ComponentModel;
using Color = Xamarin.Forms.Color;

[assembly: ExportRenderer(typeof(RoundImage), typeof(ImageCircleRendererCustom))]
namespace Pulse.Droid
{
	[Preserve(AllMembers = true)]
	public class ImageCircleRendererCustom : ImageRenderer
	{
		public async static void Init()
		{

		}

		protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement == null && (int)Android.OS.Build.VERSION.SdkInt < 21)
			{
				SetLayerType(LayerType.Software, null);
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == RoundImage.BorderColorProperty.PropertyName ||
			  e.PropertyName == RoundImage.BorderThicknessProperty.PropertyName ||
			  e.PropertyName == RoundImage.FillColorProperty.PropertyName)
			{
				this.Invalidate();
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="canvas"></param>
		/// <param name="child"></param>
		/// <param name="drawingTime"></param>
		/// <returns></returns>
		protected override bool DrawChild(Canvas canvas, Android.Views.View child, long drawingTime)
		{
			try
			{

				var radius = Math.Min(Width, Height) / ((RoundImage)Element).BorderRadius;

				var borderThickness = (float)((RoundImage)Element).BorderThickness;

				int strokeWidth = 0;

				if (borderThickness > 0)
				{
					var logicalDensity = Xamarin.Forms.Forms.Context.Resources.DisplayMetrics.Density;
					strokeWidth = (int)Math.Ceiling(borderThickness * logicalDensity + .5f);
				}

				radius -= strokeWidth / 2;

				var path = new Path();
				path.AddRoundRect(0, 0, Width, Height, radius, radius, Path.Direction.Ccw);

				canvas.Save();
				canvas.ClipPath(path);

				var paint = new Paint();
				paint.AntiAlias = true;
				paint.SetStyle(Paint.Style.Fill);
				paint.Color = ((RoundImage)Element).FillColor.ToAndroid();
				canvas.DrawPath(path, paint);
				paint.Dispose();

				var result = base.DrawChild(canvas, child, drawingTime);

				path.Dispose();
				canvas.Restore();

				path = new Path();
				path.AddRoundRect(0, 0, Width, Height, radius, radius, Path.Direction.Ccw);

				if (strokeWidth > 0.0f)
				{
					paint = new Paint();
					paint.AntiAlias = true;
					paint.StrokeWidth = strokeWidth;
					paint.SetStyle(Paint.Style.Stroke);
					paint.Color = ((RoundImage)Element).BorderColor.ToAndroid();
					canvas.DrawPath(path, paint);
					paint.Dispose();
				}

				path.Dispose();
				return result;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("Unable to create circle image: " + ex);
			}

			return base.DrawChild(canvas, child, drawingTime);
		}
	}
}