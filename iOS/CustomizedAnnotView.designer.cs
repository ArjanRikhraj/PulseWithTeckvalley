// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Pulse.iOS
{
    [Register ("CustomizedAnnotView")]
    partial class CustomizedAnnotView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel EventDesc { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView EventImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel EventTitle { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (EventDesc != null) {
                EventDesc.Dispose ();
                EventDesc = null;
            }

            if (EventImage != null) {
                EventImage.Dispose ();
                EventImage = null;
            }

            if (EventTitle != null) {
                EventTitle.Dispose ();
                EventTitle = null;
            }
        }
    }
}