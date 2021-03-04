using CoreGraphics;
using MapKit;
using UIKit;

namespace Pulse.iOS
{
    public class CustomMKPinAnnotationView : MKAnnotationView
    {
        public CustomMKPinAnnotationView(IMKAnnotation annotation, string annotationId) : base(annotation, annotationId)
        {
        }
        public string Id { get; set; }
        public bool IsMoreThanOneLocation { get; set; }
        public string Url { get; set; }
        public bool IsBoostEvent { get; set; }
        public bool IsCurrentLocation { get; set; }
        public string FormsIdentifier { get; set; }
        public string EventName { get; set; }
        public string EventDate { get; set; }
        public string Latitude { get; set; }
        public string Lognitude { get; set; }
        public override UIView HitTest(CGPoint point, UIEvent uievent)
        {
            UIView hitView = base.HitTest(point, uievent);
            if (hitView != null)
                Superview.BringSubviewToFront(this);

            return hitView;
        }
        public override bool PointInside(CGPoint point, UIEvent uievent)
        {
            CGRect rect = Bounds;
            bool isInside = rect.Contains(point);

            if (!isInside)
            {
                foreach (UIView view in Subviews)
                {
                    isInside = view.Frame.Contains(point);
                    if (isInside)
                        break;
                }
            }

            return isInside;
        }
    }
}