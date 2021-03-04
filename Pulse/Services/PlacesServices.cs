using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Pulse
{
	public class PlacesService : IPlacesService
	{
		#region Private Variables
		readonly HttpClient client;
		#endregion

		#region Constructor
		public PlacesService()
		{
			client = new HttpClient();
			client.MaxResponseContentBufferSize = Constant.MaxResponseContentBufferSize;
		}

		/// <summary>
		/// Exceptions the message.
		/// </summary>
		/// <returns>The message.</returns>
		/// <param name="error">The exception while crashing.</param>
		void ExceptionMessage(Exception error)
		{

		}
		#endregion

		#region Public Methods
		public async Task<PlacesWrapper> GetGooglePlaces(string keyword)
		{
			PlacesWrapper placesWrapper = new PlacesWrapper();

			var uri = new Uri(string.Format(Constant.GooglePlacesURL, keyword, Constant.GoogleSearchType, Constant.GoogleSearchLanguage, Constant.GoogleApiKey));

			try
			{
				var response = await client.GetAsync(uri);
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					placesWrapper = JsonConvert.DeserializeObject<PlacesWrapper>(content);

				}
			}
			catch (Exception ex)
			{
				ExceptionMessage(ex);
			}

			return placesWrapper;
		}

		public async Task<PlaceDetailWrapper> GetPlaceDetail(string placeid)
		{
			PlaceDetailWrapper placeDetailWrapper = new PlaceDetailWrapper();

			var uri = new Uri(string.Format(Constant.GooglePlaceDetailURL, placeid, Constant.GoogleApiKey));

			try
			{
				var response = await client.GetAsync(uri);
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					placeDetailWrapper = JsonConvert.DeserializeObject<PlaceDetailWrapper>(content);

				}
			}
			catch (Exception ex)
			{
				ExceptionMessage(ex);
			}

			return placeDetailWrapper;
		}

		public async Task<PlacesWrapper> GetGooglePlacesDataAsync(string keyword)
		{
			var places = new PlacesWrapper();
			var uri = new Uri(string.Format(Constant.GoogleSetDestinationURL, keyword, Constant.GoogleSearchLanguage, Constant.GoogleApiKey));

			try
			{
				var response = await client.GetAsync(uri);
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					places = JsonConvert.DeserializeObject<PlacesWrapper>(content);
				}
			}
			catch (Exception ex)
			{
				ExceptionMessage(ex);
			}

			return places;
		}

		public async Task<GeocodeDetailWrapper> GoogleGeocodeDataAsync(string lat, string lng)
		{
			var places = new GeocodeDetailWrapper();
			var googleApiKey = "";

			if (Device.RuntimePlatform == Device.iOS)
				googleApiKey = Constant.GOOGLE_MAPS_IOS_API_KEY;
			else
				googleApiKey = Constant.GOOGLE_MAPS_ANDROID_API_KEY;
			var uri = new Uri(string.Format(Constant.GoogleGeocodeRequestURL, lat, lng, googleApiKey));

			try
			{
				var response = await client.GetAsync(uri);
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					places = JsonConvert.DeserializeObject<GeocodeDetailWrapper>(content);
				}
			}
			catch (Exception)
			{
				return places;
			}
			return places;
		}

		#endregion
	}
}
