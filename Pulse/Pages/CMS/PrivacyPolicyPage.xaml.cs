using System;
using Plugin.Connectivity;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace Pulse
{
	public partial class PrivacyPolicyPage : BaseContentPage
	{
		public PrivacyPolicyPage()
		{
			InitializeComponent();
			try
			{
				IsLoading = true;
				//Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
				PrivacyPolicyView.Source = Constant.PrivacyPolicyUrl + "?access_token=" + SessionManager.AccessToken;
			}
			catch (Exception)
			{
				return;
			}
			BindingContext = this;
		}

		#region Private Methods       

		protected override void OnAppearing()
		{
			base.OnAppearing();
            if (!CrossConnectivity.Current.IsConnected)
			{
				App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
			}
		}
		void WebOnEndNavigating(object sender, WebNavigatedEventArgs e)
		{
			IsLoading = false;
		}
		#endregion
	}
}
