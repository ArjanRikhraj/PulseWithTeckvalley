using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
namespace Pulse
{
    public partial class App : Application
    {
        public static INavigation Navigation { get; set; }
        public static double ScreenHeight { get; set; }
        public static double ScreenWidth { get; set; }
        public static Stream ImageStream { get; set; }
        public static byte[] ImageByteStream { get; set; }
        public static ActivePage CurrentActivePageName { get; set; }
        public static string AndroidAppVersion { get; set; }
        public static string iOSAppVersion { get; set; }
        public static AwsDetail AWSCurrentDetails { get; set; }
        public static byte[] DroidThumbnail { get; set; }
        public static byte[] DroidImageThumbnail { get; set; }
        public static byte[] iOSImageThumbnail { get; set; }
        public static ImageSource iOSThumbnail { get; set; }
        public static bool IsChatOpened { get; set; }
        public static int CurrentChatWindow { get; set; }
        public static string Latitude { get; set; }
        public static string Lognitude { get; set; }
        readonly EventViewModel eventViewModel;
        public static App Instance
        {
            get
            {
                return (App)Current;
            }
        }

        public static string NotificationID { get; set; }
        public static string NotificationTitle { get; set; }

        public App()
        {
            InitializeComponent();
            SetCultureToUSEnglish();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mjk0NDM2QDMxMzgyZTMyMmUzMGpHRjhicm1GVS8zUEJMdkNLak1pM1pTMi84eWZSTHZuZVYwcU91dno3cjQ9");
            eventViewModel = ServiceContainer.Resolve<EventViewModel>();
            MainPage = Settings.IsOnBoardingShown ? new NavigationPage(new LoginPage()) : new NavigationPage(new OnBoardPage());
        }
        private void SetCultureToUSEnglish()
        {
            CultureInfo englishUSCulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = englishUSCulture;
        }
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        public async Task FetchEventDetailMethod(string id)
        {
            eventViewModel.IsLoading = true;
            await eventViewModel.FetchEventDetail(id, true);
        }
        public async Task GetMapEvents()
        {
            try
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await eventViewModel.Navigation.PushModalAsync(new MapEventListPage());
                });
            }
            catch(Exception e)
            {
                
            }

        }

       public Action AlertAction
        {
            get
            {
                return new Action(async () => await App.Navigation.PopAsync(true));
            }
        }

        public static void GetMessageNotification()
        {
            try
            {
                Device.BeginInvokeOnMainThread(HandleAction);
            }
            catch (Exception)
            {
                App.Instance.Alert(Constant.UnableToSyncMessages, Constant.AlertTitle, Constant.Ok);
            }

        }
        public static void GetUnreadNotification()
        {
            try
            {
                MessagingCenter.Send<App>(App.Instance, "GetNotificationCount");
            }
            catch (Exception)
            {
                App.Instance.Alert(Constant.UnableToSyncMessages, Constant.AlertTitle, Constant.Ok);
            }

        }

        static void HandleAction()
        {
            MessagingCenter.Send<App>(App.Instance, "NewMessage");
        }
        static void HandleAction1()
        {
        }

        public async Task ShareEvent()
        {
            eventViewModel.IsLoading = true;
            await eventViewModel.ShareEvent();
        }

        protected override void OnResume()
        {
        }

        public async void AppLinkRequestReceived(Uri uri)
        {
            string appDomain = Device.RuntimePlatform == Device.Android ? "http://" : "pulseapp://";
            if (!uri.ToString().ToLowerInvariant().StartsWith(appDomain, StringComparison.Ordinal))
            {
                return;
            }

            string pageUrl = uri.ToString().Replace(appDomain, string.Empty).Trim();
            var parts = pageUrl.Split('/');
            string page = Device.RuntimePlatform == Device.Android ? "Pulse." + ToPascalCase(parts[1]) : "Pulse." + ToPascalCase(parts[0]);

            var formsPage = Activator.CreateInstance(Type.GetType(page));
            var todoItemPage = formsPage as LoginPage;
            if (todoItemPage != null)
            {
                await MainPage.Navigation.PushAsync(formsPage as Page);
            }

            base.OnAppLinkRequestReceived(uri);
        }

        public string ToPascalCase(string s)
        {
            return !string.IsNullOrEmpty(s) ? s.Substring(0, 1).ToUpperInvariant() + s.Substring(1).ToLowerInvariant() : string.Empty;
        }
        #region Alert, Action alert and Display Alert

        public async Task Alert(string title, string message, string closeButton)
        {
            await MainPage.DisplayAlert(message, title, closeButton);
        }
        public async Task<bool> ConfirmAlert(string title, string message, string okButton, string closeButton)
        {
            var result = await MainPage.DisplayAlert(message, title, okButton, closeButton);
            return result;
        }

        public async Task<bool> ActionAlert(string title, string message, string okButton)
        {
            await MainPage.DisplayAlert(title, message, okButton);
            return true;
        }

        public async Task<string> DisplayActionSheet(string title, string cancelButton, string d, string camera, string gallery)
        {
            return await MainPage.DisplayActionSheet(title, cancelButton, d, camera, gallery);
        }

        #endregion

        public static void ShowMainPageLoader()
        {
            MessagingCenter.Send<App>(App.Instance, "ShowLoader");
        }
        public static void HideMainPageLoader()
        {
            MessagingCenter.Send<App>(App.Instance, "HideLoader");
        }

        public static async Task GetPermission()
        {
            try
            {

                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        await App.Instance.Alert("Need location", Constant.AlertTitle, Constant.Ok);
                        App.HideMainPageLoader();
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    status = results[Permission.Location];
                }

                if (status == PermissionStatus.Granted)
                {
                    //GetCurrentLocation();
                }
                else if (status == PermissionStatus.Unknown)
                {
                    await App.Instance.Alert("Allow location permission access to view events happening around you", Constant.AlertTitle, Constant.Ok);
                    CrossPermissions.Current.OpenAppSettings();
                    App.HideMainPageLoader();
                }
                else if (status != PermissionStatus.Unknown)
                {
                    await App.Instance.Alert("Location Denied! Can not continue, try again.", Constant.AlertTitle, Constant.Ok);
                    App.HideMainPageLoader();

                }
                else
                {
                    await App.Instance.Alert("Please Turn your Location on!.", Constant.AlertTitle, Constant.Ok);
                    App.HideMainPageLoader();
                }
                App.HideMainPageLoader();
            }
            catch (Exception)
            {

                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                App.HideMainPageLoader();
            }
        }
        public static async Task GetStoragePermission()
        {
            try
            {

                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                    {
                        App.HideMainPageLoader();
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
                    status = results[Permission.Storage];
                }
                if (status == PermissionStatus.Granted)
                {
                    //GetCurrentLocation();
                }
                else if (status == PermissionStatus.Unknown)
                {
                    await App.Instance.Alert("Allow storage access to get photos and videos", Constant.AlertTitle, Constant.Ok);
                    CrossPermissions.Current.OpenAppSettings();
                    App.HideMainPageLoader();
                }
                else if (status != PermissionStatus.Unknown)
                {
                    await App.Instance.Alert("Permission Denied! Can not continue, try again.", Constant.AlertTitle, Constant.Ok);
                    App.HideMainPageLoader();

                }
                else
                {
                    App.HideMainPageLoader();
                }
                App.HideMainPageLoader();
            }
            catch (Exception)
            {
                await App.Instance.Alert("Allow storage access to get photos and videos", Constant.AlertTitle, Constant.Ok);
                CrossPermissions.Current.OpenAppSettings();
                App.HideMainPageLoader();
            }
        }
        public static async Task GetCameraPermission()
        {
            try
            {

                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Camera))
                    {
                        App.HideMainPageLoader();
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);
                    status = results[Permission.Camera];
                }
                if (status == PermissionStatus.Granted)
                {
                    //GetCurrentLocation();
                }
                else 
                {
                    await App.Instance.Alert("Allow Camera and storage permission access to take photos and videos", Constant.AlertTitle, Constant.Ok);
                    CrossPermissions.Current.OpenAppSettings();
                    App.HideMainPageLoader();
                }
                //else if (status != PermissionStatus.Unknown)
                //{
                //    await App.Instance.Alert("Permission Denied! Can not continue, try again.", Constant.AlertTitle, Constant.Ok);
                //    App.HideMainPageLoader();

                //}
                //else
                //{
                //   App.HideMainPageLoader();
                //}
                App.HideMainPageLoader();
            }
            catch (Exception)
            {
                await App.Instance.Alert("Allow Camera and storage permission access to take photos and videos", Constant.AlertTitle, Constant.Ok);
                CrossPermissions.Current.OpenAppSettings();
                App.HideMainPageLoader();
            }
        }
    }
}
