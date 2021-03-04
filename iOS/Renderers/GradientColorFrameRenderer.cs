using CoreAnimation;
using CoreGraphics;
using Pulse;
using Pulse.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;

[assembly: ExportRenderer(typeof(GradientColorFrame), typeof(GradientColorFrameRenderer))]
namespace Pulse.iOS
{
    public class GradientColorFrameRenderer : VisualElementRenderer<Frame>
    {
        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            GradientColorFrame frame = (GradientColorFrame)this.Element;

            CGColor startColor = frame.StartColor.ToCGColor();
            CGColor endColor = frame.EndColor.ToCGColor();

            var gradientLayer = new CAGradientLayer();
            gradientLayer.Frame = rect;
            gradientLayer.Colors = new CGColor[] { startColor, endColor };
            gradientLayer.StartPoint = new CGPoint(0, 0.5);
            gradientLayer.EndPoint = new CGPoint(1, 0.5);
            object rounded;
            if(frame.IsBottomLeftNotRounded && frame.IsBottomRightNotRounded)
                rounded = UIRectCorner.TopLeft | UIRectCorner.TopRight;
            else if(frame.IsBottomRightNotRounded)
             rounded =UIRectCorner.TopLeft | UIRectCorner.TopRight | UIRectCorner.BottomLeft;
            else if(frame.IsTopLeftRoundNotRounded)
                rounded = UIRectCorner.TopRight | UIRectCorner.BottomLeft| UIRectCorner.BottomRight;
            else
                rounded =  UIRectCorner.TopLeft | UIRectCorner.TopRight | UIRectCorner.BottomLeft| UIRectCorner.BottomRight;

            if (true)
            {
                UIBezierPath path = UIBezierPath.FromRoundedRect(rect, (UIRectCorner)rounded, new CGSize(frame.BorderRadius, frame.BorderRadius));

                CAShapeLayer shape = new CAShapeLayer();
                shape.Path = path.CGPath;

                NativeView.Layer.Mask = shape;
            }

            NativeView.Layer.InsertSublayer(gradientLayer, 0);

        }
    }

}
