using Xamarin.Forms.Maps;
namespace Pulse
{
	public class CustomPin : Pin
	{
		public string Id { get; set; }
		public string UserImage { get; set; }
		public string Url { get; set; }
		public string EventName { get; set; }
		public string EventDateTime { get; set; }
        public bool IsBoostEvent { get; set; }
        public bool IsCurrentLocation { get; set; }
        public string SameLocationPinCount { get; set; }
        public bool IsMoreThanOneLocation { get; set; }
        public string Latitude { get; set; }
        public string Lognitude { get; set; }
	}
}
