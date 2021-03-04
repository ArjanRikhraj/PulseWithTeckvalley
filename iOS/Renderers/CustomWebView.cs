using Foundation;
using Pulse.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Pulse;

[assembly: ExportRenderer(typeof(CustomWebView), typeof(CustomWebViewRenderer))]
namespace Pulse.iOS
{
	public class CustomWebViewRenderer : WebViewRenderer
	{
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (NativeView != null)
            {
                var webView = (UIWebView)NativeView;

                webView.Opaque = false;
                webView.BackgroundColor = UIColor.Clear;
                webView.MediaPlaybackRequiresUserAction = false;

            }
        }

		public override void PressesCancelled(NSSet<UIPress> presses, UIPressesEvent evt)
		{
			base.PressesCancelled(presses, evt);
			this.Hidden = true;
		}
	}
}


