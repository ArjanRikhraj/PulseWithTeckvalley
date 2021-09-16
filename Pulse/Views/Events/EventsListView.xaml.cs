using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Plugin.Connectivity;
using Plugin.Geolocator;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Plugin.Toasts;
using Pulse.Helpers;
using Pulse.Views.Events;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.PancakeView;

namespace Pulse
{
    public partial class EventsListView : ContentView
    {
        List<MyEventResponse> list = new List<MyEventResponse>();
        List<MyEventResponse> EventsList = new List<MyEventResponse>();
        MainServices mainService;
        CustomMap customMap;
        string listURL = string.Empty;
        string lat = string.Empty;
        string lang = string.Empty;
        string tappedEventId;
        int page = 1;
        bool isPastEvent;
        int _tapCount = 0;
        readonly EventViewModel eventViewModel;
        bool isPageFirstLoad;
        public EventsListView()
        {
            InitializeComponent();
            eventViewModel = ServiceContainer.Resolve<EventViewModel>();
            BindingContext = eventViewModel;
            mainService = new MainServices();
            App.ShowMainPageLoader();
            SetUi();
            isPageFirstLoad = true;
            //stackList.IsVisible = false;
            //stackAddress.IsVisible = false;
            //stackFilter.IsVisible = false;
            SetInitial();
        }

        void SetUi()
        {
            loader.Easing = Easing.CubicIn;
            var effect = Effect.Resolve($"NotchEffect.{nameof(NotchEffect)}");
            GradientColorStack gradientColorStack = new GradientColorStack()
            {
                StartColor = Color.FromHex(Constant.LightPinkColor),
                EndColor = Color.FromHex(Constant.DarkPinkColor),
                Spacing = 0,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                stackOrientation = StackOrientation.Horizontal
            };
            gradientColorStack.Effects.Add(effect);
            StackLayout stack = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Horizontal,
                Margin = new Thickness(15, 20, 15, 10)
            };
            if (Device.RuntimePlatform == Device.Android)
            {
                stack.Margin = new Thickness(15, 15, 15, 8);
            }
            ExtendedLabel label = new ExtendedLabel()
            {
                Text = "My Events",
                HorizontalOptions = LayoutOptions.StartAndExpand,
                FontSize = 13,
                FontFace = FontFace.PoppinsMedium,
                TextColor = Color.FromHex(Constant.WhiteTextColor)
            };
            StackLayout stack1 = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.End,
                Margin = new Thickness(7, 0, 7, 0),
                Orientation = StackOrientation.Horizontal
            };
            ExtendedLabel label1 = new ExtendedLabel()
            {
                Text = "See All",
                HorizontalOptions = LayoutOptions.End,
                FontSize = 13,
                FontFace = FontFace.PoppinsMedium,
                TextColor = Color.FromHex(Constant.WhiteTextColor)
            };
            stack1.Children.Add(label1);
            stack.Children.Add(label);
            stack.Children.Add(stack1);
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += SeeAllTapped;
            stack1.GestureRecognizers.Add(tapGestureRecognizer);
            gradientColorStack.Children.Add(stack);
            GetEventList(gradientColorStack);
        }

        private async void SearchTap_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    if (_tapCount < 1)
                    {
                        _tapCount = 1;
                        stackAddress.IsVisible = true;
                        entryVenue.Focus();
                        _tapCount = 0;
                    }
                }
                else
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                    _tapCount = 0;
                }
            }
            catch (Exception ex)
            {
                App.HideMainPageLoader();
            }
        }

        async void GetEventList(GradientColorStack gradientColorStack)
        {
            try
            {
                if (SessionManager.AccessToken != null)
                {
                    App.ShowMainPageLoader();
                    var response = await mainService.Get<ResultWrapper<MyEventResponse>>(Constant.MyEventsUrl);
                    if (response != null && response.status == Constant.Status200 && response.response != null)
                    {
                        list = response.response;
                        if (list != null && list.Count > 0)
                        {
                            ScrollView scroll = new ScrollView()
                            {
                                Orientation = ScrollOrientation.Horizontal,
                                
                            };
                            StackLayout stack = new StackLayout()
                            {
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                Orientation = StackOrientation.Horizontal
                            };
                            foreach (var i in list)
                            {
                                PancakeView frame = new PancakeView
                                {
                                    HeightRequest=70,
                                    CornerRadius = 10,
                                    BackgroundColor = Color.FromHex(Constant.WhiteTextColor),
                                    Margin = new Thickness(15, 10, 0, 15),
                                    Padding = new Thickness(0, 5, 0, 5),
                                    WidthRequest = App.ScreenWidth / 1.8,
                                };
                                frame.Shadow = new DropShadow
                                {
                                    Color = Color.Black,
                                };
                                StackLayout innerStack = new StackLayout
                                {
                                    HorizontalOptions = LayoutOptions.FillAndExpand,
                                    VerticalOptions = LayoutOptions.FillAndExpand,
                                    Spacing = 0,
                                    ClassId = Convert.ToString(i.id)
                                };
                                ExtendedLabel lblEventName = new ExtendedLabel
                                {
                                    FontFace = FontFace.PoppinsSemiBold,
                                    TextColor = Color.FromHex(Constant.GrayTextColor),
                                    FontSize = 13,
                                    HorizontalOptions = LayoutOptions.FillAndExpand,
                                    Margin = new Thickness(10, 0, 10, 0),
                                    LineBreakMode = LineBreakMode.TailTruncation,
                                    Text = i.name
                                };
                                ExtendedLabel lblHostedby = new ExtendedLabel
                                {
                                    FontFace = FontFace.PoppinsSemiBold,
                                    TextColor = Color.FromHex(Constant.GrayTextColor),
                                    FontSize = 13,
                                    HorizontalOptions = LayoutOptions.FillAndExpand,
                                    Margin = new Thickness(10, 0, 10, 0),
                                    LineBreakMode = LineBreakMode.TailTruncation,
                                    //Text = i.
                                };
                                ExtendedLabel lblEventInviteeCount = new ExtendedLabel
                                {
                                    FontFace = FontFace.PoppinsRegular,
                                    TextColor = Color.FromHex(Constant.GrayTextColor),
                                    FontSize = 12,
                                    Margin = new Thickness(10, 0, 10, 0),
                                    Text = !string.IsNullOrEmpty(Convert.ToString(i.event_attendees_count)) ? i.event_attendees_count > 1 ? Convert.ToString(i.event_attendees_count) + " Attendees" : Convert.ToString(i.event_attendees_count) + " Attendees" : "No Attendees"
                                };
                                BoxView boxView = new BoxView
                                {
                                    HeightRequest = 1,
                                    Margin = new Thickness(0, 5, 0, 5),
                                    BackgroundColor = Color.FromHex(Constant.BoxViewColor)
                                };
                                ExtendedLabel lblEventTime = new ExtendedLabel
                                {
                                    FontFace = FontFace.PoppinsRegular,
                                    TextColor = Color.FromHex(Constant.GrayTextColor),
                                    FontSize = 10,
                                    Margin = new Thickness(10, 0, 10, 0),
                                    Text = Convert.ToDateTime(i.start_date).ToString("MMM dd, yyyy") + " " + Convert.ToDateTime(i.start_date + " " + i.start_time).ToString("HH:mm tt")
                                };
                                innerStack.Children.Add(lblEventName);
                                innerStack.Children.Add(lblEventInviteeCount);
                                innerStack.Children.Add(boxView);
                                innerStack.Children.Add(lblEventTime);
                                frame.Content = innerStack;
                                if (Device.RuntimePlatform == Device.Android)
                                {
                                    frame.CornerRadius = 10;
                                }
                                var tapGestureRecognizer = new TapGestureRecognizer();
                                tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
                                innerStack.GestureRecognizers.Add(tapGestureRecognizer);
                                stack.Children.Add(frame);
                            }

                            //if (Device.RuntimePlatform == Device.iOS)
                            //{
                            //    gradientColorStack.Padding = new Thickness(0, 20, 0, 0);
                            //}

                            scroll.Content = stack;
                            gradientColorStack.Children.Add(scroll);
                            stackMainMyevents.Children.Add(gradientColorStack);
                        }
                        else
                        {
                            stackMainMyevents.Children.Add(gradientColorStack);
                        }
                    }
                    else
                    {
                        stackMainMyevents.Children.Add(gradientColorStack);
                    }
                }
                else
                {
                    stackMainMyevents.Children.Add(gradientColorStack);
                }
            }
            catch (Exception)
            {
                stackMainMyevents.Children.Add(gradientColorStack);
                App.HideMainPageLoader();
            }
        }

        void SetInitial()
        {
            ClearFields();
            eventViewModel.IsAddressListVisible = false;
            eventViewModel.FilterLocType = "Recent";
            eventViewModel.listURL = Constant.LatLongBasedEventsUrl;
            GetCurrentLoc();
            eventViewModel.GetAllUpComingEvents();
            MessagingCenter.Subscribe<App>(this, "GetNotificationCount", (obj) => {
                eventViewModel.IsNotification = false;
                eventViewModel.GetUnreadNotificationCount();

            });
            App.HideMainPageLoader();
        }
      
        async void GetCurrentLoc()
        {
            GetPermission();
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
        void Picker_DoneClicked(object sender, System.EventArgs e)
        {
            ApplyFilter();
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
        async void MapIconTapped(object sender, System.EventArgs e)
        {
            lblList.IsVisible = true;
            mapFilter.IsVisible = true;
            listFilter.IsVisible = false;
            entryVenue.Placeholder = "Live Events";
            eventViewModel.listURL = FilterPicker.SelectedIndex == 0 || FilterPicker.SelectedIndex == -1 ? Constant.MapLatLongBasedEventsUrl : Constant.MapPopularityBasedEventsUrl;
            eventViewModel.pageNoLocBasedEvents = 1;
            eventViewModel.tempLocBasedEventList.Clear();
            await eventViewModel.GetMapLocBasedEvents();
            SelectMap(eventViewModel.tempLocBasedEventList);
        }

        void Handle_Tapped(object sender, System.EventArgs e)
        {
            if (customMap != null)
            {
                if (!string.IsNullOrEmpty(eventViewModel.eventLat) && !string.IsNullOrEmpty(eventViewModel.eventLong))
                    customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(Convert.ToDouble(eventViewModel.eventLat), Convert.ToDouble(eventViewModel.eventLong)), Distance.FromMiles(12.4274)));
            }
        }

        void SelectMap(ObservableCollection<MyEvents> list)
        {
            App.ShowMainPageLoader();
            stackMap.IsVisible = true;
            stackList.IsVisible = false;
            listViewEvents.IsVisible = false;
            imgAddEvent.IsVisible = true;
            eventViewModel.IsAddressListVisible = false;
            CreateMap(list);
        }

        void ListIconTapped(object sender, System.EventArgs e)
        {
            App.ShowMainPageLoader();
            stackAddress.IsVisible = true;
            stackList.IsVisible = true;
            imgCurrentLocation.IsVisible = false;
            eventViewModel.IsAddressListVisible = false;
            stackMap.IsVisible = false;
            lblList.IsVisible = false;
            imgMap.IsVisible = true;
            entryVenue.Placeholder = "Search Upcoming Events";
            //stackFilter.IsVisible = true;
            imgAddEvent.IsVisible = true;
            App.HideMainPageLoader();
            eventViewModel.listURL = FilterPicker.SelectedIndex == 0 ? Constant.LatLongBasedEventsUrl : Constant.PopularityBasedEventsUrl;
            GetCurrentLoc();
            isPageFirstLoad = false;
            if (isPageFirstLoad)
            {
                isPageFirstLoad = false;
                ClearFields();
                eventViewModel.IsAddressListVisible = false;
                eventViewModel.FilterLocType = "Recent";
                eventViewModel.listURL = Constant.LatLongBasedEventsUrl;
                eventViewModel.GetLocBasedEvents();
                //SetInitial();
            }
            else
            {
                eventViewModel.pageNoLocBasedEvents = 1;
                eventViewModel.tempLocBasedEventList.Clear();
                eventViewModel.listURL = Constant.LatLongBasedEventsUrl;
                eventViewModel.GetLocBasedEvents();
            }

        }
        void CreateMap(ObservableCollection<MyEvents> list)
        {
            App.ShowMainPageLoader();
            customMap = new CustomMap()
            {
                MapType = Xamarin.Forms.Maps.MapType.Street,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                IsShowingUser = true,
                HasZoomEnabled = true
            };
            try
            {
                customMap.Pins.Clear();
                DrawPin(customMap, list);
            }
            catch (Exception e)
            {
                App.HideMainPageLoader();
            }
        }


        void DrawPin(CustomMap customMap, ObservableCollection<MyEvents> list)
        {
            if (list != null && list.Count > 0)
            {
                List<CustomPin> pinsList = new List<CustomPin>();
                //int itemIndex = 0;
               // List<MyEvents> SamePoistionCount = new List<MyEvents>();
                CustomPin locationPin = new CustomPin
                {
                    IsCurrentLocation = true,
                    Label = "you are here",
                    Type = PinType.Place,
                    Id = "Xamarin",
                    Position = new Position(Convert.ToDouble(eventViewModel.eventLat), Convert.ToDouble(eventViewModel.eventLong))
                };
                //need to comment this line coz two pins is showing for current location
                //customMap.Pins.Add(locationPin);
                //need to comment this line coz two pins is showing for current location
                pinsList.Add(locationPin);
                //foreach(var i in list)
                //{
                //   SamePoistionCount = list.Where(x => x.Eventlat == i.Eventlat && x.EventLong == i.EventLong).ToList();
                //    itemIndex++;
                //}
                //var uniqueRecords = list.GroupBy(x => new { x.Eventlat }).Select(x => x.LastOrDefault()).ToList();
                foreach (var i in list)
                {
                    //var isSamePostionData = uniqueRecords.Where(x => x.Eventlat == i.Eventlat && x.EventLong == i.EventLong).ToList();
                    string lat;
                    string lng;

                    //if (isSamePostionData != null && isSamePostionData.Count > 1)
                    //{
                    //lat = i.Eventlat;
                    //lng = i.EventLong;
                    //    var getLatDigits = i.Eventlat.Split('.');
                    //    var newLatDigits = getLatDigits != null ? getLatDigits[1] != null ? getLatDigits[0] + "." + getLatDigits[1].Substring(0, 6) : string.Empty : string.Empty;
                    //    var getLongDigits = i.EventLong.Split('.');
                    //    var newLongDigits = getLongDigits != null ? getLongDigits[1] != null ? getLongDigits[0] + "." + getLongDigits[1].Substring(0, 6) : string.Empty : string.Empty;
                    //    var lastLatDigits = newLatDigits.Substring(newLatDigits.Length - 2);
                    //    var lastLongDigits = newLongDigits.Substring(newLongDigits.Length - 2);
                    //    int editedLatDigits = Convert.ToInt32(lastLatDigits) + (itemIndex == 0 ? 15 : itemIndex * 15);
                    //    int editedLongDigits = Convert.ToInt32(lastLongDigits) + (itemIndex == 0 ? 15 : itemIndex * 15);
                    //    lat = newLatDigits.Substring(0, newLatDigits.Length - 2) + Convert.ToString(editedLatDigits);
                    //    lng = newLongDigits.Substring(0, newLongDigits.Length - 2) + Convert.ToString(editedLongDigits);
                    //}
                    //else
                    //{
                    lat = i.Eventlat;
                    lng = i.EventLong;
                    //}
                    //var compare = SamePoistionCount.Where(x => x.Eventlat == lat).ToList();

                    var pin = new CustomPin
                    {
                        Type = PinType.Place,
                        Position = new Position(Convert.ToDouble(lat), Convert.ToDouble(lng)),
                        EventDateTime = i.EventDateTime,
                        EventName = i.EventName,
                        Latitude = i.Eventlat,
                        Lognitude = i.EventLong,
                        Label = i.EventName,
                        //Address = i.EventDateTime,
                        Address= i.EventAddress,
                        Id = Convert.ToString(i.EventId),
                        IsBoostEvent = i.IsBoostEvent,
                        IsCurrentLocation = i.IsCurrentLocation,
                        SameLocationPinCount = i.LatLongEventsCount > 1 ? Convert.ToString(i.LatLongEventsCount) : "0",
                        IsMoreThanOneLocation = i.LatLongEventsCount > 1
                    };
                    if (!pinsList.Contains(pin))
                    {
                        pinsList.Add(pin);
                    }
                    //need to comment this line coz two pins is showing for current location
                    customMap.Pins.Add(pin);
                }
                //need to comment this line coz two pins is showing for current location
                customMap.CustomPins = pinsList;
                customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(Convert.ToDouble(eventViewModel.eventLat), Convert.ToDouble(eventViewModel.eventLong)), Distance.FromMiles(2)));
            }
            else
            {
                if (!string.IsNullOrEmpty(eventViewModel.eventLat) && !string.IsNullOrEmpty(eventViewModel.eventLong))
                {
                    customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(Convert.ToDouble(eventViewModel.eventLat), Convert.ToDouble(eventViewModel.eventLong)), Distance.FromMiles(2)));
                }
            }
            if (stackMap.Children != null && stackMap.Children.Count > 0)
            {
                stackMap.Children.Clear();
            }
            stackMap.Children.Add(customMap);
            App.HideMainPageLoader();
        }

        void SetFilterValues()
        {
            if (FilterPicker.Items.Count > 0)
            {
                FilterPicker.Items.Clear();
            }
            FilterPicker.Items.Add("LIVE");
            FilterPicker.Items.Add("SOON");
            FilterPicker.Items.Add("ALL");
        }

      async  void Filter_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                var f = (Picker)sender;
                eventViewModel.listURL = FilterPicker.SelectedIndex == 0 || FilterPicker.SelectedIndex == -1 ? Constant.MapLatLongBasedEventsUrl : Constant.MapPopularityBasedEventsUrl;
                eventViewModel.pageNoLocBasedEvents = 1;
                eventViewModel.tempLocBasedEventList.Clear();
                await eventViewModel.GetMapLocBasedEvents();
                eventViewModel.GetEventsByFilter(f.SelectedItem.ToString());
                lblFilter.Text = f.SelectedItem.ToString();
                SelectMap(eventViewModel.tempLocBasedEventList);
                //GetEventsByFilter(f.SelectedItem);
            }
            catch (Exception ex)
            {
                return;
            }
        }
        void ApplyFilter()
        {
            if (FilterPicker.SelectedIndex >= 0)
            {
                isPageFirstLoad = false;
                lblFilter.Text = FilterPicker.Items[FilterPicker.SelectedIndex];
                eventViewModel.listURL = FilterPicker.SelectedIndex == 0 ? Constant.LatLongBasedEventsUrl : Constant.PopularityBasedEventsUrl;
                if (eventViewModel.FilterLocType != lblFilter.Text && lblFilter.Text == "Recent")
                {
                    entryVenue.Text = string.Empty;
                    eventViewModel.IsAddressListVisible = false;
                    GetCurrentLoc();
                }
                else if (lblFilter.Text == "Popular" || lblFilter.Text == "Recent")
                {
                    if (eventViewModel.IsSearchLocItemSelected)
                    {
                        ClearFields();
                        eventViewModel.GetLocBasedEvents();
                    }
                    else
                    {
                        GetCurrentLoc();
                    }
                }
                else
                {
                    ClearFields();
                    eventViewModel.GetLocBasedEvents();
                }
                eventViewModel.FilterLocType = lblFilter.Text;

            }
        }

        void ClearFields()
        {
            eventViewModel.tempLocBasedEventList.Clear();
            eventViewModel.pageNoLocBasedEvents = 1;
            eventViewModel.totalLocBasedPages = 1;
        }

        void Venue_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                eventViewModel.IsAddressListVisible = false;
                eventViewModel.IsSearchLocItemSelected = false;
                imageCross.IsVisible = false;
                ClearFields();
                GetCurrentLoc();
            }
            else
            {
                imageCross.IsVisible = true;
                stackAddress.IsVisible = true;
                isPageFirstLoad = false;
                eventViewModel.Search(e);
                eventViewModel.IsSearchLocItemSelected = false;
            }
        }

        async void lstPlacesItemTapped(object sender, ItemTappedEventArgs e)
        {
            var data = (SearchResultModel)lstPlaces.SelectedItem;
            if (data.Name != Constant.NoResultFound)
            {
                entryVenue.Unfocus();
                eventViewModel.ResultModel = ((SearchResultModel)lstPlaces.SelectedItem);
                entryVenue.Text = eventViewModel.EventLocation = eventViewModel.ResultModel.Name + eventViewModel.ResultModel.Vicinity;
                eventViewModel.IsSearchLocItemSelected = true;
                eventViewModel.IsAddressListVisible = false;
                await eventViewModel.GetlatLong(data.place_id);
                ClearFields();
                eventViewModel.GetLocBasedEvents();
                lstPlaces.SelectedItem = null;
                stackAddress.IsVisible = false;
                if(stackMap.IsVisible==true && listViewEvents.IsVisible==false)
                {
                    if (!string.IsNullOrEmpty(eventViewModel.eventLat) && !string.IsNullOrEmpty(eventViewModel.eventLong))
                    {
                        customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(Convert.ToDouble(eventViewModel.eventLat), Convert.ToDouble(eventViewModel.eventLong)), Distance.FromMiles(2)));
                    }
                }
            }
        }


        async void MyEventsList_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (_tapCount < 1)
                {
                    _tapCount = 1;
                    App.ShowMainPageLoader();
                    var selected = (MyEvents)e.Item;
                    eventViewModel.currentActiveEventType = MyEventType.Upcoming;
                    listViewMyEvents.SelectedItem = null;
                    listViewEvents.SelectedItem = null;
                    await eventViewModel.FetchEventDetail(Convert.ToString(selected.EventId), true);
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

        void EventEditTapped(object sender, System.EventArgs e)
        {
            var selected = sender as StackLayout;
            tappedEventId = selected.ClassId;
            eventViewModel.TappedEventId = Convert.ToInt32(tappedEventId);
        }

        async void CreateEvent_Tapped(object sender, System.EventArgs e)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    if (_tapCount < 1)
                    {
                        _tapCount = 1;
                        App.ShowMainPageLoader();
                        //await Task.Delay(500);
                        eventViewModel.IsBoostEvent = false;
                        eventViewModel.BoostEventConfirmation = true;
                       await Navigation.PushModalAsync(new AddEventPage());
                        _tapCount = 0;
                    }
                }
                else
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                    _tapCount = 0;
                }
            }
            catch (Exception ex)
            {
                App.HideMainPageLoader();
                await App.Instance.Alert("Can not create event right now", Constant.AlertTitle, Constant.Ok);
            }
        }

        async void SeeAllTapped(object sender, System.EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (_tapCount < 1)
                {
                    _tapCount = 1;
                   // App.ShowMainPageLoader();
                    await Navigation.PushAsync(new MyEventsPage());
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

        async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var selectedItem = (StackLayout)sender;
            if (CrossConnectivity.Current.IsConnected)
            {
                if (_tapCount < 1)
                {
                    _tapCount = 1;
                    App.ShowMainPageLoader();
                    eventViewModel.currentActiveEventType = MyEventType.Upcoming;
                    await eventViewModel.FetchEventDetail(Convert.ToString(selectedItem.ClassId), true);
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

        void EventList_Scrolled(object sender, Xamarin.Forms.ScrolledEventArgs e)
        {
            var scrollView = (ScrollView)sender;
            var scWidth = scrollView.ContentSize.Width;
            var xHorz = scrollView.ScrollX;
            var scrollDiff = scWidth - xHorz;
            if (scrollDiff <= App.ScreenWidth)
            {
                page++;
            }
        }

        void Choice_Clicked(object sender, System.EventArgs e)
        {

        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            if (_tapCount < 1)
            {
                _tapCount = 1;
                eventViewModel.SignOut();
                _tapCount = 0;
            }
        }
        async void CheckInTapped(object sender, System.EventArgs e)
        {
            var selected = sender as PancakeView;
            tappedEventId = selected.ClassId;
            grdOverlayDialog.IsVisible = true;
            eventViewModel.IsLoading = true;
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
                        grdOverlayDialog.IsVisible = true;
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
                if (CrossGeolocator.Current.IsGeolocationEnabled == true)
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
        public async void ShowToast(string title, string description)
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                MessagingCenter.Send<string>(description, "ShowToast");
            }
            else
            {
                var notificator = DependencyService.Get<IToastNotificator>();
                var options = new NotificationOptions()
                {
                    Title = title,
                    Description = description,
                    ClearFromHistory = true,
                    AllowTapInNotificationCenter = false
                };
                var result = await notificator.Notify(options);
            }
        }
        void OverLayTapped(object sender, System.EventArgs e)
        {
            stackcheckInMessage.IsVisible = false;
            grdOverlayDialog.IsVisible = false;
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
                        eventViewModel.currentActiveEventType = MyEventType.Upcoming;
                        lblFilter.Text = eventViewModel.FilterType = Constant.AllText;
                        ClearFields();
                        eventViewModel.IsEventListVisible = false;
                        eventViewModel.IsNoEventVisible = false;
                        eventViewModel.listURL = FilterPicker.SelectedIndex == 0 ? Constant.LatLongBasedEventsUrl : Constant.PopularityBasedEventsUrl;
                        await eventViewModel.GetLocEvents();
                        await eventViewModel.FetchEventDetail(eventViewModel.TappedEventId.ToString(), false);
                        await Navigation.PushModalAsync(new PartyStoryPage());
                        eventViewModel.IsLoading = false;
                        _tapCount = 0;

                    }
                    else if (response != null && response.status == Constant.Status111 && response.message.non_field_errors != null)
                    {
                        checkInTitle.Text = Constant.CheckInNotProperTitleMessage;
                        checkInMessage.Text = Constant.CheckInNotProperMessage;
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
        async void GetCurrentLocation()
        {
            try
            {
                App.ShowMainPageLoader();
                var position = await Utils.GetCurrentLocation();
                if (position.Latitude != 0)
                {
                    eventViewModel.eventLat = position.Latitude.ToString();
                    eventViewModel.eventLong = position.Longitude.ToString();
                    eventViewModel.currenteventLat = position.Latitude.ToString();
                    eventViewModel.currenteventLong = position.Longitude.ToString();
                    ClearFields();
                    //eventViewModel.GetLocBasedEvents();
                    App.HideMainPageLoader();
                }
                else
                {
                    await App.LocationOn();
                    //await App.Instance.Alert("please turn your device Location on!", Constant.AlertTitle, Constant.Ok);
                    isPageFirstLoad = false;
                    App.HideMainPageLoader();
                }
            }
            catch (Exception)
            {
                App.HideMainPageLoader();
                isPageFirstLoad = false;
                eventViewModel.IsLoading = false;
                await App.Instance.Alert("Problem in fetching location!", Constant.AlertTitle, Constant.Ok);

            }
        }

        async void GetPermission()
        {
            try
            {
               
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        await App.Instance.Alert("Need location", Constant.AlertTitle, Constant.Ok);
                        App.HideMainPageLoader();
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    status = results[Permission.Location];
                }
                if (status == PermissionStatus.Granted)
                {
                    GetCurrentLocation();
                }
                else if (status == PermissionStatus.Unknown)
                {
                   // await App.Instance.Alert("Allow location permission access to view events happening around you", Constant.AlertTitle, Constant.Ok);
                    await Xamarin.Essentials.Permissions.RequestAsync<Xamarin.Essentials.Permissions.LocationWhenInUse>();
                   // CrossPermissions.Current.OpenAppSettings();
                    App.HideMainPageLoader();
                }
                else if (status != PermissionStatus.Unknown)
                {
                    await App.Instance.Alert("Location Denied! Can not continue, try again.", Constant.AlertTitle, Constant.Ok);
                    App.HideMainPageLoader();

                }
                else
                {
                   // await App.Instance.Alert("Allow location permission access to view events happening around you", Constant.AlertTitle, Constant.Ok);
                    await Xamarin.Essentials.Permissions.RequestAsync<Xamarin.Essentials.Permissions.LocationWhenInUse>();
                    //CrossPermissions.Current.OpenAppSettings();
                    App.HideMainPageLoader();

                }
                App.HideMainPageLoader();
                eventViewModel.IsLoading = false;
            }
            catch (Exception)
            {
                await App.Instance.Alert("Please Turn your Location on!.", Constant.AlertTitle, Constant.Ok);
                App.HideMainPageLoader();
                eventViewModel.IsLoading = false;
                //await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                //App.HideMainPageLoader();
            }
        }
        //List Button Tapped
        private void ListTapped(object sender, EventArgs e)
        {
            try
            {
                mapFilter.IsVisible = false;
                listFilter.IsVisible = true;
                listViewEvents.IsVisible = true;
                stackMap.IsVisible = false;
                entryVenue.Placeholder = "Search Upcoming Events";
                SetInitial();
            }
            catch (Exception ex)
            {
                return;
            }
        }
        //Map Button Tapped
        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new Page1());
                //btnList.BackgroundColor = Color.Gray;
                //btnMap.BackgroundColor = Color.HotPink;
                //imgCurrentLocation.IsVisible = true;
                //eventViewModel.listURL = FilterPicker.SelectedIndex == 0 || FilterPicker.SelectedIndex == -1 ? Constant.MapLatLongBasedEventsUrl : Constant.MapPopularityBasedEventsUrl;
                //eventViewModel.pageNoLocBasedEvents = 1;
                //eventViewModel.tempLocBasedEventList.Clear();
                //await eventViewModel.GetMapLocBasedEvents();
                //SelectMap(eventViewModel.tempLocBasedEventList);
                //listViewEvents.IsVisible = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            ClearFields();
            eventViewModel.IsAddressListVisible = false;
            eventViewModel.FilterLocType = "Recent";
            eventViewModel.listURL = Constant.LatLongBasedEventsUrl;
            eventViewModel.GetAllUpComingEvents();
            App.HideMainPageLoader();
            GetCurrentLoc();
            stackNoEvent.IsVisible = false;
        }

        private void CameraCancel_Clicked(object sender, EventArgs e)
        {
            stackPopUp.IsVisible = false;
            grdOverlayDialog.IsVisible = false;
        }

        async void Pick_Photo_Tapped(object sender, System.EventArgs e)
        {
            try
            {
                if (_tapCount < 1)
                {
                    if (!CrossConnectivity.Current.IsConnected)
                    {
                        await Application.Current.MainPage.DisplayAlert(Constant.AlertTitle, Constant.NetworkNotificationHeader, Constant.Ok);
                        _tapCount = 0;
                    }
                    else
                    {
                        grdOverlayDialog.IsVisible = false;
                        stackPopUp.IsVisible = false;
                        //profileViewModel.IsLoading = true;
                        if (!CrossMedia.Current.IsPickPhotoSupported)
                        {
                            await Application.Current.MainPage.DisplayAlert(Constant.NotAllowedText, Constant.GalleryAccessMessage, Constant.Ok);
                           // profileViewModel.IsLoading = false;
                            _tapCount = 0;
                            return;
                        }
                        else
                        {
                            await App.GetStoragePermission();
                            ImageSource IOSImageSource = null;
                            var profilePickedFile = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
                            {
                                PhotoSize = PhotoSize.Small,
                                CustomPhotoSize = 50,
                                CompressionQuality = 50,
                                SaveMetaData = false

                            });

                            if (profilePickedFile == null)
                            {
                                //profileViewModel.IsLoading = false;
                                _tapCount = 0;
                                return;
                            }
                            if (Device.RuntimePlatform == Device.iOS)
                            {
                                IOSImageSource = ImageSource.FromFile(profilePickedFile.Path);
                            }
                            App.ImageStream = profilePickedFile.GetStream();
                            if (Device.RuntimePlatform == Device.iOS)
                            {
                                App.ImageByteStream = PageHelper.ReadFully(profilePickedFile.GetStreamWithImageRotatedForExternalStorage());
                            }
                            else
                            {
                                App.ImageByteStream = PageHelper.ReadFully(profilePickedFile.GetStream());
                            }
                            //imgUser.Source = ImageSource.FromStream(() =>
                            //{
                            //    var stream = Device.RuntimePlatform == Device.iOS ? profilePickedFile.GetStreamWithImageRotatedForExternalStorage() : profilePickedFile.GetStream();
                            //    profilePickedFile.Dispose();
                            //    return stream;
                            //});
                            //profileViewModel.IsLoading = false;
                        }
                    }
                }
            }
            catch (Exception)
            {
                // await App.Instance.Alert("Allow Camera and storage permission access to access photos and videos", Constant.AlertTitle, Constant.Ok);
                //CrossPermissions.Current.OpenAppSettings();
                //profileViewModel.IsLoading = false;
                return;
            }
        }

        private void Take_Photo_Tapped(object sender, EventArgs e)
        {

        }

        private void UploadImagetapped(object sender, EventArgs e)
        {
            //grdOverlayDialog.IsVisible = true;
            //stackPopUp.IsVisible = true;
        }

        private void CrossIcon_Tapped(object sender, EventArgs e)
        {
            entryVenue.Text = "";
        }
    }
}

