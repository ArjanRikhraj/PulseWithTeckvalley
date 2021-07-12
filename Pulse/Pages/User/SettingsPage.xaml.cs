using System;
using System.Collections.Generic;
using Plugin.Connectivity;
using System.ComponentModel;
using Xamarin.Forms;
using System.Windows.Input;

namespace Pulse
{
	public partial class SettingsPage : BaseContentPage
	{
		#region
		new event PropertyChangedEventHandler PropertyChanged;
		#endregion
		#region
		public ICommand TapCommand { set; get; }
		private bool isLoading;
		public bool IsLoading
		{
			get
			{
				return isLoading;
			}
			set
			{
				isLoading = value;
				this.OnPropertyChanged("IsLoading");
			}
		}
		#endregion
		public SettingsPage(bool isChangePasswordShown)
		{
			InitializeComponent();
			SetDefaultValues();
			BindingContext = this;
			stkChangePassword.IsVisible = isChangePasswordShown;
			boxChangePassword.IsVisible = isChangePasswordShown;
		}
		void SetDefaultValues()
		{
			this.TapCommand = new Command<string>(OnTapped);
		}

		#region Events

		async void OnTapped(string settingPage)
		{
			if (this.IsEnabled)
			{
				this.IsEnabled = false;
				switch (settingPage.ToLower())
				{
					case Constant.ChangePasswordEvent:
						IsLoading = true;
						await Navigation.PushModalAsync(new ChangePasswordPage());
						IsLoading = false;
						break;
					case Constant.AboutPulseEvent:
						IsLoading = true;
						await Navigation.PushModalAsync(new AboutPulsePage());
						IsLoading = false;
						break;
					case Constant.PrivacyPolicyEvent:
						IsLoading = true;
						await Navigation.PushModalAsync(new PrivacyPolicyPage());
						IsLoading = false;

						break;
					case Constant.FaqsEvent:
						IsLoading = true;
						await Navigation.PushModalAsync(new FAQPage());
						IsLoading = false;

						break;
					case Constant.ContactUsEvent:
						IsLoading = true;
						await Navigation.PushModalAsync(new ContactUsPage());
						IsLoading = false;
						break;
					default:
						break;
				}
				this.IsEnabled = true;

			}

		}
		public async void SignOut()
		{
			if (!CrossConnectivity.Current.IsConnected)
			{
				await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
			}
			else
			{
				try
				{
					if (!string.IsNullOrEmpty(SessionManager.AccessToken))
					{
						IsLoading = true;
						await new MainServices().Put<ResultWrapperSingle<SendEmailOTPResponse>>(Constant.SignOutUrl, null);
						await App.Instance.Alert("User has been logged out successfully.", Constant.AlertTitle, Constant.Ok);
						Settings.AppSettings.AddOrUpdateValue("SavedPulseListing", string.Empty);
						SessionManager.AccessToken = string.Empty;
						SessionManager.Email = string.Empty;
						Settings.AppSettings.AddOrUpdateValue("CustomUserAccessToken", string.Empty);
						Application.Current.MainPage = new NavigationPage(new LoginPage());
						IsLoading = false;
					}

				}
				catch (Exception)
				{
					IsLoading = false;
				}
			}
		}
		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

		}
        #endregion

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
			SignOut();
		}
    }
}
