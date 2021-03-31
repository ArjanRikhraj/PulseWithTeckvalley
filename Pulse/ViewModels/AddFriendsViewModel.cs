using Pulse.Helpers;
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
        public AddFriendsViewModel()
        {
            GetCurrentLocation();
            
        }

        private async Task GetNearByFriends()
        {
            try
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
            }
            catch (Exception ex)
            {
                App.HideMainPageLoader();
                await App.Instance.Alert("Problem in fetching location!", Constant.AlertTitle, Constant.Ok);
            }
        }

        private async void GetCurrentLocation()
        {
            try
            {
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
                await App.Instance.Alert("Problem in fetching location!", Constant.AlertTitle, Constant.Ok);
            }
        }
    }
}
