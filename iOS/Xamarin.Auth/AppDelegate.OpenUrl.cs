using System;
using Foundation;
using UIKit;

namespace Pulse.iOS
{
	public partial class AppDelegate
	{
		public override bool OpenUrl
				(
					UIApplication application,
					NSUrl url,
					string sourceApplication,
					NSObject annotation
				)
		{
#if DEBUG
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.AppendLine("OpenURL Called");
			sb.Append("     url         = ").AppendLine(url.AbsoluteUrl.ToString());
			sb.Append("     application = ").AppendLine(sourceApplication);
			sb.Append("     annotation  = ").AppendLine(annotation?.ToString());
			System.Diagnostics.Debug.WriteLine(sb.ToString());
#endif

			// Convert iOS NSUrl to C#/netxf/BCL System.Uri - common API
			Uri uri_netfx = new Uri(url.AbsoluteString);

			// load redirect_url Page
			if (AuthenticationState.Authenticator != null)
				AuthenticationState.Authenticator.OnPageLoading(uri_netfx);

			//if (url.AbsoluteString.Contains("pulse://Event"))
			//{
			//	App myApp = App.Current as App;
			//	if (null != myApp && null != url)
			//	{
			//		myApp.AppLinkRequestReceived(url);
			//	}
			//}

			return true;
		}
	}
}

