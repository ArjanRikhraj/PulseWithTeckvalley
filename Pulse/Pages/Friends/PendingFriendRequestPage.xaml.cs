using System;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Pulse
{
	public partial class PendingFriendRequestPage : BaseContentPage
	{
		#region Private Variables
		int _tapCount = 0;
		readonly FriendsViewModel friendsViewModel;
		#endregion
		#region Constructor
		public PendingFriendRequestPage()
		{
			InitializeComponent();
			friendsViewModel = ServiceContainer.Resolve<FriendsViewModel>();
			BindingContext = friendsViewModel;
			SetInitialValues();
			friendsViewModel.pageNoPending = 1;
			friendsViewModel.totalPagesPendingRequest = 1;
			friendsViewModel.tempPendingList.Clear();
			friendsViewModel.GetPendingFriendsList();

		}
		#endregion
		#region methods
		void SetInitialValues()
		{
			if (Device.RuntimePlatform == Device.Android)
			{
				topStack.Margin = new Thickness(10, 10, 10, 10);
			}
		}
		async void lstFriendRequestTapped(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					friendsViewModel.IsLoading = true;
					var selected = (Friend)e.SelectedItem;
					friendsViewModel.TappedFriendid = Convert.ToString(selected.friendId);
					listViewfriendrequests.SelectedItem = null;
					await Navigation.PushModalAsync(new FriendsProfilePage("Pending", friendsViewModel.TappedFriendid));
					friendsViewModel.IsLoading = false;
					_tapCount = 0;
				}
			}
			else
			{
				await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
				_tapCount = 0;
			}
		}
		async void BackIcon_Tapped(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					friendsViewModel.IsLoading = true;
					friendsViewModel.PendingRequestCount();
					friendsViewModel.tempFriendList.Clear();
					friendsViewModel.pageNoFriend = 1;
					friendsViewModel.totalPagesMyFriends = 1;
					friendsViewModel.GetMyFriendsList();
					await Navigation.PopModalAsync();
					friendsViewModel.IsLoading = false;
					_tapCount = 0;
				}
			}
			else
			{
				await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
				_tapCount = 0;
			}
		}

		async void Button_Clicked(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					var button = sender as ExtendedButton;
					await friendsViewModel.ChangeRequestStatus(Convert.ToInt32(button.ClassId), button.friendType, Constant.PendingText, false);
					_tapCount = 0;
				}
			}
			else
			{
				await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
				_tapCount = 0;
			}
		}


		#endregion
	}
}
