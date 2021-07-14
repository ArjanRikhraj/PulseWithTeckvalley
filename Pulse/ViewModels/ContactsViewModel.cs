using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xamarin.Essentials;
using Pulse.Models.Friends;
using System.Collections.ObjectModel;

namespace Pulse.ViewModels
{
    public class ContactsViewModel : BaseViewModel
    {
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
        public ContactsViewModel()
        {
            GetAllContacts();
        }

        private async void GetAllContacts()
        {
            try
            {
                IsLoading = true;
                var result = await Contacts.GetAllAsync();
                var contacts = result.ToList().Take(20);
                contactList = new ObservableCollection<ContactsModel>();
                foreach (var item in contacts)
                {
                    ContactsModel model = new ContactsModel();
                    model.profileImage = Constant.UserDefaultSquareImage;
                    model.name = item.DisplayName;
                    model.contactNumber = item.Phones[0].ToString();
                    model.contactIcon = Constant.ShareIcon;
                    contactList.Add(model);
                }
                ContactList = contactList;
                IsLoading = false;
            }
            catch (Exception ex)
            {
                IsLoading = false;
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
            }
        }
    }
}
