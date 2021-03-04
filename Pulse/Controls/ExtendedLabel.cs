using Xamarin.Forms;

namespace Pulse
{
	public class ExtendedLabel : Label
	{
		public ExtendedLabel()
		{

		}
		#region Properties
		public FontFace FontFace
		{
			get;
			set;
		}
        public double LineSpacing { get; set; }
		#endregion Properties
	}
}

