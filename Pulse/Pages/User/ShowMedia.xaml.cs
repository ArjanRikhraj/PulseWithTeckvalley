using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pulse.Pages.User
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShowMedia : BaseContentPage
    {
        EventGallery eventGallery;
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
                    mediaWebview.Source = eventGallery.FileUrl;
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
                        //if (response.response.msg!="You have reached maximum pinned count")
                            btnPin.Source = response.response.is_private == true ? "iconPin.png" : "iconPinned.png";
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
                    MessagingCenter.Send<object>(this, "getPhotoAlbumMedia");
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

       
    }
}