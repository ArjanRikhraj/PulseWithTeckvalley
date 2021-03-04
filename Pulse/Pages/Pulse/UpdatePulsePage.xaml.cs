using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ImageCircle.Forms.Plugin.Abstractions;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Pulse
{
	public partial class UpdatePulsePage : BaseContentPage
	{
		#region Variables
		int _tapCount = 0;
		readonly PulseViewModel pulseViewModel;
		List<UserInfo> ParticipantList = new List<UserInfo>();
		int TappeduserId;
		#endregion
		#region Constructor
		public UpdatePulsePage()
		{
			InitializeComponent();
			pulseViewModel = ServiceContainer.Resolve<PulseViewModel>();
			BindingContext = pulseViewModel;
			SetInitialValues();
		}
		#endregion
		protected override void OnAppearing()
		{
			CreateNewAddedParticipantLayout();
			LinkEvent();
		}

		void SetInitialValues()
		{
			GetPulseDetail();
			pulseViewModel.SelectedFriendsList.Clear();
		}

		void LinkEvent()
		{
			if (pulseViewModel.SelectedEventList != null && pulseViewModel.SelectedEventList.Count > 0)
			{
				pulseViewModel.LinkedEvent = pulseViewModel.SelectedEventList[0].EventName;
			}
		}
        void ClearHistoryTapped(object sender, System.EventArgs e)
        {
            MessagingCenter.Send<App>(App.Instance, "ClearChatHistory");
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<App>(this, "ClearChatHistory");
        }
		async void GetPulseDetail()
		{
			bool isDetail = await pulseViewModel.GetPulseDetail();
			if (isDetail && pulseViewModel.PulseDetail != null)
			{
				pulseViewModel.PulseSubject = pulseViewModel.PulseDetail.name;
				pulseViewModel.LinkedEvent = pulseViewModel.PulseDetail.event_info.id != 0 ? pulseViewModel.PulseDetail.event_info.name : "Link Event";
				pulseViewModel.IsEditableStackVisible = pulseViewModel.PulseDetail.event_info.id == 0;
				pulseViewModel.IsNonEditableStackVisible = !(pulseViewModel.PulseDetail.event_info.id == 0);
				pulseViewModel.ParticipantsCount = pulseViewModel.PulseDetail.participants_count == 1 ? pulseViewModel.PulseDetail.participants_count + " Participant" : pulseViewModel.PulseDetail.participants_count + " Participants";
				ParticipantList = pulseViewModel.PulseDetail.participants;
				CreateParticipantLayout(ParticipantList);
			}

		}
		void CreateParticipantLayout(List<UserInfo> list)
		{
			stackAlreadyParticipants.Children.Clear();
			if (list != null && list.Count > 0)
			{
				foreach (var item in list)
				{
					StackLayout mainStack = new StackLayout()
					{
						HorizontalOptions = LayoutOptions.FillAndExpand,
						VerticalOptions = LayoutOptions.FillAndExpand,
						BackgroundColor = Color.FromHex(Constant.WhiteTextColor),
						Spacing = 0,

					};
					StackLayout stack = new StackLayout()
					{
						HorizontalOptions = LayoutOptions.FillAndExpand,
						VerticalOptions = LayoutOptions.FillAndExpand,
						Orientation = StackOrientation.Horizontal,
						Padding = new Thickness(15, 11, 15, 11),
						Spacing = 10,
						ClassId = Convert.ToString(item.id)
					};
					CircleImage image = new CircleImage()
					{
						HeightRequest = 25,
						WidthRequest = 25,
						Source = (!string.IsNullOrEmpty(item.profile_image) ? ((item.profile_image == Constant.ProfileIcon) ? Constant.ProfileIcon : PageHelper.GetUserImage(item.profile_image)) : Constant.ProfileIcon),
						Aspect = Aspect.Fill
					};
					ExtendedLabel labelName = new ExtendedLabel()
					{
						FontFace = FontFace.PoppinsMedium,
						FontSize = 13,
						TextColor = Color.FromHex(Constant.GrayTextColor),
						Text = item.fullname,
						LineBreakMode = LineBreakMode.TailTruncation,
						HorizontalOptions = LayoutOptions.StartAndExpand
					};
					ExtendedLabel label = new ExtendedLabel()
					{
						FontFace = FontFace.PoppinsLight,
						FontSize = 11,
						TextColor = Color.FromHex(Constant.TimeColor),
						Text = "-You",
						IsVisible = item.id == SessionManager.UserId,
						HorizontalOptions = LayoutOptions.End
					};
					stack.Children.Add(image);
					stack.Children.Add(labelName);
					stack.Children.Add(label);
					var tapGestureRecognizer = new TapGestureRecognizer();
					tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
					stack.GestureRecognizers.Add(tapGestureRecognizer);
					BoxView box = new BoxView()
					{
						HeightRequest = .5,
						HorizontalOptions = LayoutOptions.FillAndExpand,
						VerticalOptions = LayoutOptions.FillAndExpand,
						Color = Color.FromHex(Constant.OutLineColor)
					};
					mainStack.Children.Add(stack);
					mainStack.Children.Add(box);
					stackAlreadyParticipants.Children.Add(mainStack);
				}
			}
		}

		void CreateNewAddedParticipantLayout()
		{
			stackNewAddedParticipants.Children.Clear();

			if (pulseViewModel.SelectedFriendsList != null && pulseViewModel.SelectedFriendsList.Count > 0)
			{
				foreach (var item in pulseViewModel.SelectedFriendsList)
				{
					StackLayout mainStack = new StackLayout()
					{
						HorizontalOptions = LayoutOptions.FillAndExpand,
						VerticalOptions = LayoutOptions.FillAndExpand,
						BackgroundColor = Color.FromHex(Constant.WhiteTextColor),
						Spacing = 0,

					};
					StackLayout stack = new StackLayout()
					{
						HorizontalOptions = LayoutOptions.FillAndExpand,
						VerticalOptions = LayoutOptions.FillAndExpand,
						Orientation = StackOrientation.Horizontal,
						Padding = new Thickness(15, 11, 15, 11),
						Spacing = 10
					};
					CircleImage image = new CircleImage()
					{
						HeightRequest = 25,
						WidthRequest = 25,
						Source = (!string.IsNullOrEmpty(item.friendPic) ? ((item.friendPic == Constant.ProfileIcon) ? Constant.ProfileIcon : PageHelper.GetUserImage(item.friendPic)) : Constant.ProfileIcon),
						Aspect = Aspect.Fill
					};
					ExtendedLabel labelName = new ExtendedLabel()
					{
						FontFace = FontFace.PoppinsMedium,
						FontSize = 13,
						TextColor = Color.FromHex(Constant.GrayTextColor),
						Text = item.friendFullname,
						LineBreakMode = LineBreakMode.TailTruncation,
						HorizontalOptions = LayoutOptions.StartAndExpand
					};
					ExtendedLabel label = new ExtendedLabel()
					{
						FontFace = FontFace.PoppinsMedium,
						FontSize = 12,
						TextColor = Color.FromHex(Constant.PinkButtonColor),
						Text = "Remove",
						HorizontalOptions = LayoutOptions.End,
						ClassId = Convert.ToString(item.friendId)
					};
					stack.Children.Add(image);
					stack.Children.Add(labelName);
					stack.Children.Add(label);
					var tapGestureRecognizer = new TapGestureRecognizer();
					tapGestureRecognizer.Tapped += RemoveTapGestureRecognizer_Tapped;
					label.GestureRecognizers.Add(tapGestureRecognizer);
					BoxView box = new BoxView()
					{
						HeightRequest = .5,
						HorizontalOptions = LayoutOptions.FillAndExpand,
						VerticalOptions = LayoutOptions.FillAndExpand,
						Color = Color.FromHex(Constant.OutLineColor)
					};
					mainStack.Children.Add(stack);
					mainStack.Children.Add(box);
					stackNewAddedParticipants.Children.Add(mainStack);
				}
			}
		}

		void RemoveTapGestureRecognizer_Tapped(object sender, EventArgs e)
		{
			var selectedItem = (ExtendedLabel)sender;
			if (pulseViewModel.SelectedFriendsList != null && pulseViewModel.SelectedFriendsList.Count > 0 && pulseViewModel.SelectedFriendsList.Any(i => i.friendId == Convert.ToInt32(selectedItem.ClassId)))
			{
				var listItem = pulseViewModel.SelectedFriendsList.Where(i => i.friendId == Convert.ToInt32(selectedItem.ClassId)).FirstOrDefault();
				if (listItem != null)
				{
					pulseViewModel.SelectedFriendsList.Remove(listItem);
				}
			}
			CreateNewAddedParticipantLayout();
		}

		void TapGestureRecognizer_Tapped(object sender, EventArgs e)
		{
			var selectedItem = (StackLayout)sender;
			if (selectedItem != null)
			{
				var listItem = ParticipantList.Where(i => i.id == Convert.ToInt32(selectedItem.ClassId)).FirstOrDefault();
				lblSelectedParticipant.Text = listItem.fullname;
				TappeduserId = Convert.ToInt32(selectedItem.ClassId);
				if (TappeduserId != SessionManager.UserId)
				{
					grdOverlayDialog.IsVisible = true;
					stackPopUp.IsVisible = true;
				}
			}
		}

		async void LinkEventTapped(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					pulseViewModel.IsLoading = true;
					await Navigation.PushModalAsync(new LinkEventPage());
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

		async void AddParticipantsTapped(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					pulseViewModel.IsLoading = true;
					await Navigation.PushModalAsync(new AddParticipantPage("EditPulse"));
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
		async void Remove_Tapped(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					pulseViewModel.IsLoading = true;
					grdOverlayDialog.IsVisible = false;
					stackPopUp.IsVisible = false;
					bool result = await App.Instance.ConfirmAlert(Constant.RemoveParticipantText, Constant.AlertTitle, Constant.Ok, Constant.CancelButtonText);
					if (result)
					{
						bool isDeleted = await pulseViewModel.PulseStatusChange(3, TappeduserId);
						if (isDeleted)
						{
							GetPulseDetail();
						}
						_tapCount = 0;
					}
					_tapCount = 0;
					pulseViewModel.IsLoading = false;
				}
			}
			else
			{
				await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
				_tapCount = 0;
			}

		}
		void Cancel_Clicked(object sender, System.EventArgs e)
		{
			grdOverlayDialog.IsVisible = false;
			stackPopUp.IsVisible = false;
		}


		async void Save_Clicked(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					pulseViewModel.IsLoading = true;
					if (string.IsNullOrEmpty(pulseViewModel.PulseSubject.Trim()))
					{
						await App.Instance.Alert("Please input subject of pulse ", Constant.AlertTitle, Constant.Ok);
					}
					else
					{
						await pulseViewModel.UpdatePulse(pulseViewModel.PulseSubject);
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
	}
}
