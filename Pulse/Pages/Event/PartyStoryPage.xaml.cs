using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Connectivity;
using Plugin.Geolocator;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

namespace Pulse
{
	public partial class PartyStoryPage : BaseContentPage
	{
		int _tapCount = 0;
		string mediaId;
		string thumbNailName;
		readonly EventViewModel eventViewModel;
		ObservableCollection<EventGallery> tempMediaList = new ObservableCollection<EventGallery>();
		string fileGotFrom;
		int fileId;
		bool isPastEvent;
		public PartyStoryPage()
		{
			InitializeComponent();
			eventViewModel = ServiceContainer.Resolve<EventViewModel>();
			BindingContext = eventViewModel;
            SetInitialValues();
            listViewMedia.ItemAppearing += ListViewMedia_ItemAppearing;
		}

        void ListViewMedia_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            
        }

		void SetInitialValues()
		{
			if (Device.RuntimePlatform == Device.Android)
			{
				topStack.Margin = new Thickness(10, 10, 10, 10);
			}
            if (Device.RuntimePlatform == Device.iOS)
            {
                
            }
			listViewMedia.RefreshCommand = LoadMediaCommand;
			ClearMediaList();
            listViewMedia.LoadMoreCommand = new Command(HandleAction);
			GetMedia();
		}

        async  void HandleAction(object obj)
        {
            GetMedia();
        }


		public ICommand LoadMediaCommand
		{
			get
			{
				return new Command(async () =>
				{
					if (CrossConnectivity.Current.IsConnected)
					{
						listViewMedia.IsRefreshing = true;
						ClearMediaList();
						GetMedia();
					}
					else
					{
						await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
					}
				});
			}
		}
		void ClearMediaList()
		{
			eventViewModel.pageNoMedia = 1;
			eventViewModel.totalLiveMediaPages = 1;
            listViewMedia.ItemsSource = null;
			tempMediaList = new ObservableCollection<EventGallery>();        
        }
        void ReasonEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (editorComment.Text.Length >= 150)
            {
                editorComment.Text = editorComment.Text.Remove(editorComment.Text.Length - 1);  // Remove Last character

            }
        }
		async void Share_Button_Tapped(object sender, System.EventArgs e)
		{


			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount++;
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
        async void Refresh_Button_Tapped(object sender, System.EventArgs e)
        {


            if (CrossConnectivity.Current.IsConnected)
            {
                if (_tapCount < 1)
                {
                    _tapCount++;
                    eventViewModel.IsLoading = true;
                    ClearMediaList();
                    await GetMedia();
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
		void WriteQR()
		{
			try
			{
				string QRData;
				QRData = "Event Name:" + eventViewModel.EventTitle + Environment.NewLine + ", Event Location:" + eventViewModel.LocationEvent + Environment.NewLine + ", Event Time:" + eventViewModel.eventDateOnMap + " and to get more details please download pulse app from Play Store or Apple Store.";
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
		async void Back_Button_Tapped(object sender, System.EventArgs e)
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

		async void CheckInClicked(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount++;
					eventViewModel.IsLoading = true;
					isPastEvent = false;
					if (eventViewModel.currentActiveEventType == MyEventType.Upcoming)
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
								grdOverlayPopUp.IsVisible = true;
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
							grdOverlayPopUp.IsVisible = true;
							stackcheckInMessage.IsVisible = true;
							_tapCount = 0;
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
				}
			}
			else
			{
				await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
				_tapCount = 0;
			}
		}

		async void PhotosClicked(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
                    App.GetCameraPermission();
					eventViewModel.IsLoading = true;

					if (eventViewModel.currentActiveEventType == MyEventType.Upcoming)
					{
						if (DateTimeValidate())
						{
							bool isEnable = await GetCurrentLoc();
							if (isEnable)
							{
								grdOverlayDialog.IsVisible = true;
								stackPopUp.IsVisible = true;
							}
							else
							{
								checkInTitle.Text = Constant.AddMediaNotProperTitleMessage;
								checkInMessage.Text = Constant.AddMediaNotProperMessage;
								grdOverlayPopUp.IsVisible = true;
								stackcheckInMessage.IsVisible = true;
							}
						}
						else if (!DateTimeValidate() && isPastEvent)
						{
							await App.Instance.Alert("You can not add photos/videos in past event", Constant.AlertTitle, Constant.Ok);
							eventViewModel.IsLoading = false;
							_tapCount = 0;
						}
						else
						{
							checkInTitle.Text = Constant.AddMediaEarlyTitleMessage;
							checkInMessage.Text = Constant.AddMediaEarlyMessage;
							grdOverlayPopUp.IsVisible = true;
							stackcheckInMessage.IsVisible = true;
						}
					}
					else
					{
						await App.Instance.Alert("You can not add photos/videos in past event", Constant.AlertTitle, Constant.Ok);
						eventViewModel.IsLoading = false;
						_tapCount = 0;

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
						//await App.Instance.Alert("Successfully checked in", Constant.AlertTitle, Constant.Ok);
						eventViewModel.IsLoading = false;
						_tapCount = 0;

					}
					else if (response != null && response.status == Constant.Status111 && response.message.non_field_errors != null)
					{
						checkInTitle.Text = Constant.CheckInNotProperTitleMessage;
						checkInMessage.Text = Constant.CheckInNotProperMessage;
						grdOverlayPopUp.IsVisible = true;
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

		void CameraCancel_Clicked(object sender, System.EventArgs e)
		{
			stackPopUp.IsVisible = false;
            stackInnerPopUp.IsVisible = false;
			grdOverlayDialog.IsVisible = false;
			stackPopUpMark.IsVisible = false;
			stackReason.IsVisible = false;
            grdOverlay.IsVisible = false;
            grdOverlayPopUp.IsVisible = false;
		}
		void OverLayTapped(object sender, System.EventArgs e)
		{
			stackInnerPopUp.IsVisible = false;
			grdOverlayPopUp.IsVisible = false;
			stackcheckInMessage.IsVisible = false;
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
			if (fileGotFrom.Equals(Constant.CameraText))
			{
				await TakePhoto();
			}
			else
			{
				await PickPhoto();
			}
		}
		async void Video_Tapped(object sender, System.EventArgs e)
		{

			stackInnerPopUp.IsVisible = false;
			grdOverlayPopUp.IsVisible = false;
			if (fileGotFrom.Equals(Constant.CameraText))
			{
				await TakeVideo();

			}
			else
			{
				await PickVideo();
			}

		}
		async void DeleteTapped(object sender, System.EventArgs e)
		{
			var selectedItem = (Frame)sender;
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					eventViewModel.IsLoading = true;
					bool result = await App.Instance.ConfirmAlert(Constant.DeleteMediaText, Constant.AlertTitle, Constant.Ok, Constant.CancelButtonText);
					if (result)
					{
						bool isDeleted = await eventViewModel.DeleteMedia(selectedItem.ClassId);
						if (isDeleted)
						{
							ClearMediaList();
							GetMedia();
							ShowToast(Constant.AlertTitle, Constant.MediaDeleted);
							//await App.Instance.Alert(Constant.MediaDeleted, Constant.AlertTitle, Constant.Ok);
						}
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
		async void ReportTapped(object sender, System.EventArgs e)
		{
			var selectedItem = (Frame)sender;
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					eventViewModel.IsLoading = true;
					mediaId = selectedItem.ClassId;
					grdOverlayDialog.IsVisible = true;
					stackPopUpMark.IsVisible = true;
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
		async void Inappropriate_Tapped(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					eventViewModel.IsLoading = true;
					stackPopUpMark.IsVisible = false;
					stackReason.IsVisible = true;
					editorComment.Text = string.Empty;
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
        public void ClearValues()
        {
            grdOverlayDialog.IsVisible = true;
            eventViewModel.IsLoading = true;
            stackPopUpMark.IsVisible = false;
            stackReason.IsVisible = true;
            editorComment.Text = string.Empty;
            eventViewModel.IsLoading = false;
        }
		async void Submit_Clicked(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					eventViewModel.IsLoading = true;
					if (string.IsNullOrEmpty(editorComment.Text))
					{
						await App.Instance.Alert("Leave your comment first", Constant.AlertTitle, Constant.Ok);
					}
					else
					{
						bool isSuccess = await eventViewModel.MarkInappropriate(editorComment.Text, mediaId);
						if (isSuccess)
						{
							stackReason.IsVisible = false;
							grdOverlayDialog.IsVisible = false;
							ShowToast(Constant.AlertTitle, "Reported successfully");
							ClearMediaList();
							GetMedia();
						}
						else
						{
							stackReason.IsVisible = false;
							grdOverlayDialog.IsVisible = false;
							eventViewModel.IsLoading = false;
							await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
						}
					}
					_tapCount = 0;
				}
			}
			else
			{
				await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
				_tapCount = 0;
				eventViewModel.IsLoading = false;
			}
		}


		async Task PickVideo()
		{
			try
			{
				eventViewModel.IsLoading = true;
				await CrossMedia.Current.Initialize();
				if (!CrossMedia.Current.IsPickVideoSupported)
				{
					await DisplayAlert(Constant.NotAllowedText, Constant.GalleryAccessMessage, Constant.Ok);
					eventViewModel.IsLoading = false;
				}
				else
				{
                    await App.GetStoragePermission();
                    var fileVideo = await CrossMedia.Current.PickVideoAsync();
                    if (fileVideo == null)
					{
						eventViewModel.IsLoading = false;
					}

					eventViewModel.IsLoading = true;
					if (IsFileValid(false, fileVideo.GetStream().Length))
					{
						App.iOSThumbnail = null;
						App.iOSImageThumbnail = null;
						App.DroidThumbnail = null;
						using (Stream fileStream = fileVideo.GetStream())
						{
							DependencyService.Get<IVideoService>().GetThumbnail(fileStream, fileVideo.Path);
							if (App.iOSThumbnail != null || App.DroidThumbnail != null || App.iOSImageThumbnail != null)
							{
								string fileName = Guid.NewGuid() + Path.GetExtension(fileVideo.Path);
								eventViewModel.IsLoading = true;
								if (Device.RuntimePlatform == Device.Android)
								{
									UploadVideo(fileVideo.GetStream(), fileName);
								}
								else
								{
									byte[] videoArray = ReadFully(fileVideo.GetStream());
									var stream = new MemoryStream(videoArray);
									bool isUploaded = await new AWSServices().UploadAWSFile(stream, App.AWSCurrentDetails.response.images_path.event_videos, fileName);
									byte[] thumbnail = Device.RuntimePlatform == Device.Android ? App.DroidThumbnail : App.iOSImageThumbnail;
									bool isThumbnailUploaded = await UploadThumbnail(thumbnail);
									if (isThumbnailUploaded && isUploaded)
									{
										AddPhotoVideo(fileName, 1);
									}
								}
							}
							else
							{
								eventViewModel.IsLoading = false;
							}
						}
					}
					else
					{
						eventViewModel.IsLoading = false;
					}
				}
			}
			catch (Exception)
			{
              //  await App.Instance.Alert("Allow Camera and storage permission access to get photos and videos", Constant.AlertTitle, Constant.Ok);
              //  CrossPermissions.Current.OpenAppSettings();
				eventViewModel.IsLoading = false;
			}
		}

		async void UploadVideo(Stream stream, string fileName)
		{
			bool isUploaded = await new AWSServices().UploadAWSFile(stream, App.AWSCurrentDetails.response.images_path.event_videos, fileName);
			byte[] thumbnail = Device.RuntimePlatform == Device.Android ? App.DroidThumbnail : App.iOSImageThumbnail;
			bool isThumbnailUploaded = await UploadThumbnail(thumbnail);
			if (isThumbnailUploaded && isUploaded)
			{
				AddPhotoVideo(fileName, 1);
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
					LiveMedia livemedia = new LiveMedia();
					livemedia.latitude = Convert.ToDouble(eventViewModel.currenteventLat);
					livemedia.longitude = Convert.ToDouble(eventViewModel.currenteventLong);
					livemedia.file_name = fileName;
					livemedia.file_type = fileType;
					livemedia.is_live = true;
					if (fileType == 1)
					{
						livemedia.file_thumbnail = thumbNailName;
					}
					var response = await new MainServices().Post<ResultWrapperSingle<SendEmailOTPResponse>>(Constant.UploadLiveMedia + eventViewModel.TappedEventId + '/', livemedia);
					if (response != null && response.status == Constant.Status200 && response.response != null)
					{
						ShowToast(Constant.AlertTitle, "Successfully Uploaded");
						ClearMediaList();
						GetMedia();
						_tapCount = 0;

					}
					else if (response != null && response.status == Constant.Status111 && response.message.non_field_errors != null)
					{
						checkInTitle.Text = Constant.AddMediaNotProperTitleMessage;
						checkInMessage.Text = Constant.AddMediaNotProperMessage;
						grdOverlayPopUp.IsVisible = true;
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

		async Task<bool> UploadThumbnail(byte[] byteThumbnail)
		{
			if (byteThumbnail != null && byteThumbnail.Length > 0)
			{
				var stream = new MemoryStream(byteThumbnail);
				thumbNailName = Guid.NewGuid().ToString().Substring(0, 7) + "_thumb" + Constant.AWS_File_Ext;
                return await new AWSServices().UploadAWSFile(stream, App.AWSCurrentDetails.response.images_path.event_videos_thumbnails, thumbNailName);
			}
			return false;
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
		async Task TakeVideo()
		{
			try
			{
				eventViewModel.IsLoading = true;
				await CrossMedia.Current.Initialize();
				if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakeVideoSupported)
				{
					await DisplayAlert(Constant.NoCameraText, Constant.CameraNotAvalaibleMessage, Constant.Ok);
					eventViewModel.IsLoading = false;
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
					}
					eventViewModel.IsLoading = true;
					if (IsFileValid(false, fileVideo.GetStream().Length))
					{
						eventViewModel.IsLoading = true;
						App.iOSThumbnail = null;
						App.DroidThumbnail = null;
						App.iOSImageThumbnail = null;
						using (Stream fileStream = fileVideo.GetStream())
						{
							DependencyService.Get<IVideoService>().GetThumbnail(fileStream, fileVideo.Path);
							if (App.iOSThumbnail != null || App.DroidThumbnail != null || App.iOSImageThumbnail != null)
							{
								string fileName = Guid.NewGuid() + Path.GetExtension(fileVideo.Path);
								fileId++;
								eventViewModel.IsLoading = true;
								UploadVideo(fileVideo.GetStream(), fileName);

							}
							else
							{
								eventViewModel.IsLoading = false;
							}
						}
					}
					else
					{
						eventViewModel.IsLoading = false;
					}
				}
			}
			catch (Exception e)
			{
               // await App.Instance.Alert("Allow Camera and storage permission access to take photos and videos", Constant.AlertTitle, Constant.Ok);
                //CrossPermissions.Current.OpenAppSettings();
				eventViewModel.IsLoading = false;
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
							var profilePickedFile = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
							{
                                PhotoSize = PhotoSize.Medium
								
							});

							if (profilePickedFile == null)
							{
								eventViewModel.IsLoading = false;
								_tapCount = 0;
							}
							ImageSource imgSource = ImageSource.FromStream(() =>
							{
								var stream = profilePickedFile.GetStream();
								return stream;
							});
							fileId++;
							string fileName = Guid.NewGuid() + Path.GetExtension(profilePickedFile.Path);
							UploadImage(fileName, profilePickedFile.GetStream());
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
		async Task<bool> GetCurrentLoc()
		{
			if (Device.RuntimePlatform == Device.iOS)
				return (await GetPermission());
			else
				return (await GetCurrentLocation());
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

        async Task GetMedia()
		{
            try
			{
				eventViewModel.IsLoading = true;
				if (!CrossConnectivity.Current.IsConnected)
				{
					await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
					_tapCount = 0;
					listViewMedia.IsRefreshing = false;
				}
				else
				{
					bool isList = await eventViewModel.GetMediaList(true);
					SetMediaList(isList, eventViewModel.MediaList);

				}
				eventViewModel.IsLoading = false;
			}

			catch (Exception)
			{
				eventViewModel.IsLoading = false;
				_tapCount = 0;
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
				listViewMedia.IsRefreshing = false;
			}

		}

		void SetMediaList(bool isList, List<EventMedia> list)
		{
			if (isList && eventViewModel.pageNoMedia < 2)
			{
				SetMedia(list);
			}
			else if (isList)
			{
				SetMedia(list);
			}
			else if (!isList && eventViewModel.pageNoMedia < 2)
			{
				listViewMedia.IsVisible = false;
				eventViewModel.IsLoading = false;
				listViewMedia.IsRefreshing = false;

			}
			else
			{
				listViewMedia.ItemsSource = tempMediaList;
				eventViewModel.IsLoading = false;
				listViewMedia.IsRefreshing = false;
			}
		}

		void SetMedia(List<EventMedia> list)
		{
			listViewMedia.IsVisible = true;
			foreach (var item in list)
			{
                tempMediaList.Add(new EventGallery
                {
                    ImageWidth = App.ScreenWidth,
                    ImageHeight = App.ScreenHeight / 1.4,
                    VideoFileName = item.file_type == 1 ? PageHelper.GetEventTranscodedVideo(item.file_name) : "",
                    MediaId = item.id,
                    IsDeleteIconVisible = (SessionManager.UserId == item.user_id),
                    IsReportIconVisible = !(SessionManager.UserId == item.user_id),
                    FileName = item.file_type == 1 ? PageHelper.GetEventVideoThumbnail(item.file_thumbnail) : PageHelper.GetEventImage(item.file_name),
                    IsPlayIconVisible = item.file_type == 1 ? true : false,
                    IsVisibleUserName = true,
                    UserName = item.user_name,
                    MediaDate = SetEventDate(item.create_date),
                    UserImage = (!string.IsNullOrEmpty(item.profile_image) ? PageHelper.GetUserImage(item.profile_image) : Constant.ProfileIcon),
                    IsImage = item.file_type == 1 ? false : true,
                    VideoThumbnailFileName = item.file_type == 1 ? PageHelper.GetEventVideoThumbnail(item.file_thumbnail) : string.Empty
				});
			}
			eventViewModel.MediaList.Clear();
            listViewMedia.ItemsSource = tempMediaList;
			eventViewModel.pageNoMedia++;
			eventViewModel.IsLoading = false;
			listViewMedia.IsRefreshing = false;

		}
		string SetEventDate(string createDate)
		{
			var dateStart = DateTime.Parse(createDate);
			return dateStart.Date.ToString("ddd,dd MMM").ToUpperInvariant() + ", " + dateStart.ToString("h:mm tt").Trim().ToUpperInvariant();
		}
		void lstMediaTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
		{
			var selected = (EventGallery)e.Item;
			if (selected.IsPlayIconVisible)
			{

				if (!string.IsNullOrEmpty(selected.VideoFileName))
				{
					if (Device.RuntimePlatform == Device.iOS)
					{
						stckVideo.IsVisible = true;
						WebView video = new WebView
						{
							HorizontalOptions = LayoutOptions.FillAndExpand,
							VerticalOptions = LayoutOptions.FillAndExpand
                           

						};
						video.Source = PageHelper.GetEventTranscodedVideo(PageHelper.GetEventTranscodedVideo(selected.VideoFileName));
                        grdVideo.Padding = new Thickness(10, 25, 10, 25);
						grdVideo.Children.Add(video, 0, 1);
					}
					else if (Device.RuntimePlatform == Device.Android)
					{
						DependencyService.Get<IVideoPlayer>().Play(PageHelper.GetEventTranscodedVideo(selected.VideoFileName));
					}
				}
			}
            listViewMedia.SelectedItem = null;
		}

		void CrossVideo_Clicked(object sender, System.EventArgs e)
		{
			stckVideo.IsVisible = false;
		}
		protected override void OnAppearing()
		{
            base.OnAppearing();
            MessagingCenter.Subscribe<App>(this, "getMedia", (obj) => {
                ClearMediaList();
                GetMedia();
            });
            MessagingCenter.Subscribe<App>(this, "ClearMediaList", (obj) => {
                ClearMediaList();
            });
            MessagingCenter.Subscribe<App>(this, "ClearValues", (obj) => {
                ClearValues();
            });
		}
		protected override void OnDisappearing()
		{
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<App>(this, "getMedia");
            MessagingCenter.Unsubscribe<App>(this, "ClearValues");
            MessagingCenter.Unsubscribe<App>(this, "ClearMediaList");
		}
	}
}

