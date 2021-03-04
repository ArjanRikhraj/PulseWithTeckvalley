using Pulse;
using Pulse.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Text;

[assembly: ExportRenderer(typeof(ExtendedEntry), typeof(ExtendedEntryRenderer))]
namespace Pulse.Droid
{
	public class ExtendedEntryRenderer : EntryRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);
			Control.Background = null;
			var view = (ExtendedEntry)Element;
			if (view != null)
			{
				SetFont(view);
				SetMaxLength(view);
				if (view.SetTextInCenter)
				{
					Control.SetPadding(0, 0, 0, 0);
				}
				if (view.IsPasscodeControl)
				{
					Control.SetCursorVisible(false);
				}
				Control.FocusChange += (sender, eargs) =>
				{
					if (view.IsOTPControl && eargs.HasFocus)
					{
						Control.SetSelection(Control.Text.Length);
					}
				};
			}
		}
		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (Control != null)
			{
				Control.Background = null;
				var view = (ExtendedEntry)Element;
				if (view != null)
				{
					SetFont(view);
				}
			}

		}

	

	private void SetMaxLength(ExtendedEntry view)
	{
		Control.SetFilters(new IInputFilter[] { new InputFilterLengthFilter(view.MaxLength) });
	}

	private void SetFont(ExtendedEntry view)
	{
		Control.Typeface = FontHelper.GetFontFace(view.FontFace);
		Control.Background = null;
	}
}
}
