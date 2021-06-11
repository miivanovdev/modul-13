using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Модуль_13_ДЗ
{
    public class VisibilitySetter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter.ToString() == "True")  //Parameter is set in the xaml file.
            {
                return SetVisibilityBasedOn(value);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private object SetVisibilityBasedOn(object value)
        {
            if (value is Boolean obj && obj.Equals(false)) //Checks the value of the object
            {
                return Visibility.Collapsed;  //Hides the row. It Returns visibility based on the value of the row.
            }

            return Visibility.Visible;
        }
    }
}
