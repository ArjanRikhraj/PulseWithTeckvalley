using Xamarin.Forms;

namespace Pulse
{
	public class ExtendedButton : Button
	{
		#region Properties

		public ExtendedButton()
		{
			this.FontSize = 17;
			this.FontFace = FontFace.PoppinsMedium;
		}
		public FontFace FontFace
		{
			get;
			set;
		}
		public bool IsSortButton
		{
			get;
			set;
		}
		public FriendType friendType
		{
			get; set;
		}
		public ActivePage ActivePage
		{
			get;
			set;
		}

		#endregion Properties
	}
}
