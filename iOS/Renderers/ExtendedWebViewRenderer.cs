using Pulse;
using Pulse.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedWebView), typeof(ExtendedWebViewRenderer))]
namespace Pulse.iOS
{
	public class ExtendedWebViewRenderer : ViewRenderer<ExtendedWebView, UIWebView>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<ExtendedWebView> e)
		{
			base.OnElementChanged(e);
			Opaque = false;
			BackgroundColor = Color.Transparent.ToUIColor();
		}
	}
}
