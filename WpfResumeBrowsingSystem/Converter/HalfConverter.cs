using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfResumeBrowsingSystem.Converter
{
    class HalfConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double)
            {
                double i = (double)value / 2;
                return i ;
            }
            else if (value is string)
            {
                if ("" == (string)value) return string.Format("0");
                else return (double.Parse((string)value) / 2).ToString();
            }
            else return null;
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}