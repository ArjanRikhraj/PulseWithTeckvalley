using System;
using System.IO;
using Plugin.Connectivity;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Xamarin.Forms;

namespace Pulse
{
	public partial class ProfilePage : BaseContentPage
	{
		#region Private variables
		int _tapCount = 0;
		readonly AuthenticationViewModel authenticationViewModel;
		#endregion
		#region Constructor
		public ProfilePage()
		{
			InitializeComponent();
			authenticationViewModel = ServiceContainer.Resolve<AuthenticationViewModel>();
			BindingContext = authenticationViewModel;
			if (Device.RuntimePlatform == Device.iOS)
			{
				farmePopUp.CornerRadius = 10;
				gridHeader.Padding = new Thickness(14,20,14,10);
			}
			authenticationViewModel.IsOTPErrorMessageVisible = false;
		}
		#endregion
		#region Methods
		 void UploadPhoto_Tapped(object sender, System.EventArgs e)
		{
			grdOverlayDialog.IsVisible = true;
			stackPopUp.IsVisible = true;
		}
		async void Take_Photo_Tapped(object sender, System.EventArgs e)
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
						authenticationViewModel.IsLoading = true;
						if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
						{
							await DisplayAlert(Constant.NoCameraText, Constant.CameraNotAvalaibleMessage, Constant.Ok);
							authenticationViewModel.IsLoading = false;
							_tapCount = 0;
							return;
						}
						else
						{
							grdOverlayDialog.IsVisible = false;
							stackPopUp.IsVisible = false;
							authenticationViewModel.IsLoading = true;
                            ImageSource IOSImageSource = null;
                            await App.GetCameraPermission();
                            await App.GetStoragePermission();
                            Stream stream = null;
                            var profileFile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
							{
								PhotoSize = PhotoSize.Small,
								CustomPhotoSize = 50,
								CompressionQuality = 50,
                                AllowCropping = true,
                                SaveMetaData = false
							});

							if (profileFile == null)
							{
								authenticationViewModel.IsLoading = false;
								_tapCount = 0;
								return;
							}
                            //if (Device.RuntimePlatform == Device.iOS)
                            //{
                            //    IOSImageSource = ImageSource.FromFile(profileFile.Path);
                            //}
                            //if (Device.RuntimePlatform == Device.iOS)
                            //{
                            //    if (IOSImageSource != null)
                            //    {
                            //        App.ImageStream = await DependencyService.Get<IiOSImageRotationService>().GetCorrectImageRotation(IOSImageSource);
                            //    }
                            //}
                            //else
                            //{
                            App.ImageStream = profileFile.GetStream();
                            if (Device.RuntimePlatform == Device.iOS)
                            {
                                App.ImageByteStream = PageHelper.ReadFully(profileFile.GetStreamWithImageRotatedForExternalStorage());
                            }
                            else
                            {
                                App.ImageByteStream = PageHelper.ReadFully(profileFile.GetStream());
                            }

                           //}
                            //App.ImageByteStream = PageHelper.ReadFully(App.ImageStream);
							imgProfile.Source = ImageSource.FromStream(() =>
							{
                                //if (Device.RuntimePlatform == Device.iOS)
                                //{
                                //    stream = profileFile.GetStreamWithImageRotatedForExternalStorage();
                                //}
                                //else
                                //{
                                    stream = profileFile.GetStream();
                                //}
                                profileFile.Dispose();
								return stream;
							});
							authenticationViewModel.IsLoading = false;
						}
					}
				}
			}
			catch (Exception ex)
			{
                //CrossPermissions.Current.OpenAppSettings();
                authenticationViewModel.IsLoading = false;
                return;
			}
		}

		async void Pick_Photo_Tapped(object sender, System.EventArgs e)
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
						authenticationViewModel.IsLoading = true;
						if (!CrossMedia.Current.IsPickPhotoSupported)
						{
							await DisplayAlert(Constant.NotAllowedText, Constant.GalleryAccessMessage, Constant.Ok);
							authenticationViewModel.IsLoading = false;
							_tapCount = 0;
							return;
						}
						else
						{
                            await App.GetStoragePermission();
							var profilePickedFile = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
							{
								PhotoSize = PhotoSize.Small,
								CustomPhotoSize = 50,
								CompressionQuality = 50,
                                SaveMetaData = false
							});

							if (profilePickedFile == null)
							{
								authenticationViewModel.IsLoading = false;
								_tapCount = 0;
								return;
							}
							App.ImageStream = profilePickedFile.GetStream();
							App.ImageByteStream = PageHelper.ReadFully(profilePickedFile.GetStream());
							imgProfile.Source = ImageSource.FromStream(() =>
							{
								var stream = profilePickedFile.GetStream();
								profilePickedFile.Dispose();
								return stream;
							});
							authenticationViewModel.IsLoading = false;
						}
					}
				}
			}
			catch (Exception ex)
			{
                //await App.Instance.Alert("Allow Camera and storage permission access to get photos and videos", Constant.AlertTitle, Constant.Ok);
               // CrossPermissions.Current.OpenAppSettings();
                authenticationViewModel.IsLoading = false;
				return;
			}
		}

		void Username_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
		{
			if (string.IsNullOrEmpty(e.NewTextValue))
			{
				lblSmall.TextColor = Color.Transparent;
			}
			else
			{
				lblSmall.TextColor = Color.FromHex(Constant.GrayTextColor);
			}
		}

		void CameraCancel_Clicked(object sender, System.EventArgs e)
		{
			stackPopUp.IsVisible = false;
			grdOverlayDialog.IsVisible = false;

		}
		#endregion
	}
}
