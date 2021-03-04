using Xamarin.Forms;

namespace Pulse
{
	public class GradientColorStack : StackLayout
	{
		public Color StartColor { get; set; }
		public Color EndColor { get; set; }
		public StackOrientation stackOrientation { get; set; }
	}
}
