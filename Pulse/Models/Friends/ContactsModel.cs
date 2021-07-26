using System;
using System.Collections.Generic;
using System.Text;

namespace Pulse.Models.Friends
{
   public class ContactsModel:BaseViewModel
    {
        public long userId { get; set; }
        public string profileImage { get; set; }
        public string ProfileImage
        {
            get
            {
                return profileImage;
            }
            set
            {
                profileImage = value;
                OnPropertyChanged("ProfileImage");
            }
        }
        public string name { get; set; }
        public string contactNumber { get; set; }
        public string friendType { get; set; }
        public string FriendType
        {
            get
            {
                return friendType;
            }
            set
            {
                friendType = value;
                OnPropertyChanged("FriendType");
            }
        }
        public string nonPulseUser { get; set; }

    }
}
