using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfResumeBrowsingSystem.Converter
{
    class ReduceConverter : IValueConverter
    {
        public double Reduce { get; set; }
        public ReduceConverter()
        {
            this.Reduce = 0;
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((double)value) - Reduce;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
