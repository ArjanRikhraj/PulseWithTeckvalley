using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Pulse
{
    public partial class GalleryImageViewPage : BaseContentPage
    {
        readonly EventViewModel eventViewModel;
        int _tapCount = 0;
        ObservableCollection<EventGallery> tempMediaList = new ObservableCollection<EventGallery>();
        public GalleryImageViewPage(string imageURL)
        {
            InitializeComponent();
            eventViewModel = ServiceContainer.Resolve<EventViewModel>();
            BindingContext = eventViewModel;
            imgFull.Source = imageURL.Replace("Uri: ","");
        }

        async void Cross_Clicked(object sender, System.EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (_tapCount < 1)
                {
                    _tapCount = 1;
                    eventViewModel.IsLoading = true;
                    await Navigation.PopModalAsync();
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
    }
}
