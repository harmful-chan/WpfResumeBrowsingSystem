using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace WpfResumeBrowsingSystem.Controls
{
    /// <summary>
    /// ExperienceUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class ExperienceUserControl : UserControl
    {
        public ExperienceUserControl()
        {
            InitializeComponent();
        }


    }

    public class ObjectToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DateConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is DateTime && values[1] is DateTime)
            {
                return ((DateTime)values[0]).ToString("yyyy/MM/dd") + " ~ " + ((DateTime)values[1]).ToString("yyyy/MM/dd");
            }
            else
            {
                return null;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
