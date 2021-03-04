using System;
using System.Collections.Generic;

namespace Pulse
{
	#region  Model for search location and location description from google API
	/// <summary>
	/// Places model.
	/// </summary>
	public class PlacesModel
	{
		public string Name { get; set; }
		public string FullName { get; set; }
		public string Description { get; set; }
		public string Vicinity { get; set; }
		public PlaceGeometry Geometry { get; set; }
	}

	/// <summary>
	/// Place geometry.
	/// </summary>
	public class PlaceGeometry
	{
		public PlaceLocation Location { get; set; }
	}

	/// <summary>
	/// Place location.
	/// </summary>
	public class PlaceLocation
	{
		public double Lat { get; set; }
		public double Lng { get; set; }
	}

	/// <summary>
	/// Search result model.
	/// </summary>
	public class SearchResultModel
	{
		public string Name { get; set; }
		public string Country { get; set; }
		public string Vicinity { get; set; }
		public string place_id { get; set; }
		public PlaceGeometry Geometry { get; set; }
	}

	/// <summary>
	/// Matched substring.
	/// </summary>
	public class MatchedSubstring
	{
		public int length { get; set; }
		public int offset { get; set; }
	}

	/// <summary>
	/// Term.
	/// </summary>
	public class Term
	{
		public int offset { get; set; }
		public string value { get; set; }
	}

	/// <summary>
	/// Prediction.
	/// </summary>
	public class Prediction
	{
		public string id { get; set; }
		public string place_id { get; set; }
		public string reference { get; set; }
		public List<Term> terms { get; set; }
		public List<string> types { get; set; }
		public string description { get; set; }
		public List<MatchedSubstring> matched_substrings { get; set; }
	}

	/// <summary>
	/// Places wrapper.
	/// </summary>
	public class PlacesWrapper
	{
		public List<Prediction> predictions { get; set; }
		public string status { get; set; }
	}

	/// <summary>
	/// Google search result.
	/// </summary>
	public class GoogleSearchResult
	{
		public string Name { get; set; }
		public string PlaceID { get; set; }
	}

	/// <summary>
	/// Place result wrapper.
	/// </summary>
	public class PlaceResultWrapper
	{
		public int count { get; set; }
		public string next { get; set; }
		public string previous { get; set; }
		public object results { get; set; }
		public object predictions { get; set; }
	}

	/// <summary>
	/// Address component.
	/// </summary>
	public class AddressComponent
	{
		public string long_name { get; set; }
		public string short_name { get; set; }
		public List<string> types { get; set; }
	}

	/// <summary>
	/// Location.
	/// </summary>
	public class Location
	{
		public double lat { get; set; }
		public double lng { get; set; }
	}

	/// <summary>
	/// Northeast.
	/// </summary>
	public class Northeast
	{
		public double lat { get; set; }
		public double lng { get; set; }
	}

	/// <summary>
	/// Southwest.
	/// </summary>
	public class Southwest
	{
		public double lat { get; set; }

		public double lng { get; set; }
	}

	/// <summary>
	/// Viewport.
	/// </summary>
	public class Viewport
	{
		public Northeast northeast { get; set; }
		public Southwest southwest { get; set; }
	}

	/// <summary>
	/// Geometry.
	/// </summary>
	public class Geometry
	{
		public Location location { get; set; }
		public string location_type { get; set; }
		public Viewport viewport { get; set; }
	}

	/// <summary>
	/// Result.
	/// </summary>
	public class Result
	{
		public List<AddressComponent> address_components { get; set; }
		public string adr_address { get; set; }
		public string formatted_address { get; set; }
		public Geometry geometry { get; set; }
		public string icon { get; set; }
		public string id { get; set; }
		public string name { get; set; }
		public string place_id { get; set; }
		public string reference { get; set; }
		public string scope { get; set; }
		public List<string> types { get; set; }
		public string url { get; set; }
		public string vicinity { get; set; }
	}

	/// <summary>
	/// Place detail wrapper.
	/// </summary>
	public class PlaceDetailWrapper
	{
		public List<object> html_attributions { get; set; }
		public Result result { get; set; }
		public string status { get; set; }
	}

	/// <summary>
	/// Google place detail result.
	/// </summary>
	public class GooglePlaceDetailResult
	{
		public string Latitude { get; set; }
		public string Longitude { get; set; }
	}

	/// <summary>
	/// Geocode detail wrapper.
	/// </summary>
	public class GeocodeDetailWrapper
	{
		public List<Result> results { get; set; }

		public string status { get; set; }
	}

	public class TransitElement
	{
		public string status { get; set; }
	}

	public class TransitRow
	{
		public List<TransitElement> elements { get; set; }
	}

	public class TransitWrapper
	{
		public List<string> destination_addresses { get; set; }
		public List<string> origin_addresses { get; set; }
		public List<TransitRow> rows { get; set; }
		public string status { get; set; }
	}
	#endregion
}
