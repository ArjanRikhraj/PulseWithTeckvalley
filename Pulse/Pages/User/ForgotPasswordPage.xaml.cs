namespace Pulse
{
	public partial class ForgotPasswordPage : BaseContentPage
	{
		#region Properties
		readonly AuthenticationViewModel authenticationViewModel;
		#endregion
		#region Constructor
		public ForgotPasswordPage()
		{
			InitializeComponent();
			authenticationViewModel = ServiceContainer.Resolve<AuthenticationViewModel>();
			BindingContext = authenticationViewModel;
			authenticationViewModel.IsSignUpPage = false;
		}
		#endregion
	}
}
