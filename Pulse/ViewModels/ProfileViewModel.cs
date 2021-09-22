using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Pulse
{
	public class ProfileViewModel : BaseViewModel
	{
		#region Private variables
		string fullName;
		string updatedFullName;
		string userName;
		string score;
		string friendCount;
		string hostedCount;
		string attendeeCount;
		string school;
		DateTime userDob;
		string mobile;
		string countryCode;
		bool isEmailVisible;
		string email;
		string uniqueImageName;
		string profileIcon;
		#endregion
		#region Public Properties
		public ICommand UpdateUserClick { get; private set; }
		public string profileImage;
		public bool isChangePasswordShown;
		public string FullName
		{
			get { return fullName; }
			set
			{
				fullName = value;
				OnPropertyChanged("FullName");
			}
		}
		public string UpdatedFullName
		{
			get { return updatedFullName; }
			set
			{
				updatedFullName = value;
				OnPropertyChanged("UpdatedFullName");
			}
		}
		public DateTime UserDob
		{
			get { return userDob; }
			set
			{
				userDob = value;
				OnPropertyChanged("UserDob");
			}
		}

		public string ProfileIcon
		{
			get { return profileIcon; }
			set
			{
				profileIcon = value;
				OnPropertyChanged("ProfileIcon");
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
		public string Score
		{
			get { return score; }
			set
			{
				score = value;
				OnPropertyChanged("Score");
			}
		}
		public string Mobile
		{
			get
			{
				return mobile;
			}
			set
			{
				mobile = value;
				OnPropertyChanged("Mobile");
			}
		}
		public string Email
		{
			get
			{
				return email;
			}
			set
			{
				email = value;
				OnPropertyChanged("Email");
			}
		}
		public string CountryCode
		{
			get
			{
				return countryCode;
			}
			set
			{
				countryCode = value;
				OnPropertyChanged("CountryCode");
			}
		}
		public string School
		{
			get { return school; }
			set
			{
				school = value;
				OnPropertyChanged("School");
			}
		}

		public string FriendCount
		{
			get { return friendCount; }
			set
			{
				friendCount = value;
				OnPropertyChanged("FriendCount");
			}
		}
		public string HostedCount
		{
			get { return hostedCount; }
			set
			{
				hostedCount = value;
				OnPropertyChanged("HostedCount");
			}
		}
		public string AttendeeCount
		{
			get { return attendeeCount; }
			set
			{
				attendeeCount = value;
				OnPropertyChanged("AttendeeCount");
			}
		}
		public bool IsEmailVisible
		{
			get { return isEmailVisible; }
			set
			{
				isEmailVisible = value;
				OnPropertyChanged("IsEmailVisible");
			}
		}
		public class MenuList
		{
			public int ID { get; set; }
			public string MenuImage{ get; set; }
			public string MenuTitle { get; set; }
		}

		private ObservableCollection<MenuList> profileMenuList;
		public ObservableCollection<MenuList> ProfileMenuList
        {

			get { return profileMenuList; }
			set
			{
				profileMenuList = value;
				OnPropertyChanged("ProfileMenuList");
			}
		}


		private bool shimmerIsActive;
		public bool ShimmerIsActive
		{
			get { return shimmerIsActive; }
			set
			{
				shimmerIsActive = value;
				OnPropertyChanged("ShimmerIsActive");
			}
		}
		
		


		#endregion
		#region constructor
		public ProfileViewModel()
		{
			UpdateUserClick = new Command(UpdateUser);
			ProfileMenuList = new ObservableCollection<MenuList>
			{
				new MenuList{ ID =1 , MenuTitle ="My Events",MenuImage=Constant.EventTitleImage},
				new MenuList{ ID =2 , MenuTitle ="Friends",MenuImage=Constant.FriendActiveIcon},
				new MenuList{ ID =3 , MenuTitle ="Photo Album",MenuImage="photo_album.png"},
				new MenuList{ ID =4 , MenuTitle ="Settings",MenuImage=Constant.SettingsIcon},
			};
			

		}
		#endregion
		#region Methods
		public async Task GetMyProfileDetail()
		{
			try
			{
				if (CrossConnectivity.Current.IsConnected)
				{
					if (!string.IsNullOrEmpty(SessionManager.AccessToken))
					{
						//App.ShowMainPageLoader();
						//IsLoading = true;
						ShimmerIsActive = true;
						var result = await new MainServices().Get<ResultWrapperSingle<FriendProfileResponse>>(Constant.MyProfileDetailUrl);
						if (result != null && result.status == Constant.Status200)
						{
							profileImage = result.response.profile_image;
							ProfileIcon = (!string.IsNullOrEmpty(result.response.profile_image)) ? PageHelper.GetUserImage(result.response.profile_image) : Constant.ProfileIcon;
							Score = Convert.ToString(result.response.scores);
							AttendeeCount = Convert.ToString(result.response.attended_events);
							FriendCount = Convert.ToString(result.response.friends);
							HostedCount = Convert.ToString(result.response.hosted_events);
							isChangePasswordShown = string.IsNullOrEmpty(result.response.fb_profileid);
							UserName = result.response.username;
							FullName = result.response.fullname;
							UpdatedFullName = result.response.fullname;
							UserDob = !string.IsNullOrEmpty(result.response.dob) ? DateTime.Parse(result.response.dob) : DateTime.Now.AddYears(-15);
							School = result.response.school;
							IsEmailVisible = !string.IsNullOrEmpty(result.response.email);
							Email = result.response.email;
							if (!string.IsNullOrEmpty(result.response.mobile))
							{
								string[] mobile = result.response.mobile.Split('-');
								if (mobile != null)
								{
									Mobile = mobile[1];
									CountryCode = !string.IsNullOrEmpty(mobile[0]) ? mobile[0].TrimStart('+') : "1";
								}
							}
							else
							{
								Mobile = string.Empty;
								CountryCode = "1";
							}
							//App.HideMainPageLoader();
							IsLoading = false;
						}
						else if (result != null && result.status == Constant.Status401)
						{
							SignOut();
							//App.HideMainPageLoader();
							IsLoading = false;
						}
						else
						{
							await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
							//App.HideMainPageLoader();
							IsLoading = false;
						}
					}
				}
				else
				{
					await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
					App.HideMainPageLoader();
					IsLoading = false;
				}
			}
			catch (Exception e)
			{
				ShimmerIsActive = false;
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
				TapCount = 0;
				App.HideMainPageLoader();
				IsLoading = false;
			}
			finally
            {
				ShimmerIsActive = false;
			}
		}

		async void UpdateUser()
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
					if (TapCount < 1)
					{
						if (SessionManager.AccessToken != null)
						{
							if (UpdateValidate())
							{
								IsLoading = true;
                                if (App.ImageByteStream!=null)
								{
									App.ImageStream = new MemoryStream(App.ImageByteStream);
									uniqueImageName = Guid.NewGuid().ToString().Substring(0, 7);
									await new AWSServices().UploadAWSFile(App.ImageStream, App.AWSCurrentDetails.response.images_path.user_profile, uniqueImageName + Constant.AWS_File_Ext);
								}
								var response = await new MainServices().Put<ResultWrapperSingle<FriendProfileResponse>>(Constant.UpdateUserUrl, GetUpdateUserData());
								if (response != null && response.status == Constant.Status200 && response.response != null)
								{
									FullName = response.response.fullname;
									uniqueImageName = string.Empty;
									ProfileIcon = (!string.IsNullOrEmpty(response.response.profile_image)) ? PageHelper.GetUserImage(response.response.profile_image) : Constant.ProfileIcon;
									if (App.ImageStream != null)
										App.ImageStream.Dispose();
									App.ImageByteStream = null;
									ShowToast(Constant.AlertTitle, Constant.UserUpdated);
									//await App.Instance.Alert(Constant.UserUpdated, Constant.AlertTitle, Constant.Ok);
									await Navigation.PopModalAsync();
									IsLoading = false;
									TapCount = 0;
								}
								else if (response != null && response.status == Constant.Status401)
								{
									SignOut();
									IsLoading = false;
								}
								else
								{
									await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
									IsLoading = false;
									TapCount = 0;
								}
							}
							else
							{
								TapCount = 0;
							}
						}
						else
						{
							await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
							IsLoading = false;
							TapCount = 0;
						}
					}
				}
			}
			catch (Exception e)
			{
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
				IsLoading = false;
				TapCount = 0;
			}
		}

		ProfileData GetUpdateUserData()
		{
			ProfileData profileData = new ProfileData();
			profileData.fullname = UpdatedFullName;
			profileData.mobile = "+" + CountryCode + "-" + Mobile;
			profileData.school = School;
			profileData.dob = UserDob;
			profileData.profile_image = !string.IsNullOrEmpty(uniqueImageName) ? uniqueImageName + Constant.AWS_File_Ext : profileImage;
			return profileData;
		}

		bool UpdateValidate()
		{
			if (string.IsNullOrEmpty(UpdatedFullName))
			{
				App.Instance.Alert(Constant.NameRequired, Constant.AlertTitle, Constant.Ok);
				return false;
			}
			else if (string.IsNullOrEmpty(CountryCode))
			{
				App.Instance.Alert(Constant.CountryCodeRequired, Constant.AlertTitle, Constant.Ok);
				return false;
			}
			else if (string.IsNullOrEmpty(Mobile))
			{
				App.Instance.Alert(Constant.MobileRequired, Constant.AlertTitle, Constant.Ok);
				return false;
			}
			else if (Mobile.Length < 10)
			{
				App.Instance.Alert(Constant.InvalidMobileMessage, Constant.AlertTitle, Constant.Ok);
				return false;
			}
			else
				return true;
		}
		#endregion
	}
}
