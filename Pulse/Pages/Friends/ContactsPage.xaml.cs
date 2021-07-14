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
        public ContactsPage()
        {
            InitializeComponent();
            BindingContext = new ContactsViewModel();
        }

        private async void Cross_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}