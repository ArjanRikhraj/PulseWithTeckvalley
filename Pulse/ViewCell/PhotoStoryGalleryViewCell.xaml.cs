using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Pulse
{
    public partial class PhotoStoryGalleryViewCell : ViewCell
    {
        readonly EventViewModel eventViewModel;
        int _tapCount;
        public PhotoStoryGalleryViewCell()
        {
            InitializeComponent();
            eventViewModel = ServiceContainer.Resolve<EventViewModel>();
           // videoFile.UpdateStatus += VideoFile_UpdateStatus;
            //loader.IsVisible = Device.RuntimePlatform == Device.iOS;

        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            var test = BindingContext as EventGallery;
            if (test!=null && !string.IsNullOrEmpty(test.VideoFileName))
            {
               // videoFile.Source = VideoSource.FromUri(test.VideoFileName);
            }
        }


        async void Image_tapped(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (_tapCount < 1)
                {
                    _tapCount = 1;
                    eventViewModel.IsLoading = true;
                    var image = (Image)sender;
                    await eventViewModel.Navigation.PushModalAsync(new GalleryImageViewPage(image.Source.ToString()));
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
                            MessagingCenter.Send(App.Instance, "ClearMediaList");
                            MessagingCenter.Send(App.Instance, "getMedia");
                            eventViewModel.ShowToast(Constant.AlertTitle, Constant.MediaDeleted);
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
       
        async void Inappropriate_Tapped(object sender, System.EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (_tapCount < 1)
                {
                    _tapCount = 1;
                    MessagingCenter.Send(App.Instance, "ClearValues");
                    _tapCount = 0;
                }
            }
            else
            {
                await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                _tapCount = 0;
            }
        }

        void Handle_Tapped(object sender, System.EventArgs e)
        {
        //    if (videoFile != null)
        //    {
        //        videoFile.Play();
        //    }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var file = BindingContext as EventGallery;
            VideoPlayer videoPlayer = new VideoPlayer()
            {
                HeightRequest = file.ImageHeight,
                WidthRequest = file.ImageWidth,
                Source = VideoSource.FromUri(file.VideoFileName)
            };
            videoPlayer.AreTransportControlsEnabled = false;
            videoPlayer.UpdateStatus += (object sender, EventArgs e) => {
                if(videoPlayer.Status == VideoStatus.Paused)
                {
                   // videothumbnail.IsVisible = true;    
                }
                if(videoPlayer.Status == VideoStatus.Playing)
                {
                    videothumbnail.IsVisible = false; 
                }
            };
            if ( file!=null && file.IsPlayIconVisible)
            {
                stack.Children.Add(videoPlayer);
                videoPlayer.Play();
                videothumbnail.IsVisible = false;

           }
          
            //if (file!=null && file.IsPlayIconVisible)
            //{
            //    videoFile.Play();
            //}
            //if (videoFile != null && file!=null)
            //{
                //videoFile.Play();
                //videoFile.AutoPlay = true;
            //}
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            var file = BindingContext as EventGallery;
            if (file != null && file.IsPlayIconVisible)
            {
                if (stack.Children.Count > 0)
                {
                    stack.Children.Clear();
                }
                videothumbnail.IsVisible = true;
            }
            //if (file!=null && file.IsPlayIconVisible)
            //{
            //    videoFile.Stop();
            //}
            //if (videoFile != null && videoFile.Status == VideoStatus.Playing)
            //{
                //videoFile.Stop();
               // videoFile.AutoPlay = false;
           // }
        }
        void VideoFile_UpdateStatus(object sender, EventArgs e)
        {
            //if (videoFile.Status == VideoStatus.NotReady)
            //{
            //    videothumbnail.IsVisible = true;
            //}
            //else
            //{
            //    videothumbnail.IsVisible = false;
            //}

        }
    }
}
