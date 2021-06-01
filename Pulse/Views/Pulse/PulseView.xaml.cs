using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Pulse
{
	public partial class PulseView : ContentView
	{
		#region Variables
		int _tapCount = 0;
		readonly PulseViewModel pulseViewModel;
		#endregion
		#region Constructor
		public PulseView()
		{
			InitializeComponent();
			pulseViewModel = ServiceContainer.Resolve<PulseViewModel>();
			BindingContext = pulseViewModel;
          
			if (pulseViewModel.isPulseListAvailable)
			{
				pulseViewModel.IsPulseListVisible = true;
				pulseViewModel.IsFirstTimePulse = false;
			}
			else
			{
				pulseViewModel.IsPulseListVisible = false;
				pulseViewModel.IsFirstTimePulse = true;
			}
            if (Device.RuntimePlatform == Device.iOS)
            {
                
            }
			SetInitialValues();

		}
		#endregion
		#region methods
		void SetInitialValues()
		{
			if (Device.RuntimePlatform == Device.Android)
			{
				topStack.Padding = new Thickness(0, 10, 0, 10);
				topStack.Margin = new Thickness(13, 0, 13, 0);
			}
            if (Device.RuntimePlatform == Device.iOS)
            {
                
            }
			pulseViewModel.pageNoPulse = 1;
			pulseViewModel.totalGroupsPage = 1;
			while (pulseViewModel.tempPulseList.Count > 0)
				pulseViewModel.tempPulseList.RemoveAt(0);
			pulseViewModel.GetPulses();
		}


		async void SearchIcon_Tapped(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					App.ShowMainPageLoader();
					await Navigation.PushModalAsync(new SearchPulsePage());
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



		async void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
		{
			try
			{

				if (CrossConnectivity.Current.IsConnected)
				{
					if (_tapCount < 1)
					{
						_tapCount = 1;
						App.ShowMainPageLoader();
						var item = (PulseModel)e.Item;
						int pulseOnwerId = item.PulseownerId;
						pulseViewModel.TappedPulseId = Convert.ToInt32(item.PulseId);
						pulseViewModel.PulseTitle = item.Subject;
						bool isPulseMember = item.IsPulseMember;
						lst.SelectedItem = null;
						/// Manage New Message Alerts
						List<PulseModel> SavedList = new List<PulseModel>();
						if (!string.IsNullOrEmpty(Settings.AppSettings.GetValueOrDefault("SavedPulseListing", string.Empty)))
						{
							SavedList = JsonConvert.DeserializeObject<List<PulseModel>>(Settings.AppSettings.GetValueOrDefault("SavedPulseListing", string.Empty));
							var matchedPulse = SavedList.Where(x => x.PulseId == item.PulseId).FirstOrDefault();
							if (matchedPulse != null)
							{
								SavedList.Where(w => w.PulseId == item.PulseId).ToList().ForEach(s => s.TotalMessagesCount = item.TotalMessagesCount);
							}
							else
							{
								SavedList.Add(item);
							}
							Settings.AppSettings.AddOrUpdateValue("SavedPulseListing", JsonConvert.SerializeObject(SavedList));
						}
						else
						{
							SavedList.Add(item);
							Settings.AppSettings.AddOrUpdateValue("SavedPulseListing", JsonConvert.SerializeObject(SavedList));
						}
						///
						await Navigation.PushModalAsync(new MessageChatPage(pulseOnwerId, isPulseMember, false));
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
			catch (Exception)
			{
				App.HideMainPageLoader();
				_tapCount = 0;
			}
		}



		async void CreatePulse_Tapped(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					App.ShowMainPageLoader();
					await Navigation.PushModalAsync(new AddPulsePage());
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
		#endregion
	}
}
