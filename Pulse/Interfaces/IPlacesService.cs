using System;
using System.Threading.Tasks;

namespace Pulse
{
	public interface IPlacesService
	{
		Task<PlacesWrapper> GetGooglePlaces(string keyword);
		Task<PlaceDetailWrapper> GetPlaceDetail(string placeid);
		Task<PlacesWrapper> GetGooglePlacesDataAsync(string keywords);
		Task<GeocodeDetailWrapper> GoogleGeocodeDataAsync(string lat, string lng);
	}
}
