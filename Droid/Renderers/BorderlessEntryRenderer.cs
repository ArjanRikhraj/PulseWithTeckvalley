using Android.App;
using Android.Content;
using Android.Text;
using Android.Views.InputMethods;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Pulse;
using Pulse.Droid;

[assembly: ExportRenderer(typeof(BorderlessEntry), typeof(BorderlessEntryRenderer))]
namespace Pulse.Droid
{
    public class BorderlessEntryRenderer : EntryRenderer
    {
        public static void Init() { }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                Control.Background = null;
                var layoutParams = new MarginLayoutParams(Control.LayoutParameters);
                layoutParams.SetMargins(0, 0, 0, 0);
                LayoutParameters = layoutParams;
                Control.LayoutParameters = layoutParams;
                Control.SetPadding(0, 0, 0, 0);
                SetPadding(0, 0, 0, 0);

                var view = (BorderlessEntry)Element;
                if (view != null)
                {
                    SetFont(view);
                    if (view.MaxLength > 0)
                    {
                        SetMaxLength(view);
                    }
                    if (Control != null && view.IsSearchControl)
                    {
                        Control.ImeOptions = ImeAction.Search;
                        Control.SetImeActionLabel("Search", ImeAction.Search);
                        Control.EditorAction += (sender, eargs) => {
                            view.OnSearched();
                            HideKeyboard();
                        };

                    }
                    if (view.HasSmallerHint)
                    {
                        Control.TextSize = (float)view.FontSize - 5;
                        HandleTextChange(view);
                    }


                }
            }
        }

        private void HandleTextChange(BorderlessEntry view)
        {
            view.TextChanged += (sender, eargs) => {
                if (!string.IsNullOrEmpty(view.Text))
                {
                    Control.TextSize = (float)view.FontSize;
                }
                else
                {
                    Control.TextSize = (float)view.FontSize - 5;
                }

            };
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

        }

        private void HideKeyboard()
        {
            var inputMethodManager = Xamarin.Forms.Forms.Context.GetSystemService(Context.InputMethodService) as InputMethodManager;
            if (inputMethodManager != null && Xamarin.Forms.Forms.Context is Activity)
            {
                var activity = Xamarin.Forms.Forms.Context as Activity;
                var focusedView = activity.CurrentFocus;
                var token = focusedView == null ? null : focusedView.WindowToken;
                inputMethodManager.HideSoftInputFromWindow(token, HideSoftInputFlags.None);
            }
        }

        private void SetFont(BorderlessEntry view)
        {
            Control.Typeface = FontHelper.GetFontFace(view.FontFace);
            Control.Background = null;

        }
        private void SetMaxLength(BorderlessEntry view)
        {
            Control.SetFilters(new IInputFilter[] { new InputFilterLengthFilter(view.MaxLength) });
        }

    }
}
