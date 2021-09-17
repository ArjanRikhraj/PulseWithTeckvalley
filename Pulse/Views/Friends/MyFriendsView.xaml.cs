using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.Connectivity;
using Pulse.Pages.Friends;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PancakeView;

namespace Pulse
{
	public partial class MyFriendsView : ContentView
	{
		readonly EventViewModel eventViewModel;
		List<MyEventResponse> list = new List<MyEventResponse>();
		int _tapCount = 0;
		readonly FriendsViewModel friendsViewModel;
		MainServices mainService;
		public MyFriendsView()
		{
			InitializeComponent();
			eventViewModel = ServiceContainer.Resolve<EventViewModel>();
			friendsViewModel = ServiceContainer.Resolve<FriendsViewModel>();
			BindingContext = friendsViewModel;
			mainService = new MainServices();
			friendsViewModel.tempFriendList.Clear();
			friendsViewModel.pageNoFriend = 1;
			friendsViewModel.totalPagesMyFriends = 1;
			SetInitialValues();
			friendsViewModel.GetEventList();
			friendsViewModel.PendingRequestCount();
			friendsViewModel.GetMyFriendsList();
		}
	
		async void SeeAllTapped(object sender, System.EventArgs e)
		{
			await Navigation.PushAsync(new MyEventsPage());
		}
	
	

		void SetInitialValues()
		{
			if (Device.RuntimePlatform == Device.Android)
			{
				stackPendingRequest.Padding = new Thickness(0, 13, 0, 13);
				topStack.Padding = new Thickness(0, 10, 0, 10);
				topStack.Margin = new Thickness(13, 0, 13, 0);
			}
			if (Device.RuntimePlatform == Device.iOS)
			{

			}
		}


		async void RightArrowIcon_Tapped(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					App.ShowMainPageLoader();
					await Navigation.PushModalAsync(new PendingFriendRequestPage());
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
		async void Contact_Tapped(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				var status = await Permissions.CheckStatusAsync<Permissions.ContactsRead>();
				if (status != PermissionStatus.Granted)
				{
					await Permissions.RequestAsync<Permissions.ContactsRead>();
					return;
				}
				await Navigation.PushModalAsync(new ContactsPage());
			}
			else
			{
				await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
			}
		}
		
		async void MyEventslst_SelectionChanged(System.Object sender, Xamarin.Forms.SelectionChangedEventArgs e)
		{
			var selecteditem = ((CollectionView)sender).SelectedItem as MyEventResponse;
			if (selecteditem == null)
				return;
			await eventViewModel.FetchEventDetail(selecteditem.id.ToString(),true);
			((CollectionView)sender).SelectedItem = null;
		}
		async void Friend_SelectionChanged(System.Object sender, Xamarin.Forms.SelectionChangedEventArgs e)
		{
			var selecteditem = ((CollectionView)sender).SelectedItem as Friend;
			if (selecteditem == null)
				return;
			await friendsViewModel.CollectionViewSelectedFriend(selecteditem);
			((CollectionView)sender).SelectedItem = null;
		}
		
	}
}
