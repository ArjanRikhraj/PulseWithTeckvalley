using Plugin.Connectivity;

namespace Pulse
{
	public partial class HaveAQueryPage : BaseContentPage
	{
		readonly CmsViewModel cmsViewModel;
		public HaveAQueryPage()
		{
			InitializeComponent();
			cmsViewModel = new CmsViewModel();
			cmsViewModel.QueryRequest = new QueryRequest();
			BindingContext = cmsViewModel;
		}

		async void Handle_Close_Tapped(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				await Navigation.PopModalAsync();
			}
			else
			{
				await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
			}
		}
		void Handle_Email_TextChanged(object sender, System.EventArgs e)
		{
			lblEmailHint.IsVisible = txtEmail.Text.Length > 0;
		}

		void Handle_Query_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
		{
			lblQueryHint.IsVisible = txtQuery.Text.Length > 0;
			if (txtQuery.Text.Length >= 150)
			{
				txtQuery.Text = txtQuery.Text.Remove(txtQuery.Text.Length - 1);  // Remove Last character
			}
		}
	}
}
