using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pulse.Helpers
{
    public static class Utils
    {
        public static async Task<Position> GetCurrentLocation()
        {
            IGeolocator locator = null;
            try
            {
                if (CrossGeolocator.Current.IsGeolocationEnabled == true)
                {
                    if (CrossGeolocator.Current != null)
                    {
                        locator = CrossGeolocator.Current;
                        locator.DesiredAccuracy = 500;
                        return await locator.GetPositionAsync(TimeSpan.FromMilliseconds(30000));
                    }
                }
                return await locator.GetPositionAsync(TimeSpan.FromMilliseconds(30000));
            }
            catch (Exception ex)
            {
                return await locator.GetPositionAsync(TimeSpan.FromMilliseconds(30000));
            }
        }
        public static List<string> ReportList()
        {
            List<string> reportCommentList = new List<string>();
            reportCommentList.Add("Bullying/Harassment");
            reportCommentList.Add("False information");
            reportCommentList.Add("Violence or dangerous organizations");
            reportCommentList.Add("Scam or fraud");
            reportCommentList.Add("Intellectual property violation");
            reportCommentList.Add("Sale of illegal or regulated goods");
            return reportCommentList;
        }
    }
}
