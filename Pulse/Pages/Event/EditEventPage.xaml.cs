using System;
using System.Linq;
using ImageCircle.Forms.Plugin.Abstractions;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Pulse
{
	public partial class EditEventPage : BaseContentPage
	{
		#region Private variables
		int _tapCount = 0;
		readonly EventViewModel eventViewModel;
		#endregion
		#region constructor
		public EditEventPage(string id)
		{
			InitializeComponent();
			eventViewModel = ServiceContainer.Resolve<EventViewModel>();
			BindingContext = eventViewModel;
			SetInitialValues(id);
		}

		#endregion
		#region Methods
		protected override void OnAppearing()
		{
			eventViewModel.IsAddressListVisible = false;
			eventViewModel.IsSearchItemSelected = true;
            eventViewModel.IsUpdateEvent = true;
            editorDetails.Placeholder = !string.IsNullOrEmpty(eventViewModel.Description) ? string.Empty : Constant.DescriptionText;
            CreateGuestGrid();
		}

		async void SetInitialValues(string id)
		{
			try
			{
				if (Device.RuntimePlatform == Device.Android)
				{
					topStack.Margin = new Thickness(0, 10, 10, 10);
					editorDetails.Margin = new Thickness(-2, 0, 0, -5);
					editorVenue.Margin = new Thickness(-2, 0, 0, -5);
					txtAddress.Margin = new Thickness(37, 0, 0, 15);
					box.Margin = new Thickness(15, 6, 0, 0);
					editorDetails.HeightRequest = 55;
				}
				lstPlaces.ItemTapped += lstPlacesItemTapped;
				dtPkrFromDate.MinimumDate = DateTime.Now.Date;
				dtPkrToDate.MinimumDate = DateTime.Now.Date;
				timePkrToTime.Time = DateTime.Now.TimeOfDay;
				timePkrFromTime.Time = DateTime.Now.TimeOfDay;
				timePkrFromTime.PropertyChanged += TimePkrFromTime_PropertyChanged;
				timePkrToTime.PropertyChanged += TimePkrToTime_PropertyChanged;
				dtPkrFromDate.DateSelected += FromDate_SelectedChanged;
				dtPkrToDate.DateSelected += ToDate_PropertyChanged;
                grdOverlay.IsVisible = true;
				await eventViewModel.FetchEventDetail(id, false);
				await eventViewModel.GetTimeZones();
                grdOverlay.IsVisible = false;
				editorVenue.Text = eventViewModel.EventLocation;
                txtContactNumber.Text = eventViewModel.MobileNumber;
				labelFromDate.Text = eventViewModel.EventFromDate.ToString("dd MMM,yyyy");
				labelToDate.Text = eventViewModel.EventToDate.ToString("dd MMM,yyyy");
				labelTimeZone.Text = eventViewModel.TimeZoneType;
				eventViewModel.IsAddressListVisible = false;
				eventViewModel.IsSearchItemSelected = true;
              
               
            }
			catch (Exception ex)
			{
				await App.Instance.Alert(ex.Message, Constant.AlertTitle, Constant.Ok);
			}
		}
        void CreateGuestGrid()
        {
            
            gridSelectedGuests.Children.Clear();
            int column = 0;
            int row = 0;
            if (eventViewModel.SelectedFriendsList != null && eventViewModel.SelectedFriendsList.Count > 0)
            {
                gridNoGuests.IsVisible = false;
                stackguests.IsVisible = true;
                foreach (var item in eventViewModel.SelectedFriendsList)
                {
                    gridSelectedGuests.RowSpacing = 2;
                    gridSelectedGuests.ColumnSpacing = 2;
                    Frame myView = CreateGuestList(item);
                    if (!item.IsCrossIconNotVisible)
                    {
                        var tapGestureRecognizer = new TapGestureRecognizer();
                        tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
                       myView.GestureRecognizers.Add(tapGestureRecognizer);
                    }             
                    if (column < 1)
                    {
                        gridSelectedGuests.Children.Add(myView, column, row);
                        column++;
                    }
                    else
                    {
                        gridSelectedGuests.Children.Add(myView, column, row);
                        gridSelectedGuests.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });
                        row++;
                        column = 0;
                    }
                }
            }
            else
            {
                gridNoGuests.IsVisible = true;
                stackguests.IsVisible = false;
            }
        }
        void Boost_Event_Tapped(object sender, System.EventArgs e)
        {
            if (_tapCount < 1)
            {
                _tapCount = 1;
                isBoostUncheck.IsVisible = !isBoostUncheck.IsVisible;
                isBoostCheck.IsVisible = !isBoostCheck.IsVisible;
                lblBoostcharges.IsVisible = isBoostCheck.IsVisible;
                eventViewModel.IsUpdateBoostEvent = isBoostCheck.IsVisible;
                eventViewModel.IsBoostEvent = isBoostCheck.IsVisible;
                _tapCount = 0;
            }
        }

        void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var selectedItem = (Frame)sender;
            if (eventViewModel.SelectedFriendsList != null && eventViewModel.SelectedFriendsList.Count > 0 && eventViewModel.SelectedFriendsList.Any(i => i.friendId == Convert.ToInt32(selectedItem.ClassId)))
            {
                var listItem = eventViewModel.SelectedFriendsList.Where(i => i.friendId == Convert.ToInt32(selectedItem.ClassId)).FirstOrDefault();
                if (listItem != null)
                {
                    eventViewModel.SelectedFriendsList.Remove(listItem);
                }
            }
            CreateGuestGrid();
        }
        Frame CreateGuestList(Friend friend)
        {
            Frame frame = new Frame
            {
                HasShadow = false,
                BackgroundColor = Color.FromHex(Constant.TagsbackgroundColor),
                CornerRadius = 18,
                Margin = 0,
                Padding = 0,
                ClassId = Convert.ToString(friend.friendId)
            };
            StackLayout stackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 0,
                Margin = 0,
                Padding = new Thickness(10, 5, 10, 5),
                BackgroundColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand

            };
            CircleImage userImage = new CircleImage
            {
                HeightRequest = 23,
                WidthRequest = 23,
                Margin = new Thickness(0, 0, 0, 0),
                Aspect = Aspect.Fill,
                Source = string.IsNullOrEmpty(friend.friendPic)? friend.friendPic:Constant.ProfileIcon
            };
            Label userName = new Label
            {
                TextColor = Color.FromHex(Constant.AddEventEntriesColor),
                FontSize = 12,
                Margin = new Thickness(0, 2, 0, 2),
                LineBreakMode = LineBreakMode.TailTruncation,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Text = friend.friendFullname
            };
            Image userCross = new Image
            {
                Aspect = Aspect.AspectFit,
                Margin = new Thickness(0, 2, 0, 2),
                Source = Constant.CrossTagImage,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                IsVisible = !friend.IsCrossIconNotVisible
            };
            stackLayout.Children.Add(userImage);
            stackLayout.Children.Add(userName);
            stackLayout.Children.Add(userCross);
            frame.Content = stackLayout;
            return frame;

        }
        async void AddGuests_Tapped(object sender, System.EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (_tapCount < 1)
                {
                    _tapCount = 1;
                    eventViewModel.IsLoading = true;
                    await Navigation.PushModalAsync(new SearchFriendForEventPage("AddEvent"));
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
		void SetTimeZones()
		{
			if (eventViewModel.TimeZoneList != null)
			{
				if (timeZonePicker.Items.Count > 0)
				{
					timeZonePicker.Items.Clear();
				}
				foreach (var timeZone in eventViewModel.TimeZoneList)
				{
					timeZonePicker.Items.Add(timeZone);
				}
			}
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

		void lstPlacesItemTapped(object sender, ItemTappedEventArgs e)
		{
			var data = (SearchResultModel)lstPlaces.SelectedItem;
			if (data.Name != Constant.NoResultFound)
			{
				editorVenue.Unfocus();
				eventViewModel.ResultModel = ((SearchResultModel)lstPlaces.SelectedItem);
				editorVenue.Text = eventViewModel.EventLocation = eventViewModel.ResultModel.Name + eventViewModel.ResultModel.Vicinity;
				eventViewModel.IsSearchItemSelected = true;
				eventViewModel.IsAddressListVisible = false;
				eventViewModel.GetlatLong(data.place_id);
				lstPlaces.SelectedItem = null;
			}
		}
		void Venue_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
		{
			if (string.IsNullOrEmpty(e.NewTextValue))
			{
				lblSmallVenue.TextColor = Color.Transparent;
				eventViewModel.IsAddressListVisible = false;
			}
			else
			{
				if (editorVenue.Text.Length > 200)
				{
					editorVenue.Text = e.OldTextValue;
				}
				else
				{
					editorVenue.Text = e.NewTextValue;
				}
				lblSmallVenue.TextColor = Color.FromHex(Constant.AddEventEntriesColor);
				eventViewModel.Search(e);
				eventViewModel.IsSearchItemSelected = false;
			}
		}

		void Editor_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
		{
			if (string.IsNullOrEmpty(e.NewTextValue))
			{
				lblDetailsSmall.TextColor = Color.Transparent;
			}
			else
			{
				if (editorDetails.Text.Length >= 200)
				{
					editorDetails.Text = editorDetails.Text.Remove(editorDetails.Text.Length - 1);  // Remove Last character

				}
				lblDetailsSmall.TextColor = Color.FromHex(Constant.AddEventEntriesColor);
			}
		}

		void StartTimeZone_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (timeZonePicker.SelectedIndex >= 0)
			{
				labelTimeZone.Text = timeZonePicker.Items[timeZonePicker.SelectedIndex];
				eventViewModel.TimeZoneType = labelTimeZone.Text;
			}
		}


		void TimeZonePicker_Tapped(object sender, System.EventArgs e)
		{
			SetTimeZones();
			Device.BeginInvokeOnMainThread(() =>
				{
					if (timeZonePicker.IsFocused)
						timeZonePicker.Unfocus();
					timeZonePicker.Focus();
				});
		}



		void FromDate_Tapped(object sender, System.EventArgs e)
		{
			Device.BeginInvokeOnMainThread(() =>
				{

					if (dtPkrFromDate.IsFocused)
						dtPkrFromDate.Unfocus();

					dtPkrFromDate.Focus();
				});
		}

		void FromTime_Tapped(object sender, System.EventArgs e)
		{

			Device.BeginInvokeOnMainThread(() =>
				{
					if (timePkrFromTime.IsFocused)
						timePkrFromTime.Unfocus();
					timePkrFromTime.Focus();
				});
		}
		void ToDate_Tapped(object sender, System.EventArgs e)
		{
			Device.BeginInvokeOnMainThread(() =>
				{
					if (dtPkrToDate.IsFocused)
						dtPkrToDate.Unfocus();

					dtPkrToDate.Focus();
				});
		}

		void ToTime_Tapped(object sender, System.EventArgs e)
		{

			Device.BeginInvokeOnMainThread(() =>
				{
					if (timePkrToTime.IsFocused)
						timePkrToTime.Unfocus();
					timePkrToTime.Focus();
				});
		}
		void TimeFormat(TimeSpan Time, Label label)
		{
			var dateTime = new DateTime(Time.Ticks);
			var formattedTime = dateTime.ToString("HH:mm");
			label.Text = formattedTime;
		}

		void TimePkrFromTime_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName.Contains("Time"))
			{
				TimeFormat(eventViewModel.EventFromTime, labelFromTime);
			}
		}
		void TimePkrToTime_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName.Contains("Time"))
			{
				TimeFormat(eventViewModel.EventToTime, lblToTime);
			}
		}

		void FromDate_SelectedChanged(object sender, DateChangedEventArgs e)
		{
			labelFromDate.Text = eventViewModel.EventFromDate.ToString("dd MMM,yyyy");
		}

		void ToDate_PropertyChanged(object sender, Xamarin.Forms.DateChangedEventArgs e)
		{
			labelToDate.Text = eventViewModel.EventToDate.ToString("dd MMM,yyyy");
		}
		#endregion
	}
}
