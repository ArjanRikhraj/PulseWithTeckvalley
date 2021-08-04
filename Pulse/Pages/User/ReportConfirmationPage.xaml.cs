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
    public partial class ReportConfirmationPage : ContentPage
    {
        public ReportConfirmationPage(string message)
        {
            InitializeComponent();
            SetInitial(message);
        }

        private async void SetInitial(string msg)
        {
            try
            {
                lblReport.Text = msg + " Successfully Reported";
            }
            catch (Exception EX)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
            }
        }

        private void ExtendedButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}