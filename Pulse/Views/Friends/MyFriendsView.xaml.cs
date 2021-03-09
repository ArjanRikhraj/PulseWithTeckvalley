using System;
using System.Collections.Generic;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Pulse
{
	public partial class MyFriendsView : ContentView
	{
		int _tapCount = 0;
		readonly FriendsViewModel friendsViewModel;
		public MyFriendsView()
		{
			InitializeComponent();
			friendsViewModel = ServiceContainer.Resolve<FriendsViewModel>();
			BindingContext = friendsViewModel;
			SetInitialValues();
			friendsViewModel.PendingRequestCount();
			friendsViewModel.tempFriendList.Clear();
			friendsViewModel.pageNoFriend = 1;
			friendsViewModel.totalPagesMyFriends = 1;
			friendsViewModel.GetMyFriendsList();
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


		async void SearchIcon_Tapped(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					App.ShowMainPageLoader();
					await Navigation.PushModalAsync(new SearchFriendPage());
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

        async void lstFriendTapped(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					App.ShowMainPageLoader();
    					var selected = (Friend)e.SelectedItem;
                       friendsViewModel.TappedFriendid = Convert.ToString(selected.friendId);
                        listViewfriends.SelectedItem = null;
                        await Navigation.PushModalAsync(new FriendsProfilePage("My Friends", friendsViewModel.TappedFriendid));
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
