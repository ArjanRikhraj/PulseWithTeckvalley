using System;
namespace Pulse
{
	public class UserData
	{
		public string username { get; set; }
		public DateTime dob { get; set; }
		public string password { get; set; }
		public string confirm_password { get; set; }
		public string email { get; set; }
		public string mobile { get; set; }
		public string token { get; set; }
		public string device_id { get; set; }
		public string device_type { get; set; }
		public string device_token { get; set; }
		public string fullname { get; set; }
		public int registration_platform { get; set; }
		public string school { get; set; }
		public string profile_image { get; set; }
		public string fb_profileid { get; set; }
		public string oldpassword { get; set; }
		public string newpassword { get; set; }
		public double latitude { get; set; }
		public double longitude { get; set; }
	}

}
