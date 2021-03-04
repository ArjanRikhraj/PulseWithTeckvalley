using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageCircle.Forms.Plugin.Abstractions;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Pulse
{
	public partial class MessageChatPage : BaseContentPage
	{
		#region Variables
		int _tapCount = 0;
		readonly PulseViewModel pulseViewModel;
		List<PulseMessageResponse> AllMessageList = new List<PulseMessageResponse>();
		int pulseOwnerid;
		bool IsPulseMember;
		int pageCount;
		int messageCount;
		bool isPosted;
		bool forNotificationClick;
		double scrollContentHeight;
		bool isDataNotFetched;
		#endregion
		#region Constructor
		public MessageChatPage(int id, bool isPulseMember, bool forNotification)
		{
			InitializeComponent();
			pulseViewModel = ServiceContainer.Resolve<PulseViewModel>();
			BindingContext = pulseViewModel;
			pulseOwnerid = id;
			pulseViewModel.Message = string.Empty;
			IsPulseMember = isPulseMember;
			forNotificationClick = forNotification;
			//scrollContent.HeightRequest = App.ScreenHeight * 0.78;
			txtmessage.WidthRequest = App.ScreenWidth * 0.80;
			SetInitialValues();
			MessagingCenter.Subscribe<App>(this, "NewMessage", (sender) =>
			{
				GetNextMessages();
			});
            MessagingCenter.Subscribe<App>(this, "ClearChatHistory", (sender) =>
            {
                Clear_Tapped();
            });
			App.CurrentChatWindow = pulseViewModel.TappedPulseId;
			App.IsChatOpened = true;
            var refreshiconheight = App.ScreenHeight * 0.80;
			//stackRefresh.Margin = new Thickness(0, -refreshiconheight, 10, 10);
		}
		#endregion
		#region Methods
		void GetNextMessages()
		{
			mainMessageStack.Children.Clear();
			AllMessageList.Clear();
			pulseViewModel.pageNoMessage = 1;
			pulseViewModel.MessageList.Clear();
			GetMessages();
		}

		void SetInitialValues()
		{
			if (Device.RuntimePlatform == Device.Android)
			{
				grdTop.Margin = new Thickness(0, 10, 10, 10);
			}
			pulseViewModel.pageNoMessage = 1;
			pulseViewModel.MessageList.Clear();
			GetMessages();
			lblPulseTitle.Text = pulseViewModel.PulseTitle;
            lblPulseTitletapHere.Text = Constant.PulseTapHereText;
			pulseViewModel.IsWriteMessageStackVisible = IsPulseMember;
			pulseViewModel.IsNotGroupMemberStackVisible = !IsPulseMember;
		}

		protected async override void OnAppearing()
		{
			base.OnAppearing();
			lblPulseTitle.Text = pulseViewModel.PulseTitle;
            lblPulseTitletapHere.Text = Constant.PulseTapHereText;
			App.IsChatOpened = true;      
            await setScroll();
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			App.IsChatOpened = false;
		}

		void ChatArea_Unfocused(object sender, FocusEventArgs e)
		{
			if (Device.RuntimePlatform == Device.iOS)
			{
				//scrollContent.HeightRequest = scrollContentHeight;
			}
		}

		void ChatArea_Focused(object sender, FocusEventArgs e)
		{
			if (Device.RuntimePlatform == Device.iOS)
			{
				//scrollContentHeight = scrollContent.Height;
				//scrollContent.HeightRequest = App.ScreenHeight / 3;
			}
		}

		async void GetMessages()
		{
			try
			{
				pulseViewModel.IsLoading = true;
				if (!CrossConnectivity.Current.IsConnected)
				{
					await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
					_tapCount = 0;
				}
				else
				{
					if (!isDataNotFetched)
					{
						isDataNotFetched = true;
						bool isList = await pulseViewModel.GetMessageList();
						if (isList)
						{
							if (AllMessageList.Count > 0)
							{
								AllMessageList.Reverse();
							}
							foreach (var item in pulseViewModel.MessageList)
							{
								var hh = AllMessageList.Where(x => x.id == item.id).FirstOrDefault();
								if (hh == null)
									AllMessageList.Add(item);
							}
							SetMessageList(isList, AllMessageList);
						}
						if (Device.RuntimePlatform == Device.Android)
						{
							if (pulseViewModel.pageNoMessage <= 2)
							{
								await setScroll();
							}
						}
						else
						{
							if (txtmessage.IsFocused)
							{
								await setScroll();
                                pulseViewModel.IsLoading = false;
							}
							else
							{
								if (mainMessageStack.Children != null && mainMessageStack.Children.Count > 7 && pulseViewModel.pageNoMessage == 1)
								{
									var restControlHeight = scrollContent.Content.Height - App.ScreenHeight / 8;
                                    pulseViewModel.IsLoading = false;
									await scrollContent.ScrollToAsync(0, restControlHeight, true);

								}
							}
						}
					}



				}

				pulseViewModel.IsLoading = false;
			}

			catch (Exception)
			{
				pulseViewModel.IsLoading = false;
				_tapCount = 0;
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
			}
		}

		async void SetMessageList(bool isList, List<PulseMessageResponse> messageList)
		{
			if (isList && messageList != null && messageList.Count > 0)
			{
				messageList.Reverse();
				foreach (var item in messageList)
				{
					SetDateMessage(item);
					messageCount = item.message_count;
					double d = Convert.ToDouble(messageCount) / 10.0;
					pageCount = Convert.ToInt32(Math.Ceiling(d));
					if (SessionManager.UserId == item.user_info.id)
					{
						CreateMyMessageLayout(item);
					}
					else
					{
						CreateOtherMessagelayout(item);
					}
				}
				isPosted = false;
				isDataNotFetched = false;
			}
		}

		async Task setScroll()
		{
			var lastChild = mainMessageStack.Children.LastOrDefault();
			if (lastChild != null)
                await scrollContent.ScrollToAsync(lastChild, ScrollToPosition.Start, true);
		}

		void SetDateMessage(PulseMessageResponse item)
		{
			string frstMessageDate = "";
			if (item.is_first_message)
			{
				if (DateTime.Parse(item.send_date).Date == DateTime.Now.Date)
				{
					frstMessageDate = "Today " + new DateTime(DateTime.Parse(item.send_date).ToLocalTime().TimeOfDay.Ticks).ToString("h:mm tt");
				}
				else
				{
					frstMessageDate = DateTime.Parse(item.send_date).ToLocalTime().ToString("dd/MM/yyyy");
				}
			}
			ExtendedLabel dayTime = new ExtendedLabel
			{
				Text = frstMessageDate,
				FontFace = FontFace.PoppinsMedium,
				FontSize = 10,
				TextColor = Color.FromHex(Constant.TodayTimeColor),
				HorizontalOptions = LayoutOptions.Center,
				IsVisible = item.is_first_message
			};
			if (item.is_first_message)
			{
				mainMessageStack.Children.Add(dayTime);
			}
		}

		async void Cross_Clicked(object sender, EventArgs e)

		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					pulseViewModel.IsLoading = true;
					if (!forNotificationClick)
					{
						pulseViewModel.pageNoPulse = 1;
						pulseViewModel.tempPulseList.Clear();
						pulseViewModel.totalGroupsPage = 1;
						pulseViewModel.GetPulses();
						App.IsChatOpened = false;
						await Navigation.PopModalAsync();
					}
					else
					{
						App.IsChatOpened = false;
						Application.Current.MainPage = new NavigationPage(new MainPage(ActivePage.Pulse));
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

		void Post_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (string.IsNullOrEmpty(e.NewTextValue))
			{
				btnPost.IsVisible = false;
			}
			else
			{
				if (string.IsNullOrEmpty(txtmessage.Text.Trim()))
				{
					btnPost.IsVisible = false;
				}
				else
				{
					btnPost.IsVisible = true;
				}
			}
		}

		async void PulseTitleTapped(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					pulseViewModel.IsLoading = true;
					if (SessionManager.UserId == pulseOwnerid)
					{
						await Navigation.PushModalAsync(new UpdatePulsePage());
					}
					else
					{
						await Navigation.PushModalAsync(new PulseDetailPage());
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


		void CreateOtherMessagelayout(PulseMessageResponse item)
		{

			Grid gridMessage = new Grid
			{
				BackgroundColor = Color.FromHex(Constant.WhiteTextColor),
				RowSpacing = 0,
				ColumnSpacing = 2
			};
			gridMessage.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(15, GridUnitType.Star) });
			gridMessage.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(85, GridUnitType.Star) });
			gridMessage.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });
			CircleImage profileImage = new CircleImage
			{
				Margin = new Thickness(15, 20, 0, 0),
				Source = !string.IsNullOrEmpty(item.user_info.profile_image) ? PageHelper.GetUserImage(item.user_info.profile_image) : Constant.ProfileIcon,
				WidthRequest = 30,
				HeightRequest = 30,
				Aspect = Aspect.AspectFill,
				HorizontalOptions = LayoutOptions.Start,
				VerticalOptions = LayoutOptions.End
			};
			StackLayout mainStack = new StackLayout
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Margin = new Thickness(0, 20, 10, 0)
			};
			StackLayout horizontalStack = new StackLayout
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Horizontal
			};
			ExtendedLabel username = new ExtendedLabel
			{
				Text = item.user_info.fullname,
				FontFace = FontFace.PoppinsSemiBold,
				FontSize = 10,
				TextColor = Color.FromHex(Constant.GrayTextColor),
				HorizontalOptions = LayoutOptions.Start,

			};
			ExtendedLabel messageTime = new ExtendedLabel
			{
				Text = new DateTime(DateTime.Parse(item.send_date).ToLocalTime().TimeOfDay.Ticks).ToString("h:mm tt"),
				FontFace = FontFace.PoppinsRegular,
				FontSize = 10,
				TextColor = Color.FromHex(Constant.TimeColor),
				HorizontalOptions = LayoutOptions.EndAndExpand
			};
			horizontalStack.Children.Add(username);
			horizontalStack.Children.Add(messageTime);
			GradientColorFrame frame = new GradientColorFrame
			{
				IsTopLeftRoundNotRounded = true,
				BorderRadius = 11,
				HasShadow = false,
				BackgroundColor = Color.FromHex(Constant.PulseMessageBackgroundColor),
				Padding = new Thickness(5, 5, 5, 5),
				Margin = new Thickness(0, 0, 80, 0)
			};
			if (Device.RuntimePlatform == Device.Android)
			{
				frame.BorderRadius = 30;
				frame.StartColor = Color.FromHex(Constant.PulseMessageBackgroundColor);
				frame.EndColor = Color.FromHex(Constant.PulseMessageBackgroundColor);
			}
			ExtendedLabel messageText = new ExtendedLabel
			{
				Text = item.text_message,
				FontFace = FontFace.PoppinsRegular,
				FontSize = 12,
				TextColor = Color.FromHex(Constant.GrayTextColor),
			};
			frame.Content = messageText;
			mainStack.Children.Add(horizontalStack);
			mainStack.Children.Add(frame);
			gridMessage.Children.Add(profileImage, 0, 0);
			gridMessage.Children.Add(mainStack, 1, 0);
			mainMessageStack.Children.Add(gridMessage);
		}


		void CreateMyMessageLayout(PulseMessageResponse item)
		{

			Grid gridMessage = new Grid
			{
				BackgroundColor = Color.FromHex(Constant.WhiteTextColor),
				RowSpacing = 0,
				ColumnSpacing = 2
			};

			StackLayout mainStack = new StackLayout
			{
				HorizontalOptions = LayoutOptions.EndAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Margin = new Thickness(30, 10, 10, 0)
			};
			GradientColorFrame frame = new GradientColorFrame
			{
				IsBottomRightNotRounded = true,
				BorderRadius = 11,
				HasShadow = false,
				StartColor = Color.FromHex(Constant.LightPinkColor),
				EndColor = Color.FromHex(Constant.DarkPinkColor),
				BackgroundColor = Color.FromHex(Constant.PulseMessageBackgroundColor),
				Padding = new Thickness(5, 5, 5, 5),
				Margin = new Thickness(30, 0, 0, 0)
			};
			if (Device.RuntimePlatform == Device.Android)
			{
				frame.BorderRadius = 30;
			}
			ExtendedLabel messageText = new ExtendedLabel
			{
				Text = item.text_message,
				FontFace = FontFace.PoppinsRegular,
				FontSize = 12,
				TextColor = Color.FromHex(Constant.WhiteTextColor),
			};
			frame.Content = messageText;
			ExtendedLabel messageTime = new ExtendedLabel
			{
				Text = new DateTime(DateTime.Parse(item.send_date).ToLocalTime().TimeOfDay.Ticks).ToString("h:mm tt"),
				FontFace = FontFace.PoppinsRegular,
				FontSize = 10,
				TextColor = Color.FromHex(Constant.TimeColor),
				HorizontalOptions = LayoutOptions.EndAndExpand
			};
			mainStack.Children.Add(frame);
			mainStack.Children.Add(messageTime);
			gridMessage.Children.Add(mainStack, 0, 0);
            mainMessageStack.ClassId = item.id.ToString();
			mainMessageStack.Children.Add(gridMessage);
            //var tapGestureRecognizer = new TapGestureRecognizer();
            ////tapGestureRecognizer.Tapped += Delete_Tapped;
            //tapGestureRecognizer.Tapped += ;

		}


		async void PostMessageClicked(object sender, EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					pulseViewModel.IsLoading = true;
					bool isPulseMember = await pulseViewModel.PulsememberOrNot();
					if (isPulseMember)
					{
						pulseViewModel.IsWriteMessageStackVisible = true;
						pulseViewModel.IsNotGroupMemberStackVisible = false;
						bool isMessagePosted = await pulseViewModel.PostMessage();
						if (isMessagePosted)
						{
							isPosted = true;
							pulseViewModel.Message = string.Empty;
							mainMessageStack.Children.Clear();
							pulseViewModel.MessageList.Clear();
							pulseViewModel.pageNoMessage = 1;
							AllMessageList.Clear();
							isDataNotFetched = false;
							GetMessages();
						}
						pulseViewModel.IsLoading = false;
						_tapCount = 0;
					}
					else
					{
						pulseViewModel.IsWriteMessageStackVisible = false;
						pulseViewModel.IsNotGroupMemberStackVisible = true;
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

		void MessageList_Scrolled(object sender, ScrolledEventArgs e)

		{
			var scrollView = (ScrollView)sender;
			var scHeight = scrollView.ContentSize.Height;
			var yHorz = scrollView.ScrollY;
			var scrollDiff = scHeight - yHorz;
			if (yHorz == 0 && pulseViewModel.pageNoMessage < pageCount && !isPosted)
			{
				if (!CrossConnectivity.Current.IsConnected)
				{
					App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
				}
				else
				{
					mainMessageStack.Children.Clear();
					pulseViewModel.MessageList.Clear();
					pulseViewModel.pageNoMessage++;
					GetMessages();
				}
			}
		}

		void ClearHistoryTapped(object sender, EventArgs e)
		{
			grdOverlayDialog.IsVisible = true;
			stackPopUp.IsVisible = true;
		}
		void Cancel_Clicked(object sender, EventArgs e)
		{
			grdOverlayDialog.IsVisible = false;
			stackPopUp.IsVisible = false;
		}
		async void Clear_Tapped()
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					int lastmessageid = 0;
					pulseViewModel.IsLoading = true;
					grdOverlayDialog.IsVisible = false;
					stackPopUp.IsVisible = false;
					if (AllMessageList.Count > 0)
					{
						lastmessageid = AllMessageList.Select(x => x.id).LastOrDefault();
					}
					bool isHostoryCleared = await pulseViewModel.ClearHistory(1, lastmessageid, SessionManager.UserId);
					if (isHostoryCleared)
					{
						mainMessageStack.Children.Clear();
						pulseViewModel.pageNoMessage = 1;
						pulseViewModel.MessageList.Clear();
						AllMessageList.Clear();
						GetMessages();
					}     
					else
					{
						await App.Instance.Alert("Not able to clear History", Constant.AlertTitle, Constant.Ok);
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

		async void Refresh_Tapped(object sender, EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					mainMessageStack.Children.Clear();
					pulseViewModel.MessageList.Clear();
					pulseViewModel.pageNoMessage = 1;
					AllMessageList.Clear();
					GetMessages();
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



