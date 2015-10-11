using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Scanner.Core.Converters
{
    public class AddedItemConverter : IValueConverter
    {
        public AddedItemConverter()
        {

        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var args = value as SelectionChangedEventArgs;
            
            if (args != null)
                return args.AddedItems;

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            string language)
        {
            throw new NotImplementedException();
        }
    }
}
