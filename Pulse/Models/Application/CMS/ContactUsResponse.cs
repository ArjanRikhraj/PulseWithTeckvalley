using System;
namespace Pulse
{
	public class ContactUsResponse
	{
		public int id { get; set; }
		public string address { get; set; }
		public string mobile { get; set; }
		public string help_text { get; set; }
		public string email { get; set; }
		public DateTime create_date { get; set; }
		public DateTime update_date { get; set; }
	}
}
