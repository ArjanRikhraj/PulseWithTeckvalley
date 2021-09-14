using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Connectivity;
using Pulse.Helpers;
using Pulse.Models.User;
using Pulse.Pages.User;
using Xamarin.Forms;

namespace Pulse
{
    public partial class SignUpPage : BaseContentPage,INotifyPropertyChanged
    {
        #region properties
        readonly AuthenticationViewModel authenticationViewModel;
        int _tapCount = 0;
        #endregion
        #region Constructor
        public SignUpPage()
        {
            InitializeComponent();
            authenticationViewModel = ServiceContainer.Resolve<AuthenticationViewModel>();
            BindingContext = authenticationViewModel;
            authenticationViewModel.IsSocialSignUp = false;
            authenticationViewModel.IsSignUpPage = true;
            authenticationViewModel.GetAWSDetails();
        }
        #endregion
        #region Private Methods


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
        async void Privacy_Tapped(object sender, System.EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (_tapCount < 1)
                {
                    _tapCount = 1;
                    authenticationViewModel.IsLoading = true;
                    await Navigation.PushModalAsync(new PrivacyPolicyPage());
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
        async void Terms_Tapped(object sender, System.EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (_tapCount < 1)
                {
                    _tapCount = 1;
                    authenticationViewModel.IsLoading = true;
                    await Navigation.PushModalAsync(new TermsConditionsPage());
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

        async void SignIn_Tapped(object sender, System.EventArgs e)

        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (_tapCount < 1)
                {
                    _tapCount = 1;
                    authenticationViewModel.IsLoading = true;
                    authenticationViewModel.ClearFields();
                    await Navigation.PushAsync(new LoginPage(), true);
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
        void Check_Uncheck_Tapped(object sender, System.EventArgs e)
        {
            termsCheck.IsVisible = !termsCheck.IsVisible;
            termsUncheck.IsVisible = !termsUncheck.IsVisible;
            authenticationViewModel.IsTermAndConditionAccepted = termsCheck.IsVisible;
        }
        #endregion
        #region Override Methods
        protected async override void OnAppearing()
		{
            base.OnAppearing();   
        }
        #endregion
    }
	}
