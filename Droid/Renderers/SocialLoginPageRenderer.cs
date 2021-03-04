using System;
using Android.App;
using Android.Content;
using Plugin.Connectivity;
using Pulse;
using Pulse.Droid;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(SocialLoginPage), typeof(SocialLoginPageRenderer))]
namespace Pulse.Droid
{
    public class SocialLoginPageRenderer : PageRenderer
    {
        bool showLogin = true;
        AuthenticationViewModel authenticationViewModel;

        public SocialLoginPageRenderer(Context context) : base(context)
        {
        }

        protected async override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);
            authenticationViewModel = ServiceContainer.Resolve<AuthenticationViewModel>();
            //Get and Assign ProviderName from ProviderLoginPage
            var loginPage = Element as SocialLoginPage;
            string providername = loginPage.ProviderName;
            var activity = this.Context as Activity;
            OAuthProviderSetting oauth = new OAuthProviderSetting();

            if (CrossConnectivity.Current.IsConnected)
            {


                var auth = oauth.LoginWithProvider(providername);
                // After facebook, google and all identity provider login completed 
                auth.Completed += async (sender, eventArgs) =>
                {

                    if (eventArgs.IsAuthenticated)
                    {
                        try
                        {
                            //var fbRequest = new OAuth2Request("GET", new Uri(Constant.FacebookProfileUrl), null, eventArgs.Account);
                            //var profileResponse = await fbRequest.GetResponseAsync();
                            //if (profileResponse != null && profileResponse.StatusCode == System.Net.HttpStatusCode.OK)
                            //{
                            //SocialUserDetails userProfile = JsonManager.DeSerialize<SocialUserDetails>(await profileResponse.GetResponseTextAsync());
                            await authenticationViewModel.GetFacebookProfile(eventArgs.Account);

                            //}
                        }
                        catch (Exception ex)
                        {
                            authenticationViewModel.AlertAction.Invoke();
                        }

                        authenticationViewModel.SuccessfullLoginAction.Invoke();

                    }
                    else
                    {
                        authenticationViewModel.AlertAction.Invoke();
                    }
                };
                activity.StartActivity(auth.GetUI(activity));
            }
            else
            {
            }
        }
    }
}
