namespace Pulse
{
	public partial class ChangePasswordPage : BaseContentPage
	{
		#region Properties
		readonly AuthenticationViewModel authenticationViewModel;

		#endregion
		public ChangePasswordPage()
		{
			InitializeComponent();
			authenticationViewModel = ServiceContainer.Resolve<AuthenticationViewModel>();
			BindingContext = authenticationViewModel;
		}
	}
}
