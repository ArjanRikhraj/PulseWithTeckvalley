using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Pulse;
using Pulse.iOS;
using UIKit;
using Foundation;

[assembly: ExportRenderer(typeof(ExtendedPicker), typeof(ExtendedPickerRenderer))]
namespace Pulse.iOS
{
	public class ExtendedPickerRenderer : PickerRenderer
	{
		void DoneBtn_Clicked(object sender, EventArgs e)
		{
			var s = (ExtendedPicker)Element;
			if (s != null)
			{
				s.PickerDoneClicked(e);
			}
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
		{
			base.OnElementChanged(e);
			if (Control == null)
			{
				return;
			}
			ExtendedPicker picker = (ExtendedPicker)Element;
			if (picker != null)
			{
				SetBorderStyle(picker);
				SetTextColor(picker);
				SetFontSize(picker);
				SetPlaceholderTextColor(picker);
				SetPlaceHolder(picker);
			}

			var toolbar = (UIToolbar)Control.InputAccessoryView;
			if (toolbar != null)
			{
				var doneBtn = toolbar.Items[1];
				doneBtn.Clicked += DoneBtn_Clicked;
			}
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (Control == null)
			{
				return;
			}

			ExtendedPicker picker = (ExtendedPicker)Element;

			if (e.PropertyName == ExtendedPicker.TextColorProperty.PropertyName)
			{
				this.Control.TextColor = picker.TextColor.ToUIColor();
			}

			if (e.PropertyName == ExtendedPicker.PlaceholderTextColorProperty.PropertyName)
				SetPlaceholderTextColor(picker);
		}

		void SetBorderStyle(ExtendedPicker picker)
		{
			this.Control.BorderStyle = picker.HasBorder ? UITextBorderStyle.RoundedRect : UITextBorderStyle.None;
		}

		void SetTextColor(ExtendedPicker picker)
		{
			this.Control.TextColor = picker.TextColor.ToUIColor();
		}

		void SetFontSize(ExtendedPicker picker)
		{
			nfloat newSize = (nfloat)picker.TextSize;
			this.Control.Font = UIFont.FromName("Poppins-Regular", newSize);
		}

		void SetPlaceHolder(ExtendedPicker picker)
		{
			this.Control.Text = picker.PlaceHolder;
		}

		void SetPlaceholderTextColor(ExtendedPicker picker)
		{
			if (!string.IsNullOrEmpty(picker.Title) && picker.PlaceholderTextColor != Color.Default)
			{
				var placeholderString = new NSAttributedString(picker.Title, new UIStringAttributes { ForegroundColor = picker.PlaceholderTextColor.ToUIColor() });
				Control.AttributedPlaceholder = placeholderString;
			}
		}
	}
}
