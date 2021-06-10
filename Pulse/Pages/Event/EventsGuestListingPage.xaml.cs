using System;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Pulse
{
	public partial class EventsGuestListingPage : BaseContentPage
	{
		GuestType CurrentActiveGuestType;
		readonly EventViewModel eventViewModel;
		int _tapCount = 0;
		public EventsGuestListingPage()
		{
			InitializeComponent();
			eventViewModel = ServiceContainer.Resolve<EventViewModel>();
			BindingContext = eventViewModel;
			SetInitialValues();
		}

		void SetInitialValues()
		{
			if (Device.RuntimePlatform == Device.Android)
			{
				topStack.Margin = new Thickness(10, 10, 10, 10);
			}
			eventViewModel.IsNoGuestFoundVisible = false;
			eventViewModel.IsListGuestVisible = false;
			TabPageGesture(lblAttending, AttendingBox);
			ClearFields(GuestType.Attending);

		}
		async void Cross_Clicked(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					eventViewModel.IsLoading = true;
					await Navigation.PopModalAsync();
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
		void TabPageGesture(Label lbl, BoxView eventsBox)
		{
			Image img = new Image();
			lblAttending.TextColor = Color.FromHex(Constant.GrayTextColor);
			lblInterested.TextColor = Color.FromHex(Constant.GrayTextColor);
			lblNotInterested.TextColor = Color.FromHex(Constant.GrayTextColor);
			lblInvited.TextColor = Color.FromHex(Constant.GrayTextColor);
			lbl.TextColor = Color.FromHex(Constant.PinkButtonColor);
			AttendingBox.BackgroundColor = Color.Transparent;
			InterestedBox.BackgroundColor = Color.Transparent;
			InvitedBox.BackgroundColor = Color.Transparent;
			eventsBox.BackgroundColor = Color.FromHex(Constant.PinkButtonColor);
		}
		void MenuItemTapped(object sender, EventArgs e)
		{
			try
			{
				if (!CrossConnectivity.Current.IsConnected)
				{
					App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
				}
				else
				{
					var button = sender as ExtendedStackLayout;
					if (button != null)
					{
						switch (button.EventGuestType)
						{
							case GuestType.Invited:

								if (button.EventGuestType != CurrentActiveGuestType)
								{
									TabPageGesture(lblInvited, InvitedBox);
									ClearFields(button.EventGuestType);
								}
								break;
							case GuestType.Interested:
								if (button.EventGuestType != CurrentActiveGuestType)
								{
									TabPageGesture(lblInterested, InterestedBox);
									ClearFields(button.EventGuestType);
								}
								break;
							case GuestType.NotInterested:
								if (button.EventGuestType != CurrentActiveGuestType)
								{
									//TabPageGesture(lblNotInterested, NotInterestedBox);
									ClearFields(button.EventGuestType);
								}
								break;
							case GuestType.CheckedIn:
								if (button.EventGuestType != CurrentActiveGuestType)
								{
									//TabPageGesture(lblNotInterested, NotInterestedBox);
									ClearFields(button.EventGuestType);
								}
								break;
							default:
								if (button.EventGuestType != CurrentActiveGuestType)
								{
									TabPageGesture(lblAttending, AttendingBox);
									ClearFields(button.EventGuestType);
								}
								break;
						}
					}
				}
			}
			catch (Exception)
			{
			}
		}

		void ClearFields(GuestType eventGuestType)
		{
			eventViewModel.tempGuestList.Clear();
			eventViewModel.pageNoGuests = 1;
			eventViewModel.totalGuestPages = 1;
			CurrentActiveGuestType = eventViewModel.currentActiveGuestType = eventGuestType;
			eventViewModel.GetGuests();
		}

		void lstGuestTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
		{
			listViewGuests.SelectedItem = null;
		}
		async void Friend_Button_Clicked(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					var button = sender as ExtendedButton;
					switch (button.friendType)
					{
						case FriendType.AddFriend:
							await eventViewModel.AddFriend(Convert.ToInt32(button.ClassId));
							break;
						default:
							await eventViewModel.ChangeRequestStatus(Convert.ToInt32(button.ClassId), button.friendType, Constant.SearchText);
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
	}
}
