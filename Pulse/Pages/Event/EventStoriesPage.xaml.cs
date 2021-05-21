using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Threading;
using Xamarin.Forms.Xaml;
using System.IO;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Connectivity;
using Pulse.ViewModels;

namespace Pulse.Pages.Event
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventStoriesPage : BaseContentPage
    {
        string fileGotFrom;
		string thumbNailName;
		int _tapCount = 0;
		int fileId;
		readonly EventViewModel eventViewModel;
		readonly EventStoryViewModel  eventStoryViewModel;
		public EventStoriesPage()
        {
            InitializeComponent();
            eventViewModel = ServiceContainer.Resolve<EventViewModel>();
			//eventStoryViewModel = ServiceContainer.Resolve<EventStoryViewModel>();
			BindingContext = new EventStoryViewModel(eventViewModel.TappedEventId);
            SetInitialValues();
        }

        private async void SetInitialValues()
        {
			//eventStoryViewModel.eventId = eventViewModel.TappedEventId;
            //progressBar.IsVisible = true;
           // await progressBar.ProgressTo(1, 5000, Easing.Linear);
           // progressBar.IsVisible = false;
        }

        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void MenuButton_Clicked(object sender, EventArgs e)
        {
            stackPopUp.IsVisible = true;
            grdOverlayDialog.IsVisible = true;
        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            stackPopUp.IsVisible = false;
            grdOverlayDialog.IsVisible = false;
        }
		private void OverLayTapped(object sender, EventArgs e)
		{
			stackInnerPopUp.IsVisible = false;
			grdOverlayPopUp.IsVisible = false;
		}

		void Camera_Tapped(object sender, System.EventArgs e)
        {
            grdOverlayDialog.IsVisible = false;
            stackPopUp.IsVisible = false;
            stackInnerPopUp.IsVisible = true;
            grdOverlayPopUp.IsVisible = true;
            fileGotFrom = Constant.CameraText;
        }

        private void Gallery_Tapped(object sender, EventArgs e)
        {
            grdOverlayDialog.IsVisible = false;
            stackPopUp.IsVisible = false;
            stackInnerPopUp.IsVisible = true;
            grdOverlayPopUp.IsVisible = true;
            fileGotFrom = Constant.GalleryText;
        }
		private void CameraCancel_Clicked(object sender, EventArgs e)
		{
			stackPopUp.IsVisible = false;
			stackInnerPopUp.IsVisible = false;
			grdOverlayDialog.IsVisible = false;
			grdOverlayPopUp.IsVisible = false;
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
		async void UploadVideo(Stream stream, string fileName)
		{
			bool isUploaded = await new AWSServices().UploadAWSFile(stream, App.AWSCurrentDetails.response.images_path.story_videos, fileName);
			//byte[] thumbnail = Device.RuntimePlatform == Device.Android ? App.DroidThumbnail : App.iOSImageThumbnail;
			//bool isThumbnailUploaded = await UploadThumbnail(thumbnail);
			if (isUploaded)
			{
				AddPhotoVideo(fileName, 1);
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
		async void UploadImage(string fileName, Stream stream)
		{
			bool isUploaded = await new AWSServices().UploadAWSFile(stream, App.AWSCurrentDetails.response.images_path.story_images, fileName);
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
					PullStoryRequest request = new PullStoryRequest();
					request.user_id = SessionManager.UserId;
					request.event_id = eventViewModel.TappedEventId;
					Media media = new Media();
					media.file_name = fileName;
					media.file_type = fileType;
					request.media = media;
					if (fileType == 1)
					{
						media.file_thumbnail = thumbNailName;
					}
					var response = await new MainServices().Post<ResultWrapperSingle<CoverImageResponse>>(Constant.AddEventStories + '/', request);
					if (response != null && response.status == Constant.Status200 && response.response != null)
					{
						ShowToast(Constant.AlertTitle, "Successfully Uploaded");
						eventViewModel.IsLoading = false;
						
						_tapCount = 0;
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
					await Navigation.PopModalAsync();
				}
			}
			catch (Exception)
			{
				eventViewModel.IsLoading = false;
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
				_tapCount = 0;
				await Navigation.PopModalAsync();
			}
		}
		async Task<bool> UploadThumbnail(byte[] byteThumbnail)
		{
			if (byteThumbnail != null && byteThumbnail.Length > 0)
			{
				var stream = new MemoryStream(byteThumbnail);
				thumbNailName = Guid.NewGuid().ToString().Substring(0, 7) + "_thumb" + Constant.AWS_File_Ext;
				return await new AWSServices().UploadAWSFile(stream, App.AWSCurrentDetails.response.images_path.story_videos_thumbnails, thumbNailName);
			}
			return false;
		}

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
			grdOverlayDialog.IsVisible = true;
			stackPopUp.IsVisible = true;
		}
    }
}