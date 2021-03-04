using System.ComponentModel;
using Pulse;
using Pulse.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BorderlessEntry), typeof(BorderlessEntryRenderer))]
namespace Pulse.iOS
{
    public class BorderlessEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {

                var view = e.NewElement as BorderlessEntry;
                if (view != null)
                {
                    SetFont(view);
                    if (view.MaxLength > 0)
                    {
                        SetMaxLength(view);
                    }

                    if (Control != null && view.IsSearchControl)
                    {
                        Control.ReturnKeyType = UIReturnKeyType.Search;
                        Control.ShouldReturn += (textField) => {
                            view.OnSearched();
                            return true;
                        };

                    }
                    if (view.HasSmallerHint)
                    {
                        view.TextChanged += (sender, eargs) => {
                            SetFont(view);
                        };
                    }
                }
            }
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            Control.Layer.BorderWidth = 0;
            Control.BorderStyle = UITextBorderStyle.None;
        }

        private void SetFont(BorderlessEntry view)
        {
            double fontSize = view.HasSmallerHint && string.IsNullOrEmpty(view.Text) ? view.FontSize - 5 : view.FontSize;
            var font = FontHelper.GetFont(view.FontFace, fontSize);
            if (font != null)
            {
                Control.Font = font;
            }
        }

        private void SetMaxLength(BorderlessEntry view)
        {
            Control.ShouldChangeCharacters = (textField, range, replacementString) =>
            {
                var newLength = textField.Text.Length + replacementString.Length - range.Length;
                return newLength <= view.MaxLength;
            };
        }
    }
}
