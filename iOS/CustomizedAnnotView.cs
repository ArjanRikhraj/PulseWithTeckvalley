using Foundation;
using ObjCRuntime;
using System;
using UIKit;

namespace Pulse.iOS
{
    public partial class CustomizedAnnotView : UIView
    {
        private string BoostEventName { get; set; }
        private string BoostEventDate { get; set; }
       
        public UILabel LblEventTitle { get; set; }
        public UILabel LblEventDesc { get; set; }
        private UIImageView ImgEventImage { get; set; }
        public CustomizedAnnotView (IntPtr handle) : base (handle)
        {
            LblEventTitle = EventTitle;
            LblEventDesc = EventDesc;
            ImgEventImage = EventImage;
        }
        public  CustomizedAnnotView(String EventName, string DateOfEvent)
        {
            BoostEventName = EventName;
            BoostEventDate = DateOfEvent;
        }

        public  CustomizedAnnotView Create()
        {
         //   SetBindingData();
            var arr = NSBundle.MainBundle.LoadNib("CustomizedAnnotView", null, null);
            var v = Runtime.GetNSObject<CustomizedAnnotView>(arr.ValueAt(0));

            return v;
        }

        public override void AwakeFromNib()
        {

            LblEventTitle = EventTitle;
            LblEventDesc = EventDesc;
            ImgEventImage = EventImage;
        }
        //public CustomizedAnnotView()
        //{
        //    LblEventTitle = EventTitle;
        //    LblEventDesc = EventDesc;
        //    ImgEventImage = EventImage;
        //}

        public void SetBindingData()
        {
            LblEventTitle.Text = BoostEventName;
            LblEventDesc.Text = BoostEventDate;
            ImgEventImage.Image = UIImage.FromBundle(name: Constant.AboutPulseIcon);
        }
    }
}