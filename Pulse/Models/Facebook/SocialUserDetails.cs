using System;
using System.Collections.Generic;

namespace Pulse
{
	public class SocialUserDetails
	{
		public string Id { get; set; }
		public string Token { get; set; }
		public string TokenSecret { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string ScreenName { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public Picture picture { get; set; }//for facebook only
		public Sizes sizes { get; set; }//for facebook twitter
		public string pictureUrl { get; set; }//for facebook linkein
		public string UserPicture { get; set; }
		public string Color { get; set; }
		public string SocialLogo { get; set; }
		public bool IsAuthenticated
		{
			get
			{
				return !string.IsNullOrWhiteSpace(Token);
			}
		}
	}
	public class Picture
	{
		public Data data { get; set; }
	}
	public class Sizes
	{
		public Mobile mobile { get; set; }
	}
	public class Data
	{
		public bool is_silhouette { get; set; }
		public string url { get; set; }
	}
	public class Mobile
	{
		public int h { get; set; }
		public int w { get; set; }
		public string url { get; set; }
	}
	public class LikesResponse
	{
		public int likes_count { get; set; }
	}
}
