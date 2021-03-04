using Foundation;
using Pulse.Controls;
using Pulse.iOS.Renderers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NoUnderlineDatePicker), typeof(NoUnderLineDatePickerRenderer))]
namespace Pulse.iOS.Renderers
{
   public class NoUnderLineDatePickerRenderer: DatePickerRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null)
                return;
            Control.Layer.BorderWidth = 0;
            Control.BorderStyle = UITextBorderStyle.None;
        }
    }
}