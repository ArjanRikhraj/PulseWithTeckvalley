using System;
using CarouselView.FormsPlugin.iOS;
using Firebase.CloudMessaging;
using Foundation;
using ImageCircle.Forms.Plugin.iOS;
using Plugin.Media;
using Plugin.Toasts;
using UIKit;
using UserNotifications;
using Xamarin.Forms;
using Pulse;
using MediaManager;
using FFImageLoading.Forms.Platform;
using Matcha.BackgroundService.iOS;

namespace Pulse.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IUNUserNotificationCenterDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            ServiceRegistrar.Startup();
            global::Xamarin.Forms.Forms.Init();
            global::Xamarin.Auth.Presenters.XamarinIOS.AuthenticationConfiguration.Init();
            global::Xamarin.Auth.WebViewConfiguration.IOS.UserAgent = "moljac++";
            NSObject ver = NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"];
            App.iOSAppVersion = ver.Description;
            App.ScreenHeight = (double)UIScreen.MainScreen.Bounds.Height;
            App.ScreenWidth = (double)UIScreen.MainScreen.Bounds.Width;
            KeyboardOverlap.Forms.Plugin.iOSUnified.KeyboardOverlapRenderer.Init();
            DependencyService.Register<ToastNotification>(); // Register your dependency
            ToastNotification.Init();
            CarouselViewRenderer.Init();
            BackgroundAggregator.Init(this);
            CachedImageRenderer.Init();
            CrossMediaManager.Current.Init();
            CrossMedia.Current.Initialize();
            ImageCircleRenderer.Init();
            ZXing.Net.Mobile.Forms.iOS.Platform.Init();
            app.ApplicationIconBadgeNumber = 0;
            UIApplication.CheckForEventAndDelegateMismatches = false;
            LoadApplication(new App());
            UIApplication.SharedApplication.RegisterForRemoteNotifications();
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // iOS 10
                var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
                UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) =>
                {
                    Console.WriteLine(granted);

                });
                //For iOS 10 display notification (sent via APNS)
                UNUserNotificationCenter.Current.Delegate = this;
            }
            else
            {
                var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
                var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }

            // Firebase component initialize
            Firebase.Analytics.App.Configure();
            Firebase.InstanceID.InstanceId.Notifications.ObserveTokenRefresh((sender, e) =>
            {
                var newToken = Firebase.InstanceID.InstanceId.SharedInstance.Token;
                if (!string.IsNullOrEmpty(newToken))
                {
                    var refreshedToken = newToken.Replace('"', ' ').Trim();
                    Settings.AppSettings.AddOrUpdateValue(Constant.FcmToken, refreshedToken);
                }
            });
            return base.FinishedLaunching(app, options);
        }

        public override bool HandleOpenURL(UIApplication application, NSUrl url)
        {
            App myApp = App.Current as App;
            if (null != myApp && null != url)
            {
                myApp.AppLinkRequestReceived(url);
            }

            return false;
        }


        /// <summary>
        /// When app comes to foreground
        /// </summary>
        /// <param name="uiApplication">User interface application.</param>
        public override void OnActivated(UIApplication uiApplication)
        {
            base.OnActivated(uiApplication);
        }
        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            #if DEBUG
            Firebase.InstanceID.InstanceId.SharedInstance.SetApnsToken(deviceToken, Firebase.InstanceID.ApnsTokenType.Sandbox);
            #endif
            #if RELEASE
            Firebase.InstanceID.InstanceId.SharedInstance.SetApnsToken(deviceToken, Firebase.InstanceID.ApnsTokenType.Prod);
            #endif
        }

        // iOS 9 <=, fire when recieve notification background
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary message, Action<UIBackgroundFetchResult> completionHandler)
        {
            NSString EventTitle = (NSString)message["title"];
            Messaging.SharedInstance.AppDidReceiveMessage(message);
            NSString EventIds = (NSString)message["id"];
            App.GetUnreadNotification();
            if (application.ApplicationState == UIApplicationState.Background || application.ApplicationState == UIApplicationState.Inactive)
            {
                App.NotificationID = Convert.ToString(EventIds) ?? string.Empty;
                App.NotificationTitle = Convert.ToString(EventTitle) ?? string.Empty;
                App.Current.MainPage = new NavigationPage(new LoginPage());
            }
           

        }
        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            base.ReceivedRemoteNotification(application, userInfo);
            Messaging.SharedInstance.AppDidReceiveMessage(userInfo);
            NSString EventIds = (NSString)userInfo["id"];
            NSString EventTitle = (NSString)userInfo["title"];
            App.GetUnreadNotification();
            if (application.ApplicationState == UIApplicationState.Background || application.ApplicationState == UIApplicationState.Inactive)
            {
                App.NotificationID = Convert.ToString(EventIds) ?? string.Empty;
                App.NotificationTitle = Convert.ToString(EventTitle) ?? string.Empty;
                App.Current.MainPage = new NavigationPage(new LoginPage());
            }
        }
        [Export("userNotificationCenter:willPresentNotification:withCompletionHandler:")]
        public void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            var title = notification.Request.Content.Title;
            var body = notification.Request.Content.Body;
            var EventIds = notification.Request.Content.UserInfo["id"];
            var EventTitle = notification.Request.Content.UserInfo["title"];
            App.NotificationID = Convert.ToString(EventIds) ?? string.Empty;
            App.NotificationTitle = Convert.ToString(EventTitle) ?? string.Empty;
            App.GetUnreadNotification();
            if (App.NotificationTitle == "Pulse Message")
            {
                if (!string.IsNullOrEmpty(App.NotificationID))
                {
                    if (App.IsChatOpened && App.CurrentChatWindow == Convert.ToInt32(App.NotificationID))
                    {
                        App.GetMessageNotification();
                    }
                }
            }
            else
            {
                debugAlert(title, body);
            }
        }
        private void debugAlert(string title, string message)
        {
            var alert = new UIAlertView(title ?? "Pulse", message ?? "Pulse", null, "Cancel", "OK");

            alert.Clicked += (sender, e) =>
            {
                Settings.AppSettings.AddOrUpdateValue("NewNotification", "True");
            };
            alert.Show();
        }

        //[Export("messaging:didReceiveMessage:")]
        //public void DidReceiveMessage(Messaging messaging, RemoteMessage remoteMessage)
        //{
        //    // Handle Data messages for iOS 10 and above.
        //    HandleMessage(remoteMessage.AppData);
        //}
        void HandleMessage(NSDictionary userInfo)
        {
            NSDictionary data = (NSDictionary)userInfo["notification"];
            NSString body = (NSString)data["body"];
            NSString title = (NSString)data["title"];
            debugAlert(title, body);
        }       
    }
}

