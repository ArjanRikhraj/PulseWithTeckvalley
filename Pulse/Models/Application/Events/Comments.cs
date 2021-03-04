using System;
using System.Collections.Generic;

namespace Pulse
{
	public class TagFriend
	{
		public int friend_id { get; set; }
	}

	public class Comments
	{
		public int event_id { get; set; }
		public string comment_text { get; set; }
		public List<TagFriend> tag_friends { get; set; }
		public string send_date { get; set; }
	}



	public class UserInfo
	{
		public int id { get; set; }
		public string profile_image { get; set; }
		public string fullname { get; set; }
		public string username { get; set; }
		public bool is_admin { get; set; }
		public int total_users { get; set; }
	}

	public class CommentResponse
	{
		public int id { get; set; }
		public string comment_text { get; set; }
		public DateTime create_date { get; set; }
		public bool is_owner { get; set; }
		public UserInfo user_info { get; set; }
		public string send_date { get; set; }
		public List<UserInfo> tagged_users { get; set; }
		public int total_comments { get; set; }
	}

	public class CommentListViewSource : BaseViewModel
	{
		public string CommentText { get; set; }
		public string CommenteeName { get; set; }
		public string CommentDate { get; set; }
		public int CommentId { get; set; }
		public string CommenteeImage { get; set; }
		bool isCommentDeleteIconVisible;
		public bool IsCommentDeleteIconVisible
		{
			get { return isCommentDeleteIconVisible; }
			set
			{
				isCommentDeleteIconVisible = value;
				OnPropertyChanged("IsCommentDeleteIconVisible");
			}
		}
	}
	public class DeleteComment
	{
		public bool is_deleted { get; set; }
	}
}
