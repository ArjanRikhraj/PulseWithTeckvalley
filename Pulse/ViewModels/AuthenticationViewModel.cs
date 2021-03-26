using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Connectivity;
using Plugin.DeviceInfo;
using Xamarin.Auth;
using Xamarin.Forms;

namespace Pulse
{
	public class AuthenticationViewModel : BaseViewModel
	{
		#region Private variables
		string id;
		string facebookEmail;
		string firstNameSocial;
		string lastNameSocial;
		string name;
		string email;
		string mobile;
		string password;
		string newPassword;
		string confirmPassword;
		bool isOTPErrorMessageVisible;
		string userName;
		Color otpTextColor;
		MainServices mainService;
		string uniqueImageName;
		bool isUserValidVisible;
		bool isUserInValidVisible;
		string confirmNewPassword;
		string oldPassword;
		string newChangePassword;
		OAuth2Authenticator authenticator;
		PulseViewModel pulseViewModel;
		#endregion
		#region Public Properties
		public string FacebookID;
		public bool IsTermAndConditionAccepted;
		public bool IsUserExistPending;
		public bool IsSocialSignUp;
		public string Username;
		public bool IsSignUpPage;
		public ICommand LoginClick { get; private set; }
		public ICommand SignUpClick { get; private set; }
		public ICommand ForgotPasswordClick { get; private set; }
		public ICommand SendOTPAgainClick { get; set; }
		public ICommand ResetClick { get; set; }
		public ICommand ChangePassword_Click { get; private set; }
		public ICommand SignUpDoneClick { get; set; }

		public bool IsUserValidVisible
		{
			get { return isUserValidVisible; }
			set
			{
				isUserValidVisible = value;
				OnPropertyChanged("IsUserValidVisible");

			}
		}
		public bool IsUserInValidVisible
		{
			get { return isUserInValidVisible; }
			set
			{
				isUserInValidVisible = value;
				OnPropertyChanged("IsUserInValidVisible");

			}
		}
		public string Email
		{
			get { return email; }
			set
			{
				email = value;
				OnPropertyChanged("Email");

			}

		}
		public string Mobile
		{
			get { return mobile; }
			set
			{
				mobile = value;
				OnPropertyChanged("Mobile");

			}

		}
		public string Password
		{
			get { return password; }
			set
			{
				password = value;
				OnPropertyChanged("Password");

			}
		}
		public string NewPassword
		{
			get { return newPassword; }
			set
			{
				newPassword = value;
				OnPropertyChanged("NewPassword");

			}
		}
		public string OldPassword
		{
			get { return oldPassword; }
			set
			{
				oldPassword = value;
				OnPropertyChanged("OldPassword");

			}
		}
		public string NewChangePassword
		{
			get { return newChangePassword; }
			set
			{
				newChangePassword = value;
				OnPropertyChanged("NewChangePassword");

			}
		}
		public string ConfirmNewPassword
		{
			get { return confirmNewPassword; }
			set
			{
				confirmNewPassword = value;
				OnPropertyChanged("ConfirmNewPassword");

			}
		}

		public string ConfirmPassword
		{
			get { return confirmPassword; }
			set
			{
				confirmPassword = value;
				OnPropertyChanged("ConfirmPassword");

			}
		}
		public Color OTPTextColor
		{
			get { return otpTextColor; }
			set
			{
				otpTextColor = value;
				OnPropertyChanged("OTPTextColor");

			}
		}

		public string Name
		{
			get { return name; }
			set
			{
				name = value;
				OnPropertyChanged("Name");

			}
		}
		public string UserName
		{
			get { return userName; }
			set
			{
				userName = value;
				OnPropertyChanged("UserName");

			}
		}

		public bool IsOTPErrorMessageVisible
		{
			get { return isOTPErrorMessageVisible; }
			set
			{
				isOTPErrorMessageVisible = value;
				OnPropertyChanged("IsOTPErrorMessageVisible");

			}
		}


		public SocialUserDetails FacebookUserDetails
		{
			get;
			set;
		}
		#endregion
		#region constructor
		public AuthenticationViewModel()
		{
			LoginClick = new Command(Login);
			SignUpClick = new Command(SignUp);
			ForgotPasswordClick = new Command(ForgotPassword);
			SendOTPAgainClick = new Command(SendOtpAgain);
			ResetClick = new Command(ResetPassword);
			SignUpDoneClick = new Command(RegisterUser);
			ChangePassword_Click = new Command(ChangePassword);
			mainService = new MainServices();
			pulseViewModel = ServiceContainer.Resolve<PulseViewModel>();
		}

		#endregion
		#region Methods
		public async Task<ResultCustom> GetFacebookProfile(Account account)
		{
			ResultCustom result = new ResultCustom();
			result.Status = false;
			try
			{

				var memberResult = await GetFacebookMember(account);
				if (memberResult != null)
				{
					id = memberResult.Id;
					facebookEmail = memberResult.Email;
					if (!string.IsNullOrEmpty(memberResult.Name))
						GetName(memberResult.Name);
					this.FacebookUserDetails = memberResult;
					result.Status = true;
				}
				else
				{
					result.Message = Constant.SOCIAL_LOGIN_ERROR_MESSAGE;
				}
			}
			catch (Exception ex)
			{
				result.IsError = true;
				result.Message = ex.Message;
			}
			return result;
		}

	public async Task<SocialUserDetails> GetFacebookMember(Account account)
		{
			var fbRequest = new OAuth2Request("GET", new Uri(Constant.FacebookProfileUrl), null, account);
			var profileResponse = await fbRequest.GetResponseAsync();
			SocialUserDetails member = null;
			if (profileResponse != null && profileResponse.StatusCode == System.Net.HttpStatusCode.OK)
			{
				member = JsonManager.DeSerialize<SocialUserDetails>(await profileResponse.GetResponseTextAsync());
			}
			return member;
		}
		public void GetName(String name)
		{
			string[] names = name.Split(' ');
			firstNameSocial = names[0];
			lastNameSocial = string.Empty;
			if (names.Length > 1)
			{
				lastNameSocial = names[names.Length - 1];
			}
		}
		UserData GetSocialUserDetails()
		{
			UserData user = new UserData();
			user.fullname = firstNameSocial + lastNameSocial;
			user.username = UserName;
			user.device_id = CrossDeviceInfo.Current.Id;
			user.profile_image = !string.IsNullOrEmpty(uniqueImageName) ? uniqueImageName + Constant.AWS_File_Ext : string.Empty;
			user.device_token = Settings.AppSettings.GetValueOrDefault(Constant.FcmToken, string.Empty) == string.Empty ? Constant.FcmToken : Settings.AppSettings.GetValueOrDefault(Constant.FcmToken, string.Empty);
			user.device_type = Device.RuntimePlatform.Equals(Constant.Android) ? "1" : "0";
			user.registration_platform = 1;
			user.fb_profileid = id;
			return user;
		}

		UserData GetSignUpUserData()
		{
			UserData user = new UserData();
			user.fullname = Name;
			user.password = Password;
			user.username = UserName;
			user.mobile = Mobile;
			user.email = Email.Trim();
			user.dob = DateTime.Now.AddYears(-15);
			user.registration_platform = 0;
			user.profile_image = !string.IsNullOrEmpty(uniqueImageName) ? uniqueImageName + Constant.AWS_File_Ext : string.Empty;
			user.device_id = CrossDeviceInfo.Current.Id;
			user.device_token = Settings.AppSettings.GetValueOrDefault(Constant.FcmToken, string.Empty) == string.Empty ? Constant.FcmToken : Settings.AppSettings.GetValueOrDefault(Constant.FcmToken, string.Empty);
			user.device_type = Device.RuntimePlatform.Equals(Constant.Android) ? "1" : "0";
			return user;
		}
        public Action SuccessfullLoginAction
        {
            get
            {
                return new Action(
                    async() =>
                    {
                        await CheckSocialUserExist();
                    });
            }
        }

		public async Task CheckSocialUserExist()
		{
			try
			{
				if (CrossConnectivity.Current.IsConnected)
				{
					if (TapCount < 1)
					{
						TapCount = 1;
						IsLoading = true;
						await mainService.GetToken();
						if (!string.IsNullOrEmpty(SessionManager.AccessToken))
						{
							UserData user = new UserData();
							user.device_id = CrossDeviceInfo.Current.Id;
							user.fb_profileid = id;
							user.device_token = Settings.AppSettings.GetValueOrDefault(Constant.FcmToken, string.Empty) == string.Empty ? Constant.FcmToken : Settings.AppSettings.GetValueOrDefault(Constant.FcmToken, string.Empty);
							user.device_type = Device.RuntimePlatform.Equals(Constant.Android) ? "1" : "0";
							var result = await mainService.Post<ResultWrapperSingle<UserResponse>>(Constant.CheckSocialUserExistUrl, user);
							if (result != null && result.status == Constant.Status200)
							{
								SessionManager.UserId = result.response.id;
								SessionManager.AccessToken = result.response.access_token;
								Settings.AppSettings.AddOrUpdateValue("CustomUserAccessToken", SessionManager.AccessToken);
                                //Device.BeginInvokeOnMainThread(() => {
                                //    Application.Current.MainPage = new NavigationPage(new MainPage());
                                //});
                                //App.Current.MainPage = new MainPage();
                               // App.SuccessfullLoginAction.Invoke();
                                Application.Current.MainPage = new NavigationPage(new MainPage());
								TapCount = 0;
								IsLoading = false;
							}
							else if (result != null && result.status == Constant.Status111 && result.message != null && result.message.non_field_errors.Count > 0)
							{
								IsSocialSignUp = true;
								await Navigation.PushModalAsync(new ProfilePage());
								TapCount = 0;
								IsLoading = false;
							}
							else
							{
								await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
								TapCount = 0;
								IsLoading = false;
							}
						}
					}
					TapCount = 0;
				}

				else
				{
					IsUserExistPending = false;
					IsUserValidVisible = false;
					IsUserInValidVisible = false;
					await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
					TapCount = 0;
					IsLoading = false;
				}
			}
			catch (Exception)
			{
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
				TapCount = 0;
				IsLoading = false;
			}
		}
        public Action AlertAction
        {
            get
            {
                return new Action(async () => await Navigation.PopModalAsync(true));
            }
        }
		async Task SocialLogin()
		{
			try
			{
				if (CrossConnectivity.Current.IsConnected)
				{
					IsLoading = true;
					await mainService.GetToken();
					if (!string.IsNullOrEmpty(SessionManager.AccessToken))
					{
						var result = await mainService.Post<ResultWrapperSingle<LoginResponse>>(Constant.SocialSignUpUrl, GetSocialUserDetails());
						if (result != null && result.status == Constant.Status200)
						{
							SessionManager.UserId = result.response.id;
							SessionManager.AccessToken = result.response.access_token;
							SessionManager.Email = result.response.email;
							Settings.AppSettings.AddOrUpdateValue("CustomUserAccessToken", SessionManager.AccessToken);
							ClearFields();
							Application.Current.MainPage = new NavigationPage(new MainPage());
							if (App.ImageStream != null)
								App.ImageStream.Dispose();
							App.ImageByteStream = null;
							IsSocialSignUp = false;
							TapCount = 0;
							IsLoading = false;
						}
						else if (result != null && result.status == Constant.Status111 && result.message.username != null && result.message.username.Count > 0)
						{
							await App.Instance.Alert(result.message.username[0], Constant.AlertTitle, Constant.Ok);
							TapCount = 0;
							IsLoading = false;
						}
						else
						{
							await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
							TapCount = 0;
							IsLoading = false;
						}
					}
					IsLoading = false;
				}
				else
				{
					await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
					TapCount = 0;
				}
			}
			catch (Exception)
			{
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
				TapCount = 0;
				IsLoading = false;
			}
		}

		async void RegisterUser()
		{
			try
			{
				if (CrossConnectivity.Current.IsConnected)
				{
					if (TapCount < 1)
					{
						TapCount = 1;
						if (!string.IsNullOrEmpty(UserName))
						{
							IsLoading = true;
							await mainService.GetToken();
							if (!string.IsNullOrEmpty(SessionManager.AccessToken))
							{
                                uniqueImageName = string.Empty;
                                if (App.ImageByteStream != null)
                                {
                                    App.ImageStream = new MemoryStream(App.ImageByteStream);
                                    uniqueImageName = Guid.NewGuid().ToString().Substring(0, 7);
                                    await new AWSServices().UploadAWSFile(App.ImageStream, App.AWSCurrentDetails.response.images_path.user_profile, uniqueImageName + Constant.AWS_File_Ext);
                                }
								if (!IsSocialSignUp)
								{
									var result = await mainService.Post<ResultWrapperSingle<UserResponse>>(Constant.SignUpUrl, GetSignUpUserData());
									if (result != null && result.status == Constant.Status200)
									{
										SessionManager.UserId = result.response.id;
										SessionManager.AccessToken = result.response.access_token;
										SessionManager.Email = result.response.email;
										//SessionManager.Mobile = result.response.mobile;
										Settings.AppSettings.AddOrUpdateValue("CustomUserAccessToken", SessionManager.AccessToken);
										ClearFields();
										ShowToast(Constant.AlertTitle, Constant.RegisteredSuccessfully);
										//await App.Instance.Alert(Constant.RegisteredSuccessfully, Constant.AlertTitle, Constant.Ok);
										Application.Current.MainPage = new NavigationPage(new MainPage());
										if (App.ImageStream != null)
											App.ImageStream.Dispose();
										App.ImageByteStream = null;
										TapCount = 0;
										IsLoading = false;
									}
									else if (result != null && result.status == Constant.Status111 && result.message.username != null && result.message.username.Count > 0)
									{
										await App.Instance.Alert(result.message.username[0], Constant.AlertTitle, Constant.Ok);
										TapCount = 0;
										IsLoading = false;
									}
									else
									{
										await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
										TapCount = 0;
										IsLoading = false;
									}
								}
								else
								{
									await SocialLogin();
								}
							}
							IsLoading = false;
						}
						else
						{
							await App.Instance.Alert(Constant.UserNameRequired, Constant.AlertTitle, Constant.Ok);
							TapCount = 0;
							IsLoading = false;
						}
						TapCount = 0;
					}

				}
				else
				{
					await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
					TapCount = 0;
				}
			}
			catch (Exception)
			{
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
				TapCount = 0;
				IsLoading = false;
			}
		}

		UserData GetLoginUserData()
		{
			UserData user = new UserData();
			user.password = Password;
			user.email = Email.Trim();
			user.device_id = CrossDeviceInfo.Current.Id;
			user.device_token = Settings.AppSettings.GetValueOrDefault(Constant.FcmToken, string.Empty) == string.Empty ? Constant.FcmToken : Settings.AppSettings.GetValueOrDefault(Constant.FcmToken, string.Empty);
			user.device_type = Device.RuntimePlatform.Equals(Constant.Android) ? "1" : "0";
			return user;
		}


		async void Login()
		{
			try
			{
				if (CrossConnectivity.Current.IsConnected)
				{
					if (TapCount < 1)
					{
						TapCount = 1;
						if (LoginValidate())
						{
							IsLoading = true;
							await mainService.GetToken();
							if (!string.IsNullOrEmpty(SessionManager.AccessToken))
							{
								var result = await mainService.Post<ResultWrapperSingle<LoginResponse>>(Constant.SignInUrl, GetLoginUserData());
								if (result != null && result.status == Constant.Status200)
								{
									SessionManager.UserId = result.response.id;
									SessionManager.AccessToken = result.response.access_token;
									SessionManager.Email = result.response.email;
                                    SessionManager.UnreadNotification = result.response.unread_notification > Constant.StatusZero;
									Settings.AppSettings.AddOrUpdateValue("CustomUserAccessToken", SessionManager.AccessToken);
									ClearFields();
									Application.Current.MainPage = new NavigationPage(new MainPage());
									TapCount = 0;
									IsLoading = false;
								}
								else if (result != null && result.status == Constant.Status111 && result.message.non_field_errors != null && result.message.non_field_errors.Count > 0)
								{
									await App.Instance.Alert(result.message.non_field_errors[0], Constant.AlertTitle, Constant.Ok);
									TapCount = 0;
									IsLoading = false;
								}
								else
								{
									await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
									TapCount = 0;
									IsLoading = false;
								}
							}
							IsLoading = false;
						}
						TapCount = 0;
					}
				}
				else
				{
					await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
					TapCount = 0;
				}
			}
			catch (Exception e)
			{
                await App.Instance.Alert(e.Message, Constant.AlertTitle, Constant.Ok);
				TapCount = 0;
				IsLoading = false;
			}
		}

		bool LoginValidate()
		{
			if (string.IsNullOrEmpty(Email))
			{
				App.Instance.Alert(Constant.EnterEmail, Constant.AlertTitle, Constant.Ok);
				return false;
			}
			else if (!Regex.IsMatch(Email.Trim(), Constant.EmailRegEx))
			{
				App.Instance.Alert(Constant.EmailNotValid, Constant.AlertTitle, Constant.Ok);
				return false;
			}
			else if (string.IsNullOrEmpty(Password))
			{
				App.Instance.Alert(Constant.EnterPassword, Constant.AlertTitle, Constant.Ok);
				return false;
			}
			return true;
		}
		bool CheckAplhaNumeric(string data)
		{
			bool isTextContains = false;
			bool isNumericContains = false;
			foreach (char c in data)
			{
				if (char.IsDigit(c))
					isNumericContains = true;
				if (char.IsLetter(c))
					isTextContains = true;
			}
			return isTextContains && isNumericContains;
		}
		async void SignUp()
		{
			try
			{
				if (CrossConnectivity.Current.IsConnected)
				{
					if (TapCount < 1)
					{
						TapCount = 1;
						if (IsSignUpValid())
						{
							IsLoading = true;
							await mainService.GetToken();
							if (!string.IsNullOrEmpty(SessionManager.AccessToken))
							{
								UserData user = new UserData();
								user.email = Email.Trim();
								user.mobile = Mobile;
								var result = await mainService.Post<ResultWrapperSingle<CheckEmailUserResponse>>(Constant.CheckEmailExistUrl, user);
								if (result != null && result.status == Constant.Status200)
								{
									if (await SendOtpOnEmail())
									{
										await Navigation.PushModalAsync(new EmailVerificationPage(Constant.SignUpText));
									}
									else
									{
										await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
									}
									TapCount = 0;
									IsLoading = false;
								}
								else if (result != null && result.status == Constant.Status111 && result.message.email != null)
								{
									await App.Instance.Alert(Constant.UserExists, Constant.AlertTitle, Constant.Ok);
									TapCount = 0;
									IsLoading = false;
								}
								else
								{
									await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
									TapCount = 0;
									IsLoading = false;
								}
							}
							IsLoading = false;
						}
						TapCount = 0;
					}
				}
				else
				{
					await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
					TapCount = 0;
				}
			}
			catch (Exception)
			{
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
				TapCount = 0;
				IsLoading = false;
			}
		}

		public async void VerifyToken(string token, string pageType)
		{
			try
			{
				if (CrossConnectivity.Current.IsConnected)
				{
					IsLoading = true;
					if (!string.IsNullOrEmpty(SessionManager.AccessToken))
					{
						UserData user = new UserData();
						user.email = Email.Trim();
						user.token = token;
						var result = await mainService.Put<ResultWrapperSingle<SendEmailOTPResponse>>(Constant.VerifyTokenUrl, user);
						if (result != null && result.status == Constant.Status200)
						{
							IsOTPErrorMessageVisible = false;
							OTPTextColor = Color.FromHex(Constant.BlackSimilarColor);
							await Navigation.PushModalAsync(new VerificationSuccessfullPage(pageType));
							TapCount = 0;
							IsLoading = false;
						}
						else if (result != null && result.status == Constant.Status111 && result.message.non_field_errors != null)
						{
							IsOTPErrorMessageVisible = true;
							OTPTextColor = Color.FromHex(Constant.OTPValidationColor);
							TapCount = 0;
							IsLoading = false;
						}
						else
						{
							IsOTPErrorMessageVisible = false;
							OTPTextColor = Color.FromHex(Constant.BlackSimilarColor);
							await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
							TapCount = 0;
							IsLoading = false;
						}
					}
					IsLoading = false;
				}
				else
				{
					await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
					TapCount = 0;
					IsLoading = false;
				}
			}
			catch (Exception)
			{
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
				TapCount = 0;
				IsLoading = false;
			}
		}

		async Task<bool> SendOtpOnEmail()
		{
			try
			{
				if (CrossConnectivity.Current.IsConnected)
				{
					IsLoading = true;
					if (!string.IsNullOrEmpty(SessionManager.AccessToken))
					{
						UserData user = new UserData();
						user.email = Email.Trim();
						user.mobile = Mobile;
						string url = IsSignUpPage ? Constant.SendOtpOnEmailUrl : Constant.ForgotPasswordUrl;
						var result = await mainService.Post<ResultWrapperSingle<SendEmailOTPResponse>>(url, user);
						if (result != null && result.status == Constant.Status200)
						{
							TapCount = 0;
							IsLoading = false;
							return true;
						}
						else
						{
							await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
							TapCount = 0;
							IsLoading = false;
							return false;
						}
					}
					IsLoading = false;
					return false;
				}
				else
				{
					await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
					TapCount = 0;
					IsLoading = false;
					return false;
				}
			}
			catch (Exception)
			{
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
				TapCount = 0;
				IsLoading = false;
				return false;
			}
		}

		async void ForgotPassword()
		{
			try
			{
				if (CrossConnectivity.Current.IsConnected)
				{
					if (TapCount < 1)
					{
						TapCount = 1;
						if (string.IsNullOrEmpty(Email))
						{
							await App.Instance.Alert(Constant.EnterEmail, Constant.AlertTitle, Constant.Ok);
							TapCount = 0;
						}
						else if (!Regex.IsMatch(Email.Trim(), Constant.EmailRegEx))
						{
							await App.Instance.Alert(Constant.EmailNotValid, Constant.AlertTitle, Constant.Ok);
							TapCount = 0;
						}
						else
						{
							IsLoading = true;
							await mainService.GetToken();
							if (!string.IsNullOrEmpty(SessionManager.AccessToken))
							{
								UserData user = new UserData();
								user.email = Email.Trim();
								var result = await mainService.Post<ResultWrapperSingle<SendEmailOTPResponse>>(Constant.ForgotPasswordUrl, user);
								if (result != null && result.status == Constant.Status200)
								{
									await Navigation.PushModalAsync(new EmailVerificationPage(Constant.ForgotPassword));
									TapCount = 0;
									IsLoading = false;
								}
								else if (result != null && result.status == Constant.Status111 && result.message.non_field_errors != null)
								{
									await App.Instance.Alert(Constant.UserNotExists, Constant.AlertTitle, Constant.Ok);
									TapCount = 0;
									IsLoading = false;
								}
								else
								{
									await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
									TapCount = 0;
									IsLoading = false;
								}
							}
							IsLoading = false;
							TapCount = 0;
						}
					}
				}
				else
				{
					await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
					TapCount = 0;
				}
			}
			catch (Exception)
			{
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
				TapCount = 0;
				IsLoading = false;
			}
		}

		async void SendOtpAgain()
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (TapCount < 1)
				{
					TapCount = 1;
					IsLoading = true;

					if (await SendOtpOnEmail())
					{
						ShowToast(Constant.AlertTitle, Constant.OtpResentText);
						//await App.Instance.Alert(Constant.OtpResentText, Constant.AlertTitle, Constant.Ok);
					}
					else
					{
						await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
					}
					TapCount = 0;
					IsLoading = false;
				}
			}
			else
			{
				await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
				TapCount = 0;
			}
		}

		async void ResetPassword()
		{
			try
			{
				if (CrossConnectivity.Current.IsConnected)
				{
					if (TapCount < 1)
					{
						TapCount = 1;
						if (IsResetPasswordValid())
						{
							IsLoading = true;
							await mainService.GetToken();
							if (!string.IsNullOrEmpty(SessionManager.AccessToken))
							{
								UserData user = new UserData();
								user.email = Email.Trim();
								user.password = NewPassword;
								user.confirm_password = ConfirmPassword;
								var result = await mainService.Put<ResultWrapperSingle<SendEmailOTPResponse>>(Constant.ResetPasswordUrl, user);
								if (result != null && result.status == Constant.Status200)
								{
									ShowToast(Constant.AlertTitle, Constant.PasswordResetSuccessfully);
									//await App.Instance.Alert(Constant.PasswordResetSuccessfully, Constant.AlertTitle, Constant.Ok);
									ClearFields();
									Application.Current.MainPage = new NavigationPage(new LoginPage());
									TapCount = 0;
									IsLoading = false;
								}
								else
								{
									await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
									TapCount = 0;
									IsLoading = false;
								}
							}
							IsLoading = false;
						}
						TapCount = 0;
					}
				}
				else
				{
					await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
					TapCount = 0;
				}
			}
			catch (Exception)
			{
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
				TapCount = 0;
				IsLoading = false;

			}
		}

		bool IsResetPasswordValid()
		{
			if (string.IsNullOrEmpty(NewPassword))
			{
				App.Instance.Alert(Constant.EnterNewPassword, Constant.AlertTitle, Constant.Ok);
				return false;
			}
			else if (NewPassword.Length < 6 || NewPassword.Length > 15)
			{
				App.Instance.Alert(Constant.PasswordLengthMessage, Constant.AlertTitle, Constant.Ok);
				return false;
			}
			else if (!CheckAplhaNumeric(NewPassword))
			{
				App.Instance.Alert(Constant.PasswordAlphanumericMessage, Constant.AlertTitle, Constant.Ok);
				return false;
			}
			else if (string.IsNullOrEmpty(ConfirmPassword))
			{
				App.Instance.Alert(Constant.EnterConfrimPassword, Constant.AlertTitle, Constant.Ok);
				return false;
			}
			else if (!NewPassword.Equals(ConfirmPassword))
			{
				App.Instance.Alert(Constant.PasswordMatchErrorMessage, Constant.AlertTitle, Constant.Ok);
				return false;
			}
			else
			{
				return true;
			}
		}

		bool IsSignUpValid()
		{

			if (string.IsNullOrEmpty(Name))
			{
				App.Instance.Alert(Constant.NameRequired, Constant.AlertTitle, Constant.Ok);
				return false;
			}
			else if (!LoginValidate())
			{
				return false;
			}
			else if (Password.Length < 6 || password.Length > 15)
			{
				App.Instance.Alert(Constant.PasswordLengthMessage, Constant.AlertTitle, Constant.Ok);
				return false;
			}
			else if (!CheckAplhaNumeric(Password))
			{
				App.Instance.Alert(Constant.PasswordAlphanumericMessage, Constant.AlertTitle, Constant.Ok);
				return false;
			}
			else if (!IsTermAndConditionAccepted)
			{
				App.Instance.Alert(Constant.TermsAccpetText, Constant.AlertTitle, Constant.Ok);
				return false;
			}
			else
			{
				return true;
			}
		}
		public void ClearFields()
		{
			Email = string.Empty;
			Password = string.Empty;
			NewPassword = string.Empty;
			ConfirmPassword = string.Empty;
			Name = string.Empty;
			UserName = string.Empty;
			OldPassword = string.Empty;
			NewChangePassword = string.Empty;
			ConfirmNewPassword = string.Empty;
		}

		public async Task GetAWSDetails()
		{
			try
			{
				if (!CrossConnectivity.Current.IsConnected)
				{
					await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
				}
				else
				{
					IsLoading = true;
					await mainService.GetToken();
					if (string.IsNullOrEmpty(SessionManager.AccessToken))
					{
						IsLoading = false;
						await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
					}
					else
					{
						var response = await mainService.Get<AwsDetail>(Constant.AwsDetailUrl);
						if (response != null && response.status == Constant.Status200 && response.response != null)
						{
							App.AWSCurrentDetails = response;
							SessionManager.RecordPerPage = response.response.records_per_page;
							await ValidateAppVersion();
						}
						else
						{
							IsLoading = false;
							await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
						}
					}
				}
			}
			catch (Exception)
			{
				IsLoading = false;
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
				return;
			}
		}

		async public Task ValidateAppVersion()
		{
			var shared = DependencyService.Get<ISharedService>();
			string deviceAppCode = shared.GetVersionCode();
			string currentAppCode = string.Empty;
			if (Device.RuntimePlatform == Device.Android)
			{
				currentAppCode = App.AWSCurrentDetails.response.andriod_app_version;
			}
			else if (Device.RuntimePlatform == Device.iOS)
			{
				currentAppCode = App.AWSCurrentDetails.response.ios_app_version;
			}
			IsLoading = false;
			int compareValue = 0;// String.Compare(deviceAppCode, currentAppCode);
            AutoLogin();

			if (compareValue != 0)
			{
				var answer = await App.Current.MainPage.DisplayAlert(Constant.ConfirmationHeader, Constant.UpdateAppMessage, Constant.Yes, Constant.No);
				if (answer)
				{
					shared.UpdateApp();
				}
				//if app version is not updated in the device then exit the app
				//to exit from the application
				DependencyService.Get<ISharedService>().CloseApp();
			}
			else
			{
				AutoLogin();
			}

		}

		public async void AutoLogin()
		{
			if (!CrossConnectivity.Current.IsConnected)
			{
				await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
			}
			else
			{
				try
				{
					IsLoading = true;
					if (!string.IsNullOrEmpty(Settings.AppSettings.GetValueOrDefault<string>("CustomUserAccessToken", string.Empty)))
					{
						string userToken = Settings.AppSettings.GetValueOrDefault<string>("CustomUserAccessToken", string.Empty);
						SessionManager.AccessToken = userToken;
						var result = await mainService.Get<UserDetailResponse>(Constant.GetUserDetail);
						if (result != null && result.status == Constant.Status200 && result.response.Count > 0)
						{
							SessionManager.AccessToken = userToken;
							SessionManager.UserId = result.response[0].id;
							SessionManager.Email = result.response[0].email;
							if (!string.IsNullOrEmpty(App.NotificationTitle) && (App.NotificationTitle == Constant.NewPulseNotificationTitle || App.NotificationTitle == "Pulse Message"))
							{
								App.NotificationTitle = string.Empty;
								Application.Current.MainPage = new NavigationPage(new MainPage(ActivePage.Pulse));
							}
							else if (!string.IsNullOrEmpty(App.NotificationTitle) && (App.NotificationTitle == Constant.FriendRequestNotificationTitle || App.NotificationTitle == Constant.FriendRequestAcceptedNotificationTitle))
							{
								Application.Current.MainPage = new NavigationPage(new MainPage(ActivePage.Friends));
							}
							else
							{
								Application.Current.MainPage = new NavigationPage(new MainPage());
							}
							IsLoading = false;
						}
						else
						{
						


							Settings.AppSettings.AddOrUpdateValue("CustomUserAccessToken", string.Empty);
							SessionManager.AccessToken = string.Empty;
							IsLoading = false;
						}
					}
					else
					{
						IsLoading = false;
					}
				}
				catch (Exception e)
				{
					Settings.AppSettings.AddOrUpdateValue("CustomUserAccessToken", string.Empty);
					SessionManager.AccessToken = string.Empty;
					IsLoading = false;
				}
			}
			IsLoading = false;
		}
		async void GetPulseDetail()
		{
			App.NotificationTitle = string.Empty;
			pulseViewModel.TappedPulseId = Convert.ToInt32(App.NotificationID);
			bool isDetail = await pulseViewModel.GetPulseDetail();
			if (isDetail && pulseViewModel.PulseDetail != null)
			{
				pulseViewModel.TappedPulseId = Convert.ToInt32(pulseViewModel.PulseDetail.id);
				pulseViewModel.PulseTitle = pulseViewModel.PulseDetail.name;
				await Navigation.PushModalAsync(new MessageChatPage(pulseViewModel.PulseDetail.user, pulseViewModel.PulseDetail.group_member_or_not, true));
			}
		}

		void InitializeFacebookAuthenticator()
		{
			authenticator
				 = new Xamarin.Auth.OAuth2Authenticator
				 (
					 clientId: Constant.FacebookAppID,
					 authorizeUrl: new Uri(Constant.FacebookAuthorizeUrl),
                     redirectUrl: new Uri(Constant.FacebookRedirectUrliOS),
					 scope: "email", // "basic", "email",
					 getUsernameAsync: null,
					 isUsingNativeUI: true

				 )
				 {
                AllowCancel = false,
				 };
		}

		async void Facebook_Authentication_Complete(object sender, AuthenticatorCompletedEventArgs e)
		{
            if(e.IsAuthenticated)
            {
                
            }
			if (e.Account != null && e.Account.Properties != null)
			{
				try
				{
					var result = await GetFacebookProfile(e.Account);
					if (result != null && result.Status)
					{
						await CheckSocialUserExist();
                        return;
					}
				}
				catch (Exception)
				{
					await App.Instance.Alert(Constant.AlertTitle, Constant.ServerNotRunningMessage, Constant.Ok);
				}
			}
			else
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("Not authenticated ").AppendLine($"Account.Properties does not exist");
				await App.Instance.Alert
									(
										"Authentication Results",
										sb.ToString(),
										"OK"
									);
			}
			return;
		}
		void Facebook_Authentication_Error(object sender, AuthenticatorErrorEventArgs e)
		{
			StringBuilder sb = new StringBuilder();
			if (!e.Message.ToLower().Contains("permissions+error"))
			{
				sb.AppendLine($"{e.Message}");
			}
			return;
		}
		void PresentUILoginScreen()
		{
			AuthenticationState.Authenticator = authenticator;
			Xamarin.Auth.Presenters.OAuthLoginPresenter presenter = null;
			presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(authenticator);
			return;
		}


		public async Task CallFacebook()
		{
			InitializeFacebookAuthenticator();
			authenticator.Completed += Facebook_Authentication_Complete;
			authenticator.Error += Facebook_Authentication_Error;
			PresentUILoginScreen();
		}


		async void ChangePassword()
		{
			if (TapCount < 1)
			{
				TapCount = 1;
				if (!CrossConnectivity.Current.IsConnected)
				{
					await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
				}
				else
				{
					try
					{
						if (ChangePasswordValidated())
						{
							IsLoading = true;
							if (!string.IsNullOrEmpty(SessionManager.AccessToken))
							{
								UserData user = new UserData();
								user.email = Email.Trim();
								user.oldpassword = OldPassword;
								user.newpassword = NewChangePassword;
								var result = await mainService.Put<ResultWrapperSingle<SendEmailOTPResponse>>(Constant.ChangePasswordUrl, user);
								if (result != null && result.status == Constant.Status200)
								{
									ShowToast(Constant.AlertTitle, Constant.PasswordResetSuccessfully);
									//await App.Instance.Alert(Constant.PasswordResetSuccessfully, Constant.AlertTitle, Constant.Ok);
									ClearFields();
									await Navigation.PopModalAsync();
									TapCount = 0;
									IsLoading = false;
								}
								else if (result != null && result.status == Constant.Status401)
								{
									SignOut();
									IsLoading = false;
								}
								else if (result != null && result.status == Constant.Status111 && result.message.non_field_errors != null && result.message.non_field_errors.Count > 0)
								{
									await App.Instance.Alert(result.message.non_field_errors[0], Constant.AlertTitle, Constant.Ok);
									IsLoading = false;
								}
								else
								{
									await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
									TapCount = 0;
									IsLoading = false;
								}
							}
							IsLoading = false;
						}

					}
					catch (Exception)
					{
						await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
					}
				}
				TapCount = 0;
				IsLoading = false;
			}
		}

		#endregion

		bool ChangePasswordValidated()
		{
			if (string.IsNullOrEmpty(OldPassword))
			{
				App.Instance.Alert(Constant.OldPasswordRequired, Constant.MessgeTitle, Constant.Ok);
				return false;
			}
			else if (OldPassword.Length < 6 || OldPassword.Length > 15)
			{
				App.Instance.Alert(Constant.PasswordLengthMessage, Constant.MessgeTitle, Constant.Ok);
				return false;
			}
			else if (!CheckAplhaNumeric(OldPassword))
			{
				App.Instance.Alert(Constant.PasswordAlphanumericMessage, Constant.MessgeTitle, Constant.Ok);
				return false;
			}
			if (string.IsNullOrEmpty(NewChangePassword))
			{
				App.Instance.Alert(Constant.EnterNewPassword, Constant.MessgeTitle, Constant.Ok);
				return false;
			}
			else if (NewChangePassword.Length < 6 || NewChangePassword.Length > 15)
			{
				App.Instance.Alert(Constant.PasswordLengthMessage, Constant.MessgeTitle, Constant.Ok);
				return false;
			}
			else if (!CheckAplhaNumeric(NewChangePassword))
			{
				App.Instance.Alert(Constant.PasswordAlphanumericMessage, Constant.MessgeTitle, Constant.Ok);
				return false;
			}
			else if (!NewChangePassword.Equals(ConfirmNewPassword))
			{
				App.Instance.Alert(Constant.ConfirmPasswordNotMatching, Constant.MessgeTitle, Constant.Ok);
				return false;
			}
			else
			{
				return true;
			}
		}
	}
}
