using System.Threading.Tasks;

namespace Pulse
{
	public class PlacesManager
	{
		#region Private Variable
		readonly IPlacesService placesService;
		#endregion

		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="T:CrossGoogleMap.PlacesManager"/> class.
		/// </summary>
		public PlacesManager()
		{
			placesService = new PlacesService();
		}
		#endregion

		#region  Public Methods

		public Task<PlacesWrapper> GetGooglePlaces(string keyword)
		{
			return placesService.GetGooglePlaces(keyword);
		}

		public Task<PlacesWrapper> GetGooglePlacesDataAsync(string keywords)
		{
			return placesService.GetGooglePlacesDataAsync(keywords);
		}
		public Task<PlaceDetailWrapper> GetPlaceDetail(string placeid)
		{
			return placesService.GetPlaceDetail(placeid);
		}
		#endregion
	}
}
