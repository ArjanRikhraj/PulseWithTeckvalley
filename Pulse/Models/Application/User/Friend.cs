using System;
namespace Pulse
{
	public class Friend : BaseViewModel
	{
		bool isUnchecked;
		bool ischecked;
		bool isYouLabelVisible;
		bool isAdminLabelVisible;
		public int friendId { get; set; }
        public bool IsCrossIconNotVisible { get; set; }
		public string friendFullname { get; set; }
		public string friendUsername { get; set; }
		public string friendPic { get; set; }
		public string friendMutual { get; set; }
		public float cornerRadius { get; set; }
		public bool IsAddFriendButtonVisible { get; set; }
		public bool IsFriendsButtonVisible { get; set; }
		public bool IsCancelRequestButtonVisible { get; set; }
		public bool IsConfirmRequestButtonVisible { get; set; }
        public bool isActionLabelVisible { get; set; }

		public bool IsYouLabelVisible
		{
			get { return isYouLabelVisible; }
			set
			{
				isYouLabelVisible = value;
				OnPropertyChanged("IsYouLabelVisible");
			}
		}
        public bool IsActionLabelVisible
        {
            get { return isActionLabelVisible; }
            set
            {
                isActionLabelVisible = value;
                OnPropertyChanged("IsActionLabelVisible");
            }
        }
		public bool IsAdminLabelVisible
		{
			get { return isAdminLabelVisible; }
			set
			{
				isAdminLabelVisible = value;
				OnPropertyChanged("IsAdminLabelVisible");
			}
		}

		public bool Ischecked
		{
			get { return ischecked; }
			set
			{
				ischecked = value;
				OnPropertyChanged("Ischecked");
			}
		}
		public bool IsUnchecked
		{
			get { return isUnchecked; }
			set
			{
				isUnchecked = value;
				OnPropertyChanged("IsUnchecked");
			}
		}
	}
	public class FriendResponseForUser
	{
		public int id { get; set; }
		public string fullname { get; set; }
		public string username { get; set; }
		public string email { get; set; }
		public string profile_image { get; set; }
		public string fb_profileid { get; set; }
		public DateTime create_date { get; set; }
		public int mutual_user_count { get; set; }
		public string request_type { get; set; }
		public bool is_admin { get; set; }
		public int total_users { get; set; }
	}
	public class FriendRequest
	{
		public int friend_id { get; set; }
		public int user_id { get; set; }
		public int friend_request_status { get; set; }
	}
	public class AddFriendResponse
	{
		public int friend_id { get; set; }
		public int id { get; set; }
		public int user { get; set; }
		public int friend_request_status { get; set; }
	}
	public class GuestResponse
	{
		public FriendResponseForUser user_info { get; set; }
		public int total_attendees { get; set; }
	}

	public class FriendProfileResponse
	{
		public int id { get; set; }
		public string email { get; set; }
		public string fullname { get; set; }
		public string username { get; set; }
		public string profile_image { get; set; }
		public string fb_profileid { get; set; }
		public string school { get; set; }
		public string dob { get; set; }
		public string mobile { get; set; }
		public DateTime create_date { get; set; }
		public string request_type { get; set; }
		public int attended_events { get; set; }
		public int hosted_events { get; set; }
        public bool already_blocked { get; set; }
		public int friends { get; set; }
		public int scores { get; set; }
	}


	public class ProfileData
	{
		public string fullname { get; set; }
		public string mobile { get; set; }
		public object dob { get; set; }
		public object school { get; set; }
		public object profile_image { get; set; }
	}

    public class BlockUnBlockFriendData
    {
        public int block_to { get; set; }

    }
}
