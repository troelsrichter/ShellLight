using System;
using System.Globalization;
using System.Windows.Data;
using ShellLight.Contract;

namespace ShellLight.Converters
{
    public class UICommandStateToBoolConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            UICommandState state = (UICommandState)value;
            if (state != null)
            {
                UICommandState compare =
                    (UICommandState) Enum.Parse(typeof (UICommandState), parameter.ToString(), true);
                return compare == state;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}