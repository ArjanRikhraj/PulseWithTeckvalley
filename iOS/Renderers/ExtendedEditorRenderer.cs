using Pulse;
using Pulse.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Foundation;
using System.Drawing;

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
			this.AddDoneButton();
			if (PlaceholderLabel != null) return;

			var element = Element as ExtendedEditor;

			PlaceholderLabel = new UILabel
			{
				Text = element?.Placeholder,
				TextColor = DefaultPlaceholderColor,
				Font = UIFont.FromName("Poppins-Regular", 13f),
				BackgroundColor = UIColor.Clear
			};

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

			if (e.PropertyName == "Text")
			{
				PlaceholderLabel.Hidden = !string.IsNullOrEmpty(Control.Text);
			}
		}
		protected void AddDoneButton()
		{
			var toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, 50.0f, 44.0f));

			var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate
			{
				this.Control.ResignFirstResponder();
				var baseEntry = this.Element.GetType();
				((IEntryController)Element).SendCompleted();
			});

			toolbar.Items = new UIBarButtonItem[] {
				new UIBarButtonItem (UIBarButtonSystemItem.FlexibleSpace),
				doneButton
			};
			this.Control.InputAccessoryView = toolbar;
		}
	}
}

