using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Pulse
{
	public class MyEventResponse
	{
		public int id { get; set; }
		public string name { get; set; }
		public string location_address { get; set; }
		public string event_venue { get; set; }
		public string time_zone_type { get; set; }
		public string latitude { get; set; }
		public string longitude { get; set; }
		public bool is_boosted_event { get; set; }
		public int event_likes_count { get; set; }
		public int event_lit_score { get; set; }
		public int event_attendees_count { get; set; }
		public List<Attendee> attendees { get; set; }
		public bool is_like { get; set; }
		public string start_date { get; set; }
		public string event_status_label { get; set; }
		public string end_date { get; set; }
		public string start_time { get; set; }
		public string end_time { get; set; }
		public int total_events { get; set; }
		public int lat_log_event_count { get; set; }
		public int user { get; set; }
	}
	public class Attendee
	{
		public int id { get; set; }
		public string profile_image { get; set; }
		public string friend_image = Constant.ProfileIcon;
		public string fullname { get; set; }
	}
	public class MyEvents : BaseViewModel
	{
		public int EventId { get; set; }
		public string EventName { get; set; }
		public string EventStatus { get; set; }
		public string EventAddress { get; set; }
		public string EventLitScore { get; set; }
		public string EventDateTime { get; set; }
		public DateTime StartDate { get; set; }
		public Color ListBackColor { get; set; }
		public string EventLikes { get; set; }
		public string AttendeeCount { get; set; }
		public int LatLongEventsCount { get; set; }
		public string AttendeeImageFirst { get; set; }
		public string AttendeeImageSecond { get; set; }
		public string Eventlat { get; set; }
		public string EventLong { get; set; }
		public bool IsBoostEvent { get; set; }
		public bool IsCurrentLocation { get; set; }
		public bool IsCheckInButtonVisible { get; set; }
		public bool IsShowViewAll { get; set; }
		Color frameBorderColor;
		int frameBorderWidth;
		bool isFirstImageVisible;
		bool isSecondImageVisible;
		bool isEditIconVisible;
		bool isOuterFrameVisible;
		public Color FrameBorderColor
		{
			get { return frameBorderColor; }
			set
			{
				frameBorderColor = value;
				OnPropertyChanged("FrameBorderColor");
			}
		}
		public int FrameBorderWidth
		{
			get { return frameBorderWidth; }
			set
			{
				frameBorderWidth = value;
				OnPropertyChanged("FrameBorderWidth");
			}
		}
		public bool IsOuterFrameVisible
		{
			get { return isOuterFrameVisible; }
			set
			{
				isOuterFrameVisible = value;
				OnPropertyChanged("IsOuterFrameVisible");
			}
		}
		public bool IsFirstImageVisible
		{
			get { return isFirstImageVisible; }
			set
			{
				isFirstImageVisible = value;
				OnPropertyChanged("IsFirstImageVisible");
			}
		}
		public bool IsSecondImageVisible
		{
			get { return isSecondImageVisible; }
			set
			{
				isSecondImageVisible = value;
				OnPropertyChanged("IsSecondImageVisible");
			}
		}
		public bool IsEditIconVisible
		{
			get { return isEditIconVisible; }
			set
			{
				isEditIconVisible = value;
				OnPropertyChanged("IsEditIconVisible");
			}
		}
		//New values
		private string remainingTime { get; set; }
		public string RemainingTime
		{
			get { return remainingTime; }
			set
			{
				remainingTime = value;
				OnPropertyChanged("RemainingTime");
			}
		}
		private string partyImage { get; set; }
		public string PartyImage
		{
			get { return partyImage; }
			set
			{
				partyImage = value;
				OnPropertyChanged("PartyImage");
			}
		}
	}
}
