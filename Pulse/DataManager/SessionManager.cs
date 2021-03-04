using System;

using Xamarin.Forms;

namespace Pulse
{
	public static class SessionManager
	{
		public static string AccessToken
		{
			get
			{
				return Application.Current.Properties.ContainsKey("AccessToken") ? string.IsNullOrEmpty(Application.Current.Properties["AccessToken"].ToString()) ? string.Empty : Convert.ToString(Application.Current.Properties["AccessToken"]) : string.Empty;
			}
			set
			{
				Application.Current.Properties["AccessToken"] = value;
			}
		}

		public static string UserName
		{
			get
			{
				return Application.Current.Properties.ContainsKey("UserName") ? string.IsNullOrEmpty(Application.Current.Properties["UserName"].ToString()) ? string.Empty : Convert.ToString(Application.Current.Properties["UserName"]) : string.Empty;
			}
			set
			{
				Application.Current.Properties["UserName"] = value;
			}
        }
        public static string Email
		{
			get
			{
                return Application.Current.Properties.ContainsKey("Email") ? string.IsNullOrEmpty(Convert.ToString(Application.Current.Properties["Email"])) ? string.Empty : Convert.ToString(Application.Current.Properties["Email"]) : string.Empty;
			}
			set
			{
				Application.Current.Properties["Email"] = value;
			}
		}
		public static int UserId { get; set; }
		public static int RecordPerPage { get; set; }
        public static bool UnreadNotification { get; set; }
	}
}