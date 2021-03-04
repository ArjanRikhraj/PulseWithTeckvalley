using System;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Pulse
{
	public partial class ContactUsPage : BaseContentPage
	{
		readonly CmsViewModel cmsViewModel;
		public ContactUsPage()
		{
			InitializeComponent();
			cmsViewModel = new CmsViewModel();
			BindingContext = cmsViewModel;
			stackAddress.WidthRequest = App.ScreenWidth / 2.2;
			stackInfo.WidthRequest = App.ScreenWidth / 1.5;
			cmsViewModel.GetContactUsInfo();
		}
		protected override void OnAppearing()
		{
			base.OnAppearing();
			if (!CrossConnectivity.Current.IsConnected)
			{
				App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
			}
		}

		async void HaveQueryClicked(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				IsLoading = true;
				await Navigation.PushModalAsync(new HaveAQueryPage(), true);
				IsLoading = false;
			}
			else
			{
				await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
			}
		}

		async void Mobile_Tapped(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				IsLoading = true;
				try
				{
					Device.OpenUri(new Uri(String.Format("tel:{0}", cmsViewModel.MobileNo)));
				}
				catch (Exception)
				{
					await App.Instance.Alert("Problem in device call action", Constant.AlertTitle, Constant.Ok);
				}
				IsLoading = false;
			}
			else
			{
				await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
			}
		}
		async void Email_Tapped(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				IsLoading = true;
				try
				{
					Device.OpenUri(new Uri("mailto:" + cmsViewModel.Email));
				}
				catch (Exception)
				{
					await App.Instance.Alert("Problem in device mail action", Constant.AlertTitle, Constant.Ok);

				}
				IsLoading = false;
			}
			else
			{
				await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
			}
		}
	}
}
