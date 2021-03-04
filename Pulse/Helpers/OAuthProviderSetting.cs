using System;
using Xamarin.Auth;

namespace Pulse
{
	public class OAuthProviderSetting
	{
		public string ConsumerKey { get; private set; }
		public string ConsumerSecret { get; private set; }
		public string RequestTokenUrl { get; private set; }
		public string AccessTokenUrl { get; private set; }
		public string AuthorizeUrl { get; private set; }
		public string CallbackUrl { get; private set; }

		public enum OauthIdentityProvider
		{
			GOOGLE,
			FACEBOOK,
			TWITTER,
			LINKEDIN,
		}

		public OAuth2Authenticator LoginWithProvider(string Provider)
		{
			OAuth2Authenticator auth = null;
			switch (Provider)
			{
				
				case Constant.FacebookText:
					{
						try
						{
							auth = new OAuth2Authenticator(
                                clientId: Constant.FacebookAppID,  // For Facebook login, for configure refer http://www.c-sharpcorner.com/article/register-identity-provider-for-new-oauth-application/
								 scope: "email",
								authorizeUrl: new Uri(Constant.FacebookAuthorizeUrl),
								redirectUrl: new Uri(Constant.FacebookRedirectUrl));
						}
						catch
						{
							App.Instance.Alert(Constant.AlertTitleDefaultText, Constant.AuthenticationFailedText, Constant.Done);
						}
						break;
					}

				

				default:
					break;
			}
			return auth;
		}
	}
}
