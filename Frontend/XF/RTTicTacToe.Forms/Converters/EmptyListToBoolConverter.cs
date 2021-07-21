using System;
using System.Collections;
using System.Globalization;
using Xamarin.Forms;

namespace RTTicTacToe.Forms.Converters
{
    public class EmptyListToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enumerable = value as IList;

            if (enumerable == null)
            {
                return true;
            }

            return enumerable.Count == 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
