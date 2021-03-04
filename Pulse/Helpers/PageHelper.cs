using System.IO;
using Xamarin.Forms;

namespace Pulse
{
	public static class PageHelper
	{
		
		#region Static Methods

		public static string GetUserImage(string imageName)
		{
			if (!string.IsNullOrEmpty(imageName) && !imageName.Contains("/"))
			{
				return App.AWSCurrentDetails.response.s3_host + App.AWSCurrentDetails.response.images_path.user_profile + "/" + imageName;
			}
			else
			{
				return imageName;
			}
		}

		public static string GetEventImage(string imageName)
		{
			if (!string.IsNullOrEmpty(imageName) && !imageName.Contains("/"))
			{
				return App.AWSCurrentDetails.response.s3_host + App.AWSCurrentDetails.response.images_path.event_images + "/" + imageName;
			}
			else
			{
				return imageName;
			}
		}
		public static string GetEventVideoThumbnail(string imageName)
		{
			if (!string.IsNullOrEmpty(imageName) && !imageName.Contains("/"))
			{
                return App.AWSCurrentDetails.response.s3_host + App.AWSCurrentDetails.response.images_path.event_videos_thumbnails + "/" + imageName;
			}
			else
			{
				return imageName;
			}
		}


		public static string GetEventTranscodedVideo(string imageName)
		{
			if (!string.IsNullOrEmpty(imageName) && !imageName.Contains("/"))
			{
                if (App.AWSCurrentDetails.response.Env_keys.Transcoding_enable)
                {
                    return App.AWSCurrentDetails.response.s3_host + App.AWSCurrentDetails.response.images_path.event_transcoded_videos + "/" + imageName;
                }
                else
                {
                    return App.AWSCurrentDetails.response.s3_host + App.AWSCurrentDetails.response.images_path.event_videos + "/" + imageName;

                }
			}
			else
			{
				return imageName;
			}
		}

		public static void ShowPlaceHolderLabel(string value, Label label, bool show)
		{
			if (!show && !string.IsNullOrEmpty(value))
			{

				return;
			}
			label.IsVisible = show;

		}

		public static byte[] ReadFully(Stream input)
		{
			byte[] buffer = new byte[16 * 1024];
			using (MemoryStream ms = new MemoryStream())
			{
				int read;
				while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
				{
					ms.Write(buffer, 0, read);
				}
				return ms.ToArray();
			}
		}
		#endregion Static Methods
	}
}
