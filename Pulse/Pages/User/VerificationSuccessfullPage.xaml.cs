using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace Pulse
{
	public partial class VerificationSuccessfullPage : BaseContentPage
	{
		public VerificationSuccessfullPage(string pageType)
		{
			InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(false);
			labelAccount.IsVisible = false;
			RunFor(2, pageType);
		}
		async void RunFor(int v, string pageType)
		{
			await Task.Delay(TimeSpan.FromSeconds(v));
			if (pageType.Equals(Constant.SignUpText))
				await Navigation.PushModalAsync(new ProfilePage());
			else
				await Navigation.PushModalAsync(new ResetPasswordPage());

		}
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
	}
}
