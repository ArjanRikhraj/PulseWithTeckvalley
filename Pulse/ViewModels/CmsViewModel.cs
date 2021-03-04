using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Pulse
{
	public class CmsViewModel : BaseViewModel
	{
		#region PrivateMembers

		private string queryEmailError;
		string address;
		string mobileNo;
		string email;
		string helpText;
		private string queryMessageError;

		private bool showQueryEmailError;

		private bool showQueryMessageError;

		private readonly MainServices mainService;

		#endregion PrivateMembers

		public CmsViewModel()
		{
			QueryRequest = new QueryRequest();
			QuerySubmitClick = new Command(SubmitQuery);
			QueryEmailError = string.Empty;
			QueryMessageError = string.Empty;
			ShowQueryEmailError = false;
			ShowQueryMessageError = false;
			mainService = new MainServices();
		}

		#region Properties

		public ICommand QuerySubmitClick { get; set; }

		public string QueryEmailError
		{
			get { return queryEmailError; }
			set
			{
				if (queryEmailError != value)
				{
					queryEmailError = value;
					OnPropertyChanged("QueryEmailError");
				}
			}
		}

		public string Address
		{
			get { return address; }
			set
			{
				address = value;
				OnPropertyChanged("Address");
			}
		}
		public string MobileNo
		{
			get { return mobileNo; }
			set
			{
				mobileNo = value;
				OnPropertyChanged("MobileNo");
			}
		}
		public string HelpText
		{
			get { return helpText; }
			set
			{
				helpText = value;
				OnPropertyChanged("HelpText");
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
		public string QueryMessageError
		{
			get { return queryMessageError; }
			set
			{
				if (queryMessageError != value)
				{
					queryMessageError = value;
					OnPropertyChanged("QueryMessageError");
				}
			}
		}

		public bool ShowQueryEmailError
		{
			get { return showQueryEmailError; }
			set
			{
				if (showQueryEmailError != value)
				{
					showQueryEmailError = value;
					OnPropertyChanged("ShowQueryEmailError");
				}
			}
		}

		public bool ShowQueryMessageError
		{
			get { return showQueryMessageError; }
			set
			{
				if (showQueryMessageError != value)
				{
					showQueryMessageError = value;
					OnPropertyChanged("ShowQueryMessageError");
				}
			}
		}

		public QueryRequest QueryRequest
		{
			get;
			set;
		}
		#endregion Properties

		#region Private Methods

		private bool ValidateQueryForm()
		{
			bool returnVal = true;
			QueryEmailError = string.Empty;
			QueryMessageError = string.Empty;
			ShowQueryEmailError = false;
			ShowQueryMessageError = false;
			if (string.IsNullOrEmpty(QueryRequest.Email))
			{
				QueryEmailError = Constant.ReqFieldCommonMessage;
				ShowQueryEmailError = true;
				returnVal = false;
			}
			else
			{
				if (!Regex.IsMatch(QueryRequest.Email.Trim(), Constant.EmailRegEx))
				{
					QueryEmailError = Constant.EmailNotValid;
					ShowQueryEmailError = true;
					returnVal = false;
				}
			}
			if (string.IsNullOrEmpty(QueryRequest.Message))
			{
				QueryMessageError = Constant.ReqFieldCommonMessage;
				ShowQueryMessageError = true;
				returnVal = false;
			}

			return returnVal;

		}
		private void InitializeTapAndLoading()
		{
			TapCount = 0;
			IsLoading = false;
		}

		#endregion Private Methods

		#region Public Methods
		private async void SubmitQuery()
		{
			try
			{
				if (!CrossConnectivity.Current.IsConnected)
				{
					await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
					InitializeTapAndLoading();
					return;
				}
				if (!ValidateQueryForm())
				{
					InitializeTapAndLoading();
					return;
				}
				if (string.IsNullOrEmpty(SessionManager.AccessToken))
				{
					InitializeTapAndLoading();
					return;
				}
				if (TapCount == 1)
				{
					return;
				}
				TapCount = 1;
				IsLoading = true;
				var result = await mainService.Post<ResultWrapperSingle<SendEmailOTPResponse>>(Constant.SubmitQueryUrl, this.QueryRequest);
				if (result != null && result.status == Constant.Status200)
				{
					TapCount = 0;
					IsLoading = false;
					//await App.Instance.Alert(result.response.details, Constant.Success, Constant.Ok);
					ShowToast(Constant.AlertTitle, result.response.details);
					QueryRequest query = new QueryRequest();
					await Navigation.PopModalAsync();
					return;
				}
				else
				{
					await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
					TapCount = 0;
					IsLoading = false;
					return;
				}


			}
			catch (Exception)
			{
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
				TapCount = 0;
				IsLoading = false;
			}
		}


		public async Task GetContactUsInfo()
		{
			try
			{
				if (!CrossConnectivity.Current.IsConnected)
				{
					await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
					TapCount = 0;
				}
				else
				{
					if (SessionManager.AccessToken != null)
					{
						IsLoading = true;
						var response = await mainService.Get<ResultWrapperSingle<ContactUsResponse>>(Constant.ContactUsUrl);
						if (response != null && response.status == Constant.Status200 && response.response != null)
						{
							Email = response.response.email;
							HelpText = response.response.help_text;
							MobileNo = response.response.mobile;
							Address = response.response.address;
							IsLoading = false;
						}
						else if (response != null && response.status == Constant.Status401)
						{
							SignOut();
							IsLoading = false;
						}
						else
						{
							await App.Instance.Alert(Constant.ApiResponseError, Constant.AlertTitle, Constant.Ok);
							IsLoading = false;
						}
					}
					else
					{
						await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
						IsLoading = false;
					}
				}
			}
			catch (Exception)
			{
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
				IsLoading = false;
				TapCount = 0;
			}
		}
		#endregion

	}
}
