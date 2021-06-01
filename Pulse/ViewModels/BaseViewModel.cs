using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Plugin.Connectivity;
using Plugin.Toasts;
using Xamarin.Forms;

namespace Pulse
{
	public class BaseViewModel : INotifyPropertyChanged
	{
		#region PrivateMembers
		private bool isEnabled = true;
		private bool isLoading;
		#endregion
		public BaseViewModel()
		{
		}

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

		public bool IsEnabled
		{
			get
			{
				return isEnabled;
			}
			set
			{
				isEnabled = value;
				this.OnPropertyChanged("IsEnabled");
			}
		}
		public int TapCount { get; set; }

		internal virtual Task Initialize(params object[] args)
		{
			return Task.FromResult(0);
		}

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged == null) return;
			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		protected void SetObservableProperty<T>(
			ref T field,
			T value,
			[CallerMemberName] string propertyName = "")
		{
			if (EqualityComparer<T>.Default.Equals(field, value)) return;
			field = value;
			OnPropertyChanged(propertyName);
		}

		#region INotifyPropertyChanged implementation
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion


		public Xamarin.Forms.INavigation Navigation
		{
			get
			{
				var mainPage = Xamarin.Forms.Application.Current.MainPage;
				if (mainPage is Xamarin.Forms.NavigationPage)
				{
					return (Xamarin.Forms.INavigation)mainPage.Navigation;
				}
				return Xamarin.Forms.Application.Current.MainPage.Navigation;
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
						await App.Instance.Alert("User has been deactivated, so you are logged out", Constant.AlertTitle, Constant.Ok);
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

		public int GetPageCount(int elementCount)
		{
			var t = Convert.ToDouble(SessionManager.RecordPerPage);
			double d = Convert.ToDouble(elementCount) / t;
			return Convert.ToInt32(Math.Ceiling(d));
		}

		public async void ShowToast(string title, string description)
		{
			try
			{
				if (Device.RuntimePlatform == Device.Android)
				{
					MessagingCenter.Send<string>(description, "ShowToast");
				}
				else
				{

					var notificator = DependencyService.Get<IToastNotificator>();
					var options = new NotificationOptions()
					{
						Title = title,
						Description = description,
						ClearFromHistory = true,
						AllowTapInNotificationCenter = false
					};
					var result = await notificator.Notify(options);
				}
			}
			catch (Exception)
			{
                await App.Instance.Alert(Constant.AlertTitle, Constant.NetworkDisabled, Constant.Ok);
			}
		}
	}
}
