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

		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

		}
		#endregion
	}
}
