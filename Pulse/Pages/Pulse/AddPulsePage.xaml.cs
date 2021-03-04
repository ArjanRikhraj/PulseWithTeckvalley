using ImageCircle.Forms.Plugin.Abstractions;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Pulse
{
	public partial class AddPulsePage : BaseContentPage
	{
		#region Private Variables
		int _tapCount = 0;
		readonly PulseViewModel pulseViewModel;
		#endregion
		#region Constructor
		public AddPulsePage()
		{
			InitializeComponent();
			pulseViewModel = ServiceContainer.Resolve<PulseViewModel>();
			BindingContext = pulseViewModel;
			SetInitialValues();
		}



		#endregion
		#region Overridden method
		protected override void OnAppearing()
		{
			CreateParticipantLayout();
			LinkEvent();
		}

		#endregion
		#region Methods
		void SetInitialValues()
		{
			if (Device.RuntimePlatform == Device.Android)
			{
				grdTop.Margin = new Thickness(0, 10, 10, 10);
			}
			pulseViewModel.SelectedFriendsList.Clear();
			pulseViewModel.SelectedEventList.Clear();
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

		void LinkEvent()
		{
			if (pulseViewModel.SelectedEventList != null && pulseViewModel.SelectedEventList.Count > 0)
			{
				lblLinkEvent.Text = pulseViewModel.SelectedEventList[0].EventName;
			}
		}

		async void Save_Clicked(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					pulseViewModel.IsLoading = true;
					if (string.IsNullOrEmpty(lblSubject.Text))
					{
						await App.Instance.Alert("Please input subject of pulse ", Constant.AlertTitle, Constant.Ok);
					}
					else if (string.IsNullOrEmpty(lblSubject.Text.Trim()))
					{
						await App.Instance.Alert("Please input subject of pulse ", Constant.AlertTitle, Constant.Ok);
					}
					else if (pulseViewModel.SelectedFriendsList.Count <= 0)
					{
						await App.Instance.Alert("Please add participants first", Constant.AlertTitle, Constant.Ok);
					}
					else
					{
						await pulseViewModel.CreatePulse(lblSubject.Text);
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

		void CreateParticipantLayout()
		{
			stackParticipants.Children.Clear();

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
						Text = item.friendFullname
					};
					stack.Children.Add(image);
					stack.Children.Add(labelName);
					BoxView box = new BoxView()
					{
						HeightRequest = .5,
						HorizontalOptions = LayoutOptions.FillAndExpand,
						VerticalOptions = LayoutOptions.FillAndExpand,
						Color = Color.FromHex(Constant.OutLineColor)
					};
					mainStack.Children.Add(stack);
					mainStack.Children.Add(box);
					stackParticipants.Children.Add(mainStack);
				}
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
					await Navigation.PushModalAsync(new AddParticipantPage("AddPulse"));
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
		#endregion
	}
}
