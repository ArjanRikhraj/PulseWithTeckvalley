using System.ComponentModel;

namespace Pulse
{
	public class PulseModel : INotifyPropertyChanged
	{
		public string ImageText { get; set; }
		public string LastMessage { get; set; }
		public string profile_image { get; set; }
		public int PulseId { get; set; }
		public int LastMessageUserId { get; set; }
		public string Subject { get; set; }
		public string Time { get; set; }
		public int PulseownerId { get; set; }
		public string GroupIconBorderColor { get; set; }
		public bool IsPulseMember { get; set; }
		public int TotalMessagesCount { get; set; }
		public string NotifyCount { get; set; }
		bool isNewMessages = false;
		public bool IsNewMessages
		{
			get
			{
				return isNewMessages;
			}
			set
			{
				if (isNewMessages != value)
				{
					isNewMessages = value;
					PropertyChanged(this, new PropertyChangedEventArgs("IsNewMessages"));
				}
			}
		}
		public event PropertyChangedEventHandler PropertyChanged = delegate { };
	}

	public class PostMessageData
	{
		public int pulsegroup_id { get; set; }
		public string text_message { get; set; }
		public string send_date { get; set; }
	}
	public class PulseMemberData
	{
		public int pulsegroup_id { get; set; }
	}
	public class PulseMessage : BaseViewModel
	{
		bool isNewTextTimeVisible;
		bool isMyMessageVisible;
		bool isOtherMessageVisible;
		public string MessageText { get; set; }
		public string MessageTime { get; set; }
		public string UserName { get; set; }
		public string UserPic { get; set; }
		public string DayViseTimeText { get; set; }
		public bool IsMyMessageVisible
		{
			get { return isMyMessageVisible; }
			set
			{
				isMyMessageVisible = value;
				OnPropertyChanged("IsMyMessageVisible");
			}
		}
		public bool IsOtherMessageVisible
		{
			get { return isOtherMessageVisible; }
			set
			{
				isOtherMessageVisible = value;
				OnPropertyChanged("IsOtherMessageVisible");
			}
		}
		public bool IsNewTextTimeVisible
		{
			get { return isNewTextTimeVisible; }
			set
			{
				isNewTextTimeVisible = value;
				OnPropertyChanged("IsNewTextTimeVisible");
			}
		}
	}
}