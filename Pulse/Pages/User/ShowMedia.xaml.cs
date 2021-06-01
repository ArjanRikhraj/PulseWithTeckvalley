using MediaManager;
using MediaManager.Forms;
using MediaManager.Playback;
using MediaManager.Player;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pulse.Pages.User
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShowMedia : BaseContentPage
    {
        EventGallery eventGallery;
       // List<string> storyVideoList;
        public ShowMedia(EventGallery selectedItem,bool isFromFriend=false)
        {
            InitializeComponent();
            this.eventGallery = selectedItem;
            SetInitial(isFromFriend);
        }

        private async void SetInitial(bool isFromFriend = false)
        {
            try
            {
                if (eventGallery != null)
                {
                    var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
                  
                    CrossMediaManager.Current.Dispose();
                    if (eventGallery.FileUrl.Contains("mp4"))
                    {
                        CrossMediaManager.Current.Init();
                        VideoView videoView = new VideoView()
                        {
                            VerticalOptions = LayoutOptions.FillAndExpand,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            ShowControls = false,
                            Repeat = RepeatMode.All,
                            AutoPlay = true,
                            HeightRequest = Application.Current.MainPage.Height,
                            WidthRequest = Application.Current.MainPage.Width,
                        };
                        await CrossMediaManager.Current.Play(eventGallery.FileUrl);
                        CrossMediaManager.Current.StateChanged += Current_StateChanged;
                        mainView.Children.Add(videoView);
                    }
                    else
                    {
                        Image image = new Image()
                        {
                            VerticalOptions = LayoutOptions.FillAndExpand,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            HeightRequest = Application.Current.MainPage.Height,
                            WidthRequest = Application.Current.MainPage.Width,
                            Source= eventGallery.FileUrl
                        };
                        mainView.Children.Add(image);
                    }
                    lblCreateDate.Text = eventGallery.MediaDate;
                    lblEventName.Text = eventGallery.EventName;
                    btnPin.Source = eventGallery.IsPrivate == true ? "iconPin.png" : "iconPinned.png";
                    if (isFromFriend)
                        btnPin.IsVisible = false;
                    else
                        btnPin.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
            }
        }
        private async void btnPin_Clicked(object sender, EventArgs e)
        {
            try
            {
                if(eventGallery!=null)
                {
                    PinMediaRequest request = new PinMediaRequest();
                    request.story_id = eventGallery.MediaId;
                    request.user_id = SessionManager.UserId;
                    if (request == null)
                        return;
                    MainServices mainService = new MainServices();
                    var response = await mainService.Post<ResultWrapperSingle<PinMediaResponse>>(Constant.PinStoryUrl, request);
                    if (response != null && response.status == Constant.Status200 && response.response != null)
                    {
                        btnPin.Source = response.response.maximumpinnedcount == true ? "iconPin.png" : "iconPinned.png";
                        ShowToast(Constant.AlertTitle, response.response.msg);
                    }
                    else
                        ShowToast(Constant.ServerNotRunningMessage, Constant.AlertTitle);
                }
            }
            catch (Exception ex)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
            }
        }
        async void Cross_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    //MessagingCenter.Send<object>(this, "getPhotoAlbumMedia");
                    if (CrossMediaManager.Current.IsPlaying())
                        await CrossMediaManager.Current.Stop();
                    CrossMediaManager.Current.Dispose();
                    await Navigation.PopModalAsync();
                }
                else
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
            }
            catch (Exception ex)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
            }
        }
        private void Current_StateChanged(object sender, MediaManager.Playback.StateChangedEventArgs e)
        {
            if (e.State == MediaManager.Player.MediaPlayerState.Loading)
                grdOverlay.IsVisible = true;
            if (e.State == MediaManager.Player.MediaPlayerState.Playing)
                grdOverlay.IsVisible = false;
            //if (e.State == MediaManager.Player.MediaPlayerState.Stopped)

        }

    }
}