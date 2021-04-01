using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Pulse
{
	public class PulseViewModel : BaseViewModel
	{
		#region private variables
		MainServices mainService;
		bool isNoUserFoundVisible;
		bool isListUserVisible;
		public List<PulseMessageResponse> MessageList;
		ObservableCollection<PulseModel> listPulse;
		ObservableCollection<PulseMessage> listMessage;
		bool isMessageListVisible;
		bool isNoEventVisible;
		bool isEventListVisible;
		bool isNoPulseFoundVisible;
		bool isPulseListVisible;
		bool isFirstTimePulse;
		bool isSearchPulseListVisible;
		bool isRefreshing;
		string message;
		string linkedEvent;
		string pulseSubject;
		string participantsCount;
		bool isNotGroupMemberStackVisible;
		bool isWriteMessageStackVisible;
		bool isEditableStackVisible;
		bool isNonEditableStackVisible;
        bool isActionLabelVisible;

		#endregion
		#region Public Properties
		public ICommand LoadMorePulses { get; private set; }
		public ICommand LoadMoreMessages { get; private set; }
		public int pageNoPulse;
		public int pageNoSearchPulse;
		public int pageNoFriend;
		public int pageNoEvent;
		public int TappedPulseId;
		public int pageNoMessage;
		public int totalPagesFriends;
		public int totalEventPages;
		public int totalGroupsPage;
		public List<PulseResponse> PulseList;
		public bool isPulseListAvailable;
		public List<MyEventResponse> EventsList;
		public ObservableCollection<Friend> SelectedFriendsList;
		public ObservableCollection<Friend> tempFriendList;
		public List<FriendResponseForUser> UsersList;
		public ObservableCollection<PulseModel> tempPulseList;
		public string PulseTitle;
		public bool isDetail;
		public ObservableCollection<MyEvents> SelectedEventList;
		public PulseDetailResponse PulseDetail;
		public int MessageCount;
		public bool IsRefreshing
		{
			get { return isRefreshing; }
			set
			{
				isRefreshing = value;
				OnPropertyChanged(nameof(IsRefreshing));
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
		public bool IsNotGroupMemberStackVisible
		{
			get { return isNotGroupMemberStackVisible; }
			set
			{
				isNotGroupMemberStackVisible = value;
				OnPropertyChanged("IsNotGroupMemberStackVisible");
			}
		}
		public bool IsWriteMessageStackVisible
		{
			get { return isWriteMessageStackVisible; }
			set
			{
				isWriteMessageStackVisible = value;
				OnPropertyChanged("IsWriteMessageStackVisible");
			}
		}

		public ObservableCollection<PulseModel> ListPulse
		{
			get { return listPulse; }
			set
			{
				listPulse = value;
				OnPropertyChanged("ListPulse");
			}
		}
		public string ParticipantsCount
		{
			get { return participantsCount; }
			set
			{
				participantsCount = value;
				OnPropertyChanged("ParticipantsCount");
			}
		}
		public string PulseSubject
		{
			get { return pulseSubject; }
			set
			{
				pulseSubject = value;
				OnPropertyChanged("PulseSubject");
			}
		}
		public string Message
		{
			get { return message; }
			set
			{
				message = value;
				OnPropertyChanged("Message");
			}
		}
		public string LinkedEvent
		{
			get { return linkedEvent; }
			set
			{
				linkedEvent = value;
				OnPropertyChanged("LinkedEvent");
			}
		}

		public ObservableCollection<PulseMessage> ListMessage
		{
			get { return listMessage; }
			set
			{
				listMessage = value;
				OnPropertyChanged("ListMessage");
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
		public bool IsMessageListVisible
		{
			get { return isMessageListVisible; }
			set
			{
				isMessageListVisible = value;
				OnPropertyChanged("IsMessageListVisible");
			}
        }
        public bool IsActionLabelVisible
        {
            get { return isActionLabelVisible; }
            set
            {
                isActionLabelVisible = value;
                OnPropertyChanged("IsActionLabelVisible");
            }
        }
		public bool IsPulseListVisible
		{
			get { return isPulseListVisible; }
			set
			{
				isPulseListVisible = value;
				OnPropertyChanged("IsPulseListVisible");
			}
		}
		public bool IsFirstTimePulse
		{
			get { return isFirstTimePulse; }
			set
			{
				isFirstTimePulse = value;
				OnPropertyChanged("IsFirstTimePulse");
			}
		}
		public bool IsEditableStackVisible
		{
			get { return isEditableStackVisible; }
			set
			{
				isEditableStackVisible = value;
				OnPropertyChanged("IsEditableStackVisible");
			}
		}
		public bool IsNonEditableStackVisible
		{
			get { return isNonEditableStackVisible; }
			set
			{
				isNonEditableStackVisible = value;
				OnPropertyChanged("IsNonEditableStackVisible");
			}
		}
		public bool IsNoPulseFoundVisible
		{
			get { return isNoPulseFoundVisible; }
			set
			{
				isNoPulseFoundVisible = value;
				OnPropertyChanged("IsNoPulseFoundVisible");
			}
		}
		public bool IsSearchPulseListVisible
		{
			get { return isSearchPulseListVisible; }
			set
			{
				isSearchPulseListVisible = value;
				OnPropertyChanged("IsSearchPulseListVisible");
			}
		}

		public bool IsEventListVisible
		{
			get { return isEventListVisible; }
			set
			{
				isEventListVisible = value;
				OnPropertyChanged("IsEventListVisible");
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
		#endregion
		#region Constructor
		public PulseViewModel()
		{
			mainService = new MainServices();
			tempFriendList = new ObservableCollection<Friend>();
			SelectedFriendsList = new ObservableCollection<Friend>();
			tempPulseList = new ObservableCollection<PulseModel>();
			SelectedEventList = new ObservableCollection<MyEvents>();
			MessageList = new List<PulseMessageResponse>();
			LoadMorePulses = new Command(GetPulses);
		}

		#endregion
		#region Methods

		public async Task<bool> GetSearchedFriends(string searchKeyword, string page)
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
					if (SessionManager.AccessToken != null && (pageNoFriend <= totalPagesFriends || pageNoFriend == 1))
					{
						UsersList = new List<FriendResponseForUser>();
						string url;
						if (page.Equals("AddPulse"))
						{
                            url = Constant.SearchPulseUserUrl + pageNoFriend + Constant.SearchString + searchKeyword;
						}
						else
						{
							url = Constant.AddMembersToPulseUrl + TappedPulseId + "/?page=" + pageNoFriend;
						}
						var response = await mainService.Get<ResultWrapper<FriendResponseForUser>>(url);
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
		PulseData GetUpdatePulseData(string text)
		{
			PulseData pulseData = new PulseData();
			pulseData.name = text;
			List<Member> memberList = new List<Member>();
			if (SelectedFriendsList.Count > 0)
			{
				foreach (var i in SelectedFriendsList)
				{
					Member member = new Member();
					member.user_id = i.friendId;
					memberList.Add(member);
				}
			}
			pulseData.members = memberList;
			if (SelectedEventList != null && SelectedEventList.Count > 0)
			{
				pulseData.event_id = SelectedEventList[0].EventId;
			}
			return pulseData;
		}
		public async Task UpdatePulse(string pulseSubject)
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
							IsLoading = true;
							var response = await mainService.Put<ResultWrapperSingle<SendEmailOTPResponse>>(Constant.PulseUpdateUrl + TappedPulseId + "/", GetUpdatePulseData(pulseSubject));
							if (response != null && response.status == Constant.Status200 && response.response != null)
							{
								//	await App.Instance.Alert("Pulse Updated Sucessfully", Constant.AlertTitle, Constant.Ok);
								ShowToast(Constant.AlertTitle, "Pulse updated sucessfully");
								PulseTitle = pulseSubject;
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
					}
					else
					{
						await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
						IsLoading = false;
						TapCount = 0;
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

		public async Task CreatePulse(string text)
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
							IsLoading = true;
							var response = await mainService.Post<ResultWrapperSingle<EventResponse>>(Constant.CreatePulseUrl, GetCreatePulseData(text));
							if (response != null && response.status == Constant.Status200 && response.response != null)
							{
								//await App.Instance.Alert(Constant.PulseCreated, Constant.AlertTitle, Constant.Ok);
								ShowToast(Constant.AlertTitle, Constant.PulseCreated);
								pageNoPulse = 1;
								totalGroupsPage = 1;
								while (tempPulseList.Count > 0)
									tempPulseList.RemoveAt(0);
								GetPulses();
								IsPulseListVisible = true;
								IsFirstTimePulse = false;
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
					}
					else
					{
						await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
						IsLoading = false;
						TapCount = 0;
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
		PulseData GetCreatePulseData(string text)
		{
			PulseData pulseData = new PulseData();
			pulseData.name = text;
			List<Member> memberList = new List<Member>();
			Member member1 = new Member();
			member1.user_id = SessionManager.UserId;
			memberList.Add(member1);
			foreach (var i in SelectedFriendsList)
			{
				Member member = new Member();
				member.user_id = i.friendId;
				memberList.Add(member);
			}

			pulseData.members = memberList;
			if (SelectedEventList != null && SelectedEventList.Count > 0)
			{
				pulseData.event_id = SelectedEventList[0].EventId;
			}
			var sendDate = (DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "+" + TimeZoneInfo.Local.BaseUtcOffset).Replace("+-", "-");
			pulseData.create_date = sendDate.Remove(sendDate.Length - 3);
			return pulseData;
		}

		public async void GetPulses()
		{
			try
			{
				App.ShowMainPageLoader();
				IsLoading = true;
				if (!CrossConnectivity.Current.IsConnected)
				{
					await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
					TapCount = 0;
				}
				else
				{

					bool isList = await GetPulseList(false, "");
					SetPulseList(isList, PulseList);

				}
				App.HideMainPageLoader();
				IsLoading = false;
			}

			catch (Exception)
			{
				IsLoading = false;
				TapCount = 0;
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
			}
		}

		public async Task<bool> GetPulseList(bool fromSearchPage, string searchValue)
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
					int page = fromSearchPage ? pageNoSearchPulse : pageNoPulse;
					if (SessionManager.AccessToken != null && (page == 1 || page <= totalGroupsPage))
					{
						PulseList = new List<PulseResponse>();
						string url = fromSearchPage ? Constant.SearchPulseUrl + pageNoSearchPulse + Constant.SearchString + searchValue : Constant.GetPulseListUrl + pageNoPulse;
						var response = await mainService.Get<ResultWrapper<PulseResponse>>(url);
						if (response != null && response.status == Constant.Status200 && response.response != null && response.response.Count > 0)
						{
							foreach (var item in response.response)
							{
								PulseList.Add(item);
							}
							totalGroupsPage = GetPageCount(response.response[response.response.Count - 1].total_groups);
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

		void SetPulseList(bool isList, List<PulseResponse> list)
		{

			if (isList && pageNoPulse < 2)
			{
				SetPulse(list);
			}
			else if (isList)
			{
				SetPulse(list);
			}
			else if (!isList && pageNoPulse < 2)
			{
				IsPulseListVisible = false;
				IsNoPulseFoundVisible = true;
				App.HideMainPageLoader();
				IsLoading = false;
			}
			else
			{
				ListPulse = tempPulseList;
				App.HideMainPageLoader();
				IsLoading = false;
			}
		}

		void SetPulse(List<PulseResponse> list)
		{
			try
			{
				IsPulseListVisible = true;
				IsNoPulseFoundVisible = false;
				int itemIndex = 0;
				foreach (var item in list)
				{
					string[] array = item.name.Split(' ');
					string imageTxt = "";
					string datetime = "";
					if (array.Length > 1 && !string.IsNullOrEmpty(array[1].Trim()))
					{
						imageTxt = array[0].Substring(0, 1).ToUpper() + array[1].Substring(0, 1).ToUpper();
					}
					else if (array.Length > 0 && array.Length <= 1 && array[0].Length > 1)
					{
						imageTxt = array[0].Substring(0, 2).ToUpper();
					}
					else if (array.Length > 0 && array.Length <= 1)
					{
						imageTxt = array[0].Substring(0, 1).ToUpper();
					}
					if (item.last_message != null)
					{
						if (item.last_message.id <= 0)
						{
							datetime = SetMessageDate(item.create_date);
						}
						else
						{
							datetime = SetMessageDate(item.last_message.send_date);
						}
					}
					List<string> colorList = GetList();
					MessageCount = item.message_count;
					string color = Constant.PinkButtonColor;
					if (colorList.Count > itemIndex)
					{
						color = colorList[itemIndex];
					}
					else
					{
						itemIndex = -1;
					}
					tempPulseList.Add(new PulseModel { ImageText = imageTxt, LastMessageUserId = item.last_message.user, Subject = item.name, TotalMessagesCount = item.message_count, IsPulseMember = item.group_member_or_not, PulseId = item.id, PulseownerId = item.user, LastMessage = item.last_message.text_message, Time = datetime, GroupIconBorderColor = color });
					itemIndex++;
				}
				PulseList.Clear();
				GetMessageNotifyCount();
				ListPulse = tempPulseList;
				pageNoPulse++;
				App.HideMainPageLoader();
				IsLoading = false;
			}
			catch (Exception)
			{
				App.HideMainPageLoader();
			}

		}

		void GetMessageNotifyCount()
		{
			List<PulseModel> SavedList = new List<PulseModel>();
			if (!string.IsNullOrEmpty(Settings.AppSettings.GetValueOrDefault<string>("SavedPulseListing", string.Empty)))
			{
				SavedList = JsonConvert.DeserializeObject<List<PulseModel>>(Settings.AppSettings.GetValueOrDefault<string>("SavedPulseListing", string.Empty));
				foreach (var pulse in tempPulseList)
				{
					var matchedPulse = SavedList.Where(x => x.PulseId == pulse.PulseId).FirstOrDefault();
					if (pulse.LastMessageUserId != SessionManager.UserId)
					{

						if (matchedPulse != null)
						{
							pulse.NotifyCount = matchedPulse.TotalMessagesCount < pulse.TotalMessagesCount ? Convert.ToString(pulse.TotalMessagesCount - matchedPulse.TotalMessagesCount) : string.Empty;
							pulse.IsNewMessages = string.IsNullOrEmpty(pulse.NotifyCount) ? false : true;
						}
						else
						{
							pulse.NotifyCount = Convert.ToString(pulse.TotalMessagesCount);
							pulse.IsNewMessages = !string.IsNullOrEmpty(pulse.NotifyCount) ? Convert.ToInt32(pulse.NotifyCount) > 0 ? true : false : false;
						}
					}
				}
			}
			else
			{
				foreach (var pulse in tempPulseList)
				{
					if (pulse.LastMessageUserId != SessionManager.UserId)
					{
						pulse.NotifyCount = Convert.ToString(pulse.TotalMessagesCount);
						pulse.IsNewMessages = !string.IsNullOrEmpty(pulse.NotifyCount) ? Convert.ToInt32(pulse.NotifyCount) > 0 ? true : false : false;
					}
				}
			}
		}

		public string SetMessageDate(string createDate)
		{
			if (DateTime.Parse(createDate).ToLocalTime().Date == DateTime.Now.Date)
			{
				var dateTime = new DateTime(DateTime.Parse(createDate).ToLocalTime().TimeOfDay.Ticks);
				return dateTime.ToString("h:mm tt");
			}
			else
			{
				return DateTime.Parse(createDate).ToLocalTime().ToString("dd/MM/yyyy");
			}
		}


		public async Task<bool> GetMyEvents(string searchValue)
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
					if (SessionManager.AccessToken != null && (pageNoEvent == 1 || pageNoEvent <= totalEventPages))
					{
						EventsList = new List<MyEventResponse>();
						var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
						var response = await mainService.Get<ResultWrapper<MyEventResponse>>(Constant.SearchEventForPulseUrl + pageNoEvent + Constant.SearchString + searchValue + "&datetime=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
						if (response != null && response.status == Constant.Status200 && response.response != null && response.response.Count > 0)
						{
							foreach (var item in response.response)
							{
								EventsList.Add(item);
							}
							totalEventPages = GetPageCount(response.response[response.response.Count - 1].total_events);
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

		public ICommand RefreshCommand
		{
			get
			{
				return new Command(async () =>
				{
					if (CrossConnectivity.Current.IsConnected)
					{
						IsRefreshing = true;
						pageNoPulse = 1;
						totalGroupsPage = 1;
						while (tempPulseList.Count > 0)
							tempPulseList.RemoveAt(0);
						GetPulses();
						IsRefreshing = false;
					}
					else
					{
						await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
					}
				});
			}
		}
		public async Task<bool> GetMessageList()
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
					if (SessionManager.AccessToken != null)
					{
						MessageList = new List<PulseMessageResponse>();
						var response = await mainService.Get<ResultWrapper<PulseMessageResponse>>(Constant.MessageListUrl + TappedPulseId + "/?page=" + pageNoMessage);
						if (response != null && response.status == Constant.Status200 && response.response != null && response.response.Count > 0)
						{
							MessageList = new List<PulseMessageResponse>();
							foreach (var item in response.response)
							{
								MessageList.Add(item);
							}
							/// Manage New Message Alerts
							List<PulseModel> SavedList = new List<PulseModel>();
							if (!string.IsNullOrEmpty(

								Settings.AppSettings.GetValueOrDefault<string>("SavedPulseListing", string.Empty)))
							{
								SavedList = JsonConvert.DeserializeObject<List<PulseModel>>(Settings.AppSettings.GetValueOrDefault<string>("SavedPulseListing", string.Empty));

								var matchedPulse = SavedList.Where(x => x.PulseId == TappedPulseId).FirstOrDefault();
								if (matchedPulse != null)
								{
									int messageCount = response.response[response.response.Count - 1].message_count;
									SavedList.Where(w => w.PulseId == TappedPulseId).ToList().ForEach(s => s.TotalMessagesCount = messageCount);
								}
								Settings.AppSettings.AddOrUpdateValue<string>("SavedPulseListing", JsonConvert.SerializeObject(SavedList));
							}
							///

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


		public async Task<bool> PostMessage()
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
					if (TapCount < 1)
					{
						if (SessionManager.AccessToken != null)
						{
							TapCount++;
							IsLoading = true;
							var response = await mainService.Post<ResultWrapperSingle<LastMessage>>(Constant.PostMessageUrl, GetMessageData());
							if (response != null && response.status == Constant.Status200 && response.response != null)
							{
								Message = string.Empty;
								IsLoading = false;
								TapCount = 0;
								return true;
							}
							else if (response != null && response.status == Constant.Status401)
							{
								SignOut();
								TapCount = 0;
								IsLoading = false;
								return false;
							}
							else
							{
								await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
								IsLoading = false;
								TapCount = 0;
								return false;
							}
						}
						return false;
					}
					TapCount = 0;
					return false;
				}
			}
			catch (Exception)
			{
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
				IsLoading = false;
				TapCount = 0;
				return false;
			}
		}

		PostMessageData GetMessageData()
		{
			PostMessageData postMessageData = new PostMessageData();
			postMessageData.pulsegroup_id = TappedPulseId;
			postMessageData.text_message = Message;
			var sendDate = (DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "+" + TimeZoneInfo.Local.BaseUtcOffset).Replace("+-", "-");
			postMessageData.send_date = sendDate.Remove(sendDate.Length - 3);
			return postMessageData;
		}


		public List<string> GetList()
		{
			var list = new List<string>{
				"#000000","#d576a5","#0000FF","#8A2BE2","#A52A2A","#DEB887","#5F9EA0","#7FFF00","#D2691E","#FF7F50","#6495ED","#DC143C","#00FFFF",
				"#00008B","#008B8B","#B8860B","#A9A9A9","#006400","#BDB76B","#8B008B","#556B2F","#FF8C00","#9932CC","#8B0000","#E9967A","#8FBC8B","#483D8B",
				"#2F4F4F","#00CED1","#9400D3","#FF1493","#00BFFF","#696969","#1E90FF","#B22222"
			};
			return list;
		}


		public async Task<bool> GetPulseDetail()
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
					if (SessionManager.AccessToken != null)
					{
						PulseDetail = null;
						var response = await mainService.Get<ResultWrapperSingle<PulseDetailResponse>>(Constant.PulseDetailUrl + TappedPulseId + "/");
						if (response != null && response.status == Constant.Status200 && response.response != null)
						{
							PulseDetail = response.response;
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

        public async Task<bool> ReportPulseChange(int status, int userId)
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
                    if (TapCount < 1)
                    {
                        if (SessionManager.AccessToken != null)
                        {
                            IsLoading = true;
                            PulseStatusUpdate pulseUpdate = new PulseStatusUpdate();
                            pulseUpdate.pulse_invitation_status = status;
                            pulseUpdate.user_id = userId;
                            var response = await mainService.Put<ResultWrapperSingle<PulseStatusUpdateResponse>>(Constant.PulseStatusChangeUrl + TappedPulseId + "/", pulseUpdate);
                            if (response != null && response.status == Constant.Status200 && response.response != null)
                            {
                                int id = 0;
                                if (response.response.last_message != null)
                                {
                                    id = response.response.last_message.id;
                                }
                                bool issuccessful = await ClearHistory(4, id, userId);
                                bool issuccessfull = await ClearHistory(1, id, userId);
                                return issuccessfull;
                            }
                            else if (response != null && response.status == Constant.Status401)
                            {
                                SignOut();
                                IsLoading = false;
                                return true;
                            }
                            else
                            {
                                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                                IsLoading = false;
                                TapCount = 0;
                                return true;
                            }
                        }
                        return false;
                    }
                    else
                    {
                        await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                        IsLoading = false;
                        TapCount = 0;
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                IsLoading = false;
                TapCount = 0;
                return true;
            }
        }

		public async Task<bool> PulseStatusChange(int status, int userId)
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
					if (TapCount < 1)
					{
						if (SessionManager.AccessToken != null)
						{
							IsLoading = true;
							PulseStatusUpdate pulseUpdate = new PulseStatusUpdate();
							pulseUpdate.pulse_invitation_status = status;
							pulseUpdate.user_id = userId;
							var response = await mainService.Put<ResultWrapperSingle<PulseStatusUpdateResponse>>(Constant.PulseStatusChangeUrl + TappedPulseId + "/", pulseUpdate);
							if (response != null && response.status == Constant.Status200 && response.response != null)
							{
								int id = 0;
								if (response.response.last_message != null)
								{
									id = response.response.last_message.id;
								}
								bool issuccessful = await ClearHistory(status, id, userId);
								return issuccessful;
							}
							else if (response != null && response.status == Constant.Status401)
							{
								SignOut();
								IsLoading = false;
								return true;
							}
							else
							{
								await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
								IsLoading = false;
								TapCount = 0;
								return true;
							}
						}
						return false;
					}
					else
					{
						await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
						IsLoading = false;
						TapCount = 0;
						return true;
					}
				}
			}
			catch (Exception)
			{
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
				IsLoading = false;
				TapCount = 0;
				return true;
			}
		}

		public async Task<bool> PulsememberOrNot()
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
					if (TapCount < 1)
					{
						if (SessionManager.AccessToken != null)
						{
							IsLoading = true;
							PulseMemberData pulseData = new PulseMemberData();
							pulseData.pulsegroup_id = TappedPulseId;
							var response = await mainService.Post<ResultWrapperSingle<SendEmailOTPResponse>>(Constant.PulseMemberOrNotUrl, pulseData);
							if (response != null && response.status == Constant.Status200 && response.response != null)
							{
								return true;
							}
							else if (response != null && response.status == Constant.Status401)
							{
								SignOut();
								IsLoading = false;
								return false;
							}
							return false;
						}
						return false;
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

		public async Task<bool> ClearHistory(int status, int lastmessageId, int userid)
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
					if (TapCount < 1)
					{
						if (SessionManager.AccessToken != null)
						{
							IsLoading = true;
							ClearHistoryModel model = new ClearHistoryModel();
							model.pulse_request_type = status;
							model.pulsegroup_id = TappedPulseId;
							model.last_message_id = lastmessageId;
							model.user_id = userid;
							var response = await mainService.Post<ResultWrapperSingle<SendEmailOTPResponse>>(Constant.ClearPulseHistoryUrl, model);
							if (response != null && response.status == Constant.Status200 && response.response != null)
							{
								return true;
							}
							else if (response != null && response.status == Constant.Status401)
							{
								SignOut();
								IsLoading = false;
								return false;
							}
							return false;
						}
						return false;
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
		#endregion
	}
}
