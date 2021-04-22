using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace Pulse
{
	public static class SessionManager
	{

        //Temporary bases access token
        //public static string accessToken { get; set; } = "MkEW7mfwCKLG1NUh5BjQl6stjV9JZX";
        //public static string AccessToken
        //{
        //    get
        //    {
        //        return accessToken;

        //    }
        //    set
        //    {
        //        accessToken = value;
        //    }
        //}


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
		public static string Mobile
		{
			get
			{
				return Application.Current.Properties.ContainsKey("Mobile") ? string.IsNullOrEmpty(Convert.ToString(Application.Current.Properties["Mobile"])) ? string.Empty : Convert.ToString(Application.Current.Properties["Mobile"]) : string.Empty;
			}
			set
			{
				Application.Current.Properties["Mobile"] = value;
			}
		}
		public static int UserId { get; set; }
		public static int RecordPerPage { get; set; }
        public static bool UnreadNotification { get; set; }
	}
}