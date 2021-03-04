using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Pulse
{
	public partial class LinkEventPage : BaseContentPage
	{

		#region Private Variables
		int _tapCount = 0;
		string searchvalue;
		readonly PulseViewModel pulseViewModel;
		bool isSearchedValue;
		public List<MyEvents> selectedEvent = new List<MyEvents>();
		ObservableCollection<MyEvents> tempEventList = new ObservableCollection<MyEvents>();
		#endregion
		#region Constructor
		public LinkEventPage()
		{
			InitializeComponent();
			pulseViewModel = ServiceContainer.Resolve<PulseViewModel>();
			BindingContext = pulseViewModel;
			ExtendedFrame ex = new ExtendedFrame();
			listViewMyEvents.LoadMoreCommand = new Command(GetMyEventsList);
			SetInitialValues();
		}
		#endregion
		#region Override Methods
		protected override void OnAppearing()
		{
			if (pulseViewModel.SelectedEventList != null && pulseViewModel.SelectedEventList.Count > 0)
			{
				foreach (var i in pulseViewModel.SelectedEventList)
				{
					selectedEvent.Add(i);
				}
			}
			tempEventList.Clear();
			pulseViewModel.pageNoEvent = 1;
			pulseViewModel.totalEventPages = 1;
			searchvalue = entryEvent.Text;
			GetMyEventsList();
		}
		#endregion
		#region Methods
		void SetInitialValues()
		{
			if (Device.RuntimePlatform == Device.Android)
			{
				topStack.Margin = new Thickness(10, 10, 10, 10);
			}
		}

		void MyEventsList_ItemTapped(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
		{
			try
			{
				if (e.SelectedItem == null)
				{
					return;
				}
				else
				{
					foreach (var item in tempEventList)
					{
						item.FrameBorderColor = Color.Transparent;
						item.FrameBorderWidth = 0;
						item.IsOuterFrameVisible = false;
					}
					var evnt = (MyEvents)e.SelectedItem;
					evnt.FrameBorderColor = Color.FromHex(Constant.PinkButtonColor);
					evnt.FrameBorderWidth = 3;
					evnt.IsOuterFrameVisible = true;
					selectedEvent.Clear();
					selectedEvent.Add(evnt);
					listViewMyEvents.SelectedItem = null;
				}
			}
			catch (Exception)
			{
				return;
			}
		}

		void CrossIcon_Tapped(object sender, System.EventArgs e)
		{
			entryEvent.Text = string.Empty;
		}

		void SearchEntry_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
		{
			try
			{
				if (e.NewTextValue == string.Empty)
				{
					pulseViewModel.IsEventListVisible = false;
					pulseViewModel.IsNoEventVisible = false;
					tempEventList.Clear();
					pulseViewModel.pageNoEvent = 1;
					pulseViewModel.totalEventPages = 1;
					searchvalue = entryEvent.Text;
					GetMyEventsList();

				}
				else
				{
					if (entryEvent.Text.Length >= 3 && !isSearchedValue)
					{
						pulseViewModel.IsLoading = true;
						isSearchedValue = true;
						tempEventList.Clear();
						pulseViewModel.pageNoEvent = 1;
						pulseViewModel.totalEventPages = 1;
						pulseViewModel.IsEventListVisible = true;
						pulseViewModel.IsNoEventVisible = false;
						searchvalue = entryEvent.Text;
						GetMyEventsList();
					}
					else
					{
						pulseViewModel.IsEventListVisible = false;
						pulseViewModel.IsNoEventVisible = false;
					}
				}
			}
			catch (Exception)
			{
				pulseViewModel.IsLoading = false;
			}
		}
		async void GetMyEventsList()
		{
			try
			{
				pulseViewModel.IsLoading = true;
				if (!CrossConnectivity.Current.IsConnected)
				{
					await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
				}
				else
				{
					bool isList = await pulseViewModel.GetMyEvents(searchvalue);
					SetEventsList(isList, pulseViewModel.EventsList);
				}
				pulseViewModel.IsLoading = false;
			}

			catch (Exception)
			{
				pulseViewModel.IsLoading = false;
				pulseViewModel.TapCount = 0;
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
			}
		}

		string SetEventDate(string startDate, string startTime)
		{
			var dateStart = DateTime.Parse(startDate + " " + startTime);
			return dateStart.Date.ToString("ddd,dd MMM").ToUpperInvariant() + ", " + dateStart.ToString("h:mm tt").Trim().ToUpperInvariant();
		}
		void SetEventsList(bool isList, List<MyEventResponse> list)
		{

			if (isList && pulseViewModel.pageNoEvent < 2)
			{
				SetEvents(list);
			}
			else if (isList)
			{
				SetEvents(list);
			}
			else if (!isList && pulseViewModel.pageNoEvent < 2)
			{
				pulseViewModel.IsEventListVisible = false;
				pulseViewModel.IsNoEventVisible = true;
				isSearchedValue = false;
				pulseViewModel.IsLoading = false;
			}
			else
			{
				listViewMyEvents.ItemsSource = tempEventList;
				isSearchedValue = false;
				pulseViewModel.IsLoading = false;
			}
		}

		void SetEvents(List<MyEventResponse> list)
		{
			pulseViewModel.IsEventListVisible = true;
			pulseViewModel.IsNoEventVisible = false;
			foreach (var item in list)
			{
				bool isAlreadySelectedItem = false;
				Color bordercolor = Color.Transparent;
				int borderwidth = 0;
				float cornerradius = Device.RuntimePlatform == Device.Android ? 1 : 4;
				string loc = item.event_venue + "," + item.location_address;
				string attendee;
				string date = SetEventDate(item.start_date, item.start_time) + " " + item.time_zone_type;
				bool isImageOneVisible;
				bool isImageSecondVisible;
				if (selectedEvent.Count > 0)
				{
					isAlreadySelectedItem = selectedEvent.Any(x => x.EventId == item.id);
				}
				string imageOne;
				string imageSecond;
				if (item.event_attendees_count <= 0)
				{
					attendee = "No attendees yet!";
					isImageOneVisible = false;
					isImageSecondVisible = false;
					imageOne = string.Empty;
					imageSecond = string.Empty;
				}
				else
				{
					attendee = item.event_attendees_count > 2 ? "+" + Convert.ToString(item.event_attendees_count - 2) + " More attendees" : "No More attendees";
					if (item.event_attendees_count >= 2)
					{
						isImageOneVisible = true;
						isImageSecondVisible = true;
						imageOne = !string.IsNullOrEmpty(item.attendees[0].profile_image) ? PageHelper.GetUserImage(item.attendees[0].profile_image) : Constant.UserDefaultSquareImage;
						imageSecond = !string.IsNullOrEmpty(item.attendees[1].profile_image) ? PageHelper.GetUserImage(item.attendees[1].profile_image) : Constant.UserDefaultSquareImage;
					}
					else
					{
						isImageOneVisible = true;
						isImageSecondVisible = false;
						imageOne = !string.IsNullOrEmpty(item.attendees[0].profile_image) ? PageHelper.GetUserImage(item.attendees[0].profile_image) : Constant.UserDefaultSquareImage;
						imageSecond = string.Empty;
					}
				}
				if (isAlreadySelectedItem)
				{
					bordercolor = Color.FromHex(Constant.PinkButtonColor);
					borderwidth = 2;
				}
				tempEventList.Add(new MyEvents
				{
					EventId = item.id,
					EventName = item.name,
					EventLikes = Convert.ToString(item.event_likes_count),
					EventAddress = loc,
					EventLitScore = Convert.ToString(item.event_lit_score) + Constant.LitScoreText,
					AttendeeCount = attendee,
					EventDateTime = date,
					IsEditIconVisible = false,
					IsFirstImageVisible = isImageOneVisible,
					IsSecondImageVisible = isImageSecondVisible,
					AttendeeImageFirst = imageOne,
					AttendeeImageSecond = imageSecond,
					FrameBorderColor = bordercolor,
					FrameBorderWidth = borderwidth,
					IsOuterFrameVisible = isAlreadySelectedItem
				});
			}
			pulseViewModel.EventsList.Clear();
			listViewMyEvents.ItemsSource = tempEventList;
			pulseViewModel.pageNoEvent++;
			isSearchedValue = false;
			pulseViewModel.IsLoading = false;
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
		async void Done_Clicked(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					pulseViewModel.IsLoading = true;
					pulseViewModel.SelectedEventList.Clear();
					foreach (var i in selectedEvent)
					{
						pulseViewModel.SelectedEventList.Add(i);
					}
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
