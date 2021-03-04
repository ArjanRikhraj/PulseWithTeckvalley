using System;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Pulse
{
	public partial class SearchFriendPage : BaseContentPage
	{
		#region Private Variables
		int _tapCount = 0;
		readonly FriendsViewModel friendsViewModel;
		static FriendType friendType;
		#endregion
		#region Constructor
		public SearchFriendPage()
		{
			InitializeComponent();
			friendsViewModel = ServiceContainer.Resolve<FriendsViewModel>();
			BindingContext = friendsViewModel;
			SetInitialValues();
		}
		#endregion
		#region Methods
		void SetInitialValues()
		{
			if (Device.RuntimePlatform == Device.Android)
			{
				topStack.Margin = new Thickness(15, 15, 15, 15);
				searchFrame.CornerRadius = 2;
				entryUser.Margin = new Thickness(0, 8, 0, 0);
			}
			friendsViewModel.IsNoUserFoundVisible = false;
			friendsViewModel.IsListUserVisible = false;
			friendsViewModel.totalPagesFriends = 1;
		}
		void SearchEntry_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
		{
			try
			{
                if (e.NewTextValue == string.Empty || string.IsNullOrEmpty(entryUser.Text))
				{
					friendsViewModel.IsNoUserFoundVisible = false;
                    friendsViewModel.searchvalue = string.Empty;
					friendsViewModel.IsListUserVisible = false;
                    friendsViewModel.pageNoUser = 0;
				}
				else
				{
					if (entryUser.Text.Length >= 1 && !friendsViewModel.isSearchedValue)
					{
						friendsViewModel.isSearchedValue = true;
						friendsViewModel.tempUserList.Clear();
						friendsViewModel.IsListUserVisible = true;
						friendsViewModel.IsNoUserFoundVisible = false;
						friendsViewModel.searchvalue = entryUser.Text;
						friendsViewModel.totalPagesFriends = 1;
						friendsViewModel.pageNoUser = 1;
						friendsViewModel.GetUsers();
					}
					else
					{
						friendsViewModel.IsNoUserFoundVisible = false;
						friendsViewModel.IsListUserVisible = false;
                       
					}
				}
			}
			catch (Exception)
			{
				friendsViewModel.IsLoading = false;
			}
		}

		async void lstSearchFriendTapped(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					friendsViewModel.IsLoading = true;
					var selected = (Friend)e.SelectedItem;
					friendsViewModel.TappedFriendid = Convert.ToString(selected.friendId);
					listViewfriends.SelectedItem = null;
					await Navigation.PushModalAsync(new FriendsProfilePage("Search Friends", friendsViewModel.TappedFriendid));
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

		void CrossIcon_Tapped(object sender, System.EventArgs e)
		{
			entryUser.Text = string.Empty;
		}

		async void Cancel_Tapped(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					friendsViewModel.IsLoading = true;
					friendsViewModel.PendingRequestCount();
					friendsViewModel.pageNoUser = 1;
					friendsViewModel.tempUserList.Clear();
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

		async void Friend_Button_Clicked(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					var button = sender as ExtendedButton;
					friendType = button.friendType;
					switch (friendType)
					{
						case FriendType.AddFriend:
							await friendsViewModel.AddFriend(Convert.ToInt32(button.ClassId), false);
							break;
						default:
							await friendsViewModel.ChangeRequestStatus(Convert.ToInt32(button.ClassId), friendType, Constant.SearchText, false);
							break;
					}
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
