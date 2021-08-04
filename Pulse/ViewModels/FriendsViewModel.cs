using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Connectivity;
using Plugin.Messaging;
using Pulse.Models.Application.Events;
using Pulse.Models.Friends;
using Pulse.Pages.Event;
using Pulse.Pages.User;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Pulse
{
	public class FriendsViewModel : BaseViewModel
	{

		#region Private variables
		MainServices mainService;
		bool isAddFriendButtonVisible;
		bool isFriendsButtonVisible;
		bool isCancelRequestButtonVisible;
		bool isConfirmRequestButtonVisible;
		bool isNoFriendFoundVisible;
		bool isListFriendVisible;
		bool isNoUserFoundVisible;
		bool isListUserVisible;
		bool isPendingFriendVisible;
		bool isPendingNoFriendFoundVisible;
        string blockUnBlockText;

        string selectedUsername;

		string pendingText;
		string contactText="Sync Contacts";
		ObservableCollection<Friend> listUsers;
		ObservableCollection<Friend> listMyFriends;
		ObservableCollection<Friend> listPendingRequest;
		ObservableCollection<MyEvents> listMyFriendsEvents;
		List<UserResponse> FriendsList;
		List<FriendResponseForUser> UsersList;
		bool isFriendEventListVisible;
		bool isMediaListVisible;
		
		bool isNoEventVisible;
		bool isNoMediaVisible = false;
		
		List<MyEventResponse> HostedEventsList;
		Friend selectedFriend;
		#endregion
		#region Public Properties
		public int pageNoUser;
		public int pageNoFriend;
		public int pageNoFriendsEvent;
		public int pageNoPending;
		public bool isSearchedValue;
        public bool isAdmin;

		public string TappedFriendid;
		public int totalPagesFriends;
		public int totalPagesMyFriends;
		public int totalPagesPendingRequest;
		public int totalHostedEventPages;
		public List<FriendProfileResponse> FriendProfileList;
		public ICommand LoadMoreUsers { get; private set; }
		
		public ICommand LoadMoreMyFriends { get; private set; }
		public ICommand LoadMorePending { get; private set; }
		public ICommand LoadMoreFriendEvents { get; private set; }
		public ICommand AddFriendPageCommand { get; private set; }
		public ICommand CloseReportPopupCommand { get; private set; }
		public ObservableCollection<Friend> tempUserList;
		public ObservableCollection<Friend> tempFriendList;
		public ObservableCollection<Friend> tempPendingList;
		public ObservableCollection<MyEvents> tempFriendEventList;
		ObservableCollection<EventGallery> friendsMediaList;

		public string searchvalue;

		public string PendingText
		{
			get { return pendingText; }
			set
			{
				pendingText = value;
				OnPropertyChanged("PendingText");
			}
		}
		public string ContactText
		{
			get { return contactText; }
			set
			{
				contactText = value;
				OnPropertyChanged("ContactText");
			}
		}
		public ObservableCollection<MyEvents> ListMyFriendsEvents
		{
			get { return listMyFriendsEvents; }
			set
			{
				listMyFriendsEvents = value;
				OnPropertyChanged("ListMyFriendsEvents");
			}
		}
		public ObservableCollection<Friend> ListPendingRequest
		{
			get { return listPendingRequest; }
			set
			{
				listPendingRequest = value;
				OnPropertyChanged("ListPendingRequest");
			}
		}
		public ObservableCollection<Friend> ListUsers
		{
			get { return listUsers; }
			set
			{
				listUsers = value;
				OnPropertyChanged("ListUsers");
			}
		}
		public ObservableCollection<Friend> ListMyFriends
		{
			get { return listMyFriends; }
			set
			{
				listMyFriends = value;
				OnPropertyChanged("ListMyFriends");
			}
		}
		public ObservableCollection<EventGallery> FriendsMediaList
		{
			get { return friendsMediaList; }
			set
			{
				friendsMediaList = value;
				OnPropertyChanged("FriendsMediaList");
			}
		}
		public bool IsAddFriendButtonVisible
		{
			get { return isAddFriendButtonVisible; }
			set
			{
				isAddFriendButtonVisible = value;
				OnPropertyChanged("IsAddFriendButtonVisible");
			}
		}
        public bool IsAdmin
        {
            get { return isAdmin; }
            set
            {
                isAdmin = value;
                OnPropertyChanged("IsAdmin");
            }
        }
        public string SelectedUsername
        {
            get { return selectedUsername; }
            set
            {
                selectedUsername = value;
                OnPropertyChanged("SelectedUsername");
            }
        }
		public bool IsFriendEventListVisible
		{
			get { return isFriendEventListVisible; }
			set
			{
				isFriendEventListVisible = value;
				OnPropertyChanged("IsFriendEventListVisible");
			}
		}
		public bool IsMediaListVisible
		{
			get { return isMediaListVisible; }
			set
			{
				isMediaListVisible = value;
				OnPropertyChanged("IsMediaListVisible");
			}
		}
		public bool IsNoEventVisible
		{
			get { return isNoEventVisible; }
			set
			{
				isNoEventVisible = value;
				OnPropertyChanged("IsNoEventVisible");
			}
		}
		public bool IsNoMediaVisible
		{
			get { return isNoMediaVisible; }
			set
			{
				isNoMediaVisible = value;
				OnPropertyChanged("IsNoMediaVisible");
			}
		}
		public bool IsCancelRequestButtonVisible
		{
			get { return isCancelRequestButtonVisible; }
			set
			{
				isCancelRequestButtonVisible = value;
				OnPropertyChanged("IsCancelRequestButtonVisible");
			}
		}
		public bool IsFriendsButtonVisible
		{
			get { return isFriendsButtonVisible; }
			set
			{
				isFriendsButtonVisible = value;
				OnPropertyChanged("IsFriendsButtonVisible");
			}
		}
		public bool IsConfirmRequestButtonVisible
		{
			get { return isConfirmRequestButtonVisible; }
			set
			{
				isConfirmRequestButtonVisible = value;
				OnPropertyChanged("IsConfirmRequestButtonVisible");
			}
		}
		public bool IsNoFriendFoundVisible
		{
			get { return isNoFriendFoundVisible; }
			set
			{
				isNoFriendFoundVisible = value;
				OnPropertyChanged("IsNoFriendFoundVisible");
			}
		}
		public bool IsListFriendVisible
		{
			get { return isListFriendVisible; }
			set
			{
				isListFriendVisible = value;
				OnPropertyChanged("IsListFriendVisible");
			}
		}

		public bool IsNoUserFoundVisible
		{
			get { return isNoUserFoundVisible; }
			set
			{
				isNoUserFoundVisible = value;
				OnPropertyChanged("IsNoUserFoundVisible");
			}
		}
		public bool IsListUserVisible
		{
			get { return isListUserVisible; }
			set
			{
				isListUserVisible = value;
				OnPropertyChanged("IsListUserVisible");
			}
		}
		public bool IsPendingFriendVisible
		{
			get { return isPendingFriendVisible; }
			set
			{
				isPendingFriendVisible = value;
				OnPropertyChanged("IsPendingFriendVisible");
			}
		}
		public bool IsPendingNoFriendFoundVisible
		{
			get { return isPendingNoFriendFoundVisible; }
			set
			{
				isPendingNoFriendFoundVisible = value;
				OnPropertyChanged("IsPendingNoFriendFoundVisible");
			}
		}
        public string BlockUnBlockText
        {
            get { return blockUnBlockText; }
            set
            {
                blockUnBlockText = value;
                OnPropertyChanged("BlockUnBlockText");
            }
        }
		public Friend SelectedFriend
		{
			get { return selectedFriend; }
			set
			{
				selectedFriend = value;
				OnPropertyChanged("SelectedFriend");
				CollectionViewSelectedFriend(selectedFriend);
			}
		}
		private EventGallery mediaSelectedItem;
		public EventGallery MediaSelectedItem
		{
			get
			{
				return mediaSelectedItem;
			}
			set
			{
				mediaSelectedItem = value;
				OnPropertyChanged("MediaSelectedItem");
				if (mediaSelectedItem != null)
					ShowMedia(MediaSelectedItem);
			}
		}
		string contactsCount;
		public string ContactsCount
		{
			get { return contactsCount; }
			set
			{
				contactsCount = value;
				OnPropertyChanged("ContactsCount");
			}
		}
		private ObservableCollection<ContactsModel> contactList { get; set; }
		public ObservableCollection<ContactsModel> ContactList
		{
			get
			{
				return contactList;
			}
			set
			{
				contactList = value;
				OnPropertyChanged("ContactList");
			}
		}
		private ICommand friendTypeCommand { get; set; }
		public ICommand FriendTypeCommand
		{
			get
			{
				return friendTypeCommand ?? (friendTypeCommand = new Command<ContactsModel>((currentObject) => OnFriendTypeCommand(currentObject)));
			}
		}

       

        private List<string> reportCommentList;
		public List<string> ReportCommentList
		{
			get
			{
				return reportCommentList;
			}
			set
			{
				reportCommentList = value;
				OnPropertyChanged("ReportCommentList");
			}
		}
		private string selectedReason;
		public string SelectedReason
		{
			get
			{
				return selectedReason;
			}
			set
			{
				selectedReason = value;
				OnPropertyChanged("SelectedReason");
				ReportUser(selectedReason);
			}
		}
		private string descriptionComment;
		public string DescriptionComment
		{
			get
			{
				return descriptionComment;
			}
			set
			{
				descriptionComment = value;
				OnPropertyChanged("DescriptionComment");
			}
		}
		bool isReportPopupVisible;
		public bool IsReportPopupVisible
		{
			get
			{
				return this.isReportPopupVisible;
			}

			set
			{
				this.isReportPopupVisible = value;
				OnPropertyChanged("IsReportPopupVisible");
			}
		}
		bool isSearchBoxVisible;
		public bool IsSearchBoxVisible
		{
			get
			{
				return this.isSearchBoxVisible;
			}

			set
			{
				this.isSearchBoxVisible = value;
				OnPropertyChanged("IsSearchBoxVisible");
			}
		}
		string searchText;
		public string SearchText
		{
			get
			{
				return this.searchText;
			}

			set
			{
				this.searchText = value;
				OnPropertyChanged("SearchText");
				OnSearchCommand(searchText);
			}
		}

       

        public ICommand LoadMoreContacts
		{
			get;
			set;
		}

		public List<ContactsModel> allContacts;
		public List<ContactsModel> loadContacts;
		List<ContactsModel> PulseUserList;
		List<ContactsModel> MatchedContactList;
		public List<ContactsModel> LoadedContacts;
		int page = 2;
		#endregion
		#region Constructor
		public FriendsViewModel()
		{
			LoadMoreContacts = new Command(OnLoadMoreContacts);
			LoadMoreUsers = new Command(GetUsers);
			LoadMoreMyFriends = new Command(GetMyFriendsList);
			LoadMorePending = new Command(GetPendingFriendsList);
			LoadMoreFriendEvents = new Command(GetFriendsHostedEventList);
			AddFriendPageCommand = new Command(GetFriendPage);
			CloseReportPopupCommand = new Command(OnCloseReportPopupCommand);
			UsersList = new List<FriendResponseForUser>();
			tempUserList = new ObservableCollection<Friend>();
			tempFriendList = new ObservableCollection<Friend>();
			tempPendingList = new ObservableCollection<Friend>();
			tempFriendEventList = new ObservableCollection<MyEvents>();
			friendsMediaList = new ObservableCollection<EventGallery>();
			contactList = new ObservableCollection<ContactsModel>();
			mainService = new MainServices();
			GetAllReportComments();
		}
		#endregion

		#region Methods
		public async void GetAllUser()
		{
			try
			{
				if (SessionManager.AccessToken != null)
				{
					var response = await mainService.Get<ResultWrapper<UserData>>(Constant.GetAllPulseUserUrl);
					if (response != null && response.status == Constant.Status200 && response.response != null && response.response.Count > 0)
					{
						PulseUserList = new List<ContactsModel>();
						foreach (var item in response.response)
						{
							ContactsModel model = new ContactsModel();
							model.userId = item.id;
							model.name = item.fullname;
							model.profileImage = item.profile_image;
							model.friendType = item.request_type;
							model.contactNumber = item.mobile;
							PulseUserList.Add(model);
						}
					}
					GetAllContacts();
				}
			}
			catch (Exception ex)
			{
				IsLoading = false;
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
			}
		}
		public async void GetAllContacts()
		{
			try
			{
				var status = await Permissions.CheckStatusAsync<Permissions.ContactsRead>();
				if (status != PermissionStatus.Granted)
				{
					await Permissions.RequestAsync<Permissions.ContactsRead>();
					return;
				}
				if (status == PermissionStatus.Granted)
				{
					var result = await Contacts.GetAllAsync();
					allContacts = new List<ContactsModel>();
					foreach (var item in result.ToList())
					{
						ContactsModel model = new ContactsModel();
						model.profileImage = Constant.UserDefaultSquareImage;
						model.name = item.DisplayName;
						if (item.Phones != null && item.Phones.Count > 0)
							model.contactNumber = item.Phones[0].ToString();
						else
							model.contactNumber = "Contact not available";
						model.nonPulseUser = "Invite";
						allContacts.Add(model);
					}
					loadContacts = allContacts;
					GetAllMatchedContacts();
				}
				else
					await Permissions.RequestAsync<Permissions.ContactsRead>();
			}
			catch (Exception ex)
			{
				IsLoading = false;
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
			}
		}
		public async void GetAllMatchedContacts()
        {
            try
            {
				if(PulseUserList.Count>0 && allContacts.Count>0)
                {
					var userList = from first in PulseUserList
											   join second in allContacts
											   on first.contactNumber equals second.contactNumber
											   select first;
					if(userList.Count()>0)
                    {
						MatchedContactList = userList.ToList();
						MatchedContactList.AddRange(allContacts.Except(MatchedContactList));
						loadContacts = MatchedContactList;
					}
					
				}
				SetContacts();
			}
			catch (Exception ex)
			{
				IsLoading = false;
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
			}
		}
		public async void SetContacts()
		{
			try
			{
				if (loadContacts.Count > 0)
				{
					LoadedContacts = loadContacts;
					ContactList = new ObservableCollection<ContactsModel>();
					foreach (var item in loadContacts)
					{
						ContactsModel model = new ContactsModel();
						model.userId = item.userId;
						model.name = item.name;
						model.ProfileImage = !string.IsNullOrEmpty(item.profileImage) ? item.profileImage : Constant.UserDefaultSquareImage;
						model.FriendType = !string.IsNullOrEmpty(item.friendType) ? item.friendType : "Invite";
						model.contactNumber = item.contactNumber;
						ContactList.Add(model);
					}
				}
				IsLoading = false;
			}
			catch (Exception ex)
			{
				IsLoading = false;
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
			}
		}
		private void OnCloseReportPopupCommand()
		{
			IsReportPopupVisible = false;
		}
		private async void OnFriendTypeCommand(ContactsModel currentObject)
		{
            try
            {
				if (currentObject.friendType == "Invite")
				{
					string messageText = "";
					var smsMessenger = CrossMessaging.Current.SmsMessenger;
					if (smsMessenger.CanSendSms)
					{
						if (Device.RuntimePlatform == Device.Android)
							messageText = "Check it out \n https://play.google.com/store/apps/details?id=com.netsol.pulse";
						else if (Device.RuntimePlatform == Device.iOS)
							messageText = "Check it out \n https://apps.apple.com/in/app/pulse-nightlife/id1370756129";
						smsMessenger.SendSms(currentObject.contactNumber, messageText);
					}
					else
						await App.Instance.Alert("Dynamic linking not supported in this device", Constant.AlertTitle, Constant.Ok);
				}
				else if (currentObject.friendType == "Add Friend")
				{
					if (currentObject.userId > 0)
					{
						FriendRequest friend = new FriendRequest();
						friend.friend_id = Convert.ToInt32(currentObject.userId);
						var result = await mainService.Post<ResultWrapperSingle<AddFriendResponse>>(Constant.AddFriendUrl, friend);
						if (result != null && result.status == Constant.Status200)
						{
							GetAllUser();
						}
					}
				}
				else
				{
					FriendRequest friend = new FriendRequest();
					friend.friend_request_status = currentObject.friendType == "Cancel Request" ? 2 : 1;
					friend.friend_id = SessionManager.UserId;
					friend.user_id = Convert.ToInt32(currentObject.userId);
					var result = await mainService.Put<ResultWrapperSingle<AddFriendResponse>>(Constant.RequestChangeUrl, friend);
					if (result != null && result.status == Constant.Status200)
					{
						if (friend.friend_request_status == 2)
							currentObject.FriendType = "Add Friend";
						else
                        {
							var itemToRemove = loadContacts.SingleOrDefault(r => r.userId == currentObject.userId);
							loadContacts.Remove(itemToRemove);
						}
					}
					else
						await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
				}
			}
			catch (FeatureNotSupportedException ex)
			{
				await App.Instance.Alert("SMS API not supported in this device", Constant.AlertTitle, Constant.Ok);
			}
			catch (Exception ex)
			{
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
			}
		}
		
		private void OnSearchCommand(string searchText)
		{
			if (!string.IsNullOrEmpty(searchText))
			{
				loadContacts = allContacts.Where(s => searchText.All(w => s.name.Contains(w))).ToList();
				//loadMoreContacts = allContacts.Where(x => x.name == searchText).ToList();
				SetContacts();
			}
			else
				loadContacts = LoadedContacts;
		}
		public async void GetContacts(int page)
		{
			try
			{
				loadContacts = allContacts.Skip(page * 20).Take(20).ToList();
			}
			catch (Exception ex)
			{
				IsLoading = false;
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
			}
		}
		private async void OnLoadMoreContacts()
		{
			GetContacts(page);
			page += 1;
		}
		private async void GetAllReportComments()
		{
			try
			{
				reportCommentList = new List<string>();
				reportCommentList.Add("Bullying/harassment");
				reportCommentList.Add("False information");
				reportCommentList.Add("Violence or dangerous organizations");
				reportCommentList.Add("Scam or fraud");
				reportCommentList.Add("Intellectual property vioation");
				reportCommentList.Add("Sale of illegal or regulated goods");
			}
			catch (Exception ex)
			{
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
			}
		}
		private async void ReportUser(string reason)
		{
			try
			{
				
				if (!string.IsNullOrEmpty(reason))
				{
					if (string.IsNullOrEmpty(DescriptionComment))
					{
						await App.Instance.Alert(Constant.ReportDescriptionMessage, Constant.AlertTitle, Constant.Ok);
						return;
					}
					ReportUserRequest request = new ReportUserRequest();
					request.report_user_id = Convert.ToInt32(TappedFriendid);
					request.reason = reason;
					request.description = DescriptionComment;
					var response = await mainService.Post<ResultWrapperSingle<Stories>>(Constant.ReportUser, request);
					if (response != null && response.status == Constant.Status200 && response.response != null)
					{
						
						IsReportPopupVisible = false;
							await Navigation.PushModalAsync(new ReportConfirmationPage("User"));
					}
					reason = null;
				}
			}
			catch (Exception ex)
			{
				reason = null;
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
			}
		}
		private async void GetFriendPage(object obj)
		{
			try
			{
				await Navigation.PushModalAsync(new AddFriendsPage());
			}
			catch (Exception ex)
			{
				IsLoading = false;
				TapCount = 0;
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
			}
		}
		private async void CollectionViewSelectedFriend(Friend SelectedFriend)
		{
			try
			{
				IsLoading = true;
				if (!CrossConnectivity.Current.IsConnected)
				{
					await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
					TapCount = 0;
				}
                else
                {
					await Navigation.PushModalAsync(new FriendsProfilePage("My Friends", SelectedFriend.friendId.ToString()));
					SelectedFriend = null;
					App.HideMainPageLoader();
				}
				IsLoading = false;
			}
			catch (Exception ex)
			{
				IsLoading = false;
				TapCount = 0;
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
			}
		}
		public async void GetUsers()
		{
			try
			{
				IsLoading = true;
				if (!CrossConnectivity.Current.IsConnected)
				{
					await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
					TapCount = 0;
				}
				else
				{
					if (!string.IsNullOrEmpty(searchvalue))
					{
						bool isList = await GetSearchedUsers(searchvalue);
						SetUserList(isList, UsersList);
					}
				}
				IsLoading = false;
			}

			catch (Exception)
			{
				IsLoading = false;
				TapCount = 0;
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
			}
		}

		public async Task<bool> GetSearchedUsers(string searchKeyword)
		{
			try
			{
				if (!CrossConnectivity.Current.IsConnected)
				{
					await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
					TapCount = 0;
					return false;
				}
				else
				{
					if (SessionManager.AccessToken != null && (pageNoUser == 1 || pageNoUser <= totalPagesFriends))
					{
						UsersList = new List<FriendResponseForUser>();
						var response = await mainService.Get<ResultWrapper<FriendResponseForUser>>(Constant.SearchUserUrl + pageNoUser + Constant.SearchString + searchKeyword);
						if (response != null && response.status == Constant.Status200 && response.response != null && response.response.Count > 0)
						{
							foreach (var item in response.response)
							{
								UsersList.Add(item);
							}
							totalPagesFriends = GetPageCount(response.response[response.response.Count - 1].total_users);
							return true;
						}
						else if (response != null && response.status == Constant.Status401)
						{
							SignOut();
							IsLoading = false;
							return false;
						}
						else
						{
							return false;
						}
					}
					else
					{
						return false;
					}
				}
			}
			catch (Exception)
			{
				return false;
			}
		}
		private async void ShowMedia(EventGallery selectedItem)
		{
			try
			{
				bool isFromFriend;
				await Navigation.PushModalAsync(new ShowMedia(selectedItem, isFromFriend=true));
			}
			catch (Exception ex)
			{
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
			}
		}
		public async Task GetFriendMediaList()
		{
			try
			{
				if (!CrossConnectivity.Current.IsConnected)
				{
					await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
					TapCount = 0;
					return;
				}
				else
				{
					if (SessionManager.AccessToken != null)
					{
						List<EventMedia> MediaList = new List<EventMedia>();
						var url = string.Format(Constant.GetFriendMediaList, TappedFriendid, 1);
						var response = await mainService.Get<ResultWrapper<EventMedia>>(url);
						if (response != null && response.status == Constant.Status200 && response.response != null && response.response.Count > 0)
						{
							foreach (var item in response.response)
							{
								friendsMediaList.Add(new EventGallery
								{
									FileUrl = item.file_name,
									MediaId = item.id,
									IsPrivate = item.is_private,
									UserId = item.user_id,
									EventId = item.event_id,
									ImageWidth = App.ScreenWidth,
									ImageHeight = App.ScreenHeight / 1.2,
									FileName = item.file_type == 1 ? PageHelper.GetEventVideoThumbnail(item.file_thumbnail) : PageHelper.GetEventImage(item.file_name),
									IsPlayIconVisible = item.file_type == 1 ? true : false,
									EventName = item.event_name,
									VideoFileName = item.file_type == 1 ? PageHelper.GetEventTranscodedVideo(item.file_name) : "",
									MediaDate = SetEventDate(item.create_date),
									IsVisibleUserName = true,
									UserImage = !string.IsNullOrEmpty(item.profile_image) ? item.profile_image : string.Empty,
									UserName = !string.IsNullOrEmpty(item.user_name) ? item.user_name : string.Empty,
									IsImage = item.file_type == 1 ? false : true,
									VideoThumbnailFileName = item.file_type == 1 ? PageHelper.GetEventVideoThumbnail(item.file_thumbnail) : string.Empty
								});
								
							}
							FriendsMediaList = friendsMediaList;
							if (friendsMediaList.Count > 0)
                            {
								IsMediaListVisible = true;
								isNoMediaVisible = false;
							}
                            else
                            {
								IsMediaListVisible = false;
								isNoMediaVisible = true;
							}
								
						}
						else
						{
							IsMediaListVisible = false;
							isNoMediaVisible = true;
						}
					}
					else
					{
						IsMediaListVisible = false;
						isNoMediaVisible = true;
					}
				}
			}
			catch (Exception)
			{
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
				IsMediaListVisible = false;
				isNoMediaVisible = true;
			}
		}
		string SetEventDate(string createDate)
		{
			var dateStart = DateTime.Parse(createDate);
			return dateStart.Date.ToString("ddd,dd MMM").ToUpperInvariant() + ", " + dateStart.ToString("h:mm tt").Trim().ToUpperInvariant();
		}
		void SetUserList(bool isList, List<FriendResponseForUser> list)
		{
            if (isList && pageNoUser < 2 && !string.IsNullOrEmpty(searchvalue))
			{
				IsListUserVisible = true;
				IsNoUserFoundVisible = false;
				foreach (var item in list)
				{
					float cornerradius = Device.RuntimePlatform == Device.Android ? 1 : 5;
					string mutual = item.mutual_user_count <= 0 ? "No" : Convert.ToString(item.mutual_user_count);
					bool isAddFriendvisible = item.request_type == Constant.AddFriendText ? true : false;
					bool isFriendsvisible = item.request_type == Constant.FriendText ? true : false;
					bool iscancelRequestvisible = item.request_type == Constant.CancelRequestText ? true : false;
					bool isconfirmRequestvisible = item.request_type == Constant.ConfirmRequestText ? true : false;
					tempUserList.Add(new Friend { friendId = item.id, cornerRadius = cornerradius, friendUsername = item.username, friendFullname = item.fullname, friendPic = string.IsNullOrEmpty(item.profile_image) ? Constant.ProfileIcon : PageHelper.GetUserImage(item.profile_image), friendMutual = mutual + " Mutual Friends", IsAddFriendButtonVisible = isAddFriendvisible, IsFriendsButtonVisible = isFriendsvisible, IsCancelRequestButtonVisible = iscancelRequestvisible, IsConfirmRequestButtonVisible = isconfirmRequestvisible });
				}
				UsersList.Clear();
				ListUsers = tempUserList;
				pageNoUser++;
				IsLoading = false;
				isSearchedValue = false;
			}
            else if (isList && !string.IsNullOrEmpty(searchvalue))
			{
				IsListUserVisible = true;
				IsNoUserFoundVisible = false;
				foreach (var itemUser in list)
				{
					float cornerradius = Device.RuntimePlatform == Device.Android ? 1 : 5;
					string mutual = itemUser.mutual_user_count < 0 ? "No" : Convert.ToString(itemUser.mutual_user_count);
					bool isAddFriendvisible = itemUser.request_type == Constant.AddFriendText ? true : false;
					bool isFriendsvisible = itemUser.request_type == Constant.FriendText ? true : false;
					bool iscancelRequestvisible = itemUser.request_type == Constant.CancelRequestText ? true : false;
					bool isconfirmRequestvisible = itemUser.request_type == Constant.ConfirmRequestText ? true : false;
					tempUserList.Add(new Friend { friendId = itemUser.id, cornerRadius = cornerradius, friendUsername = itemUser.username, friendFullname = itemUser.fullname, friendPic = string.IsNullOrEmpty(itemUser.profile_image) ? Constant.ProfileIcon : PageHelper.GetUserImage(itemUser.profile_image), friendMutual = mutual + " Mutual Friends", IsAddFriendButtonVisible = isAddFriendvisible, IsFriendsButtonVisible = isFriendsvisible, IsCancelRequestButtonVisible = iscancelRequestvisible, IsConfirmRequestButtonVisible = isconfirmRequestvisible });
				}
				UsersList.Clear();
				ListUsers = tempUserList;
				pageNoUser++;
				IsLoading = false;
				isSearchedValue = false;
			}
            else if (!isList && pageNoUser < 2 && !string.IsNullOrEmpty(searchvalue))
			{
				IsListUserVisible = false;
				IsNoUserFoundVisible = true;
				IsLoading = false;
				isSearchedValue = false;
			}
			else
			{

				ListUsers = tempUserList;
				IsLoading = false;
				isSearchedValue = false;
			}

		}
		public async Task AddFriend(int friendId, bool isProfileDetailPage)
		{
			try
			{
				if (CrossConnectivity.Current.IsConnected)
				{
					if (TapCount < 1)
					{
						TapCount = 1;
						IsLoading = true;
						if (!string.IsNullOrEmpty(SessionManager.AccessToken))
						{
							FriendRequest friend = new FriendRequest();
							friend.friend_id = friendId;
							var result = await mainService.Post<ResultWrapperSingle<AddFriendResponse>>(Constant.AddFriendUrl, friend);
							if (result != null && result.status == Constant.Status200)
							{
								if (isProfileDetailPage)
								{
									await FriendProfileDetail();

								}
								else
								{
									tempUserList.Clear();
									pageNoUser = 1;
									totalPagesFriends = 1;
									GetUsers();
								}
								TapCount = 0;

							}
							else if (result != null && result.status == Constant.Status111 && result.message != null && result.message.friend_id != null && result.message.friend_id.Count > 0)
							{
								await App.Instance.Alert(result.message.friend_id[0], Constant.AlertTitle, Constant.Ok);
								if (isProfileDetailPage)
								{
									await FriendProfileDetail();
								}
								else
								{
									tempUserList.Clear();
									pageNoUser = 1;
									totalPagesFriends = 1;
									GetUsers();
								}
								TapCount = 0;
								IsLoading = false;

							}
							else if (result != null && result.status == Constant.Status401)
							{
								SignOut();
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
							IsLoading = false;
							TapCount = 0;
						}

					}
					else
					{
						await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
						TapCount = 0;
					}
				}
			}
			catch (Exception e)
			{
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
				TapCount = 0;
				IsLoading = false;
			}
		}

		public async Task ChangeRequestStatus(int friendId, FriendType friendType, string pageType, bool isProfileDetailPage)
		{
			try
			{
				if (CrossConnectivity.Current.IsConnected)
				{
					if (TapCount < 1)
					{
						TapCount = 1;
						IsLoading = true;
						if (!string.IsNullOrEmpty(SessionManager.AccessToken))
						{
							FriendRequest friend = new FriendRequest();
							friend.friend_request_status = friendType == FriendType.CancelRequest ? 2 : 1;
							if (pageType == Constant.SearchText)
							{
								friend.friend_id = friendType == FriendType.CancelRequest ? friendId : SessionManager.UserId;
								friend.user_id = friendType == FriendType.CancelRequest ? SessionManager.UserId : friendId;
								if (friendType == FriendType.Friends)
								{
									friend.friend_id = friendId;
									friend.user_id = SessionManager.UserId;
									friend.friend_request_status = 3;
								}
							}
							else
							{
								friend.friend_id = SessionManager.UserId;
								friend.user_id = friendId;
							}

							var result = await mainService.Put<ResultWrapperSingle<AddFriendResponse>>(Constant.RequestChangeUrl, friend);
							if (result != null && result.status == Constant.Status200)
							{
								if (isProfileDetailPage)
								{
									await FriendProfileDetail();
								}
								else
								{
									if (pageType == Constant.SearchText)
									{
										tempUserList.Clear();
										pageNoUser = 1;
										totalPagesFriends = 1;
										GetUsers();
									}
									else
									{
										tempPendingList.Clear();
										pageNoPending = 1;
										totalPagesPendingRequest = 1;
										GetPendingFriendsList();
									}
								}
								TapCount = 0;
							}
							else if (result != null && result.status == Constant.Status111 && result.message != null && result.message.non_field_errors != null && result.message.non_field_errors.Count > 0)
							{
								await App.Instance.Alert(result.message.non_field_errors[0], Constant.AlertTitle, Constant.Ok);
								if (isProfileDetailPage)
								{
									await FriendProfileDetail();
								}
								else
								{
									if (pageType == Constant.SearchText)
									{
										tempUserList.Clear();
										pageNoUser = 1;
										totalPagesFriends = 1;
										GetUsers();
									}
									else
									{
										tempPendingList.Clear();
										pageNoPending = 1;
										totalPagesPendingRequest = 1;
										GetPendingFriendsList();
									}
								}
								IsLoading = false;

							}
							else if (result != null && result.status == Constant.Status401)
							{
								SignOut();
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
							IsLoading = false;
							TapCount = 0;
						}

					}
					else
					{
						await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
						TapCount = 0;
					}
				}
			}
			catch (Exception)
			{
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
				TapCount = 0;
				IsLoading = false;
			}
		}

		public async void GetMyFriendsList()
		{
			try
			{
				IsLoading = true;
				App.ShowMainPageLoader();
				if (!CrossConnectivity.Current.IsConnected)
				{
					await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
				}
				else
				{
					bool isList = await GetMyFriends();
					SetFriendsList(isList, FriendsList);
				}
				App.HideMainPageLoader();
				IsLoading = false;
			}

			catch (Exception)
			{
				IsLoading = false;
				App.HideMainPageLoader();
				TapCount = 0;
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
			}
		}
        public async Task BlockUnblockFreind()
        {
            try
            {
                IsLoading = true;
                App.ShowMainPageLoader();
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                }
                else
                {
                    var response = await mainService.Post<ResultWrapperSingle<SendEmailOTPResponse>>(Constant.BlockUnBlockUserUrl, GetBlockUserData());
                    if (response != null && response.status == Constant.Status200)
                    {
                        App.Instance.Alert(response.response.details, Constant.AlertTitle, Constant.Ok);
                        BlockUnBlockText = BlockUnBlockText.Equals(Constant.UnBlockText) ? Constant.BlockText : Constant.UnBlockText;
                       
                        IsLoading = false;
                    }
                }
                App.HideMainPageLoader();
                IsLoading = false;
            }

            catch (Exception)
            {
                IsLoading = false;
                App.HideMainPageLoader();
                TapCount = 0;
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
            }
        }

        BlockUnBlockFriendData GetBlockUserData()
        {
            BlockUnBlockFriendData friendData = new BlockUnBlockFriendData();
            friendData.block_to =Convert.ToInt32(TappedFriendid);
            return friendData;
        }

        async Task<bool> GetMyFriends()
		{
			try
			{
				if (!CrossConnectivity.Current.IsConnected)
				{
					await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
					TapCount = 0;
					return false;
				}
				else
				{
					if (SessionManager.AccessToken != null && (pageNoFriend == 1 || pageNoFriend <= totalPagesMyFriends))
					{
						FriendsList = new List<UserResponse>();
						var response = await mainService.Get<UserDetailResponse>(Constant.MyFriendsListUrl + pageNoFriend);
						if (response != null && response.status == Constant.Status200 && response.response != null && response.response.Count > 0)
						{
							foreach (var item in response.response)
							{
								FriendsList.Add(item);
							}
							totalPagesMyFriends = GetPageCount(response.response[response.response.Count - 1].total_users);
							return true;

						}
						else if (response != null && response.status == Constant.Status401)
						{
							SignOut();
							App.HideMainPageLoader();
							IsLoading = false;
							return false;
						}
						else
						{
							return false;
						}
					}
					else
					{
						return false;
					}
				}
			}
			catch (Exception)
			{
				return false;
			}
		}

		void SetFriendsList(bool isList, List<UserResponse> list)
		{
			if (isList && pageNoFriend < 2)
			{
				IsListFriendVisible = true;
				IsNoFriendFoundVisible = false;
				foreach (var item in list)
				{
					float cornerradius = Device.RuntimePlatform == Device.Android ? 1 : 5;
					tempFriendList.Add(new Friend { friendId = item.id, cornerRadius = cornerradius, friendUsername = item.username, friendFullname = item.fullname, friendPic = string.IsNullOrEmpty(item.profile_image) ? Constant.ProfileIcon : PageHelper.GetUserImage(item.profile_image) });
				}
				FriendsList.Clear();
				ListMyFriends = tempFriendList;
				pageNoFriend++;
				IsLoading = false;
				App.HideMainPageLoader();
			}
			else if (isList)
			{
				IsListFriendVisible = true;
				IsNoFriendFoundVisible = false;
				foreach (var itemUser in list)
				{
					float cornerradius = Device.RuntimePlatform == Device.Android ? 1 : 5;
					tempFriendList.Add(new Friend { friendId = itemUser.id, cornerRadius = cornerradius, friendUsername = itemUser.username, friendFullname = itemUser.fullname, friendPic = string.IsNullOrEmpty(itemUser.profile_image) ? Constant.ProfileIcon : PageHelper.GetUserImage(itemUser.profile_image) });
				}
				FriendsList.Clear();
				ListMyFriends = tempFriendList;
				pageNoFriend++;
				IsLoading = false;
				App.HideMainPageLoader();
			}
			else if (!isList && pageNoFriend < 2)
			{
				IsListFriendVisible = false;
				IsNoFriendFoundVisible = true;
				IsLoading = false;
				App.HideMainPageLoader();
			}
			else
			{
				ListMyFriends = tempFriendList;
				IsLoading = false;
				App.HideMainPageLoader();
			}
		}


		public async void GetPendingFriendsList()
		{
			try
			{
				IsLoading = true;
				if (!CrossConnectivity.Current.IsConnected)
				{
					await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
				}
				else
				{
					bool isList = await GetPendingRequests();
					SetPendingFriendsList(isList, FriendsList);
				}
				IsLoading = false;
			}

			catch (Exception)
			{
				IsLoading = false;
				TapCount = 0;
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
			}
		}

		async Task<bool> GetPendingRequests()
		{
			try
			{
				if (!CrossConnectivity.Current.IsConnected)
				{
					await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
					TapCount = 0;
					return false;
				}
				else
				{
					if (!string.IsNullOrEmpty(SessionManager.AccessToken) && (pageNoPending == 1 || pageNoPending <= totalPagesPendingRequest))
					{
						FriendsList = new List<UserResponse>();
						var response = await mainService.Get<UserDetailResponse>(Constant.PendingFriendsListUrl + pageNoPending);
						if (response != null && response.status == Constant.Status200 && response.response != null && response.response.Count > 0)
						{
							foreach (var item in response.response)
							{
								FriendsList.Add(item);
							}
							totalPagesPendingRequest = GetPageCount(response.response[response.response.Count - 1].total_users);
							return true;

						}
						else if (response != null && response.status == Constant.Status401)
						{
							SignOut();
							IsLoading = false;
							return false;
						}
						else
						{
							return false;
						}
					}
					else
					{
						return false;
					}
				}
			}
			catch (Exception)
			{
				return false;
			}
		}

		void SetPendingFriendsList(bool isList, List<UserResponse> list)
		{
			if (isList && pageNoPending < 2)
			{
				IsPendingFriendVisible = true;
				IsPendingNoFriendFoundVisible = false;
				foreach (var item in list)
				{
					float cornerradius = Device.RuntimePlatform == Device.Android ? 1 : 5;
					string mutual = item.mutual_user_count <= 0 ? "No" : Convert.ToString(item.mutual_user_count);
					tempPendingList.Add(new Friend { friendId = item.id, friendMutual = mutual + " Mutual Friends", cornerRadius = cornerradius, friendUsername = item.username, friendFullname = item.fullname, friendPic = string.IsNullOrEmpty(item.profile_image) ? Constant.ProfileIcon : PageHelper.GetUserImage(item.profile_image) });
				}
				FriendsList.Clear();
				ListPendingRequest = tempPendingList;
				pageNoPending++;
				IsLoading = false;
			}
			else if (isList)
			{
				IsPendingFriendVisible = true;
				IsPendingNoFriendFoundVisible = false;
				foreach (var itemUser in list)
				{
					float cornerradius = Device.RuntimePlatform == Device.Android ? 1 : 5;
					string mutual = itemUser.mutual_user_count <= 0 ? "No" : Convert.ToString(itemUser.mutual_user_count);
					tempPendingList.Add(new Friend { friendId = itemUser.id, friendMutual = mutual + " Mutual Friends", cornerRadius = cornerradius, friendUsername = itemUser.username, friendFullname = itemUser.fullname, friendPic = string.IsNullOrEmpty(itemUser.profile_image) ? Constant.ProfileIcon : PageHelper.GetUserImage(itemUser.profile_image) });
				}
				FriendsList.Clear();
				ListPendingRequest = tempPendingList;
				pageNoPending++;
				IsLoading = false;
			}
			else if (!isList && pageNoPending < 2)
			{
				IsPendingFriendVisible = false;
				IsPendingNoFriendFoundVisible = true;
				IsLoading = false;
			}
			else
			{
				ListPendingRequest = tempPendingList;
				IsLoading = false;
			}
		}
		public async Task PendingRequestCount()
		{
			try
			{
				if (CrossConnectivity.Current.IsConnected)
				{
					if (!string.IsNullOrEmpty(SessionManager.AccessToken))
					{
						var result = await mainService.Get<ResultWrapperSingle<PendingRequestCountResponse>>(Constant.PendingRequestCountUrl);
						if (result != null && result.status == Constant.Status200)
						{
							PendingText = (result.response.request_count <= 0 ? "No" : Convert.ToString(result.response.request_count)) + " Pending Friend Request";
						}
						else if (result != null && result.status == Constant.Status401)
						{
							SignOut();
							IsLoading = false;
						}
						else
						{
							await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
						}
					}
					else
					{
						await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
					}
				}
			}
			catch (Exception)
			{
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
				TapCount = 0;
				IsLoading = false;
			}
		}
		public async Task<bool> FriendProfileDetail()
		{
			try
			{
				if (CrossConnectivity.Current.IsConnected)
				{
					if (!string.IsNullOrEmpty(SessionManager.AccessToken))
					{
						FriendProfileList = new List<FriendProfileResponse>();
						var result = await mainService.Get<ResultWrapper<FriendProfileResponse>>(Constant.ProfileDetailUrl + TappedFriendid + "/");
						if (result != null && result.status == Constant.Status200)
						{
							foreach (var itemUser in result.response)
							{
                                SelectedUsername = !string.IsNullOrEmpty(itemUser.fullname) ? itemUser.fullname : string.Empty;
								IsAddFriendButtonVisible = itemUser.request_type == Constant.AddFriendText ? true : false;
								IsFriendsButtonVisible = itemUser.request_type == Constant.FriendText ? true : false;
								IsCancelRequestButtonVisible = itemUser.request_type == Constant.CancelRequestText ? true : false;
								IsConfirmRequestButtonVisible = itemUser.request_type == Constant.ConfirmRequestText ? true : false;
                                BlockUnBlockText = itemUser.already_blocked ? Constant.UnBlockText : Constant.BlockText;
                                FriendProfileList.Add(itemUser);
								IsLoading = false;
							}
							return true;
						}
						else if (result != null && result.status == Constant.Status401)
						{
							SignOut();
							IsLoading = false;
							return false;
						}
						else
						{
							await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
							IsLoading = false;
							return false;
						}
					}
					else
					{
						IsLoading = false;
						return false;
					}
				}
				else
				{
					await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
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
		public async void GetMedia()
        {
            try
            {
				if (!CrossConnectivity.Current.IsConnected)
				{
					await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
				}
				else
				{
					
				}
			}
            catch (Exception ex)
            {
				IsLoading = false;
				TapCount = 0;
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
			}
        }
		public async void GetFriendsHostedEventList()
		{
			try
			{
				IsLoading = true;
				if (!CrossConnectivity.Current.IsConnected)
				{
					await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
				}
				else
				{
					bool isList = await GetFriendsHostedEvents();
					SetFriendsEventsList(isList, HostedEventsList);
				}
				IsLoading = false;
			}

			catch (Exception)
			{
				IsLoading = false;
				TapCount = 0;
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
			}
		}

		async Task<bool> GetFriendsHostedEvents()
		{
			try
			{
				if (!CrossConnectivity.Current.IsConnected)
				{
					await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
					TapCount = 0;
					return false;
				}
				else
				{
					if (SessionManager.AccessToken != null && (pageNoFriendsEvent == 1 || pageNoFriendsEvent <= totalHostedEventPages))
					{
						HostedEventsList = new List<MyEventResponse>();
						var response = await mainService.Get<ResultWrapper<MyEventResponse>>(Constant.FriendsPublicHostedEventUrl + TappedFriendid + "/?page=" + pageNoFriendsEvent);
						if (response != null && response.status == Constant.Status200 && response.response != null && response.response.Count > 0)
						{
							foreach (var item in response.response)
							{
								HostedEventsList.Add(item);
							}
							totalHostedEventPages = GetPageCount(response.response[response.response.Count - 1].total_events);
							return true;

						}
						else if (response != null && response.status == Constant.Status401)
						{
							SignOut();
							IsLoading = false;
							return false;
						}
						else
						{
							return false;
						}
					}
					else
					{
						return false;
					}
				}
			}
			catch (Exception)
			{
				return false;
			}
		}
		void SetFriendsEventsList(bool isList, List<MyEventResponse> list)
		{
			if (isList && pageNoFriendsEvent < 2)
			{
				SetFriendsEvents(list);
			}
			else if (isList)
			{
				SetFriendsEvents(list);
			}
			else if (!isList && pageNoFriendsEvent < 2)
			{
				IsFriendEventListVisible = false;
				IsNoEventVisible = true;
				IsLoading = false;
			}
			else
			{
				ListMyFriendsEvents = tempFriendEventList;
				IsLoading = false;
			}
		}
		void SetFriendsEvents(List<MyEventResponse> list)
		{
			IsFriendEventListVisible = true;
			IsNoEventVisible = false;
			foreach (var item in list)
			{
				float cornerradius = Device.RuntimePlatform == Device.Android ? 1 : 4;
				string loc = item.event_venue + "," + item.location_address;
				string attendee;
				string date = SetEventDate(item.start_date, item.start_time) + " " + item.time_zone_type;
				bool isImageOneVisible;
				bool isImageSecondVisible;
				string imageOne;
				string imageSecond;
				if (item.event_attendees_count <= 0)
				{
					attendee = "No attendees yet!";
					isImageOneVisible = false;
					isImageSecondVisible = false;
					imageOne = string.Empty;
					imageSecond = string.Empty;
				}
				else
				{
					attendee = item.event_attendees_count > 2 ? "+" + Convert.ToString(item.event_attendees_count - 2) + " More attendees" : "No More attendees";
					if (item.event_attendees_count >= 2)
					{
						isImageOneVisible = true;
						isImageSecondVisible = true;
						imageOne = !string.IsNullOrEmpty(item.attendees[0].profile_image) ? PageHelper.GetUserImage(item.attendees[0].profile_image) : Constant.UserDefaultSquareImage;
						imageSecond = !string.IsNullOrEmpty(item.attendees[1].profile_image) ? PageHelper.GetUserImage(item.attendees[1].profile_image) : Constant.UserDefaultSquareImage;
					}
					else
					{
						isImageOneVisible = true;
						isImageSecondVisible = false;
						imageOne = !string.IsNullOrEmpty(item.attendees[0].profile_image) ? PageHelper.GetUserImage(item.attendees[0].profile_image) : Constant.UserDefaultSquareImage;
						imageSecond = string.Empty;
					}
				}

				tempFriendEventList.Add(new MyEvents
				{
					EventId = item.id,
					EventName = item.name,
					EventLikes = Convert.ToString(item.event_likes_count),
					EventAddress = loc,
					EventLitScore = Convert.ToString(item.event_lit_score) + Constant.LitScoreText,
					AttendeeCount = attendee,
					EventDateTime = date,
					IsEditIconVisible = false,
					IsFirstImageVisible = isImageOneVisible,
					IsSecondImageVisible = isImageSecondVisible,
					AttendeeImageFirst = imageOne,
                    AttendeeImageSecond = imageSecond,
                    IsBoostEvent = item.is_boosted_event,
                    EventStatus = item.event_status_label,
                    IsShowViewAll = item.event_attendees_count > 0,
                    ListBackColor = item.is_boosted_event ? Color.FromHex(Constant.BoostListBackColor) : Color.White

				});
			}
			HostedEventsList.Clear();
			ListMyFriendsEvents = tempFriendEventList;
			pageNoFriendsEvent++;
			IsLoading = false;
		}
		string SetEventDate(string startDate, string startTime)
		{
			var dateStart = DateTime.Parse(startDate + " " + startTime);
			return dateStart.Date.ToString("ddd,dd MMM").ToUpperInvariant() + ", " + dateStart.ToString("h:mm tt").Trim().ToUpperInvariant();
		}
		#endregion



	}

}




