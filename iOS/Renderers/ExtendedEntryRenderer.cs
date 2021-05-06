using Pulse;
using Pulse.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedEntry), typeof(ExtendedEntryRenderer))]
namespace Pulse.iOS
{
	public class ExtendedEntryRenderer : EntryRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);
            try
            {
				if(e.NewElement!=null)
                {
					var view = e.NewElement as ExtendedEntry;
					Control.BorderStyle = UITextBorderStyle.None;
					if (view != null)
					{
						SetFont(view);
						SetMaxLength(view);
					}
				}
			}
            catch (System.Exception ex)
            {
				return;
            }
		}

		void SetMaxLength(ExtendedEntry view)
		{
			Control.ShouldChangeCharacters = (textField, range, replacementString) =>
			{
				var newLength = textField.Text.Length + replacementString.Length - range.Length;
				return newLength <= view.MaxLength;
			};
		}

		void SetFont(ExtendedEntry view)
		{
			Control.Font = FontHelper.GetFont(view.FontFace, view.FontSize);
		}
	}
}
