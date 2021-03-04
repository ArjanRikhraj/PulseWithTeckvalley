using System;
using System.Collections.Generic;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Pulse
{
    public partial class NotificationView : ContentView
    {
        readonly EventViewModel eventViewModel;
        readonly FriendsViewModel friendsViewModel;
        private int _tapCount;
         #region constructor
        public NotificationView()
        {
            InitializeComponent();
            eventViewModel = ServiceContainer.Resolve<EventViewModel>();
            friendsViewModel = ServiceContainer.Resolve<FriendsViewModel>();
            BindingContext = eventViewModel;
            SetIntialValues();
        }
        #endregion
        void SetIntialValues()
        {
            eventViewModel.tmpNotificationList.Clear();
            eventViewModel.totalNotificationPages = 1;
            eventViewModel.pageNoNotification = 1;
            eventViewModel.GetNotificationList();
        }
        async void lstNotificationTapped(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (_tapCount < 1)
                {
                    _tapCount = 1;
                    App.ShowMainPageLoader();
                    var selected = (NotificationResponse)e.SelectedItem;
                    if (selected != null)
                    {
                        lstNotifications.SelectedItem = null;
                        if (selected.title.Contains("Pulse"))
                        {
                            Application.Current.MainPage = new MainPage(ActivePage.Pulse);
                        }
                        else  if(selected.title.Contains("Friend Request"))
                        {
                            if (selected.extra_data != null && selected.extra_data.id > Constant.StatusZero)
                            {
                                await Navigation.PushModalAsync(new FriendsProfilePage("My Friends",selected.extra_data.id.ToString()));
                            }
                        }
                        else if(selected.title.Contains("Friend"))
                        {
                            Application.Current.MainPage = new MainPage(ActivePage.Friends);
                        }
                        else if (selected.title.Contains("Event Cancel"))
                        {
                            
                        }
                        else 
                        {
                            if(selected.extra_data!=null && selected.extra_data.id > Constant.StatusZero)
                            {
                                await eventViewModel.FetchEventDetail(selected.extra_data.id.ToString(), true);
                            }
                        }
                    }

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
