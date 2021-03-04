using Pulse;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedWebView), typeof(WebViewRenderer))]
namespace Pulse.iOS
{
	public class ExtendedWebViewRenderer : WebViewRenderer
	{
		protected override void OnElementChanged(VisualElementChangedEventArgs e)
		{
			base.OnElementChanged(e);
			Opaque = false;
			BackgroundColor = Color.Transparent.ToUIColor();
		}
	}
}
