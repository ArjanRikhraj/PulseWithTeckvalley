using System;
using CoreAnimation;
using CoreGraphics;
using Pulse;
using Pulse.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
[assembly: ExportRenderer(typeof(GradientColorStack), typeof(GradientColorStackRenderer))]
namespace Pulse.iOS
{
	public class GradientColorStackRenderer : VisualElementRenderer<StackLayout>
	{
		public override void Draw(CGRect rect)
		{
			base.Draw(rect);
			GradientColorStack stack = (GradientColorStack)this.Element;
			CGColor startColor = stack.StartColor.ToCGColor();
			CGColor endColor = stack.EndColor.ToCGColor();
			CAGradientLayer gradientLayer = null;
			#region for Horizontal Gradient
			if (stack.stackOrientation== StackOrientation.Horizontal)
            {
				 gradientLayer = new CAGradientLayer()
				{
					StartPoint = new CGPoint(0, 0.7),
					EndPoint = new CGPoint(1, 0.7)
				};
			}
			#endregion
			#region for Vertical Gradient
			else if (stack.stackOrientation== StackOrientation.Vertical)
				 gradientLayer = new CAGradientLayer();
			#endregion
			gradientLayer.Frame = rect;
			gradientLayer.Colors = new CGColor[] { startColor, endColor
		};

			NativeView.Layer.InsertSublayer(gradientLayer, 0);
		}

	}

}
