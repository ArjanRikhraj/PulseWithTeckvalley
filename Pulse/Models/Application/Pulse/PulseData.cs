using System.Collections.Generic;

namespace Pulse
{
	public class PulseData
	{
		public string name { get; set; }
		public List<Member> members { get; set; }
		public int event_id { get; set; }
		public string create_date { get; set; }
	}

	public class LastMessage
	{
		public int id { get; set; }
		public int pulsegroup { get; set; }
		public int user { get; set; }
		public string text_message { get; set; }
		public string create_date { get; set; }
		public string send_date { get; set; }

	}

	public class PulseResponse
	{
		public int id { get; set; }
		public string name { get; set; }
		public int user { get; set; }
		public LastMessage last_message { get; set; }
		public int message_count { get; set; }
		public bool part_of_group { get; set; }
		public string create_date { get; set; }
		public bool group_member_or_not { get; set; }
		public int total_groups { get; set; }
		public string profile_image { get; set; }
	}



	public class PulseMessageResponse
	{
		public int id { get; set; }
		public int pulsegroup { get; set; }
		public int user { get; set; }
		public string text_message { get; set; }
		public string send_date { get; set; }
		public UserInfo user_info { get; set; }
		public bool is_first_message { get; set; }
		public int message_count { get; set; }

	}



	public class EventInfo
	{
		public int id { get; set; }
		public string name { get; set; }
		public int user { get; set; }
	}

	public class PulseDetailResponse
	{
		public int id { get; set; }
		public string name { get; set; }
		public int user { get; set; }
		public bool is_owner { get; set; }
		public int participants_count { get; set; }
		public EventInfo event_info { get; set; }
		public List<UserInfo> participants { get; set; }
		public bool group_member_or_not { get; set; }
	}


	public class PulseStatusUpdate
	{
		public int pulse_invitation_status { get; set; }
		public int user_id { get; set; }
	}
	public class PulseStatusUpdateResponse
	{
		public int id { get; set; }
		public int pulsegroup { get; set; }
		public int user_id { get; set; }
		public int pulse_invitation_status { get; set; }
		public LastMessage last_message { get; set; }
	}

	public class ClearHistoryModel
	{
		public int pulsegroup_id { get; set; }
		public int pulse_request_type { get; set; }
		public int last_message_id { get; set; }
		public int user_id { get; set; }
	}
}
