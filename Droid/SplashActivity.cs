using Android.App;
using Android.OS;
using Android.Views;
using Android.Content.PM;
using Android.Widget;
using System;
using Android.Content;
using System.Threading.Tasks;
using Android.Gms.Common.Apis;
using Android.Util;
using Android.Gms.Location;
using Firebase;

namespace Pulse.Droid
{
	[Activity(MainLauncher = true, Theme = "@style/NoActionBar", Icon = "@drawable/icon", NoHistory = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
	public class SplashActivity : Android.App.Activity, GoogleApiClient.IConnectionCallbacks,
	GoogleApiClient.IOnConnectionFailedListener, ILocationListener
	{
		RelativeLayout splashLAyout;
		private static int SPLASH_TIME_OUT = 1000;
		protected const string TAG = "location-settings";
		protected const int REQUEST_CHECK_SETTINGS = 0x1;
		public const long UPDATE_INTERVAL_IN_MILLISECONDS = 10000;
		public const long FASTEST_UPDATE_INTERVAL_IN_MILLISECONDS = UPDATE_INTERVAL_IN_MILLISECONDS / 2;
		protected const string KEY_REQUESTING_LOCATION_UPDATES = "requesting-location-updates";
		protected const string KEY_LOCATION = "location";
		protected const string KEY_LAST_UPDATED_TIME_STRING = "last-updated-time-string";
		protected bool mRequestingLocationUpdates;
		protected string mLastUpdateTime;
		protected GoogleApiClient mGoogleApiClient;
		protected LocationRequest mLocationRequest;
		protected LocationSettingsRequest mLocationSettingsRequest;
		protected Android.Locations.Location mCurrentLocation;
		protected override async void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.SplashScreen);
			BuildGoogleApiClient();
			StartFCMService();
			mRequestingLocationUpdates = false;
			mLastUpdateTime = "";
			CreateLocationRequest();
			BuildLocationSettingsRequest();
			//if (Android.OS.Build.VERSION.SdkInt >= Android.OS.Build.VERSION_CODES.M)
			//{
			//	await CheckLocationSettings().ConfigureAwait(false);
			//}
			//else
			//{
				splashLAyout = FindViewById<RelativeLayout>(Resource.Id.SplashLayout);
				Handler handler = new Handler();
				handler.PostDelayed(CallMainActivity, SPLASH_TIME_OUT);
			//}
		}
		public void CallMainActivity()
		{
			StartActivity(typeof(MainActivity));
			this.Finish();
		}
		public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
		{
			if (newConfig.Orientation == Android.Content.Res.Orientation.Portrait)
			{
				splashLAyout.SetBackgroundResource(Resource.Drawable.splash);
			}
			base.OnConfigurationChanged(newConfig);
		}
		public void StartFCMService()
		{
            try
            {
				FirebaseApp.InitializeApp(this);
				StartService(new Intent(this, typeof(FirebaseIdService)));
				StartService(new Intent(this, typeof(FcmMessagingService)));
			}
            catch (Exception ex)
            {

                return;
            }
		
		}


		protected async Task CheckLocationSettings()
		{
			var result = await LocationServices.SettingsApi.CheckLocationSettingsAsync(
				mGoogleApiClient, mLocationSettingsRequest);
			await HandleResult(result).ConfigureAwait(false);
		}

		protected void BuildGoogleApiClient()
		{

			Log.Info(TAG, "Building GoogleApiClient");
			mGoogleApiClient = new GoogleApiClient.Builder(this)
				.AddConnectionCallbacks(this)
				.AddOnConnectionFailedListener(this)
				.AddApi(LocationServices.API)
				.Build();
		}

		protected void CreateLocationRequest()
		{
			mLocationRequest = new LocationRequest();
			mLocationRequest.SetInterval(UPDATE_INTERVAL_IN_MILLISECONDS);
			mLocationRequest.SetFastestInterval(FASTEST_UPDATE_INTERVAL_IN_MILLISECONDS);
            mLocationRequest.SetPriority(LocationRequest.PriorityHighAccuracy);
		}

		protected void BuildLocationSettingsRequest()
		{
			LocationSettingsRequest.Builder builder = new LocationSettingsRequest.Builder();
			builder.AddLocationRequest(mLocationRequest);
			mLocationSettingsRequest = builder.Build();
		}


		public async Task HandleResult(LocationSettingsResult locationSettingsResult)
		{
			var status = locationSettingsResult.Status;
			switch (status.StatusCode)
			{
				case CommonStatusCodes.Success:
					Log.Info(TAG, "All location settings are satisfied.");
					Handler handler = new Handler();
					handler.PostDelayed(CallMainActivity, SPLASH_TIME_OUT);
                   // await App.GetPermission();
                    try
                    {
                        await StartLocationUpdates().ConfigureAwait(false);
                    }
                    catch(Exception)
                    {
                        
                    }
					break;
				case CommonStatusCodes.ResolutionRequired:
					Log.Info(TAG, "Location settings are not satisfied. Show the user a dialog to" +
					"upgrade location settings ");
					try
					{
						status.StartResolutionForResult(this, REQUEST_CHECK_SETTINGS);
					}
					catch (IntentSender.SendIntentException)
					{
						Log.Info(TAG, "PendingIntent unable to execute request.");
					}
					catch (Exception e)
					{
						Log.Info(TAG, "PendingIntent unable to execute request.");
					}
					break;
				case LocationSettingsStatusCodes.SettingsChangeUnavailable:
					Log.Info(TAG, "Location settings are inadequate, and cannot be fixed here. Dialog " +
					"not created.");
					break;
				default:
					Log.Info(TAG, "Something went wrong.");
					break;
			}
		}


		protected override async void OnActivityResult(int requestCode, Android.App.Result resultCode, Intent data)
		{
			if (requestCode == REQUEST_CHECK_SETTINGS)
			{
				switch (resultCode)
				{
					case Android.App.Result.Ok:
						Log.Info(TAG, "User agreed to make required location settings changes.");
						Handler handler = new Handler();
						handler.PostDelayed(CallMainActivity, SPLASH_TIME_OUT);
                        await App.GetPermission();
                        try
                        {
                            await StartLocationUpdates().ConfigureAwait(false);
                        }
                        catch (Exception)
                        {

                        }
						break;
					case Android.App.Result.Canceled:
						Log.Info(TAG, "User chose not to make required location settings changes.");
						Handler handler1 = new Handler();
						handler1.PostDelayed(CallMainActivity, SPLASH_TIME_OUT);
						break;
					default:
						Log.Info(TAG, "PendingIntent unable to execute request.");
						break;
				}
			}
			else
			{
				return;
			}
		}


		protected async Task StartLocationUpdates()
		{
			await LocationServices.FusedLocationApi.RequestLocationUpdates(
				mGoogleApiClient,
				mLocationRequest,
				this
			);

			mRequestingLocationUpdates = true;
		}

		protected async Task StopLocationUpdates()
		{
			await LocationServices.FusedLocationApi.RemoveLocationUpdates(
					mGoogleApiClient,
					this
				);

			mRequestingLocationUpdates = false;
		}
		protected override void OnStart()
		{
			base.OnStart();
			mGoogleApiClient.Connect();
		}

		protected override async void OnResume()
		{
			base.OnResume();
			if (mGoogleApiClient.IsConnected && mRequestingLocationUpdates)
			{
				await StartLocationUpdates().ConfigureAwait(false);
			}
		}

		protected override async void OnPause()
		{
			base.OnPause();
			if (mGoogleApiClient.IsConnected)
			{
				await StopLocationUpdates().ConfigureAwait(false);
			}
		}

		protected override void OnStop()
		{
			base.OnStop();
			mGoogleApiClient.Disconnect();
		}

		public void OnConnected(Bundle connectionHint)
		{
			Log.Info(TAG, "Connected to GoogleApiClient");

			if (mCurrentLocation == null)
			{
				mCurrentLocation = LocationServices.FusedLocationApi.GetLastLocation(mGoogleApiClient);
				mLastUpdateTime = DateTime.Now.TimeOfDay.ToString();
			}
		}

		public void OnConnectionSuspended(int cause)
		{
			Log.Info(TAG, "Connection suspended");
		}

		public void OnConnectionFailed(Android.Gms.Common.ConnectionResult result)
		{
			Log.Info(TAG, "Connection failed: ConnectionResult.getErrorCode() = " + result.ErrorCode);
		}

		public void OnLocationChanged(Android.Locations.Location location)
		{
			mCurrentLocation = location;
			mLastUpdateTime = DateTime.Now.TimeOfDay.ToString();
		}

		protected override void OnSaveInstanceState(Bundle outState)
		{
			outState.PutBoolean(KEY_REQUESTING_LOCATION_UPDATES, mRequestingLocationUpdates);
			outState.PutParcelable(KEY_LOCATION, mCurrentLocation);
			outState.PutString(KEY_LAST_UPDATED_TIME_STRING, mLastUpdateTime);
			base.OnSaveInstanceState(outState);
		}

         public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [Android.Runtime.GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {

            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

	}
}
