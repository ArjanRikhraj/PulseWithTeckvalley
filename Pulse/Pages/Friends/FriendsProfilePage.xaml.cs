using System;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Pulse
{
	public partial class FriendsProfilePage : BaseContentPage
	{
		#region variables
		int _tapCount = 0;
		readonly FriendsViewModel friendsViewModel;
		readonly EventViewModel eventViewModel;
		static FriendType friendType;
		string pageType;
		#endregion
		#region Constructor
		public FriendsProfilePage(string page, string id)
		{
			InitializeComponent();
			friendsViewModel = ServiceContainer.Resolve<FriendsViewModel>();
			eventViewModel = ServiceContainer.Resolve<EventViewModel>();
			BindingContext = friendsViewModel;
			friendsViewModel.TappedFriendid = id;
			pageType = page;
			SetInitial();

		}
		#endregion
		#region Methods
		async void SetInitial()
		{
			bool isDetail = await friendsViewModel.FriendProfileDetail();
			if (Device.RuntimePlatform == Device.Android)
			{
				//topStack.Margin = new Thickness(10, 10, 10, 10);
			}

			if (isDetail && friendsViewModel.FriendProfileList != null && friendsViewModel.FriendProfileList.Count > 0)
			{
				imgUser.Source = !string.IsNullOrEmpty(friendsViewModel.FriendProfileList[0].profile_image) ? PageHelper.GetUserImage(friendsViewModel.FriendProfileList[0].profile_image) : Constant.ProfileIcon;
				lblUsername.Text = lblUser.Text = friendsViewModel.FriendProfileList[0].username;
				lblFullname.Text = lblFullName.Text = friendsViewModel.FriendProfileList[0].fullname;
				//lblSchool.Text = friendsViewModel.FriendProfileList[0].school;
				lblScore.Text = Convert.ToString(friendsViewModel.FriendProfileList[0].scores);
				lblHosted.Text = Convert.ToString(friendsViewModel.FriendProfileList[0].hosted_events);
				lblAttended.Text = Convert.ToString(friendsViewModel.FriendProfileList[0].attended_events) ;
				lblFriends.Text = Convert.ToString(friendsViewModel.FriendProfileList[0].friends);
			}
			if (friendsViewModel.IsFriendsButtonVisible)
			{
				friendsViewModel.pageNoFriendsEvent = 1;
				friendsViewModel.totalHostedEventPages = 1;
				friendsViewModel.tempFriendEventList.Clear();
				friendsViewModel.GetFriendsHostedEventList();
			}

		}
        async void View_AllTapped(object sender, System.EventArgs e)

        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (_tapCount < 1)
                {
                    _tapCount = 1;
                    eventViewModel.IsLoading = true;
                    var selected = sender as ExtendedLabel;
                    eventViewModel.TappedEventId = Convert.ToInt32(selected.ClassId);
                    eventViewModel.FetchEventDetail(Convert.ToString(eventViewModel.TappedEventId), false);
                    await Navigation.PushModalAsync(new EventsGuestListingPage());
                    eventViewModel.IsLoading = false;
                    _tapCount = 0;
                }
            }
            else
            {
                await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                _tapCount = 0;
            }
        }
        async void View_AllGridTapped(object sender, System.EventArgs e)

        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (_tapCount < 1)
                {
                    _tapCount = 1;
                    eventViewModel.IsLoading = true;
                    var selected = sender as Grid;
                    eventViewModel.FetchEventDetail(Convert.ToString(eventViewModel.TappedEventId), false);
                    await Navigation.PushModalAsync(new EventsGuestListingPage());
                    eventViewModel.IsLoading = false;
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
							await friendsViewModel.AddFriend(Convert.ToInt32(friendsViewModel.TappedFriendid), true);
							break;
						case FriendType.Friends:
							grdOverlayDialog.IsVisible = true;
							stackPopUp.IsVisible = true;
							break;
						default:
							await friendsViewModel.ChangeRequestStatus(Convert.ToInt32(friendsViewModel.TappedFriendid), friendType, Constant.SearchText, true);
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

		void Cancel_Clicked(object sender, System.EventArgs e)
		{
			grdOverlayDialog.IsVisible = false;
			stackPopUp.IsVisible = false;
		}
        
       async void Block_Unblock_Clicked(object sender, System.EventArgs e)
        {

            if (CrossConnectivity.Current.IsConnected)
            {
                
                    if (friendsViewModel.BlockUnBlockText.Equals(Constant.BlockText))
                    {
                        var result = await App.Instance.ConfirmAlert("Are you sure you want to block " + friendsViewModel.SelectedUsername, Constant.AlertTitle, Constant.Ok, Constant.CancelText);
                        if (result)
                        {
                            await friendsViewModel.BlockUnblockFreind();
                        }
                    }
                    else
                    {
                        var result = await App.Instance.ConfirmAlert("Are you sure you want to unblock " + friendsViewModel.SelectedUsername, Constant.AlertTitle, Constant.Ok, Constant.CancelText);
                        if (result)
                        {
                            await friendsViewModel.BlockUnblockFreind();
                        }
                    }
                }

        }
	    async void FriendEventsList_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					friendsViewModel.IsLoading = true;
					var selected = (MyEvents)e.Item;
					listViewfriendsEvents.SelectedItem = null;
                    await eventViewModel.FetchEventDetail(Convert.ToString(selected.EventId), true);
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

		async void Unfriend_Tapped(object sender, EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					grdOverlayDialog.IsVisible = false;
					stackPopUp.IsVisible = false;
					await friendsViewModel.ChangeRequestStatus(Convert.ToInt32(friendsViewModel.TappedFriendid), FriendType.Friends, Constant.SearchText, true);
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
					if (pageType.Equals("My Friends"))
					{
						friendsViewModel.PendingRequestCount();
						friendsViewModel.pageNoUser = 1;
						friendsViewModel.tempUserList.Clear();
						friendsViewModel.tempFriendList.Clear();
						friendsViewModel.pageNoFriend = 1;
						friendsViewModel.totalPagesMyFriends = 1;
						friendsViewModel.GetMyFriendsList();
					}
					else if (pageType.Equals("Pending"))
					{
						friendsViewModel.pageNoPending = 1;
						friendsViewModel.tempPendingList.Clear();
						friendsViewModel.GetPendingFriendsList();
					}
					else
					{
						friendsViewModel.tempUserList.Clear();
						friendsViewModel.pageNoUser = 1;
						friendsViewModel.GetUsers();
					}
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
        #endregion

        private void btnEdit_Clicked(object sender, EventArgs e)
        {
			stackPopUp.IsVisible = true;
			grdOverlayDialog.IsVisible = true;
		}
    }
}
