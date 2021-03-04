using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Pulse
{
    public partial class TermsConditionsPage : BaseContentPage
    {
        public TermsConditionsPage()
        {
            InitializeComponent();
            try
            {
                IsLoading = true;
                TermsPulseView.Source = Constant.TermsUrl;
            }
            catch (Exception)
            {
                return;
            }
            BindingContext = this;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
            {
                App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
            }
        }

        void WebOnEndNavigating(object sender, WebNavigatedEventArgs e)
        {
            IsLoading = false;
        }
    }
}
