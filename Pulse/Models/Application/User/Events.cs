using System.Collections.Generic;

namespace Pulse
{
	public class Member
	{
		public int user_id { get; set; }
	}
	public class Medium
	{
		public string file_name { get; set; }
		public int file_type { get; set; }
		public bool is_live { get; set; }
		public string file_thumbnail { get; set; }
	}
	public class Events
	{
		public string name { get; set; }
		public string city { get; set; }
		public string state { get; set; }
		public string country { get; set; }
		public string event_venue { get; set; }
		public string location_address { get; set; }
		public string event_start_date_time { get; set; }
		public string event_end_date_time { get; set; }
		public string time_zone_type { get; set; }
		public double latitude { get; set; }
		public double longitude { get; set; }
		public bool is_free_time_event { get; set; }
        public string contact_number { get; set; }
        public bool is_boosted_event { get; set; }
        public double cover_fee_amount { get; set; }
		public bool is_bottle_service { get; set; }
		public double bottle_service_amount { get; set; }
        public double boosted_event_price { get; set; }
		public string description { get; set; }
		public bool is_public { get; set; }
		public List<Member> members { get; set; }
		public List<Medium> media { get; set; }
        public MyTransaction transaction { get; set; }
		public string cover_photo { get; set; }
	}

	public class EventResponse
	{
		public int id { get; set; }
		public int user { get; set; }
		public string name { get; set; }
		public string city { get; set; }
		public string state { get; set; }
		public string country { get; set; }
		public string description { get; set; }
		public string location_address { get; set; }
		public double latitude { get; set; }
		public double longitude { get; set; }
		public string event_venue { get; set; }
		public string time_zone_type { get; set; }
		public bool is_free_time_event { get; set; }
		public object cover_fee_amount { get; set; }
		public bool is_bottle_service { get; set; }
		public object bottle_service_amount { get; set; }
        public string transaction_email { get; set; }
		public bool is_public { get; set; }
		public string start_date { get; set; }
		public string end_date { get; set; }
		public string start_time { get; set; }
		public string end_time { get; set; }
	}
	public partial class StarEventRequest
    {
		public long event_id { get; set; }
		public long user_id { get; set; }
    }
	public partial class StarEventResponse
    {
		public bool is_star { get; set; }
		public string msg { get; set; }
	}
}
