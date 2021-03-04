using Pulse;
using Pulse.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(PlaceholderEditor), typeof(PlacehoderEditorRenderer))]
namespace Pulse.Droid
{
    public class PlacehoderEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Element == null)
                return;
            var element = (PlaceholderEditor)Element;
            if (element != null)
            {
                Control.Background = null;
                var layoutParams = new MarginLayoutParams(Control.LayoutParameters);
                layoutParams.SetMargins(0, 0, 0, 0);
                LayoutParameters = layoutParams;
                Control.LayoutParameters = layoutParams;
                Control.SetPadding(0, 0, 0, 0);
                SetPadding(0, 0, 0, 0);

                SetFont(element);
                Control.Hint = element.Placeholder;
                Control.SetHintTextColor(element.PlaceholderColor.ToAndroid());
                Control.Alpha = (float)0.7;
                Control.TextChanged += (sender, eargs) => {
                    Control.Alpha = Control.Text.Length > 0 ? (float)1 : (float)0.7;
                };

            }
        }

        private void SetFont(PlaceholderEditor view)
        {
            Control.Typeface = FontHelper.GetFontFace(view.FontFace);
            Control.Background = null;
        }
    }
}
