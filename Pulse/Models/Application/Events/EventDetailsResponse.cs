using System.Collections.Generic;

namespace Pulse
{


	public class EventMedia
	{
		public int id { get; set; }
		public int event_id { get; set; }
		public string file_name { get; set; }
		public int file_type { get; set; }
		public bool is_live { get; set; }
		public string file_thumbnail { get; set; }
		public string user_name { get; set; }
		public string profile_image { get; set; }
		public string create_date { get; set; }
		public string event_name { get; set; }
		public int user_id { get; set; }
		public int total_media { get; set; }
		public bool is_private { get; set; }
	}

	public class EventDetailsResponse
	{
		public int id { get; set; }
		public int user { get; set; }
		public string name { get; set; }
		public string description { get; set; }
		public string location_address { get; set; }
		public string event_venue { get; set; }
		public string time_zone_type { get; set; }
		public bool is_free_time_event { get; set; }
		public string cover_fee_amount { get; set; }
		public bool is_bottle_service { get; set; }
		public bool is_boosted_event { get; set; }
		public string bottle_service_amount { get; set; }
		public bool is_owner { get; set; }
		public bool is_public { get; set; }
		public int event_likes_count { get; set; }
		public string contact_number { get; set; }
		public int event_lit_score { get; set; }
		public bool is_like { get; set; }
		public List<Attendee> attendees { get; set; }
		public string event_invitee_status { get; set; }
		public List<EventMedia> event_media { get; set; }
		public List<CommentResponse> comments { get; set; }
		public string start_date { get; set; }
		public string end_date { get; set; }
		public string start_time { get; set; }
		public string end_time { get; set; }
		public bool reported_spam { get; set; }
		public string host_name { get; set; }
		public string host_profile_image { get; set; }
		public double latitude { get; set; }
		public double longitude { get; set; }
		public bool is_checkin { get; set; }
		public string transaction_email { get; set; }
		public int total_comments { get; set; }
		public string cover_photo{get; set;}
		public bool is_star { get; set; }
	}

	public class CancelEvent
	{
		public string comment { get; set; }
	}

	public class EventAttend
	{
		public int invitee_status { get; set; }
	}
	public class PinMediaRequest
	{
		public int story_id { get; set; }
		public int user_id { get; set; }
	}
	public class PinMediaResponse
	{
		public string msg { get; set; }
		public int pinned_media_count { get; set; }
		public bool maximumpinnedcount { get; set; }
	}
}
