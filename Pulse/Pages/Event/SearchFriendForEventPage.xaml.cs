using System;
using System.Collections.Generic;
using System.Linq;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Pulse
{
    public partial class SearchFriendForEventPage : BaseContentPage
    {
        #region Private Variables
        int _tapCount = 0;
        string page;
        readonly EventViewModel eventViewModel;
        string searchvalue;
        public List<Friend> selectedFriends;
        public List<Friend> selectedFriendsForUpdate;
        bool isSearchedValue;
        bool isAlreadySelectedItem;
        #endregion
        #region Constructor
        public SearchFriendForEventPage(string pageType)
        {
            InitializeComponent();
            eventViewModel = ServiceContainer.Resolve<EventViewModel>();
            BindingContext = eventViewModel;
            selectedFriends = new List<Friend>();
            selectedFriendsForUpdate = new List<Friend>();
            listViewfriends.LoadMoreCommand = new Command(GetFriends);
            eventViewModel.totalPagesFriends = 1;
            page = pageType;
            if (page.Equals("AddEvent"))
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
            eventViewModel.UpdatedSelectedFriendsList.Clear();
            var list = eventViewModel.SelectedFriendsList;
            if (list != null && list.Count > 0)
            {
                eventViewModel.SelectedFriendsList.Where(w => w.Ischecked == false).Select(w => w.Ischecked = true).ToList();
                foreach (var i in eventViewModel.SelectedFriendsList)
                {
                    selectedFriends.Add(i);
                }
                SetHeader();
            }
            eventViewModel.tempFriendList.Clear();
            searchvalue = entryUser.Text;
            eventViewModel.pageNoFriend = 1;
            eventViewModel.totalPagesFriends = 1;
            GetFriends();
            eventViewModel.GetAllUser();
            FriendsView.IsVisible = true;
            FriendsBoxView.BackgroundColor = Color.FromHex(Constant.PinkButtonColor);
            ContactsView.IsVisible = false;
            ContactsBoxView.BackgroundColor = Color.FromHex(Constant.WhiteTextColor);
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
            eventViewModel.IsNoUserFoundVisible = false;
            eventViewModel.IsListUserVisible = false;

        }

        async void Done_Tapped(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (_tapCount < 1)
                {
                    _tapCount = 1;
                    eventViewModel.SelectedFriendsList.Clear();
                    eventViewModel.UpdatedSelectedFriendsList.Clear();
                    foreach (var i in selectedFriends)
                    {
                          eventViewModel.SelectedFriendsList.Add(i);
                     
                    }
                    if (eventViewModel.IsUpdateEvent)
                    {
                        if (selectedFriendsForUpdate != null && selectedFriendsForUpdate.Count > Constant.StatusZero)
                        {
                            foreach (var i in selectedFriendsForUpdate)
                            {
                                eventViewModel.UpdatedSelectedFriendsList.Add(i);
                            }

                        }
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
        void SearchEntry_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            try
            {
                if (e.NewTextValue == string.Empty)
                {
                    eventViewModel.IsNoUserFoundVisible = false;
                    eventViewModel.IsListUserVisible = false;
                    eventViewModel.tempFriendList.Clear();
                    searchvalue = entryUser.Text;
                    eventViewModel.pageNoFriend = 1;
                    eventViewModel.totalPagesFriends = 1;
                    GetFriends();
                }
                else
                {
                    if (entryUser.Text.Length >= 3 && !isSearchedValue)
                    {
                        isSearchedValue = true;
                        eventViewModel.tempFriendList.Clear();
                        eventViewModel.IsListUserVisible = false;
                        eventViewModel.IsNoUserFoundVisible = false;
                        searchvalue = entryUser.Text;
                        eventViewModel.pageNoFriend = 1;
                        eventViewModel.totalPagesFriends = 1;
                        GetFriends();
                    }
                    else
                    {
                        eventViewModel.IsNoUserFoundVisible = false;
                        eventViewModel.IsListUserVisible = false;
                    }
                }
            }
            catch (Exception)
            {
                eventViewModel.IsLoading = false;
            }
        }

        void lstSearchFriendTapped(object sender, SelectedItemChangedEventArgs e)
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
                        selectedFriendsForUpdate.Add(user);
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
                eventViewModel.IsLoading = true;
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                }
                else
                {

                    bool isList = await eventViewModel.GetSearchedFriends(searchvalue, page);
                    SetFriendsList(isList, eventViewModel.UsersList);

                }
                eventViewModel.IsLoading = false;
            }

            catch (Exception)
            {
                eventViewModel.IsLoading = false;
                eventViewModel.TapCount = 0;
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
            }
        }
        void SetFriendsList(bool isList, List<FriendResponseForUser> list)
        {
            if (isList && eventViewModel.pageNoFriend < 2)
            {
                isAlreadySelectedItem = false;
                eventViewModel.IsListUserVisible = true;
                eventViewModel.IsNoUserFoundVisible = false;
                foreach (var item in list)
                {
                    if (selectedFriends.Count > 0)
                    {
                        isAlreadySelectedItem = selectedFriends.Any(x => x.friendId == item.id);
                    }
                    float cornerradius = Device.RuntimePlatform == Device.Android ? 1 : 5;
                    eventViewModel.tempFriendList.Add(new Friend { friendId = item.id, cornerRadius = cornerradius, friendUsername = item.username, friendFullname = item.fullname, friendPic = string.IsNullOrEmpty(item.profile_image) ? Constant.ProfileIcon : PageHelper.GetUserImage(item.profile_image), Ischecked = isAlreadySelectedItem, IsUnchecked = !isAlreadySelectedItem });

                }
                eventViewModel.UsersList.Clear();
                listViewfriends.ItemsSource = eventViewModel.tempFriendList;
                eventViewModel.pageNoFriend++;
                eventViewModel.IsLoading = false;
                isSearchedValue = false;
            }
            else if (isList)
            {
                isAlreadySelectedItem = false;
                eventViewModel.IsListUserVisible = true;
                eventViewModel.IsNoUserFoundVisible = false;
                foreach (var item in list)
                {
                    if (selectedFriends.Count > 0)
                    {
                        isAlreadySelectedItem = selectedFriends.Any(x => x.friendId == item.id);
                    }
                    float cornerradius = Device.RuntimePlatform == Device.Android ? 1 : 5;
                    eventViewModel.tempFriendList.Add(new Friend { friendId = item.id, cornerRadius = cornerradius, friendUsername = item.username, friendFullname = item.fullname, friendPic = string.IsNullOrEmpty(item.profile_image) ? Constant.ProfileIcon : PageHelper.GetUserImage(item.profile_image), Ischecked = isAlreadySelectedItem, IsUnchecked = !isAlreadySelectedItem });
                }
                eventViewModel.UsersList.Clear();
                listViewfriends.ItemsSource = eventViewModel.tempFriendList;
                eventViewModel.pageNoFriend++;
                eventViewModel.IsLoading = false;
                isSearchedValue = false;
            }
            else if (!isList && eventViewModel.pageNoFriend < 2)
            {
                eventViewModel.IsListUserVisible = false;
                eventViewModel.IsNoUserFoundVisible = true;
                eventViewModel.IsLoading = false;
                isSearchedValue = false;
            }
            else
            {
                eventViewModel.IsListUserVisible = true;
                listViewfriends.ItemsSource = eventViewModel.tempFriendList;
                eventViewModel.IsLoading = false;
                isSearchedValue = false;
            }
            if (eventViewModel.IsUpdateEvent && eventViewModel.tempFriendList != null && eventViewModel.tempFriendList.Count > Constant.StatusZero)
            {
                var lst = eventViewModel.tempFriendList.Where(X => X.Ischecked).ToList();
                foreach (var item in lst)
                {
                    eventViewModel.tempFriendList.Remove(item);
                }
            }
        }
        #endregion

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            FriendsView.IsVisible = true;
            FriendsBoxView.BackgroundColor =Color.FromHex(Constant.PinkButtonColor);
            ContactsView.IsVisible = false;
            ContactsBoxView.BackgroundColor = Color.FromHex(Constant.WhiteTextColor);
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            FriendsView.IsVisible = false;
            FriendsBoxView.BackgroundColor = Color.FromHex(Constant.WhiteTextColor);
            ContactsView.IsVisible = true;
            ContactsBoxView.BackgroundColor = Color.FromHex(Constant.PinkButtonColor);
        }

        private void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {
            entryContact.Text = string.Empty;
        }

        private void ExtendedEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(!string.IsNullOrEmpty(entryContact.Text))
            {

            }
        }
    }
}
