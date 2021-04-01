using Pulse.Helpers;
using Pulse.Models.Friends;
using Pulse.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Pulse.ViewModels
{
    public class AddFriendsViewModel : BaseViewModel
    {
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
        private List<UserData> userInfo;
        public List<UserData> UserInfo
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
        public AddFriendsViewModel()
        {
            GetCurrentLocation();
            
        }

        private async Task GetNearByFriends()
        {
                var userCurrentLoc = new Xamarin.Essentials.Location(loc.Latitude, loc.Longitude);
                userInfo = new List<UserData>();
                List<UserData> userList = null;
                foreach (var item in userList)
                {
                   var nearByFriendLoc= new Xamarin.Essentials.Location(item.latitude, item.longitude);
                    var distance = userCurrentLoc.CalculateDistance(nearByFriendLoc, DistanceUnits.Kilometers);
                    if(distance<=25)
                    {
                        userInfo.Add(item);
                    }
                }
                var topSixUser = userInfo.Take(6);
                var firstUser = userInfo[0];
                friendModel = new AddFriendModel();
                friendModel.firstFriendName = firstUser.username;
                friendModel.firstFriendImage = firstUser.profile_image;
                var secondUser = userInfo[1];
                friendModel.secondFriendName = secondUser.username;
                friendModel.secondFriendImage = secondUser.profile_image;
                var thirdUser = userInfo[2];
                friendModel.thirdFriendName = thirdUser.username;
                friendModel.thirdFriendImage = thirdUser.profile_image;
                var fourthUser = userInfo[3];
                friendModel.fourthFriendName = fourthUser.username;
                friendModel.fourthFriendImage = fourthUser.profile_image;
                var fifthUser = userInfo[4];
                friendModel.fifthFriendName = fifthUser.username;
                friendModel.fifthFriendImage = fifthUser.profile_image;
                var sixthUser = userInfo[5];
                friendModel.sixthFriendName = sixthUser.username;
                friendModel.sixthFriendImage = sixthUser.profile_image;
                IsLoading = false;
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
                    //if(loc!=null)
                   //await GetNearByFriends();
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
