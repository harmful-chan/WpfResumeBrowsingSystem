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

    public class ObjectToString : IValueConverter
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
}
