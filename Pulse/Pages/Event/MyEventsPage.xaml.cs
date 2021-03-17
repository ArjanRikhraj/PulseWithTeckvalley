using System;
using System.Threading.Tasks;
using Plugin.Connectivity;
using Plugin.Geolocator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

namespace Pulse
{
	public partial class MyEventsPage : BaseContentPage
	{
		#region Private Variables
		int _tapCount = 0;
        bool IsFirstLoad;
		string tappedEventId;
        bool isPastEvent;
		readonly EventViewModel eventViewModel;
		MyEventType CurrentActiveEventType;
		#endregion
		#region Constructor
		public MyEventsPage()
		{
			InitializeComponent();
            IsFirstLoad = true;
			eventViewModel = ServiceContainer.Resolve<EventViewModel>();
			BindingContext = eventViewModel;
			SetInitialValues();

		}
        #endregion
        #region Methods
        
        void ClearFields()
		{
			eventViewModel.tempEventList.Clear();
			eventViewModel.totalEventPages = 1;
			eventViewModel.pageNoMyEvents = 1;
		}
		void SetInitialValues()
		{
			if (Device.RuntimePlatform == Device.Android)
			{
				topStack.Margin = new Thickness(10, 10, 10, 10);
			}
			CurrentActiveEventType = eventViewModel.currentActiveEventType = MyEventType.Upcoming;
			lblFilter.Text = eventViewModel.FilterType = Constant.AllText;
			ClearFields();
			eventViewModel.IsEventListVisible = false;
			eventViewModel.IsNoEventVisible = false;
			eventViewModel.GetMyEventsList();
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
						switch (button.EventType)
						{
							case MyEventType.Upcoming:

								if (button.EventType != CurrentActiveEventType)
								{
									lblUpcoming.TextColor = UpcomingsBox.BackgroundColor = Color.FromHex(Constant.PinkButtonColor);
									PastBox.BackgroundColor = Color.Transparent;
									lblPast.TextColor = Color.FromHex(Constant.GrayTextColor);
									lblFilter.Text = eventViewModel.FilterType = Constant.AllText;
									ClearFields();
									CurrentActiveEventType = eventViewModel.currentActiveEventType = button.EventType;
									eventViewModel.GetMyEventsList();
								}
								break;
							default:
								if (button.EventType != CurrentActiveEventType)
								{
									lblPast.TextColor = PastBox.BackgroundColor = Color.FromHex(Constant.PinkButtonColor);
									UpcomingsBox.BackgroundColor = Color.Transparent;
									lblUpcoming.TextColor = Color.FromHex(Constant.GrayTextColor);
									lblFilter.Text = eventViewModel.FilterType = Constant.AllText;
									ClearFields();
									CurrentActiveEventType = eventViewModel.currentActiveEventType = button.EventType;
									eventViewModel.GetMyEventsList();
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
                    tappedEventId = selected.ClassId;
                    eventViewModel.TappedEventId = Convert.ToInt32(tappedEventId);
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
		async void CancelEvent_Clicked(object sender, System.EventArgs e)
		{
			stackPopUp.IsVisible = false;
			await eventViewModel.CancelEvent();
		}

		void Filter_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (Device.RuntimePlatform == Device.Android)
			{
				ApplyFilter();
			}
		}

		void Picker_DoneClicked(object sender, System.EventArgs e)
		{
			ApplyFilter();
		}

		void ApplyFilter()
		{
			if (FilterPicker.SelectedIndex >= 0)
			{
				lblFilter.Text = FilterPicker.Items[FilterPicker.SelectedIndex];
				eventViewModel.FilterType = lblFilter.Text;
				ClearFields();
				eventViewModel.GetMyEventsList();
			}
		}

		void FilterItemTapped(object sender, System.EventArgs e)
		{
			SetFilterValues();
			Device.BeginInvokeOnMainThread(() =>
				{
					if (FilterPicker.IsFocused)
						FilterPicker.Unfocus();
					FilterPicker.Focus();
				});
		}

		void SetFilterValues()
		{
			if (FilterPicker.Items.Count > 0)
			{
				FilterPicker.Items.Clear();
			}
			FilterPicker.Items.Add(Constant.AllText);

			if (CurrentActiveEventType == MyEventType.Upcoming)
			{
				FilterPicker.Items.Add(Constant.HostingText);
				FilterPicker.Items.Add(Constant.AttendingText);
                FilterPicker.Items.Add(Constant.InterestedText);
                FilterPicker.Items.Add(Constant.CheckedInText);

			}
			else
			{
				FilterPicker.Items.Add(Constant.HostedText);
				FilterPicker.Items.Add(Constant.AttendedText);
                FilterPicker.Items.Add(Constant.CheckedInText);
			}
		}

		async void BackIcon_Tapped(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					eventViewModel.IsLoading = true;
                    eventViewModel.GetAllUpComingEvents();
                    await Navigation.PopAsync();
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
		void Cancel_Clicked(object sender, System.EventArgs e)
		{
			stackPopUp.IsVisible = false;
			grdOverlayDialog.IsVisible = false;
			stackReason.IsVisible = false;
		}

		async void EditTapped(object sender, System.EventArgs e)
		{
			stackPopUp.IsVisible = false;
			grdOverlayDialog.IsVisible = false;
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					eventViewModel.IsLoading = true;
                    eventViewModel.IsUpdateBoostEvent = false;
					await Navigation.PushModalAsync(new EditEventPage(tappedEventId));
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

		void ReasonEditor_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
		{
			if (editorReasonToCancel.Text.Length >= 150)
			{
				editorReasonToCancel.Text = editorReasonToCancel.Text.Remove(editorReasonToCancel.Text.Length - 1);  // Remove Last character

			}
		}

        async void MyEventsList_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					eventViewModel.IsLoading = true;
					var selected = (MyEvents)e.Item;
					listViewMyEvents.SelectedItem = null;
                    await eventViewModel.FetchEventDetail(Convert.ToString(selected.EventId), true);
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

		async void DeleteTapped(object sender, System.EventArgs e)
		{
			stackPopUp.IsVisible = false;
			grdOverlayDialog.IsVisible = false;
			bool result = await App.Instance.ConfirmAlert(Constant.DeleteQuestionText, Constant.AlertTitle, Constant.Ok, Constant.CancelButtonText);
			if (result)
			{
				grdOverlayDialog.IsVisible = true;
				stackReason.IsVisible = true;
			}

		}
		void EventEditTapped(object sender, System.EventArgs e)
		{
			var selected = sender as Frame;
			stackPopUp.IsVisible = true;
			grdOverlayDialog.IsVisible = true;
			tappedEventId = selected.ClassId;
			eventViewModel.TappedEventId = Convert.ToInt32(tappedEventId);
		}
        async void CheckInTapped(object sender, System.EventArgs e)
        {
            var selected = sender as Frame;
            stackPopUp.IsVisible = false;
            eventViewModel.IsLoading = true;
            tappedEventId = selected.ClassId;
            grdOverlayDialog.IsVisible = true;
            eventViewModel.TappedEventId = Convert.ToInt32(tappedEventId);
            await eventViewModel.FetchEventDetail(tappedEventId, false);
            if (eventViewModel.currentActiveEventType == MyEventType.Upcoming)
            {
                if (DateTimeValidate() && !isPastEvent)
                {
                    bool isEnable = await GetCurrentLocforCheckIn();
                    if (isEnable)
                    {
                        CheckIn();
                        grdOverlayDialog.IsVisible = false;
                    }
                    else
                    {
                        checkInTitle.Text = Constant.CheckInNotProperTitleMessage;
                        checkInMessage.Text = Constant.CheckInNotProperMessage;
                        stackcheckInMessage.IsVisible = true;
                        _tapCount = 0;
                    }
                }
                else if (!DateTimeValidate() && isPastEvent)
                {
                    await App.Instance.Alert("You can not check-in past event", Constant.AlertTitle, Constant.Ok);
                    grdOverlayDialog.IsVisible = false;
                    eventViewModel.IsLoading = false;
                    _tapCount = 0;
                }
                else
                {
                    checkInTitle.Text = Constant.CheckInEarlyTitleMessage;
                    checkInMessage.Text = Constant.CheckInEarlyMessage;
                    stackcheckInMessage.IsVisible = true;
                    _tapCount = 0;
                }

            }
            else
            {
                await App.Instance.Alert("You can not check-in past event", Constant.AlertTitle, Constant.Ok);
                grdOverlayDialog.IsVisible = false;
                eventViewModel.IsLoading = false;
                _tapCount = 0;

            }
            eventViewModel.IsLoading = false;
        }
        bool DateTimeValidate()
        {
            if (eventViewModel.EventFromDate.Date == DateTime.Now.Date && eventViewModel.EventFromTime > DateTime.Now.TimeOfDay)
            {
                return false;
            }
            else if (eventViewModel.EventToDate.Date == DateTime.Now.Date && eventViewModel.EventToTime < DateTime.Now.TimeOfDay)
            {
                isPastEvent = true;
                return false;
            }
            else if (eventViewModel.EventFromDate.Date > DateTime.Now.Date)
            {
                return false;
            }
            else
                return true;
        }
        async Task<bool> GetCurrentLocforCheckIn()
        {
            if (Device.RuntimePlatform == Device.iOS)
                return (await GetPermissionForCheckIn());
            else
                return (await GetCurrentLocationForCheckIn());
        }
        async Task<bool> GetPermissionForCheckIn()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        eventViewModel.IsLoading = false;
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    status = results[Permission.Location];
                }

                if (status == PermissionStatus.Granted)
                {
                    return (await GetCurrentLocationForCheckIn());
                }
                else if (status != PermissionStatus.Unknown)
                {
                    eventViewModel.IsLoading = false;
                }
                return false;

            }
            catch (Exception)
            {
                eventViewModel.IsLoading = false;
                return false;
            }
        }
        async Task<bool> GetCurrentLocationForCheckIn()
        {
            try
            {
                if (Plugin.Geolocator.CrossGeolocator.Current.IsGeolocationEnabled == true)
                {
                    if (CrossGeolocator.Current != null)
                    {
                        var locator = CrossGeolocator.Current;
                        locator.DesiredAccuracy = 500;
                        var position = await locator.GetPositionAsync(System.TimeSpan.FromMilliseconds(30000));
                        eventViewModel.currenteventLat = position.Latitude.ToString();
                        eventViewModel.currenteventLong = position.Longitude.ToString();
                        return true;
                    }
                    return false;
                }
                else
                {
                    eventViewModel.IsLoading = false;
                    return false;
                }
            }
            catch (Exception)
            {
                eventViewModel.IsLoading = false;
                return false;

            }
        }
        void OverLayTapped(object sender, System.EventArgs e)
        {
           grdOverlay.IsVisible = false;
            grdOverlayDialog.IsVisible = false;
            stackcheckInMessage.IsVisible = false;
        }
        async void CheckIn()
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                    _tapCount = 0;
                    eventViewModel.IsLoading = false;
                }
                else
                {
                    eventViewModel.IsLoading = true;
                    UserCheckIn userCheckIn = new UserCheckIn();
                    userCheckIn.latitude = Convert.ToDouble(eventViewModel.currenteventLat);
                    userCheckIn.longitude = Convert.ToDouble(eventViewModel.currenteventLong);
                    var response = await new MainServices().Post<ResultWrapperSingle<SendEmailOTPResponse>>(Constant.CheckInUrl + eventViewModel.TappedEventId + '/', userCheckIn);
                    if (response != null && response.status == Constant.Status200 && response.response != null)
                    {
                        eventViewModel.IsUserNotCheckedIn = false;
                        eventViewModel.IsUserCheckedIn = true;
                        ShowToast(Constant.AlertTitle, "Successfully checked in");
                        CurrentActiveEventType = eventViewModel.currentActiveEventType = MyEventType.Upcoming;
                        lblFilter.Text = eventViewModel.FilterType = Constant.AllText;
                        ClearFields();
                        eventViewModel.IsEventListVisible = false;
                        eventViewModel.IsNoEventVisible = false;
                        eventViewModel.GetMyEventsList();
                        await eventViewModel.FetchEventDetail(eventViewModel.TappedEventId.ToString(), false);
                        await Navigation.PushModalAsync(new PartyStoryPage());
                        grdOverlayDialog.IsVisible = false;
                        eventViewModel.IsLoading = false;
                        _tapCount = 0;

                    }
                    else if (response != null && response.status == Constant.Status111 && response.message.non_field_errors != null)
                    {
                        checkInTitle.Text = Constant.CheckInNotProperTitleMessage;
                        checkInMessage.Text = Constant.CheckInNotProperMessage;
                        grdOverlay.IsVisible = true;
                        stackcheckInMessage.IsVisible = true;
                        eventViewModel.IsLoading = false;
                        _tapCount = 0;

                    }
                    else
                    {
                        eventViewModel.IsLoading = false;
                        await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                        _tapCount = 0;
                    }
                }
            }
            catch (Exception)
            {

            }
        }
		#endregion
	}
}
