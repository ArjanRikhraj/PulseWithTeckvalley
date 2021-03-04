using System;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Pulse
{
	public partial class FAQPage : BaseContentPage
	{
		#region Private Variables
		BaseViewModel baseViewModel = new BaseViewModel();
		#endregion


		public FAQPage()
		{
			InitializeComponent();
			try
			{
				IsLoading = true;
				faqsView.Source = Constant.FAQsUrl + "?access_token=" + SessionManager.AccessToken;
			}
			catch (Exception)
			{
				return;
			}
			BindingContext = this;
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			if (!CrossConnectivity.Current.IsConnected)
			{
				await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
			}
		}

		void WebOnEndNavigating(object sender, WebNavigatedEventArgs e)
		{
			IsLoading = false;
		}
	}
}
