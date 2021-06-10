using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageCircle.Forms.Plugin.Abstractions;
using Plugin.Connectivity;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Xamarin.Forms;

namespace Pulse
{
	public partial class AddEventPage : BaseContentPage
	{
		#region Private variables
		int _tapCount = 0;
		string fileGotFrom;
		bool isVideoSelected;
		int fileId = 0;
		readonly EventViewModel eventViewModel;
		ObservableCollection<MediaData> fileResult;
		#endregion
		#region constructor
		public AddEventPage()
		{
			InitializeComponent();
			eventViewModel = ServiceContainer.Resolve<EventViewModel>();
			BindingContext = eventViewModel;
			SetInitialValues();
		}
		#endregion

		#region Override methods
		protected override void OnAppearing()
		{
			base.OnAppearing();
			eventViewModel.BoostEventConfirmation = false;
			CreateGuestGrid();
			CreatePhotosGrid();
		}

		#endregion

		#region Methods
		async void SetInitialValues()
		{
			try
			{
				if (Device.RuntimePlatform == Device.Android)
				{
					topStack.Margin = new Thickness(0, 10, 10, 10);
					//editorDetails.Margin = new Thickness(-2, 0, 0, -5);
					//editorVenue.Margin = new Thickness(-2, 0, 0, -5);
					//txtAddress.Margin = new Thickness(37, 0, 0, 15);
					//box.Margin = new Thickness(15, 6, 0, 0);
					//editorDetails.HeightRequest = 55;
					farmePopUp.CornerRadius = 5;
				}
				dtPkrFromDate.MinimumDate = DateTime.Now.Date;
				dtPkrToDate.MinimumDate = DateTime.Now.Date;
				timePkrToTime.Time = DateTime.Now.TimeOfDay;
				timePkrFromTime.Time = DateTime.Now.TimeOfDay;
				timePkrFromTime.PropertyChanged += TimePkrFromTime_PropertyChanged;
				timePkrToTime.PropertyChanged += TimePkrToTime_PropertyChanged;
				dtPkrFromDate.DateSelected += FromDate_SelectedChanged;
				dtPkrToDate.DateSelected += ToDate_PropertyChanged;
				eventViewModel.ClearFields();
				eventViewModel.EventFromDate = DateTime.Now.Date;
				eventViewModel.EventToDate = DateTime.Now.AddDays(1);
				await eventViewModel.GetTimeZones();
				eventViewModel.IsAddressListVisible = false;
				eventViewModel.IsSearchItemSelected = false;
				eventViewModel.IsScreenFirstVisible = true;
				eventViewModel.IsScreenSecondVisible = false;
				//labelFromDate.Text = eventViewModel.EventFromDate.ToString("dd MMM,yyyy");
				//labelToDate.Text = DateTime.Now.AddDays(1).ToString("dd MMM,yyyy");
				if (eventViewModel.TimeZoneList != null && eventViewModel.TimeZoneList.Count > 0)
				{
					labelTimeZone.Text = eventViewModel.TimeZoneType = eventViewModel.TimeZoneList[0];
				}
				eventViewModel.IsLoading = false;
				//App.HideMainPageLoader();
			}
			catch (Exception ex)
			{
				await App.Instance.Alert(ex.Message, Constant.AlertTitle, Constant.Ok);
				eventViewModel.IsLoading = false;
				//App.HideMainPageLoader();
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
					eventViewModel.ClearFields();
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

		async void CreateGuestGrid()
		{
			try
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
						var tapGestureRecognizer = new TapGestureRecognizer();
						tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
						Frame myView = CreateGuestList(item);
						//myView.GestureRecognizers.Add(tapGestureRecognizer);
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
			catch (Exception ex)
			{
				await App.Instance.Alert(ex.Message, Constant.AlertTitle, Constant.Ok);
				eventViewModel.IsLoading = false;
				//App.HideMainPageLoader();
			}
		}

		async void CreatePhotosGrid()
		{
			try
			{
				gridSelectedPhotos.Children.Clear();
				int column = 0;
				int row = 0;
				if (eventViewModel.SelectedMediaList != null && eventViewModel.SelectedMediaList.Count > 0)
				{
					grdNoFiles.IsVisible = false;
					stackFiles.IsVisible = true;
					foreach (var item in eventViewModel.SelectedMediaList)
					{
						gridSelectedPhotos.RowSpacing = 2;
						gridSelectedPhotos.ColumnSpacing = 2;
						var tapGestureRecognizer = new TapGestureRecognizer();
						tapGestureRecognizer.Tapped += PhotosTapGestureRecognizer_Tapped;
						StackLayout myView = CreatPhotosList(item);
						//myView.GestureRecognizers.Add(tapGestureRecognizer);
						if (column < 2)
						{
							gridSelectedPhotos.Children.Add(myView, column, row);
							column++;
						}
						else
						{
							gridSelectedPhotos.Children.Add(myView, column, row);
							gridSelectedGuests.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });
							row++;
							column = 0;
						}
					}
				}
				else
				{
					grdNoFiles.IsVisible = true;
					stackFiles.IsVisible = false;
				}
			}
			catch (Exception ex)
			{
				await App.Instance.Alert(ex.Message, Constant.AlertTitle, Constant.Ok);
				eventViewModel.IsLoading = false;
				//App.HideMainPageLoader();
			}
		}

		StackLayout CreatPhotosList(MediaData item)
		{
			StackLayout stackLayout = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				Spacing = 0,
				Margin = 0,
				Padding = new Thickness(10, 5, 10, 5),
				BackgroundColor = Color.Transparent,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				ClassId = Convert.ToString(item.id)

			};
			Grid grid = new Grid()
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Margin = new Thickness(0, 0, 0, 0)
			};

			RoundImage userImage = new RoundImage
			{
				HeightRequest = 80,
				Margin = new Thickness(0, 0, 0, 0),
				Aspect = Aspect.AspectFill,
				Source = item.imageSource,
				BorderRadius = 18
			};
			Image videoImage = new Image
			{
				Aspect = Aspect.AspectFit,
				Source = Constant.PlayIcon,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center
			};
			Image userCross = new Image
			{
				Aspect = Aspect.AspectFit,
				Margin = new Thickness(12, -7, 0, 2),
				Source = Constant.PhotosCrossIcon,
				HorizontalOptions = LayoutOptions.End,
				VerticalOptions = LayoutOptions.Start,
				ClassId = item.id.ToString(),
			};
			var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped1;
			userCross.GestureRecognizers.Add(tapGestureRecognizer);
			grid.Children.Add(userImage, 0, 0);
			grid.Children.Add(userCross, 0, 0);
			if (item.FileType == 1) grid.Children.Add(videoImage, 0, 0);
			stackLayout.Children.Add(grid);
			return stackLayout;

		}

        private void TapGestureRecognizer_Tapped1(object sender, EventArgs e)
        {
			var selectedItem = (Image)sender;
			var listitem = eventViewModel.SelectedMediaList.Where(i => i.id == Convert.ToInt32(selectedItem.ClassId)).FirstOrDefault();
			if (listitem != null)
			{
				isVideoSelected = listitem.FileType == 1 ? false : isVideoSelected;
				eventViewModel.SelectedMediaList.Remove(listitem);
			}
			CreatePhotosGrid();
		}

        void PhotosTapGestureRecognizer_Tapped(object sender, EventArgs e)
		{
			var selectedItem = (StackLayout)sender;
			var listitem = eventViewModel.SelectedMediaList.Where(i => i.id == Convert.ToInt32(selectedItem.ClassId)).FirstOrDefault();
			if (listitem != null)
			{
				isVideoSelected = listitem.FileType == 1 ? false : isVideoSelected;
				eventViewModel.SelectedMediaList.Remove(listitem);
			}
			CreatePhotosGrid();
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
				HeightRequest = 28,
				WidthRequest = 28,
				Margin = new Thickness(0, 0, 0, 0),
				Aspect = Aspect.Fill,
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
				VerticalOptions = LayoutOptions.CenterAndExpand,
				ClassId = friend.friendId.ToString(),
			};
			var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped2;
			userCross.GestureRecognizers.Add(tapGestureRecognizer);
			stackLayout.Children.Add(userImage);
			stackLayout.Children.Add(userName);
			stackLayout.Children.Add(userCross);
			frame.Content = stackLayout;
			return frame;

		}

        private void TapGestureRecognizer_Tapped2(object sender, EventArgs e)
        {
			var selectedItem = (Image)sender;
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

        void CameraCancel_Clicked(object sender, System.EventArgs e)
		{
			stackPopUp.IsVisible = false;
			stackInnerPopUp.IsVisible = false;
			grdOverlayDialog.IsVisible = false;
			grdOverlay.IsVisible = false;
			grdOverlayPopUp.IsVisible = false;
		}

		void Venue_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
		{
			if (string.IsNullOrEmpty(e.NewTextValue))
			{
				//lblSmallVenue.TextColor = Color.Transparent;
				eventViewModel.IsAddressListVisible = false;
				eventViewModel.AddressListLoading = false;
				imageCross.IsVisible = false;
				editorVenue.Unfocus();
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
					imageCross.IsVisible = true;
				eventViewModel.Search(e);
				eventViewModel.IsSearchItemSelected = false;
			}
		}

		void Editor_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
		{
			if (string.IsNullOrEmpty(e.NewTextValue))
			{
				//lblDetailsSmall.TextColor = Color.Transparent;
			}
			else
			{
				//if (editorDetails.Text.Length >= 200)
				//{
				//	editorDetails.Text = editorDetails.Text.Remove(editorDetails.Text.Length - 1);  // Remove Last character

				//}
				//lblDetailsSmall.TextColor = Color.FromHex(Constant.AddEventEntriesColor);
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
				//TimeFormat(eventViewModel.EventFromTime, labelFromTime);
			}
		}
		void TimePkrToTime_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName.Contains("Time"))
			{
				//TimeFormat(eventViewModel.EventToTime, lblToTime);
			}
		}

		void FromDate_SelectedChanged(object sender, DateChangedEventArgs e)
		{
			//labelFromDate.Text = eventViewModel.EventFromDate.ToString("dd MMM,yyyy");
		}

		void ToDate_PropertyChanged(object sender, Xamarin.Forms.DateChangedEventArgs e)
		{
			//labelToDate.Text = eventViewModel.EventToDate.ToString("dd MMM,yyyy");

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

		async void AddPhotos_Tapped(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					if (gridSelectedPhotos.Children != null && gridSelectedPhotos.Children.Count == 6)
					{
						await App.Instance.Alert(Constant.CanNotAddMore, Constant.AlertTitle, Constant.Ok);
					}
					else
					{
						grdOverlayDialog.IsVisible = true;
						stackPopUp.IsVisible = true;
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

		void More_Tapped(object sender, System.EventArgs e)
		{
			if (_tapCount < 1)
			{
				_tapCount = 1;
				stackMore.IsVisible = !stackMore.IsVisible;
				//lblMore.Text = lblMore.Text.Equals(Constant.MoreText) ? Constant.LessText : Constant.MoreText;
				_tapCount = 0;
			}
		}

		void ImageCoverFee_Tapped(object sender, System.EventArgs e)
		{
			if (_tapCount < 1)
			{
				_tapCount = 1;
				isCoverFeeUncheck.IsVisible = !isCoverFeeUncheck.IsVisible;
				isCoverFeeCheck.IsVisible = !isCoverFeeCheck.IsVisible;
				gridCoverFee.IsVisible = isCoverFeeCheck.IsVisible;
				eventViewModel.isCoverFeeChecked = isCoverFeeCheck.IsVisible;
				//for Bottle Fee 
				gridBottleFee.IsVisible = isCoverFeeCheck.IsVisible;
				eventViewModel.isBottleFeeChecked = isCoverFeeCheck.IsVisible;
				_tapCount = 0;
			}
		}
		void ImageBottleFee_Tapped(object sender, System.EventArgs e)
		{
			if (_tapCount < 1)
			{
				_tapCount = 1;
				isBottleFeeUncheck.IsVisible = !isBottleFeeUncheck.IsVisible;
				isBottleFeeCheck.IsVisible = !isBottleFeeCheck.IsVisible;
				gridBottleFee.IsVisible = isBottleFeeCheck.IsVisible;
				eventViewModel.isBottleFeeChecked = isBottleFeeCheck.IsVisible;
				_tapCount = 0;
			}
		}
		void ImagePublic_Tapped(object sender, System.EventArgs e)
		{
			if (_tapCount < 1)
			{
				_tapCount = 1;
				isPublicUncheck.IsVisible = !isPublicUncheck.IsVisible;
				isPublicCheck.IsVisible = !isPublicCheck.IsVisible;
				eventViewModel.isPublic = isPublicUncheck.IsVisible;
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
		void Camera_Tapped(object sender, System.EventArgs e)
		{
			grdOverlayDialog.IsVisible = false;
			stackPopUp.IsVisible = false;
			stackInnerPopUp.IsVisible = true;
			grdOverlayPopUp.IsVisible = true;
			fileGotFrom = Constant.CameraText;
		}
		void Gallery_Tapped(object sender, System.EventArgs e)
		{
			grdOverlayDialog.IsVisible = false;
			stackPopUp.IsVisible = false;
			stackInnerPopUp.IsVisible = true;
			grdOverlayPopUp.IsVisible = true;
			fileGotFrom = Constant.GalleryText;
		}

		async void Photo_Tapped(object sender, System.EventArgs e)
		{
			stackInnerPopUp.IsVisible = false;
			grdOverlayPopUp.IsVisible = false;
			fileResult = new ObservableCollection<MediaData>();
			if (fileGotFrom.Equals(Constant.CameraText))
			{
				fileResult = await TakePhoto();
			}
			else
			{
				fileResult = await PickPhoto();
			}
			if (fileResult != null && fileResult.Count > 0)
			{
				CreatePhotosGrid();
			}
		}
		async void Video_Tapped(object sender, System.EventArgs e)
		{

			stackInnerPopUp.IsVisible = false;
			grdOverlayPopUp.IsVisible = false;
			fileResult = new ObservableCollection<MediaData>();
			if (fileGotFrom.Equals(Constant.CameraText))
			{
				fileResult = await TakeVideo();

			}
			else
			{
				fileResult = await PickVideo();
			}
			if (fileResult != null)
			{
				CreatePhotosGrid();
			}
		}

		async Task<ObservableCollection<MediaData>> PickVideo()
		{
			try
			{
				eventViewModel.IsLoading = true;
				await CrossMedia.Current.Initialize();
				if (!CrossMedia.Current.IsPickVideoSupported)
				{
					await DisplayAlert(Constant.NotAllowedText, Constant.GalleryAccessMessage, Constant.Ok);
					eventViewModel.IsLoading = false;
					return null;
				}
				else if (isVideoSelected)
				{
					await DisplayAlert(Constant.AlertTitle, Constant.NotMoreThanOneVideo, Constant.Ok);
					eventViewModel.IsLoading = false;
					return null;
				}
				else
				{
					await App.GetStoragePermission();
					var fileVideo = await CrossMedia.Current.PickVideoAsync();
					if (fileVideo == null)
					{
						eventViewModel.IsLoading = false;
						return null;
					}

					eventViewModel.IsLoading = true;
					if (IsFileValid(false, fileVideo.GetStream().Length))
					{
						App.iOSThumbnail = null;
						App.DroidThumbnail = null;
						using (Stream fileStream = fileVideo.GetStream())
						{
							DependencyService.Get<IVideoService>().GetThumbnail(fileStream, fileVideo.Path);
							if (App.iOSThumbnail != null || App.DroidThumbnail != null)
							{
								string fileName = Guid.NewGuid() + Path.GetExtension(fileVideo.Path);
								isVideoSelected = true;
								fileId++;
								eventViewModel.IsLoading = false;
								if (Device.RuntimePlatform == Device.Android)
								{
									ImageSource imgSource = ImageSource.FromStream(() =>
									{
										Stream ss = new MemoryStream(App.DroidThumbnail);
										return ss;
									});
									eventViewModel.SelectedMediaList.Add(new MediaData(1, fileId, fileName, fileVideo.Path, fileVideo, fileVideo.GetStream(), imgSource));
									fileVideo.Dispose();
									return eventViewModel.SelectedMediaList;
								}
								else
								{
									byte[] videoArray = ReadFully(fileVideo.GetStream());
									eventViewModel.SelectedMediaList.Add(new MediaData(1, fileId, fileName, fileVideo.Path, fileVideo, fileVideo.GetStream(), videoArray, App.iOSThumbnail));
									fileVideo.Dispose();
									return eventViewModel.SelectedMediaList;
								}

							}
							else
							{
								eventViewModel.IsLoading = false;
								return null;
							}
						}


					}
					else
					{
						eventViewModel.IsLoading = false;
						return null;
					}
				}
			}
			catch (Exception)
			{
				//  await App.Instance.Alert("Allow Camera and storage permission access to get photos and videos", Constant.AlertTitle, Constant.Ok);
				// CrossPermissions.Current.OpenAppSettings();
				eventViewModel.IsLoading = false;
				return null;

			}
		}
		public static byte[] ReadFully(Stream input)
		{
			byte[] buffer = new byte[16 * 1024];
			using (MemoryStream ms = new MemoryStream())
			{
				int read;
				while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
				{
					ms.Write(buffer, 0, read);
				}
				return ms.ToArray();
			}
		}
		async Task<ObservableCollection<MediaData>> TakeVideo()
		{
			try
			{
				eventViewModel.IsLoading = true;
				await CrossMedia.Current.Initialize();
				if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakeVideoSupported)
				{
					await DisplayAlert(Constant.NoCameraText, Constant.CameraNotAvalaibleMessage, Constant.Ok);
					eventViewModel.IsLoading = false;
					return null;
				}
				else if (isVideoSelected)
				{
					await DisplayAlert(Constant.AlertTitle, Constant.NotMoreThanOneVideo, Constant.Ok);
					eventViewModel.IsLoading = false;
					return null;
				}
				else
				{
					await App.GetCameraPermission();
					await App.GetStoragePermission();
					var fileVideo = await CrossMedia.Current.TakeVideoAsync(new Plugin.Media.Abstractions.StoreVideoOptions
					{
						DesiredLength = TimeSpan.FromSeconds(3)
					});

					if (fileVideo == null)
					{
						eventViewModel.IsLoading = false;
						return null;
					}
					eventViewModel.IsLoading = true;
					if (IsFileValid(false, fileVideo.GetStream().Length))
					{
						App.iOSThumbnail = null;
						App.DroidThumbnail = null;
						using (Stream fileStream = fileVideo.GetStream())
						{
							DependencyService.Get<IVideoService>().GetThumbnail(fileStream, fileVideo.Path);
							if (App.iOSThumbnail != null || App.DroidThumbnail != null)
							{
								string fileName = Guid.NewGuid() + Path.GetExtension(fileVideo.Path);
								isVideoSelected = true;
								fileId++;
								eventViewModel.IsLoading = false;
								if (Device.RuntimePlatform == Device.Android)
								{
									ImageSource imgSource = ImageSource.FromStream(() =>
									{
										Stream ss = new MemoryStream(App.DroidThumbnail);
										return ss;
									});

									eventViewModel.SelectedMediaList.Add(new MediaData(1, fileId, fileName, fileVideo.Path, fileVideo, fileVideo.GetStream(), imgSource));
									fileVideo.Dispose();
									return eventViewModel.SelectedMediaList;
								}
								else
								{
									eventViewModel.SelectedMediaList.Add(new MediaData(1, fileId, fileName, fileVideo.Path, fileVideo, fileVideo.GetStream(), App.iOSThumbnail));
									fileVideo.Dispose();
									return eventViewModel.SelectedMediaList;
								}

							}
							else
							{
								eventViewModel.IsLoading = false;
								return null;
							}
						}
					}
					else
					{
						eventViewModel.IsLoading = false;
						return null;
					}
				}
			}
			catch (Exception)
			{
				//await App.Instance.Alert("Allow Camera and storage permission access to get photos and videos", Constant.AlertTitle, Constant.Ok);
				// CrossPermissions.Current.OpenAppSettings();
				eventViewModel.IsLoading = false;
				return null;

			}
		}

		async Task<ObservableCollection<MediaData>> TakePhoto()
		{
			try
			{
				if (_tapCount < 1)
				{
					if (!CrossConnectivity.Current.IsConnected)
					{
						await DisplayAlert(Constant.AlertTitle, Constant.NetworkNotificationHeader, Constant.Ok);
						_tapCount = 0;
						return null;
					}
					else
					{
						eventViewModel.IsLoading = true;
						if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
						{
							await DisplayAlert(Constant.NoCameraText, Constant.CameraNotAvalaibleMessage, Constant.Ok);
							eventViewModel.IsLoading = false;
							_tapCount = 0;
							return null;
						}
						else
						{
							grdOverlayDialog.IsVisible = false;
							stackPopUp.IsVisible = false;
							eventViewModel.IsLoading = true;
							await App.GetCameraPermission();
							await App.GetStoragePermission();
							eventViewModel.IsLoading = false;
							var profileFile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
							{
								PhotoSize = PhotoSize.Small,
								CustomPhotoSize = 50,
								CompressionQuality = 60,
								AllowCropping = true,
								SaveMetaData = false, 
							});

							if (profileFile == null)
							{
								eventViewModel.IsLoading = false;
								_tapCount = 0;
								return null;
							}
							ImageSource imgSource = ImageSource.FromStream(() =>
							{
								var stream = profileFile.GetStream();
								return stream;
							});
							fileId++;
							string fileName = Guid.NewGuid() + Path.GetExtension(profileFile.Path);
							eventViewModel.SelectedMediaList.Add(new MediaData(0, fileId, fileName, profileFile.Path, profileFile, profileFile.GetStream(), imgSource));
							eventViewModel.IsLoading = false;
							return eventViewModel.SelectedMediaList;
						}
					}
				}
				else
				{
					eventViewModel.IsLoading = false;
					return null;
				}
			}
			catch (Exception e)
			{
				//await App.Instance.Alert("Allow Camera and storage permission access to take photos and videos", Constant.AlertTitle, Constant.Ok);
				//  CrossPermissions.Current.OpenAppSettings();
				return null;
			}
		}

		async Task<ObservableCollection<MediaData>> PickPhoto()
		{
			try
			{
				if (_tapCount < 1)
				{
					if (!CrossConnectivity.Current.IsConnected)
					{
						await DisplayAlert(Constant.AlertTitle, Constant.NetworkNotificationHeader, Constant.Ok);
						_tapCount = 0;
						return null;
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
							return null;
						}
						else
						{
							await App.GetStoragePermission();
							var profilePickedFile = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
							{
								PhotoSize = PhotoSize.Small,
								CustomPhotoSize = 50,
								CompressionQuality = 50
							});

							if (profilePickedFile == null)
							{
								eventViewModel.IsLoading = false;
								_tapCount = 0;
								return null;
							}
							ImageSource imgSource = ImageSource.FromStream(() =>
							{
								var stream = profilePickedFile.GetStream();
								return stream;
							});
							fileId++;
							string fileName = Guid.NewGuid() + Path.GetExtension(profilePickedFile.Path);
							eventViewModel.SelectedMediaList.Add(new MediaData(0, fileId, fileName, profilePickedFile.Path, profilePickedFile, profilePickedFile.GetStream(), imgSource));
							eventViewModel.IsLoading = false;
							return eventViewModel.SelectedMediaList;
						}
					}
				}
				else
				{
					// await App.Instance.Alert("Allow Camera and storage permission access to get photos and videos", Constant.AlertTitle, Constant.Ok);
					// CrossPermissions.Current.OpenAppSettings();
					eventViewModel.IsLoading = false;
					return null;
				}
			}
			catch (Exception)
			{
				eventViewModel.IsLoading = false;
				return null;
			}
		}

		void OverLayTapped(object sender, System.EventArgs e)

		{
			stackInnerPopUp.IsVisible = false;
			grdOverlayPopUp.IsVisible = false;
		}

		bool IsFileValid(bool IsPhoto, long size)
		{
			double sizeStream = (size / 1024) / 1024;
			if (sizeStream > Constant.VideoValidationMB)
			{
				App.Instance.Alert(Constant.VideoValidation, Constant.AlertTitle, Constant.Ok);
				return false;
			}
			else
				return true;
		}
		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			eventViewModel.IsAddressListVisible = false;
			eventViewModel.IsLoading = false;
			eventViewModel.BoostEventConfirmation = false;
		}
        #endregion

        private void CrossIcon_Tapped(object sender, EventArgs e)
        {
			editorVenue.Text = "";
			editorVenue.Unfocus();
		}
    }
}
