using System;
using System.Collections.Generic;

namespace Pulse
{
	public class Event : BaseViewModel
	{

		public string EventTime { get; set; }
		public string EventName { get; set; }
		public int EventId { get; set; }
		public string EventInviteeCount { get; set; }
	}


	public class EventConfirmation
	{
		public int id { get; set; }
		public int user { get; set; }
		public int event_id { get; set; }
		public int invitee_status { get; set; }
	}
    public class EventReport
    {
        public int event_id { get; set; }
        public string reason_to_spam { get; set; }
    }
    public class EventCommentReport
    {
        public int event_id { get; set; }
        public int event_comment_id { get; set; }
        public string reason_to_spam { get; set; }
    }
    public class NotificationResponse
    {
        public int id { get; set; }
        public string title { get; set; }
        public string message { get; set; }
        public bool notification_unread_exist { get; set; }
        public DateTime create_date { get; set; }
        public string date { get; set; }
        public ExtraData extra_data { get; set; }
       
    }
    public class ExtraData
    {
        public int id { get; set; }
    }
    public class Response
    {
        public List<string> notification { get; set; }
    }

    public class MarkReadResponse
    {
        public Response response { get; set; }
        public int status { get; set; }
    }
}
