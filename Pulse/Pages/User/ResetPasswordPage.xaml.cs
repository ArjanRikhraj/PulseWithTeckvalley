using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Pulse
{
	public partial class ResetPasswordPage : BaseContentPage
	{
		#region Properties
		readonly AuthenticationViewModel authenticationViewModel;
		#endregion
		#region Constructor
		public ResetPasswordPage()
		{
			InitializeComponent();
			authenticationViewModel = ServiceContainer.Resolve<AuthenticationViewModel>();
			BindingContext = authenticationViewModel;
			authenticationViewModel.IsOTPErrorMessageVisible = false;

		}
		#endregion
	}
}
