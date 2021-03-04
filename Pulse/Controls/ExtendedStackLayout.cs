using Xamarin.Forms;

namespace Pulse
{
	public class ExtendedStackLayout : StackLayout
	{
		public ExtendedStackLayout()
		{
		}
		public ActivePage ActivePage
		{
			get;
			set;
		}
		public MyEventType EventType
		{
			get;set;
		}
		public EventStatusType StatusType
		{
			get;set;
		}
		public GuestType EventGuestType
		{
			get;set;
		}
	}
}
