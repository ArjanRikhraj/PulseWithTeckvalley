using System;
using System.Collections.Generic;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Pulse
{
    public partial class GalleryViewCell : ViewCell
    {
        readonly EventViewModel eventViewModel;
        int _tapCount;
        public GalleryViewCell()
        {
            InitializeComponent();
            eventViewModel = ServiceContainer.Resolve<EventViewModel>();      
            //videoFile.UpdateStatus += VideoFile_UpdateStatus;
            loader.IsVisible = Device.RuntimePlatform == Device.iOS;
          }

		protected override void OnBindingContextChanged()
		{
            base.OnBindingContextChanged();
            var test = BindingContext as EventGallery;
            if (!string.IsNullOrEmpty(test.VideoFileName))
            {
              //  videoFile.Source = VideoSource.FromUri(test.VideoFileName);
            }
       }

        void Handle_Tapped(object sender, System.EventArgs e)
        {
            //if(videoFile!=null)
            //{
            //    videoFile.Play();
            //}
        }

		async void Image_tapped(object sender, System.EventArgs e)
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
          

        }

		protected override void OnDisappearing()
		{
            base.OnDisappearing();
            var file = BindingContext as EventGallery;
            //if(this.videoFile!=null && (this.videoFile.Status == VideoStatus.Playing || this.videoFile.Status == VideoStatus.Paused) && file.IsPlayIconVisible)
            //{
            //    this.videoFile.Stop();
            //    videoFile.IsVisible = false;
            //    videothumbnail.IsVisible = true;               
            //}
            if(file!=null && file.IsPlayIconVisible)
            {
                if (stack.Children.Count > 0)
                {
                    stack.Children.Clear();
                }
                videothumbnail.IsVisible = true;               
            }
        }

        void VideoFile_UpdateStatus(object sender, EventArgs e)
        {
            //if(videoFile.Status == VideoStatus.Playing)
            //{
            //    videothumbnail.IsVisible = false;
            //    loader.IsVisible = false;
            //}
            //else
            //{
            //    videothumbnail.IsVisible = true;
            //}
        }

	}
}
