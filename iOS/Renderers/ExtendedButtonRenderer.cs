using Xamarin.Forms.Platform.iOS;
using Pulse;
using Pulse.iOS;
using Xamarin.Forms;
using UIKit;

[assembly: ExportRenderer(typeof(ExtendedButton), typeof(ExtendedButtonRenderer))]
namespace Pulse.iOS
{
	public class ExtendedButtonRenderer : ButtonRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
		{
			base.OnElementChanged(e);
			var view = e.NewElement as ExtendedButton;
			if (view != null)
			{
				SetFont(view);
				if (view.IsSortButton)
				{
					UIButton thisButton = Control as UIButton;
					thisButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
					thisButton.ContentEdgeInsets = new UIEdgeInsets(0, 50, 0, 0);
				}
			}
		}
		private void SetFont(ExtendedButton view)
		{
			Control.Font = FontHelper.GetFont(view.FontFace, view.FontSize);
		}
	}
}

