using System;
using System.Collections.Generic;
using Plugin.Connectivity;
using Pulse.Pages.Friends;
using Xamarin.Forms;
using Xamarin.Forms.PancakeView;

namespace Pulse
{
	public partial class MyFriendsView : ContentView
	{
		readonly EventViewModel eventViewModel;
		List<MyEventResponse> list = new List<MyEventResponse>();
		int _tapCount = 0;
		readonly FriendsViewModel friendsViewModel;
		MainServices mainService;
		public MyFriendsView()
		{
			InitializeComponent();
			eventViewModel = ServiceContainer.Resolve<EventViewModel>();
			friendsViewModel = ServiceContainer.Resolve<FriendsViewModel>();
			BindingContext = friendsViewModel;
			App.ShowMainPageLoader();
			mainService = new MainServices();
			SetUi();
			SetInitialValues();
			friendsViewModel.PendingRequestCount();
			friendsViewModel.tempFriendList.Clear();
			friendsViewModel.pageNoFriend = 1;
			friendsViewModel.totalPagesMyFriends = 1;
			friendsViewModel.GetMyFriendsList();
			friendsViewModel.GetAllUser();
			friendsViewModel.GetAllContacts();
		}
		void SetUi()
		{
			var effect = Effect.Resolve($"NotchEffect.{nameof(NotchEffect)}");
			GradientColorStack gradientColorStack = new GradientColorStack()
			{
				StartColor = Color.FromHex(Constant.LightPinkColor),
				EndColor = Color.FromHex(Constant.DarkPinkColor),
				Spacing = 0,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				stackOrientation = StackOrientation.Horizontal
			};
			gradientColorStack.Effects.Add(effect);
			StackLayout stack = new StackLayout()
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Horizontal,
				Margin = new Thickness(15, 20, 15, 10)
			};
			if (Device.RuntimePlatform == Device.Android)
			{
				stack.Margin = new Thickness(15, 15, 15, 8);
			}
			ExtendedLabel label = new ExtendedLabel()
			{
				Text = "My Events",
				HorizontalOptions = LayoutOptions.StartAndExpand,
				FontSize = 13,
				FontFace = FontFace.PoppinsMedium,
				TextColor = Color.FromHex(Constant.WhiteTextColor)
			};
			StackLayout stack1 = new StackLayout()
			{
				HorizontalOptions = LayoutOptions.End,
				Margin = new Thickness(7, 0, 7, 0),
				Orientation = StackOrientation.Horizontal
			};
			ExtendedLabel label1 = new ExtendedLabel()
			{
				Text = "See All",
				HorizontalOptions = LayoutOptions.End,
				FontSize = 13,
				FontFace = FontFace.PoppinsMedium,
				TextColor = Color.FromHex(Constant.WhiteTextColor)
			};
			stack1.Children.Add(label1);
			stack.Children.Add(label);
			stack.Children.Add(stack1);
			var tapGestureRecognizer = new TapGestureRecognizer();
			tapGestureRecognizer.Tapped += SeeAllTapped;
			stack1.GestureRecognizers.Add(tapGestureRecognizer);
			gradientColorStack.Children.Add(stack);
			GetEventList(gradientColorStack);
		}
		async void SeeAllTapped(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					// App.ShowMainPageLoader();
					await Navigation.PushAsync(new MyEventsPage());
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
		async void GetEventList(GradientColorStack gradientColorStack)
		{
			try
			{
				if (SessionManager.AccessToken != null)
				{
					App.ShowMainPageLoader();
					var response = await mainService.Get<ResultWrapper<MyEventResponse>>(Constant.NextSevenDaysEventUrl + "1" + "&datetime=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
					if (response != null && response.status == Constant.Status200 && response.response != null)
					{
						list = response.response;
						if (list != null && list.Count > 0)
						{
							ScrollView scroll = new ScrollView()
							{
								Orientation = ScrollOrientation.Horizontal
							};
							StackLayout stack = new StackLayout()
							{
								HorizontalOptions = LayoutOptions.FillAndExpand,
								Orientation = StackOrientation.Horizontal
							};
							foreach (var i in list)
							{
								PancakeView frame = new PancakeView
								{
									HeightRequest = 80,
									CornerRadius = 10,
									BackgroundColor = Color.FromHex(Constant.WhiteTextColor),
									Margin = new Thickness(15, 10, 0, 15),
									Padding = new Thickness(0, 5, 0, 5),
									WidthRequest = App.ScreenWidth / 1.8,
								};
								frame.Shadow = new DropShadow
								{
									Color = Color.Black,
								};
								StackLayout innerStack = new StackLayout
								{
									HorizontalOptions = LayoutOptions.FillAndExpand,
									VerticalOptions = LayoutOptions.FillAndExpand,
									Spacing = 0,
									ClassId = Convert.ToString(i.id)
								};
								ExtendedLabel lblEventName = new ExtendedLabel
								{
									FontFace = FontFace.PoppinsSemiBold,
									TextColor = Color.FromHex(Constant.GrayTextColor),
									FontSize = 13,
									HorizontalOptions = LayoutOptions.FillAndExpand,
									Margin = new Thickness(10, 0, 10, 0),
									LineBreakMode = LineBreakMode.TailTruncation,
									Text = i.name
								};
								ExtendedLabel lblHostedby = new ExtendedLabel
								{
									FontFace = FontFace.PoppinsSemiBold,
									TextColor = Color.FromHex(Constant.GrayTextColor),
									FontSize = 13,
									HorizontalOptions = LayoutOptions.FillAndExpand,
									Margin = new Thickness(10, 0, 10, 0),
									LineBreakMode = LineBreakMode.TailTruncation,
									//Text = i.
								};
								ExtendedLabel lblEventInviteeCount = new ExtendedLabel
								{
									FontFace = FontFace.PoppinsRegular,
									TextColor = Color.FromHex(Constant.GrayTextColor),
									FontSize = 12,
									Margin = new Thickness(10, 0, 10, 0),
									Text = !string.IsNullOrEmpty(Convert.ToString(i.event_attendees_count)) ? i.event_attendees_count > 1 ? Convert.ToString(i.event_attendees_count) + " Attendees" : Convert.ToString(i.event_attendees_count) + " Attendees" : "No Attendees"
								};
								BoxView boxView = new BoxView
								{
									HeightRequest = 1,
									Margin = new Thickness(0, 5, 0, 5),
									BackgroundColor = Color.FromHex(Constant.BoxViewColor)
								};
								ExtendedLabel lblEventTime = new ExtendedLabel
								{
									FontFace = FontFace.PoppinsRegular,
									TextColor = Color.FromHex(Constant.GrayTextColor),
									FontSize = 10,
									Margin = new Thickness(10, 0, 10, 0),
									Text = Convert.ToDateTime(i.start_date).ToString("MMM dd, yyyy") + " " + Convert.ToDateTime(i.start_date + " " + i.start_time).ToString("HH:mm tt")
								};
								innerStack.Children.Add(lblEventName);
								innerStack.Children.Add(lblEventInviteeCount);
								innerStack.Children.Add(boxView);
								innerStack.Children.Add(lblEventTime);
								frame.Content = innerStack;
								if (Device.RuntimePlatform == Device.Android)
								{
									frame.CornerRadius = 10;
								}
								var tapGestureRecognizer = new TapGestureRecognizer();
								tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
								innerStack.GestureRecognizers.Add(tapGestureRecognizer);
								stack.Children.Add(frame);
							}

							//if (Device.RuntimePlatform == Device.iOS)
							//{
							//    gradientColorStack.Padding = new Thickness(0, 20, 0, 0);
							//}

							scroll.Content = stack;
							gradientColorStack.Children.Add(scroll);
							stackMainMyevents.Children.Add(gradientColorStack);
						}
						else
						{
							stackMainMyevents.Children.Add(gradientColorStack);
						}
					}
					else
					{
						stackMainMyevents.Children.Add(gradientColorStack);
					}
				}
				else
				{
					stackMainMyevents.Children.Add(gradientColorStack);
				}
			}
			catch (Exception)
			{
				stackMainMyevents.Children.Add(gradientColorStack);
				App.HideMainPageLoader();
			}
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

		void SetInitialValues()
		{
			if (Device.RuntimePlatform == Device.Android)
			{
				stackPendingRequest.Padding = new Thickness(0, 13, 0, 13);
				topStack.Padding = new Thickness(0, 10, 0, 10);
				topStack.Margin = new Thickness(13, 0, 13, 0);
			}
            if (Device.RuntimePlatform == Device.iOS)
            {
                
            }
		}


		async void SearchIcon_Tapped(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					App.ShowMainPageLoader();
					await Navigation.PushModalAsync(new SearchFriendPage());
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

		async void RightArrowIcon_Tapped(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					App.ShowMainPageLoader();
					await Navigation.PushModalAsync(new PendingFriendRequestPage());
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
		async void Contact_Tapped(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					friendsViewModel.IsLoading=true;
					await Navigation.PushModalAsync(new ContactsPage());
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
		async void lstFriendTapped(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					App.ShowMainPageLoader();
    					var selected = (Friend)e.SelectedItem;
                       friendsViewModel.TappedFriendid = Convert.ToString(selected.friendId);
                        listViewfriends.SelectedItem = null;
                        await Navigation.PushModalAsync(new FriendsProfilePage("My Friends", friendsViewModel.TappedFriendid));
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
    }
}
