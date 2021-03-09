using System;
using System.Threading.Tasks;
using Android.App;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using Pulse.Droid.Shared_Services;
using Pulse.Interfaces;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(LocationZ))]
namespace Pulse.Droid.Shared_Services
{
    public class LocationZ : ILocSettings
    {
        Int64
   interval = 1000 * 60 * 1,
   fastestInterval = 1000 * 50;
        [Obsolete]
        public async Task IsLocation()
        {
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
            }
            catch (Exception ex)
            {
                return;
            }
        }
    }
}