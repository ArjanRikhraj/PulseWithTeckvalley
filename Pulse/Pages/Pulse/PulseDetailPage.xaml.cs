using System;
using System.Collections.Generic;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Pulse
{
    public partial class PulseDetailPage : BaseContentPage
	{
		#region Variables
		int _tapCount = 0;
		readonly PulseViewModel pulseViewModel;
        readonly FriendsViewModel friendsViewModel;
		List<Friend> participantList = new List<Friend>();
		#endregion
		#region Constructor
		public PulseDetailPage()
		{
			InitializeComponent();
			pulseViewModel = ServiceContainer.Resolve<PulseViewModel>();
            friendsViewModel = ServiceContainer.Resolve<FriendsViewModel>();
			BindingContext = pulseViewModel;
			SetInitialValues();
		}

		#endregion
        async void ReportPulseTapped(object sender, System.EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                bool result = await DisplayAlert("Report & Leave", "Report spam and leave this group? If you report and leave, this chat's history will also be deleted.", "Report", "Cancel");
                if (result)
                {
                    if (_tapCount < 1)
                    {
                        _tapCount = 1;
                        pulseViewModel.IsLoading = true;
                        bool isLeft = await pulseViewModel.ReportPulseChange(5, SessionManager.UserId);
                        if (isLeft)
                        {
                            pulseViewModel.IsNotGroupMemberStackVisible = true;
                            pulseViewModel.IsWriteMessageStackVisible = false;
                            participantList = new List<Friend>();
                            GetPulseDetail();
                        }
                        pulseViewModel.IsLoading = false;
                        _tapCount = 0;
                    }
                }
            }
            else
            {
                await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                _tapCount = 0;
            }
        }

		async void LeavePulseTapped(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					pulseViewModel.IsLoading = true;
					bool isLeft = await pulseViewModel.PulseStatusChange(2, SessionManager.UserId);
					if (isLeft)
					{
						pulseViewModel.IsNotGroupMemberStackVisible = true;
						pulseViewModel.IsWriteMessageStackVisible = false;
						participantList = new List<Friend>();
						GetPulseDetail();
					}
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

		void SetInitialValues()
		{
			if (Device.RuntimePlatform == Device.Android)
			{
				grdTop.Margin = new Thickness(15, 15, 15, 15);
			}
			GetPulseDetail();
		}

		async void GetPulseDetail()
		{
			bool isDetail = await pulseViewModel.GetPulseDetail();
			if (isDetail && pulseViewModel.PulseDetail != null)
			{
				pulseViewModel.PulseSubject = pulseViewModel.PulseDetail.name;
				pulseViewModel.LinkedEvent = pulseViewModel.PulseDetail.event_info.id != 0 ? pulseViewModel.PulseDetail.event_info.name : "No Event Linked Yet";
				pulseViewModel.ParticipantsCount = pulseViewModel.PulseDetail.participants_count == 1 ? pulseViewModel.PulseDetail.participants_count + " Participant" : pulseViewModel.PulseDetail.participants_count + " Participants";
				leavePulseStackVisible.IsVisible = pulseViewModel.PulseDetail.group_member_or_not;
                ReportPulseStackVisible.IsVisible = pulseViewModel.PulseDetail.group_member_or_not;
				foreach (var item in pulseViewModel.PulseDetail.participants)
				{
                    participantList.Add(new Friend
                    {
                        friendId = item.id,
                        friendPic = !string.IsNullOrEmpty(item.profile_image) ? PageHelper.GetUserImage(item.profile_image) : Constant.ProfileIcon,
                        friendFullname = item.fullname,
                        IsYouLabelVisible = item.id == SessionManager.UserId,
                        IsActionLabelVisible = item.id != SessionManager.UserId,
                        IsAdminLabelVisible = item.is_admin
					});
				}
				listparticipants.ItemsSource = participantList;
			}
		}

        void ClearHistoryTapped(object sender, System.EventArgs e)
        {
            MessagingCenter.Send<App>(App.Instance, "ClearChatHistory");
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

	    async  void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
		{

            if (CrossConnectivity.Current.IsConnected)
            {
                if (_tapCount < 1)
                {
                    _tapCount = 1;
                    friendsViewModel.IsLoading = true;
                    var selected = (Friend)e.Item;
                    friendsViewModel.TappedFriendid = Convert.ToString(selected.friendId);
                    listparticipants.SelectedItem = null;
                    if (selected.friendId != SessionManager.UserId)
                    {
                        friendsViewModel.isAdmin = selected.IsAdminLabelVisible;
                        await Navigation.PushModalAsync(new FriendsProfilePage("My Friends", friendsViewModel.TappedFriendid));
                    }
                    friendsViewModel.IsLoading = false;
                    _tapCount = 0;
                }
            }
            else
            {
                await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                _tapCount = 0;
            }
			
		}
		protected override void OnDisappearing()
		{
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<App>(this, "ClearChatHistory");
		}
	}
}
