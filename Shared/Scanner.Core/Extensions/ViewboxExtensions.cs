using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Scanner.Core.Extensions
{
    public static class ViewboxExtensions
    {
        public static double GetChildScaleX(this Viewbox viewbox)
        {
            if (viewbox.Child == null)
                throw new InvalidOperationException("Can't tell effective scale of a Viewbox child for a Viewbox with no child.");

            var fe = viewbox.Child as FrameworkElement;

            if (fe == null)
                throw new InvalidOperationException("Can't tell effective scale of a Viewbox child for a Viewbox with a child that is not a FrameworkElement.");

            if (fe.ActualWidth == 0)
                throw new InvalidOperationException("Can't tell effective scale of a Viewbox child for a Viewbox with a child that is not laid out.");

            return fe.ActualWidth / viewbox.ActualWidth;
        }

        public static double GetChildScaleY(this Viewbox viewbox)
        {
            if (viewbox.Child == null)
                throw new InvalidOperationException("Can't tell effective scale of a Viewbox child for a Viewbox with no child.");

            var fe = viewbox.Child as FrameworkElement;

            if (fe == null)
                throw new InvalidOperationException("Can't tell effective scale of a Viewbox child for a Viewbox with a child that is not a FrameworkElement.");

            if (fe.ActualWidth == 0)
                throw new InvalidOperationException("Can't tell effective scale of a Viewbox child for a Viewbox with a child that is not laid out.");

            return fe.ActualHeight / viewbox.ActualHeight;
        }
    }
}
