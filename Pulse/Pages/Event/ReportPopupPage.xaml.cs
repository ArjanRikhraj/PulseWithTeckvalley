using Pulse.Helpers;
using Pulse.Models.Application.Events;
using Pulse.Pages.User;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pulse.Pages.Event
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportPopupPage : PopupPage
    {
        MainServices mainServices;
        int Id;
        public ReportPopupPage(int id)
        {
             mainServices = new MainServices();
            this.Id = id;
            InitializeComponent();
            SetInitials();
        }

        private void SetInitials()
        {
            reportListview.ItemsSource = Utils.ReportList();
        }

        private async void ReportListview_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedItem = e.Item as string;
            if (selectedItem != null)
            {
                if (string.IsNullOrEmpty(descEditor.Text))
                {
                    await App.Instance.Alert(Constant.ReportDescriptionMessage, Constant.AlertTitle, Constant.Ok);
                    return;
                }
                ReportEventMedia request = new ReportEventMedia();
                request.media_id = Id;
                request.reason = selectedItem;
                request.description = descEditor.Text;
                var response = await mainServices.Post<ResultWrapperSingle<Stories>>(Constant.ReportMedia, request);
                if (response != null && response.status == Constant.Status200 && response.response != null)
                {
                    await PopupNavigation.Instance.PushAsync(new ReportConfirmationPage("Media"));
                    await PopupNavigation.Instance.PopAllAsync();
                }
            }
            reportListview.SelectedItem = null;
        }

        private async void ExtendedButton_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAllAsync();
        }
    }
}