
using Plugin.Connectivity;
using Xamarin.Forms;
using static Pulse.ProfileViewModel;

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
            App.HideMainPageLoader();
			profileViewModel.GetMyProfileDetail();
		}

		void SetInitialValues()
		{
			if (Device.RuntimePlatform == Device.Android)
			{
				topStack.Margin = new Thickness(10, 10, 10, 10);
			}
        
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
		
       async  void profileCollectionView_SelectionChanged(System.Object sender, Xamarin.Forms.SelectionChangedEventArgs e)
        {

			var selecteditem = ((CollectionView)sender).SelectedItem as MenuList;
			if (selecteditem == null)
				return;
			if(selecteditem.ID ==1)
            {
				//::Note Do not remove
				Common.Constants.AppConstants.PopNavigationFromProfileEnabled = true;
				await Navigation.PushModalAsync(new MyEventsPage());
			}
			else if (selecteditem.ID == 2)
			{
				await Navigation.PushModalAsync(new MyFriendsView());
			}
			else if(selecteditem.ID ==3)
            {
				await Navigation.PushModalAsync(new PhotoAlbumPage());
			}
			else if(selecteditem.ID ==4)
            {
				await Navigation.PushModalAsync(new SettingsPage(profileViewModel.isChangePasswordShown));

			}
			((CollectionView)sender).SelectedItem = null;

		}
    }
}
