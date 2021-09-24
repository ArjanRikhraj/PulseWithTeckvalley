using Plugin.Toasts;
using Pulse.Models.Application.Events;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pulse.Pages.Event.EventMedia
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FullSize_Image : ContentPage
    {
        int id;
        MainServices mainServices;
        public FullSize_Image(EventGallery eventGallery)
        {
            mainServices = new MainServices();
            InitializeComponent();
            id = eventGallery.MediaId;
            SetInitials(eventGallery);
        }

        private void SetInitials(EventGallery eventGallery)
        {
            image.Source = eventGallery.FileName;
        }

        private async void Cross_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void btnReportTapped(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new ReportPopupPage(id));
        }

        private void btnShareTapped(object sender, EventArgs e)
        {

        }

        private async void btnDeleteTapped(object sender, EventArgs e)
        {
            try
            {
                ReportEventMedia request = new ReportEventMedia();
                request.media_id = id;
                var response = await mainServices.Put<ResultWrapperSingle<Stories>>(Constant.DeleteEventMedia, request);
                if (response != null && response.status == Constant.Status200 && response.response != null)
                {
                    ShowToast(Constant.AlertTitle, "Successfully Deleted");
                    await Navigation.PopModalAsync();
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