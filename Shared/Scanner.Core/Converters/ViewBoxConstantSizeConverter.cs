using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Scanner.Core.Converters
{
    public class ViewBoxConstantSizeConverter : IValueConverter
    {
        public ViewBoxConstantSizeConverter()
        {

        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is double)) return null;
            double d = (double)value;
            return d / 100 * 3;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            string language)
        {
            throw new NotImplementedException();
        }
    }
}
