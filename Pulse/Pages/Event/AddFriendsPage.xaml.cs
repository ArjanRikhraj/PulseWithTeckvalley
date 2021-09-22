using Plugin.Connectivity;
using Pulse.Models.Friends;
using Pulse.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PancakeView;
using Xamarin.Forms.Xaml;

namespace Pulse.Pages.Event
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddFriendsPage : BaseContentPage
    {
		readonly EventViewModel eventViewModel;
		int _tapCount = 0;
		MainServices mainService;
		List<MyEventResponse> list = new List<MyEventResponse>();
		public AddFriendsPage()
        {
            InitializeComponent();
			BindingContext = new AddFriendsViewModel();
        }
		
		async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
		{
			var selectedItem = (StackLayout)sender;
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					App.ShowMainPageLoader();
					eventViewModel.currentActiveEventType = MyEventType.Upcoming;
					await eventViewModel.FetchEventDetail(Convert.ToString(selectedItem.ClassId), true);
					App.HideMainPageLoader();
					_tapCount = 0;
				}
			}
			else
			{
				await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
				_tapCount = 0;
			}
		}

        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            try
            {
				await Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
				App.HideMainPageLoader();
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
			}
        }

        private async void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {
            try
            {
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					//App.ShowMainPageLoader();
					await Navigation.PushModalAsync(new SearchFriendPage());
					//App.HideMainPageLoader();
					_tapCount = 0;
				}
			}
			else
			{
				await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
				_tapCount = 0;
			}
			}
			catch (Exception ex)
			{
				App.HideMainPageLoader();
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
			}
		}

        private async void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)
        {
            try
            {
				
				if(!lblFirstFriendName.IsVisible)
					lblFirstFriendName.IsVisible = true;
                else
                {
					var stack = (Grid)sender;
					var friends = stack.BindingContext as AddFriendModel;
					if (friends.firstFriendUserId != 0)
						await Navigation.PushModalAsync(new FriendsProfilePage("My Friends", friends.FirstFriendUserId.ToString()));
				}
				
			}
            catch (Exception ex)
            {
				App.HideMainPageLoader();
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
			}
			
        }

        private async void TapGestureRecognizer_Tapped_4(object sender, EventArgs e)
        {
			try
			{
				if (!lblSecondFriendName.IsVisible)
					lblSecondFriendName.IsVisible = true;
                else
                {
					var stack = (Grid)sender;
					var friends = stack.BindingContext as AddFriendModel;
					if (friends.secondFriendUserId != 0)
						await Navigation.PushModalAsync(new FriendsProfilePage("My Friends", friends.SecondFriendUserId.ToString()));
				}
			}
			catch (Exception ex)
			{
				App.HideMainPageLoader();
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
			}
		}

        private async void TapGestureRecognizer_Tapped_5(object sender, EventArgs e)
        {
			try
			{
				if (!lblThirdFriendName.IsVisible)
					lblThirdFriendName.IsVisible = true;
				else
				{
					var stack = (Grid)sender;
					var friends = stack.BindingContext as AddFriendModel;
					if (friends.thirdFriendUserId != 0)
						await Navigation.PushModalAsync(new FriendsProfilePage("My Friends", friends.ThirdFriendUserId.ToString()));
				}
			}
			catch (Exception ex)
			{
				App.HideMainPageLoader();
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
			}
		}

        private async void TapGestureRecognizer_Tapped_6(object sender, EventArgs e)
        {
			try
			{
				if (!lblFourthFriendName.IsVisible)
					lblFourthFriendName.IsVisible = true;
				else
				{
					var stack = (Grid)sender;
					var friends = stack.BindingContext as AddFriendModel;
					if (friends.fourthFriendUserId != 0)
						await Navigation.PushModalAsync(new FriendsProfilePage("My Friends", friends.FourthFriendUserId.ToString()));
				}
			}
			catch (Exception ex)
			{
				App.HideMainPageLoader();
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
			}
		}

        private async void TapGestureRecognizer_Tapped_7(object sender, EventArgs e)
        {
			try
			{
				if (!lblFifthFriendName.IsVisible)
					lblFifthFriendName.IsVisible = true;
				else
				{
					var stack = (Grid)sender;
					var friends = stack.BindingContext as AddFriendModel;
					if (friends.fifthFriendUserId != 0)
						await Navigation.PushModalAsync(new FriendsProfilePage("My Friends", friends.FifthFriendUserId.ToString()));
				}
			}
			catch (Exception ex)
			{
				App.HideMainPageLoader();
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
			}
		}

        private async void TapGestureRecognizer_Tapped_8(object sender, EventArgs e)
        {
			try
			{
				if (!lblSixthFriendName.IsVisible)
					lblSixthFriendName.IsVisible = true;
				else
				{
					var stack = (Grid)sender;
					var friends = stack.BindingContext as AddFriendModel;
					if (friends.sixthFriendUserId != 0)
						await Navigation.PushModalAsync(new FriendsProfilePage("My Friends", friends.sixthFriendUserId.ToString()));
				}
			}
			catch (Exception ex)
			{
				App.HideMainPageLoader();
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
			}
		}
    }
}