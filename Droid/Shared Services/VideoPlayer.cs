
using Pulse.Droid;
using Android.Content;
using Android.OS;

[assembly: Xamarin.Forms.Dependency(typeof(VideoPlayerR))]
namespace Pulse.Droid
{
	public class VideoPlayerR : Java.Lang.Object,IVideoPlayer
	{
		#region IVideoPlayer implementation
		public void Play (string url)
		{
			var intent = new Intent (Xamarin.Forms.Forms.Context,typeof(VideoPlayerActivity));
			var bundle = new Bundle();
			bundle.PutString("url",url);
			intent.PutExtras(bundle);
			Xamarin.Forms.Forms.Context.StartActivity (intent);
		}
		#endregion
		
	}
}

