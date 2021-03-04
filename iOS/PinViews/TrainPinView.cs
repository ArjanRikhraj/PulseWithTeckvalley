using System;

using UIKit;
using CoreGraphics;

namespace Pulse.iOS
{
	public class NormalEventView : UIView
	{
        public NormalEventView ()
		{
			Frame = new CGRect (0, 0, 200, 100);
			BackgroundColor = UIColor.LightGray;

			UIImageView image = new UIImageView (new CGRect(100,0,100,100));
			image.Image = UIImage.FromFile ("Icon.png");
			UILabel label = new UILabel (new CGRect (0, 0, 100, 100)) {
				Text = "Train!!!!",
				TextAlignment = UITextAlignment.Center,
			};

			AddSubview (image);
			AddSubview (label);
		}
	}
    public class BoostedeventView : UIView
    {
        public BoostedeventView()
        {
            Frame = new CGRect(0, 0, 200, 100);
            BackgroundColor = UIColor.LightGray;

            UIImageView image = new UIImageView(new CGRect(100, 0, 100, 100));
            image.Image = UIImage.FromFile("Icon.png");
            UILabel label = new UILabel(new CGRect(0, 0, 100, 100))
            {
                Text = "Train!!!!",
                TextAlignment = UITextAlignment.Center,
            };

            AddSubview(image);
            AddSubview(label);
        }
    }
}