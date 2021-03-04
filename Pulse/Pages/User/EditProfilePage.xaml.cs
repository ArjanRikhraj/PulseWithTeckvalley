using System;
using System.IO;
using Plugin.Connectivity;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Xamarin.Forms;

namespace Pulse
{
	public partial class EditProfilePage : BaseContentPage
	{
		int _tapCount = 0;
		readonly ProfileViewModel profileViewModel;
		public EditProfilePage()
		{
			InitializeComponent();
			profileViewModel = ServiceContainer.Resolve<ProfileViewModel>();
			BindingContext = profileViewModel;
			SetInitialValues();
		}

		async void SetInitialValues()
		{
			if (App.ImageStream != null)
				App.ImageStream.Dispose();
			App.ImageByteStream = null;
			if (Device.RuntimePlatform == Device.Android)
			{
				topStack.Margin = new Thickness(10, 10, 10, 10);
			}
			await profileViewModel.GetMyProfileDetail();
			imgUser.Source = (!string.IsNullOrEmpty(profileViewModel.profileImage)) ? PageHelper.GetUserImage(profileViewModel.profileImage) : Constant.ProfileIcon;

			labelDob.Text = profileViewModel.UserDob.ToString("dd MMM,yyyy");
			dtPkrDob.MaximumDate = DateTime.Now.Date;
			dtPkrDob.DateSelected += DOB_SelectedChanged;
		}

		void UploadPhoto_Tapped(object sender, System.EventArgs e)
		{
			grdOverlayDialog.IsVisible = true;
			stackPopUp.IsVisible = true;
		}

		async void BackIcon_Tapped(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					profileViewModel.IsLoading = true;
					await Navigation.PopModalAsync();
					profileViewModel.IsLoading = false;
					_tapCount = 0;
				}
			}
			else
			{
				await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
				_tapCount = 0;
			}
		}

		void Dob_Tapped(object sender, System.EventArgs e)
		{
			Device.BeginInvokeOnMainThread(() =>
				{
					if (dtPkrDob.IsFocused)
						dtPkrDob.Unfocus();

					dtPkrDob.Focus();
				});
		}

		void Mobile_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
		{
			lblSmallMobile.TextColor = string.IsNullOrEmpty(e.NewTextValue) ? Color.Transparent : Color.FromHex(Constant.AddEventEntriesColor);
		}

		void DOB_SelectedChanged(object sender, DateChangedEventArgs e)
		{
			labelDob.Text = profileViewModel.UserDob.ToString("dd MMM,yyyy");
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
						profileViewModel.IsLoading = true;
						if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
						{
							await DisplayAlert(Constant.NoCameraText, Constant.CameraNotAvalaibleMessage, Constant.Ok);
							profileViewModel.IsLoading = false;
							_tapCount = 0;
							return;
						}
						else
						{
							grdOverlayDialog.IsVisible = false;
							stackPopUp.IsVisible = false;
							profileViewModel.IsLoading = true;
                            ImageSource IOSImageSource = null;
                            StoreCameraMediaOptions storageOptions=null;
                            storageOptions = new StoreCameraMediaOptions()
                            {
                                PhotoSize = PhotoSize.Small,
                                CustomPhotoSize = 50,
                                CompressionQuality = 50,
                                AllowCropping = true,
                                SaveMetaData = false,
                                RotateImage = false
                            };


                            Stream stream = null;
                            await App.GetCameraPermission();
                            await App.GetStoragePermission();
                            var profileFile = await CrossMedia.Current.TakePhotoAsync(storageOptions);
                            if (profileFile == null)
							{
								profileViewModel.IsLoading = false;
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
                            //    App.ImageStream = profileFile.GetStream();
                            //}
                            App.ImageStream = profileFile.GetStream();
							App.ImageByteStream = PageHelper.ReadFully(App.ImageStream);
                            imgUser.Source = ImageSource.FromStream(() =>
							{
                                //if (Device.RuntimePlatform == Device.iOS)
                                //{
                                //    stream =  profileFile.GetStreamWithImageRotatedForExternalStorage();
                                //}
                                //else
                                //{
                                //    stream = profileFile.GetStream();
                                //}
                                stream = profileFile.GetStream();
                                profileFile.Dispose();
								return stream;
							});
							profileViewModel.IsLoading = false;
						}
					}
				}
			}
			catch (Exception)
			{
                //await App.Instance.Alert("Allow Camera and storage permission access to take photos and videos", Constant.AlertTitle, Constant.Ok);
               // CrossPermissions.Current.OpenAppSettings();
                profileViewModel.IsLoading = false;
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
						profileViewModel.IsLoading = true;
                       if (!CrossMedia.Current.IsPickPhotoSupported)
						{
							await DisplayAlert(Constant.NotAllowedText, Constant.GalleryAccessMessage, Constant.Ok);
							profileViewModel.IsLoading = false;
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
								profileViewModel.IsLoading = false;
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
							imgUser.Source = ImageSource.FromStream(() =>
							{
                                var stream = Device.RuntimePlatform == Device.iOS? profilePickedFile.GetStreamWithImageRotatedForExternalStorage(): profilePickedFile.GetStream();
								profilePickedFile.Dispose();
								return stream;
							});
							profileViewModel.IsLoading = false;
						}
					}
				}
			}
			catch (Exception)
			{
               // await App.Instance.Alert("Allow Camera and storage permission access to access photos and videos", Constant.AlertTitle, Constant.Ok);
                //CrossPermissions.Current.OpenAppSettings();
                profileViewModel.IsLoading = false;
				return;
			}
		}
		void CameraCancel_Clicked(object sender, System.EventArgs e)
		{
			stackPopUp.IsVisible = false;
			grdOverlayDialog.IsVisible = false;
		}
	}
}
