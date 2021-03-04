using System;
using System.Collections.Generic;
using System.Linq;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Pulse
{
	public partial class AddParticipantPage : BaseContentPage
	{
		#region Private Variables
		int _tapCount = 0;
		string page;
		readonly PulseViewModel pulseViewModel;
		string searchvalue;
		public List<Friend> selectedFriends;
		bool isSearchedValue;
		bool isAlreadySelectedItem;
		#endregion
		#region Constructor
		public AddParticipantPage(string pageType)
		{
			InitializeComponent();
			pulseViewModel = ServiceContainer.Resolve<PulseViewModel>();
			BindingContext = pulseViewModel;
			selectedFriends = new List<Friend>();
			listViewfriends.LoadMoreCommand = new Command(GetFriends);
			page = pageType;
			if (page.Equals("AddPulse"))
			{
				gridSearchFrame.IsVisible = true;
			}
			else
			{
				gridSearchFrame.IsVisible = false;
			}
			SetInitialValues();
		}
		#endregion
		#region Override Methods
		protected override void OnAppearing()
		{
			var list = pulseViewModel.SelectedFriendsList;
			if (list != null && list.Count > 0)
			{
				pulseViewModel.SelectedFriendsList.Where(w => w.Ischecked == false).Select(w => w.Ischecked = true).ToList();
				foreach (var i in pulseViewModel.SelectedFriendsList)
				{
					selectedFriends.Add(i);
				}
				SetHeader();
			}
			pulseViewModel.tempFriendList.Clear();
			searchvalue = entryUser.Text;
			pulseViewModel.pageNoFriend = 1;
			pulseViewModel.totalPagesFriends = 1;
			GetFriends();
		}
		#endregion
		#region Methods
		void SetInitialValues()
		{
			if (Device.RuntimePlatform == Device.Android)
			{
				topStack.Margin = new Thickness(0, 10, 10, 10);
				searchFrame.CornerRadius = 2;
				entryUser.Margin = new Thickness(0, 8, 0, 0);
			}
			pulseViewModel.IsNoUserFoundVisible = false;
			pulseViewModel.IsListUserVisible = false;
			pulseViewModel.totalPagesFriends = 1;
		}

		async void Done_Tapped(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					pulseViewModel.SelectedFriendsList.Clear();
					foreach (var i in selectedFriends)
					{
						pulseViewModel.SelectedFriendsList.Add(i);
					}
					await Navigation.PopModalAsync();
					_tapCount = 0;
				}
			}
			else
			{
				await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
				_tapCount = 0;
			}
		}

		async void Cross_Clicked(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					pulseViewModel.IsLoading = true;
					await Navigation.PopModalAsync();
					pulseViewModel.IsLoading = false;
					_tapCount = 0;
				}
			}
			else
			{
				await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
				_tapCount = 0;
			}
		}
		void SearchEntry_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
		{
			try
			{
				if (e.NewTextValue == string.Empty)
				{
					pulseViewModel.IsNoUserFoundVisible = false;
					pulseViewModel.IsListUserVisible = false;
					pulseViewModel.tempFriendList.Clear();
					searchvalue = entryUser.Text;
					pulseViewModel.pageNoFriend = 1;
					pulseViewModel.totalPagesFriends = 1;
					GetFriends();
				}
				else
				{
					if (entryUser.Text.Length >= 1 && !isSearchedValue)
					{
						isSearchedValue = true;
						pulseViewModel.tempFriendList.Clear();
						pulseViewModel.IsListUserVisible = false;
						pulseViewModel.IsNoUserFoundVisible = false;
						searchvalue = entryUser.Text;
						pulseViewModel.pageNoFriend = 1;
						pulseViewModel.totalPagesFriends = 1;
						GetFriends();
					}
					else
					{
						pulseViewModel.IsNoUserFoundVisible = false;
						pulseViewModel.IsListUserVisible = false;
					}
				}
			}
			catch (Exception)
			{
				pulseViewModel.IsLoading = false;
			}
		}

		void lstSearchFriendTapped(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
		{
			try
			{
				if (e.SelectedItem == null)
				{
					return;
				}
				else
				{
					var user = (Friend)e.SelectedItem;
					user.Ischecked = !user.Ischecked;
					user.IsUnchecked = !user.IsUnchecked;
					if (!selectedFriends.Any(x => x.friendId == user.friendId))
					{
						selectedFriends.Add(user);
					}
					else
					{
						selectedFriends.Remove(selectedFriends.Where(x => x.friendId == user.friendId).Single());
					}
					SetHeader();
					listViewfriends.SelectedItem = null;
				}
			}
			catch (Exception)
			{
				return;
			}
		}

		void SetHeader()
		{
			if (selectedFriends.Count < 1)
			{
				lblPageTitle.Text = "Friends";
			}
			else if (selectedFriends.Count < 2)
			{
				lblPageTitle.Text = selectedFriends.Count.ToString() + " Friend Selected";
			}
			else
			{
				lblPageTitle.Text = selectedFriends.Count.ToString() + " Friends Selected";
			}
		}

		void CrossIcon_Tapped(object sender, System.EventArgs e)
		{
			entryUser.Text = string.Empty;
		}

		async void GetFriends()
		{
			try
			{
				pulseViewModel.IsLoading = true;
				if (!CrossConnectivity.Current.IsConnected)
				{
					await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
				}
				else
				{
					bool isList = await pulseViewModel.GetSearchedFriends(searchvalue, page);
					SetFriendsList(isList, pulseViewModel.UsersList);
				}
				pulseViewModel.IsLoading = false;
			}

			catch (Exception)
			{
				pulseViewModel.IsLoading = false;
				pulseViewModel.TapCount = 0;
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
			}
		}
		void SetFriendsList(bool isList, List<FriendResponseForUser> list)
		{
			if (isList && pulseViewModel.pageNoFriend < 2)
			{
				isAlreadySelectedItem = false;
				pulseViewModel.IsListUserVisible = true;
				pulseViewModel.IsNoUserFoundVisible = false;
				foreach (var item in list)
				{
					if (selectedFriends.Count > 0)
					{
						isAlreadySelectedItem = selectedFriends.Any(x => x.friendId == item.id);
					}
					float cornerradius = Device.RuntimePlatform == Device.Android ? 1 : 5;
					pulseViewModel.tempFriendList.Add(new Friend { friendId = item.id, cornerRadius = cornerradius, friendUsername = item.username, friendFullname = item.fullname, friendPic = string.IsNullOrEmpty(item.profile_image) ? Constant.ProfileIcon : PageHelper.GetUserImage(item.profile_image), Ischecked = isAlreadySelectedItem, IsUnchecked = !isAlreadySelectedItem });
				}
				pulseViewModel.UsersList.Clear();
				listViewfriends.ItemsSource = pulseViewModel.tempFriendList;
				pulseViewModel.pageNoFriend++;
				pulseViewModel.IsLoading = false;
				isSearchedValue = false;
			}
			else if (isList)
			{
				isAlreadySelectedItem = false;
				pulseViewModel.IsListUserVisible = true;
				pulseViewModel.IsNoUserFoundVisible = false;
				foreach (var item in list)
				{
					if (selectedFriends.Count > 0)
					{
						isAlreadySelectedItem = selectedFriends.Any(x => x.friendId == item.id);
					}
					float cornerradius = Device.RuntimePlatform == Device.Android ? 1 : 5;
					pulseViewModel.tempFriendList.Add(new Friend { friendId = item.id, cornerRadius = cornerradius, friendUsername = item.username, friendFullname = item.fullname, friendPic = string.IsNullOrEmpty(item.profile_image) ? Constant.ProfileIcon : PageHelper.GetUserImage(item.profile_image), Ischecked = isAlreadySelectedItem, IsUnchecked = !isAlreadySelectedItem });
				}
				pulseViewModel.UsersList.Clear();
				listViewfriends.ItemsSource = pulseViewModel.tempFriendList;
				pulseViewModel.pageNoFriend++;
				pulseViewModel.IsLoading = false;
				isSearchedValue = false;
			}
			else if (!isList && pulseViewModel.pageNoFriend < 2)
			{
				pulseViewModel.IsListUserVisible = false;
				pulseViewModel.IsNoUserFoundVisible = true;
				pulseViewModel.IsLoading = false;
				isSearchedValue = false;
			}
			else
			{
				listViewfriends.ItemsSource = pulseViewModel.tempFriendList;
				pulseViewModel.IsLoading = false;
				isSearchedValue = false;
			}
		}
		#endregion
	}
}
