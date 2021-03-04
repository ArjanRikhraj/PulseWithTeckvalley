using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Pulse
{
	public partial class EmailVerificationPage : BaseContentPage
	{
		#region variables
		string pageType;
		readonly AuthenticationViewModel authenticationViewModel;
		#endregion
		#region Constructor
		public EmailVerificationPage(string page)
		{
			InitializeComponent();
			authenticationViewModel = ServiceContainer.Resolve<AuthenticationViewModel>();
			BindingContext = authenticationViewModel;
			pageType = page;
			SetInitialValues();

		}
		#endregion
		#region Methods
		void SetInitialValues()
		{
			lblEmail.Text = authenticationViewModel.Email;
			headerBar.HeaderTitle = (pageType == Constant.SignUpText) ? Constant.Register : Constant.ForgotPassword;
			authenticationViewModel.OTPTextColor = Color.FromHex(Constant.BlackSimilarColor);
			authenticationViewModel.IsOTPErrorMessageVisible = false;
		}

		void txtFirst_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
		{
			if (e.NewTextValue.Length > 0)
			{
				txtSecond.Focus();
				VerifyToken();
			}
			else if (string.IsNullOrEmpty(txtFirst.Text))
			{
				txtFirst.Focus();
			}
			else
			{
				return;
			}
		}

		void txtSecond_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
		{
			if (e.NewTextValue.Length > 0)
			{
				txtThird.Focus();
				VerifyToken();
			}
			else if (string.IsNullOrEmpty(txtSecond.Text))
			{
				txtFirst.Focus();
			}

		}
		void txtThird_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
		{
			if (e.NewTextValue.Length > 0)
			{
				txtFourth.Focus();
				VerifyToken();
			}
			else if (string.IsNullOrEmpty(txtThird.Text))
			{
				txtSecond.Focus();
			}
			else
			{
				return;
			}
		}

		void txtFourth_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
		{
			if (e.NewTextValue.Length > 0)
			{
				txtFourth.Focus();
				VerifyToken();
			}
			else if (string.IsNullOrEmpty(txtFourth.Text))
			{
				txtThird.Focus();
			}
			else
			{
				return;
			}
		}

		void TxtSecond_Focused(object sender, Xamarin.Forms.FocusEventArgs e)
		{
			txtSecond.Focus();
			if (string.IsNullOrEmpty(txtSecond.Text) && string.IsNullOrEmpty(txtFirst.Text))
			{
				txtFirst.Focus();
			}
		}
		void TxtThird_Focused(object sender, Xamarin.Forms.FocusEventArgs e)
		{
			txtThird.Focus();
			if (string.IsNullOrEmpty(txtThird.Text) && string.IsNullOrEmpty(txtSecond.Text))
			{
				txtSecond.Focus();
			}
		}
		void TxtFourth_Focused(object sender, Xamarin.Forms.FocusEventArgs e)
		{
			txtFourth.Focus();
			if (string.IsNullOrEmpty(txtFourth.Text) && string.IsNullOrEmpty(txtThird.Text))
			{
				txtThird.Focus();
			}
		}
		void clear()
		{
			txtFirst.Text = string.Empty;
			txtSecond.Text = string.Empty;
			txtThird.Text = string.Empty;
			txtFourth.Text = string.Empty;
		}
		void VerifyToken()
		{
			if (!string.IsNullOrEmpty(txtFirst.Text) && !string.IsNullOrEmpty(txtSecond.Text) && !string.IsNullOrEmpty(txtThird.Text) && !string.IsNullOrEmpty(txtFourth.Text))
			{
				DependencyService.Get<IKeyboardHelper>().HideKeyboard();
				authenticationViewModel.VerifyToken(txtFirst.Text + txtSecond.Text + txtThird.Text + txtFourth.Text, pageType);

			}
		}
		#endregion

	}
}
