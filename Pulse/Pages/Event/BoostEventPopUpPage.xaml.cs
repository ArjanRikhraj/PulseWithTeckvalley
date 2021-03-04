using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageCircle.Forms.Plugin.Abstractions;
using Plugin.Connectivity;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Xamarin.Forms;

namespace Pulse
{
    public partial class BoostEventPopUpPage : BaseContentPage
    {
        #region Private variables
        int _tapCount = 0;
        string fileGotFrom;
        bool isVideoSelected;
        int fileId = 0;
        readonly EventViewModel eventViewModel;
        ObservableCollection<MediaData> fileResult;
        #endregion
        #region constructor
        public BoostEventPopUpPage()
        {
            InitializeComponent();
            eventViewModel = ServiceContainer.Resolve<EventViewModel>();
            BindingContext = eventViewModel;
        }
        #endregion

        #region Override methods
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        #endregion

        #region Methods
     
        async void Cross_Clicked(object sender, System.EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (_tapCount < 1)
                {
                    _tapCount = 1;
                    eventViewModel.IsLoading = true;
                    eventViewModel.ClearFields();
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
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            eventViewModel.IsAddressListVisible = false;
            eventViewModel.BoostEventConfirmation = false;
        }
        #endregion
    }
}
