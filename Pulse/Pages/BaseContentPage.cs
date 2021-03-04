using System;
using Plugin.Toasts;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace Pulse
{
	public class BaseContentPage : ContentPage
	{
		bool isLoading;
		public BaseContentPage()
		{
			PageInitialization();
		}

		private void PageInitialization()
		{
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            var safeinsects = On<Xamarin.Forms.PlatformConfiguration.iOS>().SafeAreaInsets();
            if(safeinsects.Bottom > 0)
            {
                this.Padding = new Thickness(0, 0, 0, 20);
            }
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(false);

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
				OnPropertyChanged(Constant.IsLoading);
			}
		}

		public async void ShowToast(string title, string description)
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

	}
}

