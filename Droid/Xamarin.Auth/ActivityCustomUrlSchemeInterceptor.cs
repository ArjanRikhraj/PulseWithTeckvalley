using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace Pulse.Droid
{
    [Activity(Label = "ActivityCustomUrlSchemeInterceptor", NoHistory = true, LaunchMode = LaunchMode.SingleTask)]
	[
		IntentFilter
		(
			actions: new[] { Intent.ActionView },
			Categories = new[]
					{
						Intent.CategoryDefault,
						Intent.CategoryBrowsable
					},
			DataSchemes = new[]
					{
						"com.netsol.nssociallogin.oauth.providers.android",
						"com.googleusercontent.apps.335441009882-51b787bul87rrq09u8ucook1nuejctea",
						"fb"+Constant.FacebookAppID,
						"xamarin-auth",
					},
			DataPaths = new[]
					{
						"/",                        // Facebook
						"/oauth2redirect",          // Google
                        "/oauth2redirectpath"      // MeetUp
			},
			 AutoVerify = true
		)
	]
	public class ActivityCustomUrlSchemeInterceptor : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			global::Android.Net.Uri uri_android = Intent.Data;

#if DEBUG
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.AppendLine("ActivityCustomUrlSchemeInterceptor.OnCreate()");
			sb.Append("     uri_android = ").AppendLine(uri_android.ToString());
			System.Diagnostics.Debug.WriteLine(sb.ToString());
#endif

			// Convert iOS NSUrl to C#/netxf/BCL System.Uri - common API
			Uri uri_netfx = new Uri(uri_android.ToString());

			// load redirect_url Page
			AuthenticationState.Authenticator.OnPageLoading(uri_netfx);

			Finish();

			return;
		}
	}
}
