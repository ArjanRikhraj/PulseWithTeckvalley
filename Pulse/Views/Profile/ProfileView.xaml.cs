
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Pulse
{
	public partial class ProfileView : ContentView
	{
		int _tapCount = 0;
		readonly ProfileViewModel profileViewModel;

		public ProfileView()
		{
			InitializeComponent();
			profileViewModel = ServiceContainer.Resolve<ProfileViewModel>();
			BindingContext = profileViewModel;
			SetInitialValues();
            profileViewModel.ProfileIcon = Constant.ProfileIcon;
			profileViewModel.GetMyProfileDetail();
            App.HideMainPageLoader();
		}

		void SetInitialValues()
		{
			if (Device.RuntimePlatform == Device.Android)
			{
				topStack.Margin = new Thickness(10, 10, 10, 10);
			}
            //if (Device.RuntimePlatform == Device.iOS)
            //{
            //    gradient
            //}
		}

		async void EditTapped(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					App.ShowMainPageLoader();
					await Navigation.PushModalAsync(new EditProfilePage());
					App.HideMainPageLoader();
					_tapCount = 0;
				}
			}
			else
			{
				await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
				_tapCount = 0;
			}

		}
		async void Handle_Clicked(object sender, System.EventArgs e)
		{
			if (_tapCount < 1)
			{
				_tapCount = 1;
				await Navigation.PushModalAsync(new SettingsPage(profileViewModel.isChangePasswordShown));
				_tapCount = 0;
			}
		}

		async void MyEventsTapped(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					App.ShowMainPageLoader();
					await Navigation.PushModalAsync(new MyEventsPage());
					App.HideMainPageLoader();
					_tapCount = 0;
				}
			}
			else
			{
				await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
				_tapCount = 0;
			}
		}

		async void SettingsTapped(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					App.ShowMainPageLoader();
					await Navigation.PushModalAsync(new SettingsPage(profileViewModel.isChangePasswordShown));
					App.HideMainPageLoader();
					_tapCount = 0;
				}
			}
			else
			{
				await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
				_tapCount = 0;
			}
		}

		async void PhotoAlbum_Tapped(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					App.ShowMainPageLoader();
					await Navigation.PushModalAsync(new PhotoAlbumPage());
					App.HideMainPageLoader();
					_tapCount = 0;
				}
			}
			else
			{
				await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
				_tapCount = 0;
			}
		}
	}
}
