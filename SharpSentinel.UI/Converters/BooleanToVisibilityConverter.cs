using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SharpSentinel.UI.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public bool Inverse { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = default(bool);

            if (value == null)
            {

            }
            else
            {
                if (value is bool == false)
                    throw new ArgumentException();
                
                result = (bool)value;
            }

            if (this.Inverse)
                result = !result;

            return result ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}