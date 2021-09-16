using System;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Pulse
{
	public partial class AboutPulsePage : BaseContentPage
	{


		#region Private Methods

		async void Handle_Query_Tapped(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				await Navigation.PushModalAsync(new HaveAQueryPage(), true);
			}
			else
			{
				await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
			}

		}



		#endregion
		public AboutPulsePage()
		{
			InitializeComponent();
			try
			{
				IsLoading = true;
				AboutPulseView.Source = Constant.AboutPulseUrl + "?access_token=" + SessionManager.AccessToken;
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
			if (!CrossConnectivity.Current.IsConnected)
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
