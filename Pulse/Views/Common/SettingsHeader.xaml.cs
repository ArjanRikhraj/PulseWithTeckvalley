using System;
using System.Collections.Generic;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Pulse
{
	public partial class SettingsHeader : ContentView
	{
		readonly BaseViewModel baseviewModelObj;
		int tapcount = 0;
		public SettingsHeader()
		{
			InitializeComponent();
			baseviewModelObj = new BaseViewModel();
			if (Device.RuntimePlatform == Device.Android)
			{
				topStack.Margin = new Thickness(15, 15, 15, 15);
			}
            if (Device.RuntimePlatform == Device.iOS)
            {
                
            }
		}
		public string HeadingText
		{
			get { return lblHeader.Text; }
			set { lblHeader.Text = value; }
		}

		public bool LogoutVisible
		{
			get { return btnLogout.IsVisible; }
			set { btnLogout.IsVisible = value; }
		}

		private async void BackTapped(object sender, EventArgs e)
		{
			//if (CrossConnectivity.Current.IsConnected)
			//{
			
		      await Navigation.PopModalAsync();
				
			//}
		}
        public bool IsBackButtonVisible
        {
            set { btnBackArrow.IsVisible = value; }
            get { return btnBackArrow.IsVisible; }
        }
		private async void HandleSignout(object sender, EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (tapcount < 1)
				{
					tapcount++;
					SignOut();
				}
				else
				{
					tapcount = 0;
				}
			}
			else
			{
				await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
			}
		}

		public async void SignOut()
		{
			if (!CrossConnectivity.Current.IsConnected)
			{
				await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
				tapcount = 0;
			}
			else
			{
				try
				{
					if (!string.IsNullOrEmpty(SessionManager.AccessToken))
					{
						baseviewModelObj.IsLoading = true;
						await new MainServices().Put<ResultWrapperSingle<SendEmailOTPResponse>>(Constant.SignOutUrl, null);
						await App.Instance.Alert("User has been logged out successfully.", Constant.AlertTitle, Constant.Ok);
						Settings.AppSettings.AddOrUpdateValue("SavedPulseListing", string.Empty);
						SessionManager.AccessToken = string.Empty;
						SessionManager.Email = string.Empty;
						Settings.AppSettings.AddOrUpdateValue("CustomUserAccessToken", string.Empty);
						Application.Current.MainPage = new NavigationPage(new LoginPage());
						baseviewModelObj.IsLoading = false;
						tapcount = 0;
					}
					tapcount = 0;

				}
				catch (Exception)
				{
					baseviewModelObj.IsLoading = false;
					tapcount = 0;

				}
			}
		}
	}
}
