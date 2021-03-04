using System;
using System.Threading.Tasks;
using Plugin.Connectivity;
using Plugin.Geolocator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;


namespace Pulse
{
	public partial class MainPage : BaseContentPage
	{
		#region variables
		MyFriendsView myFriendsView;
		EventsListView eventsListView;
		PulseView pulseView;
		ProfileView profileView;
        NotificationView notificationView;
		ActivePage activePage;
		readonly PulseViewModel pulseViewModel;
        readonly EventViewModel eventViewModel;
		bool isLoading;
		#endregion
		#region Public properties
		public ActivePage CurrentActivePage
		{
			get
			{
				return activePage;
			}
			set
			{
				
				activePage = value;
			}
		}
		#endregion
		#region Constructor
		public MainPage()
		{
			InitializeComponent();
			grdOverlay.IsVisible = true;
			eventsListView = new EventsListView();
			CurrentActivePage = App.CurrentActivePageName = ActivePage.Event;
			pulseViewModel = ServiceContainer.Resolve<PulseViewModel>();
            eventViewModel = ServiceContainer.Resolve<EventViewModel>();
            BindingContext = eventViewModel;
            eventViewModel.GetUnreadNotificationCount();
            grdInnerView.Children.Add(eventsListView, 0, 0);
            //grdInnerView.Children.Add(pulseView, 0, 0);
			//grdOverlay.IsVisible = false;
		}

		public MainPage(ActivePage page)
		{
			InitializeComponent();
            grdOverlay.IsVisible = true;
			pulseViewModel = ServiceContainer.Resolve<PulseViewModel>();
            eventViewModel = ServiceContainer.Resolve<EventViewModel>();
            BindingContext = eventViewModel;
			if (page == ActivePage.Pulse)
			{
               GetPulseView();
			}
			else if (page == ActivePage.Profile)
			{
				profileView = new ProfileView();
				CurrentActivePage = App.CurrentActivePageName = ActivePage.Profile;
                TabPageGesture(profileView, profileActiveImage, profileInActiveImage, lblProfile, ProfileBox,stackProfile);
			}
			else if (page == ActivePage.Friends)
			{
				myFriendsView = new MyFriendsView();
				CurrentActivePage = App.CurrentActivePageName = ActivePage.Friends;
                TabPageGesture(myFriendsView, friendActiveImage, friendInActiveImage, lblFriends, FriendsBox,stackFriends);
			}
			else
			{
				pulseViewModel.IsLoading = false;
			}
		}

       	protected async override void OnAppearing()
		{
			if (!string.IsNullOrEmpty(App.NotificationTitle) && (App.NotificationTitle == Constant.EventInvitiationNotificationTitle || App.NotificationTitle == Constant.InterestedNotificationTitle || App.NotificationTitle == Constant.InviteAcceptNotificationTitle || App.NotificationTitle == Constant.EventReminderNotificationTitle || App.NotificationTitle == Constant.TagOnCommentNotificationTitle))
			{
				App.NotificationTitle = string.Empty;
				App myApp = App.Current as App;
				myApp.FetchEventDetailMethod(App.NotificationID);
			}
			else if (!string.IsNullOrEmpty(App.NotificationTitle) && (App.NotificationTitle == Constant.FriendRequestNotificationTitle || App.NotificationTitle == Constant.FriendRequestAcceptedNotificationTitle))
			{
				App.NotificationTitle = string.Empty;
				//await Navigation.PushModalAsync(new FriendsProfilePage("My Friends", App.NotificationID));
			}

			MessagingCenter.Subscribe<App>(this, "ShowLoader", (obj) => { 
				grdOverlay.IsVisible = true;
			});

			MessagingCenter.Subscribe<App>(this, "HideLoader", (obj) => {
				grdOverlay.IsVisible = false;
			});
            MessagingCenter.Subscribe<App>(this, "GetNotificationCount", (obj) => {
                eventViewModel.IsNotification = false;
                eventViewModel.GetUnreadNotificationCount();

            });
            var safeinsects = On<Xamarin.Forms.PlatformConfiguration.iOS>().SafeAreaInsets();
            if (safeinsects.Bottom > 0)
            {
                this.Padding = new Thickness(0, 0, 0, 20);
            }

                      
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			grdOverlay.IsVisible = false;
			MessagingCenter.Unsubscribe<App>(this, "ShowLoader");
			MessagingCenter.Unsubscribe<App>(this, "HideLoader");
            MessagingCenter.Unsubscribe<App>(this, "GetNotificationCount");
		}

		async void GetPulseView()
		{
			grdOverlay.IsVisible = true;
			pulseViewModel.pageNoPulse = 1;
			pulseViewModel.totalGroupsPage = 1;
			bool isList = await pulseViewModel.GetPulseList(false, "");
			pulseViewModel.isPulseListAvailable = isList;
            pulseViewModel.IsPulseListVisible = true;
			pulseView = new PulseView();
			CurrentActivePage = App.CurrentActivePageName = ActivePage.Pulse;
            TabPageGesture(pulseView, pulseActiveImage, pulseInActiveImage, lblPulse, PulseBox,stackPulse);
		}

		#endregion
		#region private methods
        void  TabPageGesture(ContentView tabName, Image ActiveImage, Image InActiveImage, Label lbl, BoxView box,StackLayout stack)
		{
			grdInnerView.Children.Clear();
			grdInnerView.Children.Add(tabName, 0, 0);
			eventActiveImage.IsVisible = false;
			eventInActiveImage.IsVisible = true;
			friendActiveImage.IsVisible = false;
			friendInActiveImage.IsVisible = true;
			pulseActiveImage.IsVisible = false;
			pulseInActiveImage.IsVisible = true;
			profileActiveImage.IsVisible = false;
			profileInActiveImage.IsVisible = true;
            notificationInActiveImage.IsVisible = true;
            notificationActiveImage.IsVisible = false;
			ActiveImage.IsVisible = true;
			InActiveImage.IsVisible = false;
			lblEvent.TextColor = Color.FromHex(Constant.InActiveMenuTextColor);
			lblPulse.TextColor = Color.FromHex(Constant.InActiveMenuTextColor);
			lblFriends.TextColor = Color.FromHex(Constant.InActiveMenuTextColor);
			lblProfile.TextColor = Color.FromHex(Constant.InActiveMenuTextColor);
			lblProfile.TextColor = Color.FromHex(Constant.InActiveMenuTextColor);
            lblNotification.TextColor = Color.FromHex(Constant.InActiveMenuTextColor);
			lbl.TextColor = Color.FromHex(Constant.PinkButtonColor);
			EventsBox.BackgroundColor = Color.Transparent;
			PulseBox.BackgroundColor = Color.Transparent;
			ProfileBox.BackgroundColor = Color.Transparent;
			FriendsBox.BackgroundColor = Color.Transparent;
            stackEvents.BackgroundColor = Color.Transparent;
            stackPulse.BackgroundColor = Color.Transparent;
            stackFriends.BackgroundColor = Color.Transparent;
            stackNotifications.BackgroundColor = Color.Transparent;;
            stackProfile.BackgroundColor = Color.Transparent;
            stack.BackgroundColor = Color.FromHex(Constant.BoostListBackColor);
            Notificationbox.BackgroundColor = Color.Transparent;
			box.BackgroundColor = Color.FromHex(Constant.PinkButtonColor);
			grdOverlay.IsVisible = false;
		}

		async protected void MenuItemTapped(object sender, EventArgs e)
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
					switch (button.ActivePage)
					{

						case ActivePage.Pulse:
							if (button.ActivePage != CurrentActivePage)
							{
								GetPulseView();
							}
                            eventViewModel.GetUnreadNotificationCount();
							CurrentActivePage = App.CurrentActivePageName = button.ActivePage;

							break;
                        case ActivePage.Notification:
                            if (button.ActivePage != CurrentActivePage)
                            {
                                grdOverlay.IsVisible = true;
                                await Task.Delay(1000);

                                notificationView = new NotificationView();
                                TabPageGesture(notificationView, notificationActiveImage, notificationInActiveImage, lblNotification, Notificationbox,stackNotifications);
                            }
                            await eventViewModel.MarkReadNotification();
                            CurrentActivePage = App.CurrentActivePageName = button.ActivePage;
                            break;
						case ActivePage.Friends:
							if (button.ActivePage != CurrentActivePage)
							{
								grdOverlay.IsVisible = true;
								await Task.Delay(1000);
                                eventViewModel.GetUnreadNotificationCount();
								myFriendsView = new MyFriendsView();
                                TabPageGesture(myFriendsView, friendActiveImage, friendInActiveImage, lblFriends, FriendsBox,stackFriends);
							}
                           
							CurrentActivePage = App.CurrentActivePageName = button.ActivePage;
							break;
						case ActivePage.Profile:
							if (button.ActivePage != CurrentActivePage)
							{
								grdOverlay.IsVisible = true;
								await Task.Delay(1000);
                                eventViewModel.GetUnreadNotificationCount();
								profileView = new ProfileView();
                                TabPageGesture(profileView, profileActiveImage, profileInActiveImage, lblProfile, ProfileBox,stackProfile);
							}

							CurrentActivePage = App.CurrentActivePageName = button.ActivePage;
							break;
						default:
							if (button.ActivePage != CurrentActivePage)
							{
								grdOverlay.IsVisible = true;
								await Task.Delay(1000);
                                eventViewModel.GetUnreadNotificationCount();
								eventsListView = new EventsListView();
                                TabPageGesture(eventsListView, eventActiveImage, eventInActiveImage, lblEvent, EventsBox,stackEvents);
							}
							CurrentActivePage = App.CurrentActivePageName = button.ActivePage;
							break;
					}
				}
			}
		}
		#endregion
	}



}
