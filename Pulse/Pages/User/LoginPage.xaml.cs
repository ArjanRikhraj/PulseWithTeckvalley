using System;
using Plugin.Connectivity;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

namespace Pulse
{
	public partial class LoginPage : BaseContentPage
	{
		#region Properties
		readonly AuthenticationViewModel authenticationViewModel;
		int _tapCount = 0;
		#endregion
		#region constructor
		public LoginPage()
		{
			InitializeComponent();
			authenticationViewModel = ServiceContainer.Resolve<AuthenticationViewModel>();
			BindingContext = authenticationViewModel;
			authenticationViewModel.IsSocialSignUp = true;
			authenticationViewModel.ClearFields();
#if DEBUG
			txtEmail.Text = "admin@gmail.com";
			txtPassword.Text = "admin@123";
#endif
		}
		#endregion
		#region Methods
        protected async override void OnAppearing()
		{
			if (App.AWSCurrentDetails == null)
			{
				authenticationViewModel.GetAWSDetails();
			}
			else
			{
				authenticationViewModel.AutoLogin();
			}
            App.GetPermission();
        }
		async void SignUp_Tapped(object sender, EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					authenticationViewModel.IsLoading = true;
					authenticationViewModel.ClearFields();
					await Navigation.PushAsync(new SignUpPage(), true);
					authenticationViewModel.IsLoading = false;
					_tapCount = 0;
				}
			}
			else
			{
				await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
				_tapCount = 0;
			}
		}
       
		async void ForgetPassword_Tapped(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					authenticationViewModel.IsLoading = true;
					authenticationViewModel.ClearFields();
					await Navigation.PushModalAsync(new ForgotPasswordPage());
					authenticationViewModel.IsLoading = false;
					_tapCount = 0;
				}
			}
			else
			{
				await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
				_tapCount = 0;
			}
		}
		async void SignUpfacebook_Click(object sender, EventArgs e)
		{
			//TODO Facebook Login
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount++;
                    if (Device.RuntimePlatform == Device.Android)
                    {
                        await Navigation.PushModalAsync(new SocialLoginPage(Constant.FacebookText));
                        _tapCount = 0;
                    }
                    else
                    {
                        await authenticationViewModel.CallFacebook();
                    }

					_tapCount = 0;
				}
			}
			else
			{
				await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
				_tapCount = 0;
			}
		}
		#endregion
	}
}
