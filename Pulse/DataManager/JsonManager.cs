using System;
using Newtonsoft.Json;

namespace Pulse
{
	public static class JsonManager
	{
		#region Static Methods

		public static string Serialize<T>(T obj)
		{
			return JsonConvert.SerializeObject(obj);
		}

		public static T DeSerialize<T>(string value)
		{
			return JsonConvert.DeserializeObject<T>(value);
		}

		#endregion  Static Methods
	}
}
