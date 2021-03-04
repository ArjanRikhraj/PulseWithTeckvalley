using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Pulse;
using Pulse.iOS;

[assembly: ExportRenderer(typeof(PlaceholderEditor), typeof(PlaceholderEditorRenderer))]
namespace Pulse.iOS
               
{
    public class PlaceholderEditorRenderer : EditorRenderer
    {
        private UILabel _placeholderLabel;

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Element == null)
                return;
            var view = (PlaceholderEditor)Element;
            if (view != null)
            {
                SetFont(view);
                CreatePlaceholderLabel(view, Control);
                Control.Ended += OnEnded;
                view.TextChanged += OnChanged;
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            Control.Layer.BorderWidth = 0;
        }

        private void CreatePlaceholderLabel(PlaceholderEditor element, UITextView parent)
        {
            _placeholderLabel = new UILabel(new CoreGraphics.CGRect(3, 10, App.ScreenWidth / 2, element.FontSize));
            _placeholderLabel.Text = element.Placeholder;
            _placeholderLabel.TextColor = element.PlaceholderColor.ToUIColor();
            _placeholderLabel.BackgroundColor = UIColor.Clear;
            _placeholderLabel.Font = FontHelper.GetFont(element.FontFace, element.FontSize);
            _placeholderLabel.Alpha = (nfloat)0.5;
            parent.AddSubview(_placeholderLabel);
            parent.LayoutIfNeeded();

            _placeholderLabel.Hidden = parent.HasText;
        }

        private void OnEnded(object sender, EventArgs args)
        {
            if (!((UITextView)sender).HasText && _placeholderLabel != null)
                _placeholderLabel.Hidden = false;
        }

        private void OnChanged(object sender, EventArgs args)
        {
            if (_placeholderLabel != null)
                _placeholderLabel.Hidden = ((PlaceholderEditor)sender).Text.Length > 0;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Control.Ended -= OnEnded;
                Control.Changed -= OnChanged;

                _placeholderLabel?.Dispose();
                _placeholderLabel = null;
            }

            base.Dispose(disposing);
        }

        private void SetFont(PlaceholderEditor view)
        {
            var font = FontHelper.GetFont(view.FontFace, view.FontSize);
            if (font != null)
            {
                Control.Font = font;
            }
        }

    }
}
