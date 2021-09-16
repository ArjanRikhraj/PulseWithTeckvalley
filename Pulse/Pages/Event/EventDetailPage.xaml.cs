using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageCircle.Forms.Plugin.Abstractions;
using Plugin.Connectivity;
using Plugin.Geolocator;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Pulse.Helpers;
using Pulse.Pages;
using Pulse.Pages.Event;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Pulse
{
	public partial class EventDetailPage : BaseContentPage
	{
		#region Private variables
		readonly EventViewModel eventViewModel;
		int _tapCount = 0;
		string selectedEventId;
        bool commentTapped;
		bool isPastEvent;
		string fileGotFrom;
		int fileId;
		#endregion
		#region Constructor
		public EventDetailPage(string id)
		{
			InitializeComponent();
			eventViewModel = ServiceContainer.Resolve<EventViewModel>();
			BindingContext = eventViewModel;
			selectedEventId = id;
			eventViewModel.CommentToPost = string.Empty;
			eventViewModel.SelectedFriendsList.Clear();
			gridSelectedGuests.Children.Clear();
			SetInitialValues();
		}
		#endregion
		#region Override methods
		protected override void OnAppearing()
		{
			base.OnAppearing();
            eventViewModel.IsJoinEvent = false;
			CreateAttendeeGrid();
			CreateMediaGrid();
			CreateGuestGrid();
		}

		#endregion
		#region Methods
		async private void SetInitialValues()
		{
			if (Device.RuntimePlatform == Device.Android)
			{
				stackMap.HeightRequest = 180;
			}
            if (Device.RuntimePlatform == Device.iOS)
            {
                
            }
			editorComments.Placeholder = "Write a comment…";
            //btnReport.IsEnabled = !eventViewModel.IsReportedSpam;
			DrawPin();
		}

		async void PostCommentTapped(object sender, System.EventArgs e)
		{
			bool isPosted = await eventViewModel.PostComment();
			if (isPosted)
			{
				eventViewModel.IsLoading = true;
				eventViewModel.CommentToPost = string.Empty;
				eventViewModel.SelectedFriendsList.Clear();
				gridSelectedGuests.Children.Clear();
				eventViewModel.FetchEventDetail(Convert.ToString(eventViewModel.TappedEventId), false);
				ShowToast(Constant.AlertTitle, Constant.CommentPostedSuccessfully);
				//await App.Instance.Alert(Constant.CommentPostedSuccessfully, Constant.AlertTitle, Constant.Ok);
			}
		}

		void CreateMediaGrid()
		{
			if (eventViewModel.EventMediaList != null && eventViewModel.EventMediaList.Count > 0)
			{
				var count = eventViewModel.EventMediaList.Count;
				gridMedia.Children.Clear();
				gridMedia.ColumnDefinitions.Clear();
				gridMedia.RowDefinitions.Clear();
				gridMedia.Margin = new Thickness(0, 5, 0, 5);
				Image imageVideo = new Image()
				{
					Source = Constant.VideoIcon,                  
                    HeightRequest = 30,
                    WidthRequest = 30,
				};

				if (count == 1)
				{
					Image image = new Image()
					{
						HeightRequest = App.ScreenWidth / 2 +2,
						WidthRequest = App.ScreenWidth,
                        Aspect = Aspect.AspectFill,
						Source = eventViewModel.EventMediaList[0].file_type == 1 ? PageHelper.GetEventVideoThumbnail(eventViewModel.EventMediaList[0].file_thumbnail) : PageHelper.GetEventImage(eventViewModel.EventMediaList[0].file_name)
					};
					gridMedia.Children.Add(image, 0, 0);
					if (eventViewModel.EventMediaList[0].file_type == 1)
					{
						gridMedia.Children.Add(imageVideo, 0, 0);
					}
				}
				else if (count == 2)
				{
					for (int i = 0; i < count; i++)
					{
						Image image = new Image()
						{
							HeightRequest = App.ScreenWidth / 2 + 2,
							WidthRequest = App.ScreenWidth / 2,
                            Aspect = Aspect.AspectFill,
							Source = eventViewModel.EventMediaList[i].file_type == 1 ? PageHelper.GetEventVideoThumbnail(eventViewModel.EventMediaList[i].file_thumbnail) : PageHelper.GetEventImage(eventViewModel.EventMediaList[i].file_name)
						};
						gridMedia.Children.Add(image, i, 0);
						if (eventViewModel.EventMediaList[i].file_type == 1)
						{
							gridMedia.Children.Add(imageVideo, 0, 0);
						}
					}
				}
				else
				{
					gridMedia.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });
					gridMedia.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });
					gridMedia.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60, GridUnitType.Star) });
					gridMedia.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(40, GridUnitType.Star) });
					Image image1 = new Image()
					{
						HeightRequest = App.ScreenWidth / 2 +2,
						WidthRequest = App.ScreenWidth / 1.5,
                        Aspect = Aspect.AspectFill,
						Source = eventViewModel.EventMediaList[0].file_type == 1 ? PageHelper.GetEventVideoThumbnail(eventViewModel.EventMediaList[0].file_thumbnail) : PageHelper.GetEventImage(eventViewModel.EventMediaList[0].file_name)

					};

					Image image2 = new Image()
					{
						HeightRequest = App.ScreenWidth / 4 +2,
						WidthRequest = App.ScreenWidth / 2.5,
                        Aspect = Aspect.AspectFill,
						Source = eventViewModel.EventMediaList[1].file_type == 1 ? PageHelper.GetEventVideoThumbnail(eventViewModel.EventMediaList[1].file_thumbnail) : PageHelper.GetEventImage(eventViewModel.EventMediaList[1].file_name)

					};
					Image image3 = new Image()
					{
						HeightRequest = App.ScreenWidth / 4 +2,
						WidthRequest = App.ScreenWidth / 2.5,
                        Aspect = Aspect.AspectFill,
						Source = eventViewModel.EventMediaList[2].file_type == 1 ? PageHelper.GetEventVideoThumbnail(eventViewModel.EventMediaList[2].file_thumbnail) : PageHelper.GetEventImage(eventViewModel.EventMediaList[2].file_name)

					};
					gridMedia.Children.Add(image1, 0, 0);
					Grid.SetRowSpan(image1, 2);
					gridMedia.Children.Add(image2, 1, 0);
					gridMedia.Children.Add(image3, 1, 1);
					if (eventViewModel.EventMediaList[0].file_type == 1)
					{
						gridMedia.Children.Add(imageVideo, 0, 0);
						Grid.SetRowSpan(imageVideo, 2);
					}
					else if (eventViewModel.EventMediaList[1].file_type == 1)
					{
						gridMedia.Children.Add(imageVideo, 1, 0);
					}
					else if (eventViewModel.EventMediaList[2].file_type == 1)
					{
						gridMedia.Children.Add(imageVideo, 1, 1);
					}
				}
			}
		}

		void CreateAttendeeGrid()
		{
			if (eventViewModel.EventAttendeeList != null && eventViewModel.EventAttendeeList.Count > 0)
			{
				for (int i = 0; i < eventViewModel.EventAttendeeList.Count; i++)
				{
					CircleImage userImage = new CircleImage
					{
						HeightRequest = 36,
						WidthRequest = 36,
						Margin = new Thickness(0, 0, 0, 0),
						Aspect = Aspect.AspectFill,
						Source = !string.IsNullOrEmpty(eventViewModel.EventAttendeeList[i].profile_image) ? PageHelper.GetUserImage(eventViewModel.EventAttendeeList[i].profile_image) : Constant.UserDefaultSquareImage,
						BorderThickness = 2
					};
					gridGuests.Children.Add(userImage, i, 0);
					if (i == 6)
						return;
					gridGuests.IsVisible = true;
				}
			}
			else
				gridGuests.IsVisible = false;
		}
		void CreateGuestGrid()
		{
			gridSelectedGuests.Children.Clear();
			int column = 0;
			int row = 0;
			if (eventViewModel.SelectedFriendsList != null && eventViewModel.SelectedFriendsList.Count > 0)
			{
				foreach (var item in eventViewModel.SelectedFriendsList)
				{
					gridSelectedGuests.RowSpacing = 2;
					gridSelectedGuests.ColumnSpacing = 2;
					var tapGestureRecognizer = new TapGestureRecognizer();
					tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
					Frame myView = CreateGuestList(item);
					myView.GestureRecognizers.Add(tapGestureRecognizer);
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
				HeightRequest = 30,
				WidthRequest = 30,
				BorderThickness=2,
				Margin = new Thickness(0, 0, 0, 0),
				Aspect = Aspect.AspectFill,
				Source = friend.friendPic
			};
			Label userName = new Label
			{
				TextColor = Color.FromHex(Constant.AddEventEntriesColor),
				FontSize = 12,
				Margin = new Thickness(0, 2, 0, 2),
				LineBreakMode = LineBreakMode.TailTruncation,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Text = friend.friendUsername
			};
			Image userCross = new Image
			{
				Aspect = Aspect.AspectFit,
				Margin = new Thickness(0, 2, 0, 2),
				Source = Constant.CrossTagImage,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};
			stackLayout.Children.Add(userImage);
			stackLayout.Children.Add(userName);
			stackLayout.Children.Add(userCross);
			frame.Content = stackLayout;
			return frame;
		}
		async void TagFriends_Tapped(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					eventViewModel.IsLoading = true;
					await Navigation.PushModalAsync(new SearchFriendForEventPage("EventDetail"));
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
		async void CheckInClicked(object sender, System.EventArgs e)
		{
            try
            {

			var btn = (Button)sender;
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount++;
					eventViewModel.IsLoading = true;
					isPastEvent = false;
					stackPopUp.IsVisible = false;
					if (eventViewModel.currentActiveEventType == MyEventType.Upcoming)
					{
							if (btn.Text == Constant.CheckInText)
							{


								if (DateTimeValidate() && !isPastEvent)
								{
									bool isEnable = await GetCurrentLoc();
									if (isEnable)
									{
										CheckIn();
									}
									else
									{
										checkInTitle.Text = Constant.CheckInNotProperTitleMessage;
										checkInMessage.Text = Constant.CheckInNotProperMessage;
										grdOverlayDialog.IsVisible = true;
										stackcheckInMessage.IsVisible = true;
										_tapCount = 0;
									}
								}
								else if (!DateTimeValidate() && isPastEvent)
								{
									await App.Instance.Alert("You can not check-in past event", Constant.AlertTitle, Constant.Ok);
									eventViewModel.IsLoading = false;
									_tapCount = 0;
								}
								else
								{
									checkInTitle.Text = Constant.CheckInEarlyTitleMessage;
									checkInMessage.Text = Constant.CheckInEarlyMessage;
									grdOverlayDialog.IsVisible = false;
									stackcheckInMessage.IsVisible = true;
									_tapCount = 0;
								}
							}
							else if(btn.Text == Constant.JoinGuestListText)
							{
								eventViewModel.eventStatus = 3;
								await eventViewModel.EventAttendingConfirmation("Detail");
								//JoinEvent();
							}
                            else
                            {
								await Navigation.PushModalAsync(new EventsGuestListingPage());
							}
					}
					else
					{
						await App.Instance.Alert("You can not check-in past event", Constant.AlertTitle, Constant.Ok);
						eventViewModel.IsLoading = false;
						_tapCount = 0;

					}
					eventViewModel.IsLoading = false;
					_tapCount = 0;
					grdOverlayDialog.IsVisible = false;
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
				eventViewModel.IsLoading = false;
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
				_tapCount = 0;
			}
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
						eventViewModel.FetchEventDetail(eventViewModel.TappedEventId.ToString(),true);
						//await App.Instance.Alert("Successfully checked in", Constant.AlertTitle, Constant.Ok);
						eventViewModel.IsLoading = false;
						_tapCount = 0;

					}
					else if (response != null && response.status == Constant.Status111 && response.message.non_field_errors != null)
					{
						checkInTitle.Text = Constant.CheckInNotProperTitleMessage;
						checkInMessage.Text = Constant.CheckInNotProperMessage;
						//grdOverlayPopUp.IsVisible = true;
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
				eventViewModel.IsLoading = false;
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
				_tapCount = 0;

			}
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
		async Task<bool> GetCurrentLoc()
		{
			
			if (Device.RuntimePlatform == Device.iOS)
				return (await GetPermission());
			else
				return (await GetCurrentLocation());
		}
		async Task<bool> GetPermission()
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
					return (await GetCurrentLocation());
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
		async Task<bool> GetCurrentLocation()
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
		async void Delete_Tapped(object sender, System.EventArgs e)
		{
            stackPopUpForComment.IsVisible = false;
            grdOverlayDialog.IsVisible = false;
            var selected = (sender) as StackLayout;
			eventViewModel.TappedCommentId = selected.ClassId;
			bool result = await App.Instance.ConfirmAlert(Constant.DeleteCommentText, Constant.AlertTitle, Constant.Ok, Constant.CancelButtonText);
			if (result)
			{
				bool isDeleted = await eventViewModel.DeleteComment();
				if (isDeleted)
				{
					await eventViewModel.FetchEventDetail(Convert.ToString(eventViewModel.TappedEventId), false);
					ShowToast(Constant.AlertTitle, Constant.CommentDeleted);
					//await App.Instance.Alert(Constant.CommentDeleted, Constant.AlertTitle, Constant.Ok);
				}
			}
		}

		void DrawPin()
		{
			var pin = new CustomPin
			{
				Type = PinType.Place,
				Position = new Position(eventViewModel.eventLattitude, eventViewModel.eventLogitude),
				EventDateTime = eventViewModel.eventDateOnMap,
				EventName = eventViewModel.EventTitle,
				Label = eventViewModel.EventTitle,
				Address = eventViewModel.eventDateOnMap,
				Id = "Xamarin",
                IsBoostEvent = eventViewModel.IsBoostEvent
			};
			customMap.CustomPins = new List<CustomPin> { pin };
			customMap.Pins.Add(pin);
			customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(eventViewModel.eventLattitude, eventViewModel.eventLogitude), Distance.FromMiles(12.4274)));

		}

		async void Share_Button_Tapped(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount++;
                    stackPopUp.IsVisible = false;
                    grdOverlayDialog.IsVisible = false;
					eventViewModel.IsLoading = true;
					WriteQR();
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
        async void Report_Button_Clicked(object sender, System.EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (_tapCount < 1)
                {
                    _tapCount++;
					eventViewModel.IsReportPopupVisible = true;
                    stackPopUp.IsVisible = false;
                    grdOverlayDialog.IsVisible = false;
                    //await DisplayActionSheet();
                    _tapCount = 0;
                }
            }
            else
            {
                await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                _tapCount = 0;
            }
        }
         async void Report_Comment_Button_Clicked(object sender, System.EventArgs e)
        {
            stackPopUpForComment.IsVisible = false;
            grdOverlayDialog.IsVisible = false;
            if (CrossConnectivity.Current.IsConnected)
            {
                if (_tapCount < 1)
                {
                    _tapCount++;
                    await DisplayActionSheetForComment();
                    _tapCount = 0;
                }
            }
            else
            {
                await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                _tapCount = 0;
            }
        }
        async void CancelComment_Clicked(object sender, System.EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (_tapCount < 1)
                {
                    _tapCount++;
                    stackReport.IsVisible = false;
                    grdOverlayDialog.IsVisible = false;
                    _tapCount = 0;
                }
            }
            else
            {
                await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                _tapCount = 0;
            }
        }
        async System.Threading.Tasks.Task DisplayActionSheet()
        {
            var actionSheet = await DisplayActionSheet(Constant.ReportEventTitle, Constant.CancelText, null,  Constant.SpamText, Constant.NudityText,Constant.ViolenceText,Constant.HarassTxt,Constant.OtherText);
            switch (actionSheet)
            {
                case  Constant.SpamText :
                    eventViewModel.EvenetReportedData = Constant.SpamText;
                    await eventViewModel.ReportEvent();
                    break;

                case Constant.NudityText:
                    eventViewModel.EvenetReportedData = Constant.NudityText;
                    await eventViewModel.ReportEvent();

                    break;
                case Constant.ViolenceText:
                    eventViewModel.EvenetReportedData = Constant.ViolenceText;
                    await eventViewModel.ReportEvent();

                    break;
                case Constant.HarassTxt:
                    eventViewModel.EvenetReportedData = Constant.HarassTxt;
                    await eventViewModel.ReportEvent();

                    break;
                case Constant.OtherText:
                    stackReport.IsVisible = true;
                    grdOverlayDialog.IsVisible = true;
                    editorReportComment.Text = string.Empty;
                    editorReportComment.Placeholder = "write a comment...";
                    commentTapped = false;
                    break;

            }
        }
        async System.Threading.Tasks.Task DisplayActionSheetForComment()
        {
            eventViewModel.TappedCommentId = eventViewModel.CommentId;
            var actionSheet = await DisplayActionSheet(Constant.ReportEventTitle, Constant.CancelText, null, Constant.SpamText, Constant.NudityText, Constant.ViolenceText, Constant.HarassTxt, Constant.OtherText);
            switch (actionSheet)
            {
                case Constant.SpamText:
                    eventViewModel.EvenetCommentReportedData = Constant.SpamText;
                    await eventViewModel.ReportEventComment();

                    break;

                case Constant.NudityText:
                    eventViewModel.EvenetCommentReportedData = Constant.NudityText;
                    await eventViewModel.ReportEventComment();

                    break;
                case Constant.ViolenceText:
                    eventViewModel.EvenetCommentReportedData = Constant.ViolenceText;
                    await eventViewModel.ReportEventComment();

                    break;
                case Constant.HarassTxt:
                    eventViewModel.EvenetCommentReportedData = Constant.HarassTxt;
                    await eventViewModel.ReportEventComment();

                    break;
                case Constant.OtherText:
                    stackReport.IsVisible = true;
                    grdOverlayDialog.IsVisible = true;
                    editorReportComment.Text = string.Empty;
                    editorReportComment.Placeholder = "write a comment...";
                    commentTapped = true;
                    break;

            }
        }
		void WriteQR()
		{
			try
			{
				string QRData;
                QRData = "Event Name:" + eventViewModel.EventTitle + Environment.NewLine + ", Event Location:" + eventViewModel.LocationEvent + Environment.NewLine + ", Event Time:" + eventViewModel.eventDateOnMap + Environment.NewLine + (eventViewModel.IsBottleAmount? "Bottle Amount : " + eventViewModel.EventBottleAmount :string.Empty) + " and to get more details please download pulse app from Play Store or Apple Store.";
				if (Device.RuntimePlatform == Device.Android)
				{
					MessagingCenter.Send<string>(QRData, "Share");
				}
				else
				{
					DependencyService.Get<IQRCode>().Share(QRData);
				}
			}
			catch (Exception)
			{
				eventViewModel.IsLoading = false;
				App.Instance.Alert("Problem in generating QR code", Constant.AlertTitle, Constant.Ok);

			}
		}

        async void ReportEventCommentSubmit(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (string.IsNullOrEmpty(editorReportComment.Text))
                {
                    App.Instance.Alert("Please write a comment", Constant.AlertTitle, Constant.Ok);
                    eventViewModel.IsLoading = false;
                    _tapCount = 0;
                    return;
                }
                if (_tapCount < 1)
                {
                    _tapCount = 1;
                    eventViewModel.IsLoading = true;
                    ReportEvenetOrComment(editorReportComment.Text);
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
        async  void ReportEvenetOrComment(string comment)
        {
            if (!commentTapped)
            {
                eventViewModel.EvenetReportedData = editorReportComment.Text;
                await eventViewModel.ReportEvent();
            }
            else
            {
                eventViewModel.EvenetCommentReportedData = comment;
                eventViewModel.TappedCommentId = eventViewModel.CommentId;
                await eventViewModel.ReportEventComment();
            }
            grdOverlayDialog.IsVisible = false;
            stackReason.IsVisible = false;
            stackReport.IsVisible = false;
            eventViewModel.IsLoading = false;
            
        }

		async void Back_Button_Tapped(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
                    App.ShowMainPageLoader();
                    eventViewModel.pageNoLocBasedEvents = 1;
                    eventViewModel.totalLocBasedPages = 1;
                    eventViewModel.tempLocBasedEventList.Clear();
                    grdOverlay.IsVisible = true;
                    eventViewModel.IsLoading = true;
                    await eventViewModel.GetLocEvents();
					App.GetUnreadNotification();
					await Navigation.PopModalAsync();
                    grdOverlay.IsVisible = false;
                    eventViewModel.IsLoading = false;
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

		void Comment_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
		{
			if (!string.IsNullOrEmpty(e.NewTextValue))
			{
				if (editorComments.Text.Length > 200)
				{
					editorComments.Text = e.OldTextValue;
				}
				else
				{
					editorComments.Text = e.NewTextValue;
				}
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



		async void InterestedOption_Tapped(object sender, System.EventArgs e)

		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					eventViewModel.IsLoading = true;
					if (eventViewModel.EventToDate.Date == DateTime.Now.Date && eventViewModel.EventToTime < DateTime.Now.TimeOfDay)
					{
						await App.Instance.Alert("You can not show any of interest in past event", Constant.AlertTitle, Constant.Ok);
						eventViewModel.IsLoading = false;
						_tapCount = 0;
					}
					else
					{
						eventViewModel.eventStatus = 2;
						await eventViewModel.EventAttendingConfirmation("Detail");
					}
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

		async void NotInterestedOption_Tapped(object sender, System.EventArgs e)

		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					eventViewModel.IsLoading = true;
					if (eventViewModel.EventToDate.Date == DateTime.Now.Date && eventViewModel.EventToTime < DateTime.Now.TimeOfDay)
					{
						await App.Instance.Alert("You can not show any of interest in past event", Constant.AlertTitle, Constant.Ok);
						eventViewModel.IsLoading = false;
						_tapCount = 0;
					}
					else
					{
						eventViewModel.eventStatus = 1;
						await eventViewModel.EventAttendingConfirmation("Detail");
					}
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

		async void ViewCommentsTapped(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					eventViewModel.IsLoading = true;
					await Navigation.PushModalAsync(new CommentListingPage());
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

		async void JoinEvent()
        {
			if (CrossConnectivity.Current.IsConnected)
			{
					_tapCount = 1;
					eventViewModel.IsLoading = true;
					if (eventViewModel.EventToDate.Date == DateTime.Now.Date && eventViewModel.EventToTime < DateTime.Now.TimeOfDay)
					{
						await App.Instance.Alert("You can not join past event", Constant.AlertTitle, Constant.Ok);
						eventViewModel.IsLoading = false;
						_tapCount = 0;
					}
					else
					{
						if (eventViewModel.IsCoverAmount || eventViewModel.IsBottleAmount)
						{
							eventViewModel.IsJoinEvent = true;
							await Navigation.PushModalAsync(new JoinEventPage());
						}
						else
						{
							eventViewModel.eventStatus = 3;
							await eventViewModel.EventAttendingConfirmation("Detail");
						}
					}
					eventViewModel.IsLoading = false;
					_tapCount = 0;
			}
			else
			{
				await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
				_tapCount = 0;
			}
		}
		async void Join_Clicked(object sender, System.EventArgs e)
		{
            try
            {
				var btn = (Button)sender;
				if (btn.Text == Constant.ViewTicketText)
					await Navigation.PushModalAsync(new EventsGuestListingPage());
				else
				JoinEvent();
			}
            catch (Exception)
            {
				await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
				_tapCount = 0;
			}
		}

		async void GridMedia_Tapped(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					eventViewModel.IsLoading = true;
					await Navigation.PushModalAsync(new EventGalleryPage());
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
		async void PartyStory_Clicked(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					eventViewModel.IsLoading = true;
					await Navigation.PushModalAsync(new EventStoriesPage());
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
            stackReport.IsVisible = false;
			grdOverlayDialog.IsVisible = false;
            eventViewModel.IsLoading = false;
			stackReason.IsVisible = false;
		}
        void Cancel_Clicked_Commentpopup(object sender, System.EventArgs e)
        {
            stackPopUpForComment.IsVisible = false;
            stackReport.IsVisible = false;
            grdOverlayDialog.IsVisible = false;
            eventViewModel.IsLoading = false;
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
                    eventViewModel.SelectedFriendsList.Clear();
                    if (eventViewModel.EventAttendeeList != null && eventViewModel.EventAttendeeList.Count > 0)
                    {
                        foreach (var invitee in eventViewModel.EventAttendeeList)
                        {
                            eventViewModel.SelectedFriendsList.Add(new Friend { friendId = invitee.id, IsCrossIconNotVisible = true, friendFullname = invitee.fullname, friendPic = invitee.friend_image });
                        }
                    }
                    eventViewModel.IsUpdateBoostEvent = false;
                    eventViewModel.UpdatedSelectedFriendsList.Clear();
					await Navigation.PushModalAsync(new EditEventPage(Convert.ToString(eventViewModel.TappedEventId)));
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
        void ReportEditor_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (editorReportComment.Text.Length >= 150)
            {
                editorReportComment.Text = editorReportComment.Text.Remove(editorReportComment.Text.Length - 1);  // Remove Last character

            }
        }

		async void CancelEvent_Clicked(object sender, System.EventArgs e)
		{
			stackPopUp.IsVisible = false;
			await eventViewModel.CancelEvent();
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

		async void Direction_Tapped(object sender, System.EventArgs e)
		{
			try
			{
				if (CrossConnectivity.Current.IsConnected)
				{
					switch (Device.RuntimePlatform)
					{
						case Device.iOS:
							var uri = string.Format(Constant.iOSGoogleNavigation, Convert.ToString(eventViewModel.eventLattitude), Convert.ToString(eventViewModel.eventLogitude));
							Device.OpenUri(new Uri(string.Format(Constant.iOSGoogleNavigation, Convert.ToString(eventViewModel.eventLattitude), Convert.ToString(eventViewModel.eventLogitude))));
							break;
						case Device.Android:
							Device.OpenUri(new Uri(string.Format(Constant.androidGoogleNavigation, Convert.ToString(eventViewModel.eventLattitude), Convert.ToString(eventViewModel.eventLogitude))));
							break;
						default:
							break;
					}
				}
				else
				{
					await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
				}
			}
			catch (Exception)
			{
				await App.Instance.Alert("Problem in device maps", Constant.AlertTitle, Constant.Ok);
			}

		}

		void EventEditTapped(object sender, System.EventArgs e)
		{
			stackPopUp.IsVisible = true;
			grdOverlayDialog.IsVisible = true;
		}

        void Option_Tapped(object sender, System.EventArgs e)
        {
            stackPopUpForComment.IsVisible = true;
            grdOverlayDialog.IsVisible = true;
        }


		void FetchComment(List<CommentResponse> comments)
		{
			if (comments != null && comments.Count > 0)
			{
				eventViewModel.IsStackCommentVisible = true;
				eventViewModel.CommenteeName = comments[0].user_info.fullname;
				eventViewModel.CommentText = comments[0].comment_text;
				eventViewModel.CommentDate = DateTime.Parse(comments[0].send_date).ToLocalTime().ToString("ddd,dd MMM, h:mm tt");
				eventViewModel.CommentId = Convert.ToString(comments[0].id);
				eventViewModel.IsCommentDeleteIconVisible = comments[0].is_owner ? true : false;
				eventViewModel.CommenteeImage = !string.IsNullOrEmpty(comments[0].user_info.profile_image) ? PageHelper.GetUserImage(comments[0].user_info.profile_image) : Constant.UserDefaultSquareImage;
			}
			else
			{
				eventViewModel.IsStackCommentVisible = false;
			}
		}
		void OverLayTapped(object sender, System.EventArgs e)
		{
			grdOverlayPopUp.IsVisible = false;
			grdOverlayDialog.IsVisible = false;
			stackUploadImagePopUp.IsVisible = false;
			stackcheckInMessage.IsVisible = false;
			stackInnerPopUp.IsVisible = false;
		}
       

        private void Camera_Tapped(object sender, EventArgs e)
        {
			grdOverlayDialog.IsVisible = false;
			stackUploadImagePopUp.IsVisible = false;
			stackInnerPopUp.IsVisible = true;
			grdOverlayPopUp.IsVisible = true;
			fileGotFrom = "Take Photo";
		}

        private void Gallery_Tapped(object sender, EventArgs e)
        {
			grdOverlayDialog.IsVisible = false;
			stackUploadImagePopUp.IsVisible = false;
			stackInnerPopUp.IsVisible = true;
			grdOverlayPopUp.IsVisible = true;
			fileGotFrom = "Choose Photo";
		}

        private async void Photo_Tapped(object sender, EventArgs e)
        {
			stackInnerPopUp.IsVisible = false;
			grdOverlayPopUp.IsVisible = false;
			if (fileGotFrom.Equals("Take Photo"))
			{
				await TakePhoto();
			}
			else
			{
				await PickPhoto();
			}
		}
		async Task TakePhoto()
		{
			try
			{
				if (_tapCount < 1)
				{
					if (!CrossConnectivity.Current.IsConnected)
					{
						await DisplayAlert(Constant.AlertTitle, Constant.NetworkNotificationHeader, Constant.Ok);
						_tapCount = 0;
					}
					else
					{
						eventViewModel.IsLoading = true;
						if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
						{
							await DisplayAlert(Constant.NoCameraText, Constant.CameraNotAvalaibleMessage, Constant.Ok);
							eventViewModel.IsLoading = false;
							_tapCount = 0;
						}
						else
						{
							grdOverlayDialog.IsVisible = false;
							stackPopUp.IsVisible = false;
							eventViewModel.IsLoading = true;
							await App.GetCameraPermission();
							await App.GetStoragePermission();
							var profileFile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
							{
								PhotoSize = PhotoSize.Medium,
								AllowCropping = true,
								SaveMetaData = false
							});

							if (profileFile == null)
							{
								eventViewModel.IsLoading = false;
								_tapCount = 0;
							}
							ImageSource imgSource = ImageSource.FromStream(() =>
							{
								var stream = profileFile.GetStream();
								return stream;
							});
							fileId++;
							string fileName = Guid.NewGuid() + Path.GetExtension(profileFile.Path);
							UploadImage(fileName, profileFile.GetStream());
						}
					}
				}
				else
				{
					eventViewModel.IsLoading = false;
				}
			}
			catch (Exception)
			{
				// await App.Instance.Alert("Allow Camera and storage permission access to take photos and videos", Constant.AlertTitle, Constant.Ok);
				// CrossPermissions.Current.OpenAppSettings();
				eventViewModel.IsLoading = false;
			}
		}
		async Task PickPhoto()
		{
			try
			{
				if (_tapCount < 1)
				{
					if (!CrossConnectivity.Current.IsConnected)
					{
						await DisplayAlert(Constant.AlertTitle, Constant.NetworkNotificationHeader, Constant.Ok);
						_tapCount = 0;
					}
					else
					{
						grdOverlayDialog.IsVisible = false;
						stackPopUp.IsVisible = false;
						eventViewModel.IsLoading = true;
						if (!CrossMedia.Current.IsPickPhotoSupported)
						{
							await DisplayAlert(Constant.NotAllowedText, Constant.GalleryAccessMessage, Constant.Ok);
							eventViewModel.IsLoading = false;
							_tapCount = 0;
						}
						else
						{
							await App.GetStoragePermission();
							var storyPickedFile = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
							{
								PhotoSize = PhotoSize.Medium

							});

							if (storyPickedFile == null)
							{
								eventViewModel.IsLoading = false;
								_tapCount = 0;
							}
							ImageSource imgSource = ImageSource.FromStream(() =>
							{
								var stream = storyPickedFile.GetStream();
								return stream;
							});
							fileId++;
							string fileName = Guid.NewGuid() + Path.GetExtension(storyPickedFile.Path);
							UploadImage(fileName, storyPickedFile.GetStream());
						}
					}
				}
				else
				{
					eventViewModel.IsLoading = false;
				}
			}
			catch (Exception)
			{
				// await App.Instance.Alert("Allow Camera and storage permission access to get photos and videos", Constant.AlertTitle, Constant.Ok);
				// CrossPermissions.Current.OpenAppSettings();
				eventViewModel.IsLoading = false;
			}
		}
		async void UploadImage(string fileName, Stream stream)
		{
			bool isUploaded = await new AWSServices().UploadAWSFile(stream, App.AWSCurrentDetails.response.images_path.event_images, fileName);
			if (isUploaded)
			{
				AddPhotoVideo(fileName, 0);
			}
		}
		async void AddPhotoVideo(string fileName, int fileType)
		{
			try
			{
				if (!CrossConnectivity.Current.IsConnected)
				{
					await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
				}
				else
				{
					eventViewModel.IsLoading = true;
					CoverImageRequest request = new CoverImageRequest();
					request.event_id = eventViewModel.TappedEventId;
					Media media = new Media();
					media.file_name = fileName;
					request.media = media;
					if (fileType == 1)
					{
						//livemedia.file_thumbnail = thumbNailName;
					}
					var response = await new MainServices().Post<ResultWrapperSingle<CoverImageResponse>>(Constant.UploadEventCoverImage + '/', request);
					if (response != null && response.status == Constant.Status200 && response.response != null)
					{
					    await eventViewModel.FetchEventDetail(eventViewModel.TappedEventId.ToString(), false);
						ShowToast(Constant.AlertTitle, "Successfully Uploaded");
						eventViewModel.GetAllUpComingEvents();
						_tapCount = 0;
						eventViewModel.IsLoading = false;
					}
					else if (response != null && response.status == Constant.Status111 && response.message.non_field_errors != null)
					{
						grdOverlayPopUp.IsVisible = true;
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
				eventViewModel.IsLoading = false;
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
				_tapCount = 0;

			}
		}
        #endregion

        private void ExtendedButton_Clicked(object sender, EventArgs e)
        {
			grdOverlayDialog.IsVisible = true;
			stackUploadImagePopUp.IsVisible = true;
		}

        private void Report_Media_Clicked(object sender, EventArgs e)
        {
			eventViewModel.IsReportPopupVisible = !eventViewModel.IsReportPopupVisible;
        }
    }
}
