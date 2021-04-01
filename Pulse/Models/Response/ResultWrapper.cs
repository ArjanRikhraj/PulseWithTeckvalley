using System;
using System.Collections.Generic;
namespace Pulse
{
	public class ResultWrapper<T>
	{
		public int status { get; set; }
		public List<T> message { get; set; }
		public List<T> response { get; set; }
	}

	public class ResultWrapperSingle<T>
	{
		public int status { get; set; }
		public MessageError message { get; set; }
		public T response { get; set; }
	}

	public class CheckEmailUserResponse
	{
		public int mobile { get; set; }
		public string email { get; set; }
	}
	public class SendEmailOTPResponse
	{
		public string details { get; set; }
	}
	public class PostUserLocationResponse
    {
		public int status { get; set; }
		public Response response { get; set; }
	}
	public class MessageError
	{
		public List<string> non_field_errors { get; set; }
		public List<string> email { get; set; }
		public List<string> username { get; set; }
		public List<string> friend_id { get; set; }
	}
	public class LoginResponse
	{
		public string email { get; set; }
		public int id { get; set; }
		public string access_token { get; set; }
		public bool notification_status { get; set; }
		public bool is_existing_user { get; set; }
        public int unread_notification { get; set; }
	}

	public class UserResponse
	{
		public string email { get; set; }
		public string device_id { get; set; }
		public string device_token { get; set; }
		public string device_type { get; set; }
		public string access_token { get; set; }
		public string fullname { get; set; }
		public string username { get; set; }
		public DateTime create_date { get; set; }
		public int id { get; set; }
		public bool notification_status { get; set; }
		public string profile_image { get; set; }
		public object fb_profileid { get; set; }
		public bool is_active { get; set; }
		public object mobile { get; set; }
		public int registration_platform { get; set; }
		public object last_login { get; set; }
		public object dob { get; set; }
		public int mutual_user_count { get; set; }
		public int total_users { get; set; }
	}


	public class UserDetailResponse
	{
		public List<UserResponse> response { get; set; }
		public int status { get; set; }
	}
	public class PendingRequestCountResponse
	{
		public int request_count { get; set; }
	}
	public class TimeZones
	{
		public List<string> zone_names { get; set; }
	}
    public class UnreadNotificationcountResponse
    {
        public int unread_notification_count { get; set; }
    }
    public class NotificationCountResponse<T>
    {
        public T Response { get; set; }
        public int status { get; set; }
    }
}