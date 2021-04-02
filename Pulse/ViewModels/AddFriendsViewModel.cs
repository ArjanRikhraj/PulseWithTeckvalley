using Pulse.Helpers;
using Pulse.Models.Friends;
using Pulse.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Pulse.ViewModels
{
    public class AddFriendsViewModel : BaseViewModel
    {
        public ICommand Scan { private set; get; }
        public ICommand FirstFriendTapped { private set; get; }
        
        private bool isLoading;
        public bool IsLoading
        {
            get
            {
                return isLoading;
            }
            set
            {
                isLoading = value;
                OnPropertyChanged("IsLoading");
            }
        }
        private string friendsCount;
        public string FriendsCount
        {
            get
            {
                return friendsCount;
            }
            set
            {
                friendsCount = value;
                OnPropertyChanged("FriendsCount");
            }
        }
        private CurrentLocation loc;
        public CurrentLocation Loc
        {
            get
            {
                return loc;
            }
            set
            {
                loc = value;
                OnPropertyChanged("Loc");
            }
        }
        private List<AddfriendListResponse> userInfo;
        public List<AddfriendListResponse> UserInfo
        {
            get
            {
                return userInfo;
            }
            set
            {
                userInfo = value;
                OnPropertyChanged("UserInfo");
            }
        }
        private AddFriendModel  friendModel;
        public AddFriendModel FriendModel
        {
            get
            {
                return friendModel;
            }
            set
            {
                friendModel = value;
                OnPropertyChanged("FriendModel");
            }
        }
        MainServices mainService;
        public AddFriendsViewModel()
        {
            Scan = new Command(GetCurrentLocation);
            FirstFriendTapped = new Command<object>(GetFriendProfilePage);
            GetCurrentLocation();
            mainService = new MainServices();
        }

        private void GetFriendProfilePage(object obj)
        {
           
        }

        private async Task GetNearByFriends()
        {
            if (SessionManager.AccessToken != null)
            {
                App.ShowMainPageLoader();
                var result = await mainService.Get<ResultWrapper<AddfriendListResponse>>(Constant.GetAllUsersUrl);
                if (result != null && result.status == Constant.Status200)
                {
                    if(result.response.Count==0)
                    {
                        await App.Instance.Alert(Constant.NoFriendsFound, Constant.AlertTitle, Constant.Ok);
                        return;
                    }
                    var userCurrentLoc = new Xamarin.Essentials.Location(loc.Latitude, loc.Longitude);
                    userInfo = new List<AddfriendListResponse>();
                    foreach (var item in result.response)
                    {
                        var nearByFriendLoc = new Xamarin.Essentials.Location(double.Parse(item.latitude), double.Parse(item.longitude));
                        var distance = userCurrentLoc.CalculateDistance(nearByFriendLoc, DistanceUnits.Kilometers);
                        if (distance <= 25 )
                        {
                            userInfo.Add(item);
                        }
                    }
                    var topSixUser = userInfo.Take(6);
                    if (topSixUser.Count() == 0)
                        FriendsCount = "No";
                    else if (topSixUser.Count()> 0)
                        FriendsCount = topSixUser.Count().ToString();
                    if (topSixUser.Count() >= 1)
                    {
                        var firstUser = userInfo[0];
                        FriendModel = new AddFriendModel();
                        FriendModel.FirstFriendName = firstUser.username;
                        FriendModel.FirstFriendImage = firstUser.profile_image_url;
                        FriendModel.FirstFriendUserId = firstUser.id;
                        FriendModel.FirstFriendIsVisible = true;
                        if (string.IsNullOrEmpty(firstUser.profile_image_url) || (!firstUser.profile_image_url.Contains("png") && !firstUser.profile_image_url.Contains("jpg")))
                            FriendModel.FirstFriendImage = "iconUserEvents.png";
                    }
                    else
                        FriendModel.FirstFriendIsVisible = false;
                    if (topSixUser.Count() >= 2)
                    {
                        var secondUser = userInfo[1];
                        FriendModel.SecondFriendName = secondUser.username;
                        FriendModel.SecondFriendImage = secondUser.profile_image_url;
                        FriendModel.SecondFriendUserId = secondUser.id;
                        FriendModel.SecondFriendIsVisible = true;
                        if (string.IsNullOrEmpty(secondUser.profile_image_url) || (!secondUser.profile_image_url.Contains("png") && !secondUser.profile_image_url.Contains("jpg")))
                            FriendModel.SecondFriendImage = "iconUserEvents.png";
                    }
                    else
                        FriendModel.SecondFriendIsVisible = false;
                    if (topSixUser.Count() >= 3)
                    {
                        var thirdUser = userInfo[2];
                        FriendModel.ThirdFriendName = thirdUser.username;
                        FriendModel.ThirdFriendImage = thirdUser.profile_image_url;
                        FriendModel.ThirdFriendUserId = thirdUser.id;
                        FriendModel.ThirdFriendIsVisible = true;
                        if (string.IsNullOrEmpty(thirdUser.profile_image_url) || (!thirdUser.profile_image_url.Contains("png") && !thirdUser.profile_image_url.Contains("jpg")))
                            FriendModel.ThirdFriendImage = "iconUserEvents.png";
                    }
                    else
                        FriendModel.ThirdFriendIsVisible = false;
                    if (topSixUser.Count() >= 4)
                    {
                        var fourthUser = userInfo[3];
                        FriendModel.FourthFriendName = fourthUser.username;
                        FriendModel.FourthFriendImage = fourthUser.profile_image_url;
                        FriendModel.FourthFriendUserId = fourthUser.id;
                        FriendModel.FourthFriendIsVisible = true;
                        if (string.IsNullOrEmpty(fourthUser.profile_image_url) || (!fourthUser.profile_image_url.Contains("png") && !fourthUser.profile_image_url.Contains("jpg")))
                            FriendModel.FourthFriendImage = "iconUserEvents.png";
                    }
                    else
                        FriendModel.FourthFriendIsVisible = false;
                    if (topSixUser.Count() >= 5)
                    {
                        var fifthUser = userInfo[4];
                        FriendModel.FifthFriendName = fifthUser.username;
                        FriendModel.FifthFriendImage = fifthUser.profile_image_url;
                        FriendModel.FifthFriendUserId = fifthUser.id;
                        FriendModel.FifthFriendIsVisible = true;
                        if (string.IsNullOrEmpty(fifthUser.profile_image_url) || (!fifthUser.profile_image_url.Contains("png") && !fifthUser.profile_image_url.Contains("jpg")))
                            FriendModel.FifthFriendImage = "iconUserEvents.png";
                    }
                    else
                        FriendModel.FifthFriendIsVisible = false;
                    if (topSixUser.Count() >= 6)
                    {
                        var sixthUser = userInfo[5];
                        FriendModel.SixthFriendName = sixthUser.username;
                        FriendModel.SixthFriendImage = sixthUser.profile_image_url;
                        FriendModel.SixthFriendUserId = sixthUser.id;
                        FriendModel.SixthFriendIsVisible = true; if (string.IsNullOrEmpty(sixthUser.profile_image_url) || (!sixthUser.profile_image_url.Contains("png") && !sixthUser.profile_image_url.Contains("jpg")))
                            FriendModel.SixthFriendImage = "iconUserEvents.png";
                    }
                    else
                        FriendModel.SixthFriendIsVisible = false;
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

        private async void GetCurrentLocation()
        {
            try
            {
                IsLoading = true;
                var currentLocation=  await Utils.GetCurrentLocation();
                if (currentLocation.Latitude != 0)
                {
                    loc = new CurrentLocation();
                    loc.Latitude = currentLocation.Latitude;
                    loc.Longitude = currentLocation.Longitude;
                    if(loc!=null)
                   await GetNearByFriends();
                }
                else
                    await App.LocationOn();
            }
            catch (Exception ex)
            {
                App.HideMainPageLoader();
                IsLoading = false;
                await App.Instance.Alert("Problem in fetching location!", Constant.AlertTitle, Constant.Ok);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
