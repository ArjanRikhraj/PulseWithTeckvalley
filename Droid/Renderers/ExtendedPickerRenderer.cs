using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Pulse.Droid;
using Android.Graphics;

[assembly: ExportRenderer(typeof(Pulse.ExtendedPicker), typeof(ExtendedPickerRenderer))]
namespace Pulse.Droid
{
	class ExtendedPickerRenderer : PickerRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
		{
			base.OnElementChanged(e);

			ExtendedPicker picker = (ExtendedPicker)Element;
			Typeface font = Typeface.CreateFromAsset(Forms.Context.Assets, "Poppins-Regular.ttf");
			Control.Typeface = font;
			if (Control != null && e.NewElement != null)
			{
				Control.SetBackgroundColor(Android.Graphics.Color.Transparent);
			}
			if (picker != null)
			{
				SetTextColor(picker);
				SetFontSize(picker);
				SetBackground();
				SetPlaceHolder(picker);
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
				this.Control.SetTextColor(picker.TextColor.ToAndroid());
			}
		}

		void SetTextColor(ExtendedPicker picker)
		{
			this.Control.SetTextColor(picker.TextColor.ToAndroid());
		}

		void SetFontSize(ExtendedPicker picker)
		{
			float newSize = (float)picker.TextSize;

			this.Control.SetTextSize(Android.Util.ComplexUnitType.Sp, newSize);
		}

		void SetPlaceHolder(ExtendedPicker picker)
		{
			this.Control.Text = picker.PlaceHolder;
		}

		private void SetBackground()
		{
			Control.SetBackgroundColor(Android.Graphics.Color.Transparent);
			Control.SetSingleLine(true);
			Control.SetPadding(10, 0, 0, 0);
		}
	}
}