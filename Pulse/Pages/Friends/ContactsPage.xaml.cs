using Pulse.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pulse.Pages.Friends
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactsPage : ContentPage
    {
        readonly FriendsViewModel friendsViewModel;
        public ContactsPage()
        {
            InitializeComponent();
            friendsViewModel = ServiceContainer.Resolve<FriendsViewModel>();
            BindingContext = friendsViewModel;
            friendsViewModel.GetAllMatchedContacts();
        }

        private async void Cross_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void CrossIcon_Tapped(object sender, EventArgs e)
        {
            friendsViewModel.SearchText = string.Empty;
        }

        private void ExtendedButton_Clicked(object sender, EventArgs e)
        {
            friendsViewModel.IsSearchBoxVisible = true;
        }

        private void ExtendedListView_Scrolled(object sender, ScrolledEventArgs e)
        {
            if (e.ScrollY >= 40)
                friendsViewModel.IsSearchBoxVisible = false;
            else if(e.ScrollY<=40)
                friendsViewModel.IsSearchBoxVisible = true;
        }
    }
}