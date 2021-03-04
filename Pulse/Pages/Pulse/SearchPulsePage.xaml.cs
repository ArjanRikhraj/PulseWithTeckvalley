using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Pulse
{
	public partial class SearchPulsePage : BaseContentPage
	{
		#region Private Variables
		int _tapCount = 0;
		readonly PulseViewModel pulseViewModel;
		string searchvalue;
		bool isSearchedValue;
		ObservableCollection<PulseModel> tempPulseList = new ObservableCollection<PulseModel>();
		#endregion
		#region Constructor
		public SearchPulsePage()
		{
			InitializeComponent();
			pulseViewModel = ServiceContainer.Resolve<PulseViewModel>();
			BindingContext = pulseViewModel;
			SetInitialValues();
		}
		#endregion
		#region Methods
		protected override void OnAppearing()
		{
			base.OnAppearing();
			tempPulseList.Clear();
			searchvalue = entryUser.Text;
			pulseViewModel.pageNoSearchPulse = 1;
			pulseViewModel.totalGroupsPage = 1;
			GetPulses();
		}
		void SetInitialValues()
		{
			if (Device.RuntimePlatform == Device.Android)
			{
				topStack.Margin = new Thickness(15, 15, 15, 15);
				searchFrame.CornerRadius = 2;
				entryUser.Margin = new Thickness(0, 8, 0, 0);
			}
			listSearchedPulse.IsVisible = false;
			lblNoPulse.IsVisible = false;
			listSearchedPulse.LoadMoreCommand = new Command(GetPulses);

		}
		void SearchEntry_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
		{
			try
			{
				if (e.NewTextValue == string.Empty)
				{
					listSearchedPulse.IsVisible = false;
					lblNoPulse.IsVisible = false;
					tempPulseList.Clear();
					searchvalue = entryUser.Text;
					pulseViewModel.pageNoSearchPulse = 1;
					pulseViewModel.totalGroupsPage = 1;
					GetPulses();
				}
				else
				{
					if (entryUser.Text.Length >= 3 && !isSearchedValue)
					{
						isSearchedValue = true;
						tempPulseList.Clear();
						listSearchedPulse.IsVisible = true;
						lblNoPulse.IsVisible = false;
						searchvalue = entryUser.Text;
						pulseViewModel.pageNoSearchPulse = 1;
						pulseViewModel.totalGroupsPage = 1;
						GetPulses();
					}
					else
					{
						listSearchedPulse.IsVisible = false;
						lblNoPulse.IsVisible = false;
					}
				}
			}
			catch (Exception)
			{
				pulseViewModel.IsLoading = false;
			}
		}
		async void GetPulses()
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

					bool isList = await pulseViewModel.GetPulseList(true, searchvalue);
					SetPulseList(isList, pulseViewModel.PulseList);

				}
				pulseViewModel.IsLoading = false;
			}

			catch (Exception)
			{
				IsLoading = false;
				_tapCount = 0;
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
			}
		}
		void SetPulseList(bool isList, List<PulseResponse> list)
		{

			if (isList && pulseViewModel.pageNoSearchPulse < 2)
			{
				SetPulse(list);
			}
			else if (isList)
			{
				SetPulse(list);
			}
			else if (!isList && pulseViewModel.pageNoSearchPulse < 2)
			{
				listSearchedPulse.IsVisible = false;
				lblNoPulse.IsVisible = true;
				pulseViewModel.IsLoading = false;
				isSearchedValue = false;
			}
			else
			{
				listSearchedPulse.ItemsSource = tempPulseList;
				pulseViewModel.IsLoading = false;
				isSearchedValue = false;
			}
		}

		void SetPulse(List<PulseResponse> list)
		{
			listSearchedPulse.IsVisible = true;
			lblNoPulse.IsVisible = false;
			int itemIndex = 0;
			foreach (var item in list)
			{
				string[] array = item.name.Split(' ');
				string imageTxt = "";
				string datetime = "";
				if (array.Length > 1 && !string.IsNullOrEmpty(array[1].Trim()))
				{
					imageTxt = array[0].Substring(0, 1).ToUpper() + array[1].Substring(0, 1).ToUpper();
				}
				else if (array.Length > 0 && array.Length <= 1 && array[0].Length > 1)
				{
					imageTxt = array[0].Substring(0, 2).ToUpper();
				}
				else if (array.Length > 0 && array.Length <= 1)
				{
					imageTxt = array[0].Substring(0, 1).ToUpper();
				}
				if (item.last_message != null)
				{
					if (item.last_message.id <= 0)
					{
						datetime = pulseViewModel.SetMessageDate(item.create_date);
					}
					else
					{
						datetime = pulseViewModel.SetMessageDate(item.last_message.send_date);
					}
				}
				List<string> colorList = pulseViewModel.GetList();
				string color = Constant.PinkButtonColor;
				if (colorList.Count > itemIndex)
				{
					color = colorList[itemIndex];
				}
				else
				{
					itemIndex = -1;
				}
				tempPulseList.Add(new PulseModel { ImageText = imageTxt, Subject = item.name, IsPulseMember = item.group_member_or_not, PulseId = item.id, PulseownerId = item.user, LastMessage = item.last_message.text_message, Time = datetime, GroupIconBorderColor = color });
				itemIndex++;
			}
			pulseViewModel.PulseList.Clear();
			listSearchedPulse.ItemsSource = tempPulseList;
			pulseViewModel.pageNoSearchPulse++;
			pulseViewModel.IsLoading = false;
			isSearchedValue = false;

		}
		async void lstSearchPulseTapped(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					pulseViewModel.IsLoading = true;
					var selected = (PulseModel)e.SelectedItem;
					pulseViewModel.TappedPulseId = selected.PulseId;
					int pulseOnwerId = selected.PulseownerId;
					pulseViewModel.PulseTitle = selected.Subject;
					bool isPulseMember = selected.IsPulseMember;
					listSearchedPulse.SelectedItem = null;
					await Navigation.PushModalAsync(new MessageChatPage(pulseOnwerId, isPulseMember, false));
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

		void CrossIcon_Tapped(object sender, System.EventArgs e)
		{
			entryUser.Text = string.Empty;
		}

		async void Cancel_Tapped(object sender, System.EventArgs e)
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
