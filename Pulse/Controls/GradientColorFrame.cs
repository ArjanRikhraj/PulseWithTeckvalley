using Xamarin.Forms;

namespace Pulse
{
	public class GradientColorFrame : Frame
	{
		public Color StartColor { get; set; }
		public Color EndColor { get; set; }
        public bool IsBottomRightNotRounded { get; set; }
        public bool IsTopLeftRoundNotRounded { get; set; }
        public bool IsBottomLeftNotRounded { get; set; }
        public int BorderRadius{ get; set;}
	}
}
