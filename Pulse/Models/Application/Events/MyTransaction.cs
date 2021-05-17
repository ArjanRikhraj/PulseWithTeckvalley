namespace Pulse
{
	public class MyTransaction
	{
		public string transaction_id { get; set; }
		public string email { get; set; }
		public int status { get; set; }
		public bool is_bottle_service { get; set; }
		public double total_amount { get; set; }
        public PaymentType payment_type { get; set; }
	}
	public class Countries
	{
		public string name { get; set; }
		public string iso { get; set; }
	}
	public class UserCheckIn
	{
		public double longitude { get; set; }
		public double latitude { get; set; }
	}

	public class LiveMedia
	{
		public double latitude { get; set; }
		public double longitude { get; set; }
		public string file_name { get; set; }
		public int file_type { get; set; }
		public bool is_live { get; set; }
		public string file_thumbnail { get; set; }
	}
	public class Media
	{
		public string file_name { get; set; }
		public int file_type { get; set; }
		public string file_thumbnail { get; set; }
	}

	public class CoverImageRequest
	{
		public int event_id { get; set; }
		public Media media { get; set; }
	}
	public class CoverImageResponse
	{
		public string msg { get; set; }
	}
	public class PullStoryRequest
    {
		public long user_id { get; set; }
		public long event_id { get; set; }
		public Media media { get; set; }
	}
}
