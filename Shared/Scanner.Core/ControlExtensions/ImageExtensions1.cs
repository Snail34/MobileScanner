using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Scanner.Core.ControlExtensions
{
    public class ImageExtensions1 : DependencyObject
    {
        public static readonly DependencyProperty FileProperty =
          DependencyProperty.RegisterAttached(
          "BitmapImageSource",
          typeof(BitmapImage),
          typeof(ImageExtensions1),
          new PropertyMetadata(null, OnBitmapImageSourceChanged));

        public static void SetBitmapImageSource(UIElement element, BitmapImage value)
        {
            element.SetValue(FileProperty, value);
        }

        public static BitmapImage GetBitmapImageSource(UIElement element)
        {
            return (BitmapImage)element.GetValue(FileProperty);
        }

        private static void OnBitmapImageSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var image = d as Image;

            var storageFile = e.NewValue as BitmapImage;

            if (image != null && storageFile != null)
            {
                image.Source = storageFile;

                image.Width = storageFile.PixelWidth;

                image.Height = storageFile.PixelHeight;
            }
        }
    }
}
