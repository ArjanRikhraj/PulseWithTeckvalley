using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Widget;
using Android.OS;
using CarouselView.FormsPlugin.Android;
using ImageCircle.Forms.Plugin.Droid;
using Android.Graphics;
using Xamarin.Forms;
using System.Threading.Tasks;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using MediaManager;
using Plugin.Messaging;
using FFImageLoading.Forms.Platform;
using Matcha.BackgroundService.Droid;

namespace Pulse.Droid
{
	[Activity(ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
	[
		IntentFilter
		(
			actions: new[] { Intent.ActionView },
			Categories = new[]
					{
						Intent.CategoryDefault,
						Intent.CategoryBrowsable
					},
			DataSchemes = new[]
					{
						"http"
					},
			DataHost = Constant.DataHostName
		)
	]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		readonly long doublePressInterval_ms = 1000;
		DateTime lastPressTime = DateTime.Now;
		public static Context APPLICATION_CONTEXT;
		protected override void OnCreate(Bundle bundle)
		{
            Current = this;
			BackgroundAggregator.Init(this);
			base.OnCreate(bundle);
			Xamarin.Essentials.Platform.Init(this, bundle);
			APPLICATION_CONTEXT = this;
			App.NotificationID = Intent.GetStringExtra("data") ?? string.Empty;
			ServiceRegistrar.Startup();
			global::Xamarin.Forms.Forms.Init(this, bundle);
			global::Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, bundle);
			global::Xamarin.Auth.CustomTabsConfiguration.ActionLabel = null;
			global::Xamarin.Auth.CustomTabsConfiguration.MenuItemTitle = null;
			global::Xamarin.Auth.CustomTabsConfiguration.AreAnimationsUsed = true;
			global::Xamarin.Auth.CustomTabsConfiguration.IsShowTitleUsed = false;
			global::Xamarin.Auth.CustomTabsConfiguration.IsUrlBarHidingUsed = false;
			global::Xamarin.Auth.CustomTabsConfiguration.IsCloseButtonIconUsed = false;
			global::Xamarin.Auth.CustomTabsConfiguration.IsActionButtonUsed = false;
			global::Xamarin.Auth.CustomTabsConfiguration.IsActionBarToolbarIconUsed = false;
			global::Xamarin.Auth.CustomTabsConfiguration.IsDefaultShareMenuItemUsed = false;
			global::Xamarin.Auth.CustomTabsConfiguration.CustomTabsClosingMessage = null;

			global::Android.Graphics.Color color_xamarin_blue;
			color_xamarin_blue = new global::Android.Graphics.Color(0x34, 0x98, 0xdb);
			global::Xamarin.Auth.CustomTabsConfiguration.ToolbarColor = color_xamarin_blue;

			global::Xamarin.Auth.CustomTabsConfiguration.
				  ActivityFlags =
					   global::Android.Content.ActivityFlags.NoHistory
					   |
					   global::Android.Content.ActivityFlags.SingleTop
					   |
					   global::Android.Content.ActivityFlags.NewTask
					   ;

			Context context = this.ApplicationContext;

			App.AndroidAppVersion = context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName;
			/// <summary>
			/// Get device size.
			/// </summary>
			App.ScreenHeight = Resources.Configuration.ScreenHeightDp;
			App.ScreenWidth = Resources.Configuration.ScreenWidthDp;
			CarouselViewRenderer.Init();
			ImageCircleRenderer.Init();
			CachedImageRenderer.Init(true);
			CrossMediaManager.Current.Init(this);
			Window.SetSoftInputMode(Android.Views.SoftInput.AdjustResize);
			OpenSettings();
			ZXing.Net.Mobile.Forms.Android.Platform.Init();
			MessagingCenter.Subscribe<string>(this, "Share", Share, null);
			MessagingCenter.Subscribe<string>(this, "ShowToast", ShowToast, null);
			LoadApplication(new App());

			App myApp = App.Current as App;
			if (null != myApp && null != Intent.DataString)
			{
				myApp.AppLinkRequestReceived(new Uri(Intent.DataString));
			}


		}
        public static MainActivity Current { private set; get; }

        public static readonly int PickImageId = 1000;

        public TaskCompletionSource<string> PickImageTaskCompletionSource { set; get; }
		
        protected override void OnActivityResult(int requestCode, Android.App.Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == PickImageId)
            {
                if ((resultCode == Android.App.Result.Ok) && (data != null))
                {
                    // Set the filename as the completion of the Task
                    PickImageTaskCompletionSource.SetResult(data.DataString);
                }
                else
                {
                    PickImageTaskCompletionSource.SetResult(null);
                }
            }
        }
		public override void OnBackPressed()
		{
			DateTime pressTime = DateTime.Now;
			if ((pressTime - lastPressTime).TotalMilliseconds <= doublePressInterval_ms)
			{
				Java.Lang.JavaSystem.Exit(0);
			}
			lastPressTime = pressTime;
			Toast.MakeText(this, "Click back to exit.", ToastLength.Short).Show();
		}

		async void ShowToast(string text)
		{
			bool IsLengthShort = false;
			Handler mainHandler = new Handler(Looper.MainLooper);
			Java.Lang.Runnable runnableToast = new Java.Lang.Runnable(() =>
			{
				var duration = IsLengthShort ? ToastLength.Short : ToastLength.Long;
				Toast.MakeText(Forms.Context, text, duration).Show();
			});

			mainHandler.Post(runnableToast);
		}

		async void Share(string qrData)
		{
			try
			{
				var barcodeWriter = new ZXing.Mobile.BarcodeWriter
				{
					Format = ZXing.BarcodeFormat.QR_CODE,
					Options = new ZXing.Common.EncodingOptions
					{
						Width = 300,
						Height = 300
					}
				};
				var bitmap = barcodeWriter.Write(qrData);

				var intent = new Intent(Intent.ActionSend);
				intent.SetType("image/png");
				var path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads
					+ Java.IO.File.Separator + "logo.png");

				using (var os = new System.IO.FileStream(path.AbsolutePath, System.IO.FileMode.Create))
				{
					bitmap.Compress(Bitmap.CompressFormat.Png, 100, os);
				}

				intent.PutExtra(Intent.ExtraStream, Android.Net.Uri.FromFile(path));
				var intentChooser = Intent.CreateChooser(intent, "Share via");
				StartActivityForResult(intentChooser, 89999);
			}
			catch (Exception e)
			{
				await App.Instance.Alert("Problem in sharing QR code in this device", Constant.AlertTitle, Constant.Ok);
			}
		}
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [Android.Runtime.GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
			Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
			Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
		public async Task OpenSettings()
		{
			Int64
	interval = 1000 * 60 * 1,
	fastestInterval = 1000 * 50;

			try
			{
				GoogleApiClient
					googleApiClient = new GoogleApiClient.Builder(Forms.Context)
						.AddApi(LocationServices.API)
						.Build();

				googleApiClient.Connect();

				LocationRequest
					locationRequest = LocationRequest.Create()
						.SetPriority(LocationRequest.PriorityBalancedPowerAccuracy)
						.SetInterval(interval)
						.SetFastestInterval(fastestInterval);

				LocationSettingsRequest.Builder
					locationSettingsRequestBuilder = new LocationSettingsRequest.Builder()
						.AddLocationRequest(locationRequest);

				locationSettingsRequestBuilder.SetAlwaysShow(false);

				LocationSettingsResult
					locationSettingsResult = await LocationServices.SettingsApi.CheckLocationSettingsAsync(
						googleApiClient, locationSettingsRequestBuilder.Build());
				if (locationSettingsResult.Status.StatusCode == LocationSettingsStatusCodes.ResolutionRequired)
				{
					locationSettingsResult.Status.StartResolutionForResult((Activity)Forms.Context, 0);
				}
			}
			catch (Exception exception)
			{
				return;
			}
		}
	}
}