using Android.App;
using Firebase.Iid;

namespace Pulse.Droid
{
	[Service]
	[IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
	public class FirebaseIdService : FirebaseInstanceIdService
	{
		public override void OnTokenRefresh()
		{
			var refreshedToken = FirebaseInstanceId.Instance.Token;
			if (!string.IsNullOrEmpty(refreshedToken))
			{
				var r = refreshedToken;
				Settings.AppSettings.AddOrUpdateValue(Constant.FcmToken, r);
			}
		}

	}
}

