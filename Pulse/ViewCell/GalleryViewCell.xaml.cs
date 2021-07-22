using System;
using System.Collections.Generic;
using Plugin.Connectivity;
using Plugin.Toasts;
using Pulse.Models.Application.Events;
using Xamarin.Forms;

namespace Pulse
{
    public partial class GalleryViewCell : ViewCell
    {
        readonly EventViewModel eventViewModel;
        int _tapCount;
        private List<string> reportCommentList;
        public List<string> ReportCommentList
        {
            get
            {
                return reportCommentList;
            }
            set
            {
                reportCommentList = value;
                OnPropertyChanged("ReportCommentList");
            }
        }
        MainServices mainService;
        public GalleryViewCell()
        {
            InitializeComponent();
             mainService = new MainServices();
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
            GetAllReportComments();
           
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
        private async void GetAllReportComments()
        {
            try
            {
                reportCommentList = new List<string>();
                reportCommentList.Add("Bullying or harassment");
                reportCommentList.Add("False information");
                reportCommentList.Add("Violence or dangerous organizations");
                reportCommentList.Add("Scam or fraud");
                reportCommentList.Add("Intellectual property vioation");
                reportCommentList.Add("Sale of illegal or regulated goods");
                ReportListview.ItemsSource = reportCommentList;
            }
            catch (Exception ex)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
            }
        }
        private void btnEdit_Clicked(object sender, EventArgs e)
        {
            var id = (EventGallery)sender;
            reportPopup.IsVisible = true;
        }
        private async void OnDeleteMediaCommand(object sender)
        {
            try
            {
                var currentObject = (Story)sender;
                if (currentObject != null)
                {
                    ReportStoryRequest request = new ReportStoryRequest();
                    request.story_id = currentObject.id;
                    var response = await mainService.Put<ResultWrapperSingle<Stories>>(Constant.DeleteEventStories, request);
                    if (response != null && response.status == Constant.Status200 && response.response != null)
                    {
                        ShowToast(Constant.AlertTitle, "Successfully Deleted");
                        currentObject.IsMenuOptionVisible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
            }
        }
        private async void ReportListview_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
               
                var selectedItem = (string)sender;
                if (selectedItem != null)
                {
                    ReportStoryRequest request = new ReportStoryRequest();
                    //request.story_id = StoryDetails.id;
                    request.reason = selectedItem;
                    request.description = descEditor.Text;
                    var response = await mainService.Post<ResultWrapperSingle<Stories>>(Constant.ReportEventStories, request);
                    if (response != null && response.status == Constant.Status200 && response.response != null)
                    {
                        ShowToast(Constant.AlertTitle, "Successfully Reported");
                        reportPopup.IsVisible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
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
    }
}
