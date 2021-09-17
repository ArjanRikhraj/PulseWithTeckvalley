using System;
using System.Globalization;
using Xamarin.Forms;

namespace Pulse.Helpers.Converters.Friends
{
    public class FriendsEventsListDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            return DateTime.Today.ToString("MMM dd, yyyy");
            var item = DateTime.Parse(value.ToString());
            return item.ToString("MMM dd, yyyy");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
