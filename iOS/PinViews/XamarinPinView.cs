using System;

using UIKit;
using CoreGraphics;

namespace Pulse.iOS
{
    public class XamarinPinView : UIView
    {
        public string DateTime { get; set; }
        public string EventName { get; set; }
        public string EventId { get; set; }
        public XamarinPinView(string EventName, string DateTime, string EventId)
        {
            this.EventName = EventName;
            this.DateTime = DateTime;
            this.EventId = EventId;
            UIImageView image = new UIImageView(new CGRect(0, 0, 210, 70));
            image.Image = UIImage.FromFile("img_boost_event_pop.png");
            Frame = new CGRect(0, 0, 200, 70);
            BackgroundColor = UIColor.Clear;
            UIImageView imageboostIcon = new UIImageView(new CGRect(10, 20, 20, 20));
            imageboostIcon.Image = UIImage.FromFile("icBoostEvent.png");

            UILabel eventName = new UILabel(new CGRect(35, 5, 170, 30))
            {
                Text = EventName,
                TextAlignment = UITextAlignment.Left,
                LineBreakMode = UILineBreakMode.TailTruncation,
                TextColor = UIColor.White,
                Font = UIFont.FromName("Poppins-SemiBold", 14f),
            };
            UILabel datetime = new UILabel(new CGRect(35, 25, 190, 30))
            {
                Text = DateTime,
                TextAlignment = UITextAlignment.Left,
                LineBreakMode = UILineBreakMode.TailTruncation,
                TextColor = UIColor.White,
                Font = UIFont.FromName("Poppins-Regular", 12f),

            };
            UIButton button = new UIButton(new CGRect(0, 0, 210, 70))
            {
                BackgroundColor = UIColor.Clear,

            };
            button.TouchUpInside += Button_TouchUpInside;
            AddSubview(image);
            AddSubview(imageboostIcon);
            AddSubview(eventName);
            AddSubview(datetime);
            AddSubview(button);


        }


        void Button_TouchUpInside(object sender, EventArgs e)
        {
            App myApp = App.Current as App;
            if (!string.IsNullOrEmpty(EventId))
            {
                if (EventId == "Xamarin")
                {
                }
                else
                {
                    myApp.FetchEventDetailMethod(EventId);
                }
            }
        }


    }

    public class XamarinNormalPinView : UIView
    {
        public string DateTime { get; set; }
        public string EventName { get; set; }
        public string EventId { get; set; }
        public XamarinNormalPinView(string EventName, string DateTime, string EventId)
        {
            this.EventName = EventName;
            this.DateTime = DateTime;
            this.EventId = EventId;
            UIImageView image = new UIImageView(new CGRect(0, 0, 210, 70));
            image.Image = UIImage.FromFile("event_pop.png");
            Frame = new CGRect(0, 0, 200, 70);
            BackgroundColor = UIColor.Clear;
            UIImageView imageboostIcon = new UIImageView(new CGRect(10, 20, 20, 20));
            imageboostIcon.Image = UIImage.FromFile("icBoostEvent.png");

            UILabel eventName = new UILabel(new CGRect(10, 5, 170, 30))
            {
                Text = EventName,
                TextAlignment = UITextAlignment.Left,
                LineBreakMode = UILineBreakMode.TailTruncation,
                TextColor = UIColor.Black,
                Font = UIFont.FromName("Poppins-SemiBold", 14f),
            };
            UILabel datetime = new UILabel(new CGRect(10, 25, 190, 30))
            {
                Text = DateTime,
                TextAlignment = UITextAlignment.Left,
                LineBreakMode = UILineBreakMode.TailTruncation,
                TextColor = UIColor.Black,
                Font = UIFont.FromName("Poppins-Regular", 12f),

            };
            UIButton button = new UIButton(new CGRect(0, 0, 210, 70))
            {
                BackgroundColor = UIColor.Clear,

            };
            button.TouchUpInside += Button_TouchUpInside1;
            AddSubview(image);
            //  AddSubview(imageboostIcon);
            AddSubview(eventName);
            AddSubview(datetime);
            AddSubview(button);


        }

        void Button_TouchUpInside1(object sender, EventArgs e)
        {

            App myApp = App.Current as App;
            if (!string.IsNullOrEmpty(EventId))
            {
                if (EventId == "Xamarin")
                {
                }
                else
                {
                    myApp.FetchEventDetailMethod(EventId);
                }
            }
        }
    }

    public class CurrentLocationPinview : UIView
    {


        public CurrentLocationPinview()
        {

            UIImageView image = new UIImageView(new CGRect(0, 0, 210, 70));
            image.Image = UIImage.FromFile("event_pop.png");
            Frame = new CGRect(0, 0, 200, 70);
            BackgroundColor = UIColor.Clear;
            UIImageView imageboostIcon = new UIImageView(new CGRect(10, 20, 20, 20));
            imageboostIcon.Image = UIImage.FromFile("icBoostEvent.png");

            UILabel eventName = new UILabel(new CGRect(10, 10, 170, 30))
            {
                Text = "You are here",
                TextAlignment = UITextAlignment.Center,
                LineBreakMode = UILineBreakMode.TailTruncation,
                TextColor = UIColor.Black,
                Font = UIFont.FromName("Poppins-SemiBold", 14f),
            };

            AddSubview(image);
           AddSubview(eventName);


        }


    }
}


