using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.Connectivity;
using Plugin.Geolocator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

namespace Pulse
{
    public partial class MapEventListPage : BaseContentPage
    {
        readonly EventViewModel eventViewModel;
        int _tapCount = 0;
        string tappedEventId;
        bool isPastEvent;
        MyEventType CurrentActiveEventType;
        public MapEventListPage()
        {
            InitializeComponent();
            eventViewModel = ServiceContainer.Resolve<EventViewModel>();
            BindingContext = eventViewModel;
            CurrentActiveEventType = eventViewModel.currentActiveEventType = MyEventType.Upcoming;
        }

        async void BackIcon_Tapped(object sender, System.EventArgs e)
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
        async void CheckInTapped(object sender, System.EventArgs e)
        {
            var selected = sender as Frame;
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
        void EventEditTapped(object sender, System.EventArgs e)
        {
            var selected = sender as Frame;
            grdOverlayDialog.IsVisible = true;
            tappedEventId = selected.ClassId;
            eventViewModel.TappedEventId = Convert.ToInt32(tappedEventId);
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
                        eventViewModel.IsEventListVisible = false;
                        eventViewModel.IsNoEventVisible = false;
                        eventViewModel.GetLocBasedEvents();
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
        protected async override void OnAppearing()
		{
            base.OnAppearing();
            eventViewModel.tempLatlongBasedEventList = new System.Collections.ObjectModel.ObservableCollection<MyEvents>();
            eventViewModel.tempLatlongBasedEventList.Clear();
            eventViewModel.pageNoLatLongBasedEvents = 1;
            eventViewModel.GetMapLatLongEvents();
		}

	}

}
