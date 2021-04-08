using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Threading;
using Xamarin.Forms.Xaml;

namespace Pulse.Pages.Event
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventStoriesPage : BaseContentPage
    {
        readonly EventViewModel eventViewModel;
        public EventStoriesPage()
        {
            InitializeComponent();
            eventViewModel = ServiceContainer.Resolve<EventViewModel>();
            BindingContext = eventViewModel;
            SetInitialValues();
        }

        private async void SetInitialValues()
        {
            progressBar.IsVisible = true;
            await progressBar.ProgressTo(1, 5000, Easing.Linear);
            progressBar.IsVisible = false;
        }

        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void MenuButton_Clicked(object sender, EventArgs e)
        {
            stackPopUp.IsVisible = true;
            grdOverlayDialog.IsVisible = true;
        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            stackPopUp.IsVisible = false;
            grdOverlayDialog.IsVisible = false;
        }
    }
}