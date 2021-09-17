using System;
using System.Globalization;
using Xamarin.Forms;

namespace Pulse.Helpers.Converters.Friends
{
    public class FriendsEventsListTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            return DateTime.Today.ToString("HH: mm tt");
            var item = DateTime.Parse(value.ToString());
            return item.ToString("HH: mm tt");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
