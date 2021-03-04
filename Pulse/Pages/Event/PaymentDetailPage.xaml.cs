using System;
using System.Linq;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Pulse
{
	public partial class PaymentDetailPage : BaseContentPage
	{
		#region Private variables
		readonly EventViewModel eventViewModel;
		int _tapCount = 0;
        string email;
		#endregion
		public PaymentDetailPage(string email)
		{
			InitializeComponent();
			eventViewModel = ServiceContainer.Resolve<EventViewModel>();
			BindingContext = eventViewModel;
			SetInitialValues();
            stackEmail.IsVisible = eventViewModel.IsBoostEvent;
            if (!string.IsNullOrEmpty(email))
            {
                SetContent(email);
                eventViewModel.IsEmailVerified = true;
                eventViewModel.PaymentEmail = email;
            }
		}
       
        void SetContent(string emailId)
        {
            lblEmail.Text = emailId;
            txtEmail.Text = emailId;
            email = eventViewModel.PaymentEmail = emailId;
            stackNoEmail.IsVisible = false;
            stackEmail.IsVisible = eventViewModel.IsBoostEvent;
            stackOtp.IsVisible = false;
        }
		async void SetInitialValues()
		{
			string amount = string.Format("{0:0.00}", eventViewModel.TotalAmount);
			if (!string.IsNullOrEmpty(amount))
			{
				string[] amounts = amount.Split('.');
				if (amounts != null)
				{
					lblSpending.Text = "$" + amounts[0] + ".";
					lblDecimalSpending.Text = amounts[1];
				}
			}
			if (Device.RuntimePlatform == Device.Android)
			{
				stackMainCard.Padding = new Thickness(0, 2, 0, 0);
				stackMainCard.Spacing = 0;
				stackMainCard.Margin = new Thickness(0, 0, 0, 0);
				lblSmallcard.Margin = new Thickness(0, 0, 0, 0);
				lblSmallcard.FontSize = 11;
				entCardNo.FontSize = 14;
				entCardNo.Margin = new Thickness(0, 0, 0, 0);
				boxCardNo.Margin = new Thickness(0, 0, 0, 0);

			}
			eventViewModel.isMonthSelected = false;
			eventViewModel.isYearSelected = false;
			eventViewModel.ClearPaymentFields();
			lblMonth.Text = "MM";
			lblYear.Text = "YYYY";
            await eventViewModel.GetCountries();
            SetCountryPickervalue();
		}
		void SetCountryPickervalue()
		{
            int selectedIndex = 0;
            if (eventViewModel.CountryList != null && eventViewModel.CountryList.Count > 0)
			{
				if (countryPicker.Items.Count > 0)
				{
					countryPicker.Items.Clear();
				}
				foreach (var country in eventViewModel.CountryList)
				{
                    if (!string.IsNullOrEmpty(country.name) && country.name.ToLower().Equals("canada"))
                    {
                        countryPicker.Items.Add(country.name);
                        selectedIndex = countryPicker.Items.IndexOf(country.name);
                    }
                    else
                    {
                        countryPicker.Items.Add(country.name);
                    }
				}
                countryPicker.SelectedIndex = selectedIndex;
                lblCountry.Text = eventViewModel.EventBillingCountry = eventViewModel.CountryList[selectedIndex].name;
			}

		}


		void MonthPicker_DoneClicked(object sender, System.EventArgs e)
		{
			ApplyMonthPicker();
		}

		void YearPicker_DoneClicked(object sender, System.EventArgs e)
		{
			ApplyYearPicker();
		}
		void CountryPicker_DoneClicked(object sender, System.EventArgs e)
		{
			ApplyCountryPicker();
		}

		void MonthPickerTapped(object sender, System.EventArgs e)
		{
			SetMonthPickerValues();
			Device.BeginInvokeOnMainThread(() =>
							{
								if (monthPicker.IsFocused)
									monthPicker.Unfocus();
								monthPicker.Focus();
							});
		}

		void CountryPickerTapped(object sender, System.EventArgs e)
		{
			SetCountryPickervalue();
			Device.BeginInvokeOnMainThread(() =>
							{
								if (countryPicker.IsFocused)
									countryPicker.Unfocus();
								countryPicker.Focus();
							});
		}

		void YearPickerTapped(object sender, System.EventArgs e)
		{
			SetYearPickerValues();
			Device.BeginInvokeOnMainThread(() =>
										{
											if (yearPicker.IsFocused)
												yearPicker.Unfocus();
											yearPicker.Focus();
										});
		}

		void SetYearPickerValues()
		{
			yearPicker.Items.Clear();

			for (int i = Constant.ExpiryYearStart; i < (Constant.ExpiryYearStart + Constant.ExpriryYearRange); i++)
			{
				yearPicker.Items.Add(i.ToString());
			}
		}


		void SetMonthPickerValues()
		{
			monthPicker.Items.Clear();
			var monthItems = Enum.GetValues(typeof(Months)).Cast<Months>();
			foreach (var item in monthItems)
				monthPicker.Items.Add(item.ToString());
		}

		void Year_SelectedIndexChanged(object sender, System.EventArgs e)

		{
			if (Device.RuntimePlatform == Device.Android)
			{
				ApplyYearPicker();
			}
		}

		void Month_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (Device.RuntimePlatform == Device.Android)
			{
				ApplyMonthPicker();
			}
		}
		void Country_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (Device.RuntimePlatform == Device.Android)
			{
				ApplyCountryPicker();
			}
		}

		void ApplyCountryPicker()
		{
			if (countryPicker.SelectedIndex >= 0)
			{
				lblCountry.Text = eventViewModel.EventBillingCountry = countryPicker.Items[countryPicker.SelectedIndex];
			}
		}

		void ApplyMonthPicker()
		{
			if (monthPicker.SelectedIndex >= 0)
			{
				lblMonth.Text = monthPicker.SelectedIndex < 9 ? "0" + Convert.ToString(monthPicker.SelectedIndex + 1) : Convert.ToString(monthPicker.SelectedIndex + 1);
				eventViewModel.ExpiryMonth = monthPicker.SelectedIndex + 1;
				eventViewModel.isMonthSelected = true;
			}
		}

		void ApplyYearPicker()
		{
			if (yearPicker.SelectedIndex >= 0)
			{
				lblYear.Text = eventViewModel.ExpiryYear = yearPicker.Items[yearPicker.SelectedIndex];
				eventViewModel.isYearSelected = true;
			}
		}

		async void BackIcon_Tapped(object sender, System.EventArgs e)

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

		void EntryCard_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
		{
			if (string.IsNullOrEmpty(e.NewTextValue))
			{
				lblSmallcard.TextColor = Color.Transparent;
			}
			else if (e.OldTextValue != null && (e.NewTextValue.Length > e.OldTextValue.Length))
			{
				if (entCardNo.Text.Replace(" ", "").Length % 4 == 0 && entCardNo.Text.Substring(entCardNo.Text.Length - 1, 1) != " " && entCardNo.Text.Replace(" ", "").Length <= 16)
				{
					entCardNo.Text = entCardNo.Text + " ";
				}
				lblSmallcard.TextColor = Color.FromHex(Constant.GrayTextColor);
			}
			else
			{
				lblSmallcard.TextColor = Color.FromHex(Constant.GrayTextColor);
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
                eventViewModel.PaymentEmail = e.NewTextValue;
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
                    else if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmail.Text.Trim(), Constant.EmailRegEx))
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
                            eventViewModel.transactionEmail = email;
                            _tapCount = 0;
                            ShowToast(Constant.AlertTitle, "Verified successfully");
                            eventViewModel.IsEmailVerified = true;
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
        protected override void OnAppearing()
        {
            base.OnAppearing();       
        }
    }
}
