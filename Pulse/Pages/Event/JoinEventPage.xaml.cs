using System;
using System.Text.RegularExpressions;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Pulse
{
	public partial class JoinEventPage : BaseContentPage
	{
		#region Private variables
		readonly EventViewModel eventViewModel;
		int _tapCount = 0;
		double totalAmount;
		string email;
		#endregion
		#region Constructor
		public JoinEventPage()
		{
			InitializeComponent();
			eventViewModel = ServiceContainer.Resolve<EventViewModel>();
			BindingContext = eventViewModel;
			SetInitialValues();
		}
		#endregion
		#region Methods
		void SetInitialValues()
		{
			labelCoverFee.Text = (eventViewModel.IsCoverAmount) && Convert.ToDouble(eventViewModel.EventCoverAmount) > 0.0 ? "$" + eventViewModel.EventCoverAmount : "$0.00";
			labelBottleAmount.Text = (eventViewModel.IsBottleAmount) && Convert.ToDouble(eventViewModel.EventBottleAmount) > 0.0 ? "$" + eventViewModel.EventBottleAmount + " per bottle would be charged extra over cover fee." : string.Empty;
			labelBottleAmount.TextColor = (eventViewModel.IsBottleAmount) && Convert.ToDouble(eventViewModel.EventBottleAmount) > 0.0 ? Color.FromHex(Constant.GrayTextColor) : Color.Transparent;
			stackBottle.IsVisible = (eventViewModel.IsBottleAmount) && Convert.ToDouble(eventViewModel.EventBottleAmount) > 0.0;
			stackNoBottle.IsVisible = !((eventViewModel.IsBottleAmount) && Convert.ToDouble(eventViewModel.EventBottleAmount) > 0.0);
			eventViewModel.isUserTakenBottleService = eventViewModel.IsBottleAmount;
			if (!string.IsNullOrEmpty(eventViewModel.EventBottleAmount) && !string.IsNullOrEmpty(eventViewModel.EventCoverAmount))
			{
				totalAmount = Convert.ToDouble(eventViewModel.EventCoverAmount) + Convert.ToDouble(eventViewModel.EventBottleAmount);
			}
			else if (!string.IsNullOrEmpty(eventViewModel.EventBottleAmount) && string.IsNullOrEmpty(eventViewModel.EventCoverAmount))
			{
				totalAmount = Convert.ToDouble(eventViewModel.EventBottleAmount);
			}
			else
			{
				totalAmount = Convert.ToDouble(eventViewModel.EventCoverAmount);
				eventViewModel.isUserTakenBottleService = false;
			}
			labelTotal.Text = "$" + string.Format("{0:0.00}", totalAmount);
			eventViewModel.TotalAmount = totalAmount;
			if (!string.IsNullOrEmpty(eventViewModel.transactionEmail))
			{
				SetContent(eventViewModel.transactionEmail);
			}
			else if (!string.IsNullOrEmpty(SessionManager.Email))
			{
				SetContent(SessionManager.Email);
			}
			else
			{
				txtEmail.Text = string.Empty;
				stackNoEmail.IsVisible = true;
				stackEmail.IsVisible = false;
				stackOtp.IsVisible = false;
			}
		}

		void SetContent(string emailId)
		{
			lblEmail.Text = emailId;
			txtEmail.Text = emailId;
			email = emailId;
			stackNoEmail.IsVisible = false;
			stackEmail.IsVisible = true;
			stackOtp.IsVisible = false;
		}

		async void Cancel_Tapped(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					eventViewModel.IsLoading = true;
					await Navigation.PopModalAsync();
					eventViewModel.IsLoading = false;
					_tapCount = 0;
				}
			}
			else
			{
				await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
				_tapCount = 0;
			}
		}

		async void ProceedPaymentClicked(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					eventViewModel.IsLoading = true;
                    eventViewModel.IsBoostEvent = false;
					if (stackEmail.IsVisible)
					{
						if (eventViewModel.TotalAmount > 0)
						{
                            await Navigation.PushModalAsync(new PaymentDetailPage(email));
						}
						else
						{
							eventViewModel.eventStatus = 3;
							await eventViewModel.EventAttendingConfirmation("Join");
						}
					}
					else
					{
						await App.Instance.Alert("Please verify your email first", Constant.AlertTitle, Constant.Ok);
					}
					eventViewModel.IsLoading = false;
					_tapCount = 0;
				}
			}
			else
			{
				await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
				_tapCount = 0;
			}
		}
		async void VerifyEmailClicked(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;

					if (string.IsNullOrEmpty(txtEmail.Text))
					{
						await App.Instance.Alert(Constant.EnterEmail, Constant.AlertTitle, Constant.Ok);
						_tapCount = 0;
					}
					else if (!Regex.IsMatch(txtEmail.Text.Trim(), Constant.EmailRegEx))
					{
						await App.Instance.Alert(Constant.EmailNotValid, Constant.AlertTitle, Constant.Ok);
						_tapCount = 0;
					}
					else
					{
						email = txtEmail.Text;
						SendOtpOnEmail();
						_tapCount = 0;
					}
				}
				else
				{
					await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
					_tapCount = 0;
				}
			}
		}

		async void SendOtpOnEmail()
		{
			try
			{
				if (CrossConnectivity.Current.IsConnected)
				{
					eventViewModel.IsLoading = true;
					if (!string.IsNullOrEmpty(SessionManager.AccessToken))
					{
						UserData user = new UserData();
						user.email = email;
						var result = await new MainServices().Post<ResultWrapperSingle<SendEmailOTPResponse>>(Constant.TransactionEmailVerifyUrl, user);
						if (result != null && result.status == Constant.Status200)
						{
							_tapCount = 0;
							txtOtp.Text = "";
							otpText.Text = "OTP received on " + email;
							stackOtp.IsVisible = true;
							stackEmail.IsVisible = false;
							stackNoEmail.IsVisible = false;
							eventViewModel.IsLoading = false;
						}
						else
						{
							await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
							_tapCount = 0;
							eventViewModel.IsLoading = false;
						}
					}
					_tapCount = 0;
					eventViewModel.IsLoading = false;
				}
				else
				{
					await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
					_tapCount = 0;
					eventViewModel.IsLoading = false;
				}
			}
			catch (Exception)
			{
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
				_tapCount = 0;
				eventViewModel.IsLoading = false;
			}
		}

		async void VerifyOtpClicked(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					if (string.IsNullOrEmpty(txtOtp.Text))
					{
						await App.Instance.Alert(Constant.EnterOtpText, Constant.AlertTitle, Constant.Ok);
						_tapCount = 0;
					}
					else
					{
						VerifyToken();
					}
					_tapCount = 0;

				}
				else
				{
					await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
					_tapCount = 0;
				}
			}
		}
		void BackOtpClicked(object sender, System.EventArgs e)
		{
			stackOtp.IsVisible = false;
			stackEmail.IsVisible = false;
			txtEmail.Text = email;
			stackNoEmail.IsVisible = true;
		}

		void EditStackTapped(object sender, System.EventArgs e)
		{
			if (_tapCount < 1)
			{
				_tapCount = 1;
				eventViewModel.IsLoading = true;
				stackEmail.IsVisible = false;
				stackOtp.IsVisible = false;
				stackNoEmail.IsVisible = true;

				eventViewModel.IsLoading = false;
				_tapCount = 0;
			}
		}

		void Email_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
		{
			if (string.IsNullOrEmpty(e.NewTextValue))
			{
				btnVerify.IsVisible = false;
			}
			else
			{
				btnVerify.IsVisible = true;
			}
		}

		void BottleStackTapped(object sender, System.EventArgs e)
		{
			imageActiveBottleService.IsVisible = !imageActiveBottleService.IsVisible;
			imageInActiveBottleService.IsVisible = !imageInActiveBottleService.IsVisible;
			labelTotal.Text = imageInActiveBottleService.IsVisible ? "$" + string.Format("{0:0.00}", totalAmount - Convert.ToDouble(eventViewModel.EventBottleAmount)) : "$" + string.Format("{0:0.00}", totalAmount);
			eventViewModel.TotalAmount = imageInActiveBottleService.IsVisible ? totalAmount - Convert.ToDouble(eventViewModel.EventBottleAmount) : totalAmount;
			eventViewModel.isUserTakenBottleService = imageActiveBottleService.IsVisible;
		}

		public async void VerifyToken()
		{
			try
			{
				if (CrossConnectivity.Current.IsConnected)
				{
					eventViewModel.IsLoading = true;
					if (!string.IsNullOrEmpty(SessionManager.AccessToken))
					{
						UserData user = new UserData();
						user.email = email;
						user.token = txtOtp.Text;
						var result = await new MainServices().Put<ResultWrapperSingle<SendEmailOTPResponse>>(Constant.VerifyTokenUrl, user);
						if (result != null && result.status == Constant.Status200)
						{
							stackEmail.IsVisible = true;
							stackNoEmail.IsVisible = false;
							stackOtp.IsVisible = false;
							lblEmail.Text = email;
                            eventViewModel.PaymentEmail = email;
                            _tapCount = 0;
							ShowToast(Constant.AlertTitle, "Verified successfully");
							//await App.Instance.Alert("Verified successfully", Constant.AlertTitle, Constant.Ok);
							eventViewModel.IsLoading = false;
						}
						else if (result != null && result.status == Constant.Status111 && result.message.non_field_errors != null)
						{
							_tapCount = 0;
							await App.Instance.Alert("Wrong OTP", Constant.AlertTitle, Constant.Ok);
							eventViewModel.IsLoading = false;
						}
						else
						{
							await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
							_tapCount = 0;
							eventViewModel.IsLoading = false;
						}
					}
					IsLoading = false;
				}
				else
				{
					await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
					_tapCount = 0;
					eventViewModel.IsLoading = false;
				}
			}
			catch (Exception)
			{
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
				_tapCount = 0;
				eventViewModel.IsLoading = false;
			}
		}
		#endregion
		protected override void OnAppearing()
		{
            base.OnAppearing();
           
		}
	}
}
