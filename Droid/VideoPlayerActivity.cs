using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Pulse.Droid
{
	[Activity(Label = "Video", WindowSoftInputMode = SoftInput.AdjustPan, Theme = "@android:style/Theme.Light.NoTitleBar", ScreenOrientation=Android.Content.PM.ScreenOrientation.Unspecified,  ConfigurationChanges=Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class VideoPlayerActivity : Activity , Android.Media.MediaPlayer.IOnPreparedListener
	{
		ProgressDialog progDailog;
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView(Resource.Layout.VideoPlayerView);
			
		}

		protected override void OnStart()
		{
            base.OnStart();
            progDailog = new ProgressDialog(this, Resource.Style.MyProgressDialogTheme);
            progDailog.Indeterminate = true;
            var videoView = FindViewById<VideoView>(Resource.Id.placeVideoView);
            MediaController mc = new MediaController(this);
            mc.SetAnchorView(videoView);
            mc.SetMediaPlayer(videoView);
            videoView.SetMediaController(mc);
            string url = Intent.Extras.GetString("url");
            var uri = Android.Net.Uri.Parse(url);
            videoView.SetVideoURI(uri);
            videoView.Start();
            videoView.SetOnPreparedListener(this);
            progDailog.Show();
		}

		public void OnPrepared (Android.Media.MediaPlayer mp)
		{
            mp.Looping = true;
			progDailog.Dismiss();
		}
		//public override void OnBackPressed()
		//{
		//  //  MessagingCenter.Send(App.Instance, "getMedia");       
		//}
		protected override void OnDestroy()
		{
            base.OnDestroy();
            MessagingCenter.Send(App.Instance, "getMedia");
            MessagingCenter.Send(App.Instance, "getPhotoAlbumMedia");
            MessagingCenter.Send(App.Instance, "getPhotoEventGalleryMedia");


		}
		protected override void OnPause()
		{
            base.OnPause();
		}
	}
}

