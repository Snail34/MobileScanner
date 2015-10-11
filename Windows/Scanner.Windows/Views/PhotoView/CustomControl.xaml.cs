using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Шаблон элемента пользовательского элемента управления задокументирован по адресу http://go.microsoft.com/fwlink/?LinkId=234236

namespace Scanner.Windows.Views.PhotoView
{
    public sealed partial class CustomControl : UserControl
    {
        public CustomControl()
        {
            this.InitializeComponent();
        }

        public Point CurrentPosition
        {
            get { return (Point)GetValue(CurrentPositionProperty); }
            set { SetValue(CurrentPositionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentPosition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentPositionProperty =
            DependencyProperty.Register("CurrentPosition", 
            typeof(Point), 
            typeof(CustomControl), 
            new PropertyMetadata(null));
    }
}
