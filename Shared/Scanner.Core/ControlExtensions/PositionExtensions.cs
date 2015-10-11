using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace Scanner.Core.ControlExtensions
{
    public class PositionExtensions : DependencyObject
    {
        public static Point GetCurrentPoint(DependencyObject obj)
        {
            return (Point)obj.GetValue(CurrentPointProperty);
        }

        public static void SetCurrentPoint(DependencyObject obj, Point value)
        {
            obj.SetValue(CurrentPointProperty, value);
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentPointProperty =
            DependencyProperty.RegisterAttached("CurrentPoint", 
            typeof(Point), 
            typeof(PositionExtensions), 
            new PropertyMetadata(null, OnCurrentPointChanged));

        private static void OnCurrentPointChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
    }
}
