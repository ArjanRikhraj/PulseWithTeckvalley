using Pulse;
using Pulse.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Foundation;
using System.Drawing;
using System;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(ExtendedEditor), typeof(ExtendedEditorRenderer))]

namespace Pulse.iOS
{
	public class ExtendedEditorRenderer : EditorRenderer
	{
		///// <summary>
		///// Ons the element changed.
		///// </summary>
		///// <returns>The element changed.</returns>
		///// <param name="e">E.</param>

		readonly UIColor DefaultPlaceholderColor = Xamarin.Forms.Color.FromRgb(86, 88, 92).ToUIColor();

		private UILabel PlaceholderLabel { get; set; }

		protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
		{
			base.OnElementChanged(e);
			if (Control == null) return;
			if (PlaceholderLabel != null) return;

			var element = Element as ExtendedEditor;

			PlaceholderLabel = new UILabel
			{
				Text = element?.Placeholder,
				TextColor = DefaultPlaceholderColor,
				Font = UIFont.FromName("Poppins-Regular", 13f),
				BackgroundColor = UIColor.Clear
			};
			//if ((Control != null) && (e.NewElement != null))
				//Control.ReturnKeyType = GetValueFromDescription<UIReturnKeyType>(element.ReturnType);
			var edgeInsets = Control.TextContainerInset;
			var lineFragmentPadding = Control.TextContainer.LineFragmentPadding;
			Control.AddSubview(PlaceholderLabel);
			var vConstraints = NSLayoutConstraint.FromVisualFormat(
				"V:|-" + edgeInsets.Top + "-[PlaceholderLabel]-" + edgeInsets.Bottom + "-|", 0, new NSDictionary(),
				NSDictionary.FromObjectsAndKeys(
					new NSObject[] { PlaceholderLabel }, new NSObject[] { new NSString("PlaceholderLabel") })
			);
			var hConstraints = NSLayoutConstraint.FromVisualFormat(
				"H:|-" + lineFragmentPadding + "-[PlaceholderLabel]-" + lineFragmentPadding + "-|",
				0, new NSDictionary(),
				NSDictionary.FromObjectsAndKeys(
					new NSObject[] { PlaceholderLabel }, new NSObject[] { new NSString("PlaceholderLabel") })
			);
			PlaceholderLabel.TranslatesAutoresizingMaskIntoConstraints = false;
			Control.AddConstraints(hConstraints);
			Control.AddConstraints(vConstraints);
			Control.InputAccessoryView = null;
            PlaceholderLabel.Hidden = !string.IsNullOrEmpty(Control.Text);
		}

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			var element = Element as ExtendedEditor;
			if (e.PropertyName == "Text")
			{
				PlaceholderLabel.Hidden = !string.IsNullOrEmpty(Control.Text);
			}
			if (e.PropertyName == element.ReturnType.ToString())
			{
				//Control.ReturnKeyType = GetValueFromDescription<UIReturnKeyType>(element.ReturnType);
			}
		
		
		}
	}
}

