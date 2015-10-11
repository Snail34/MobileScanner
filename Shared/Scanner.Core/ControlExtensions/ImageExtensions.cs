namespace Scanner.Core.ControlExtentions
{
    using System;
    using Windows.Storage;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media.Imaging;

    public class ImageExtensions : DependencyObject
    {
        public static readonly DependencyProperty FileProperty = 
          DependencyProperty.RegisterAttached(
          "File",
          typeof(StorageFile),
          typeof(ImageExtensions),
          new PropertyMetadata(null, OnFileChanged));

        public static void SetFile(UIElement element, StorageFile value)
        {
            element.SetValue(FileProperty, value);
        }

        public static StorageFile GetFile(UIElement element)
        {
            return (StorageFile)element.GetValue(FileProperty);
        }

        private async static void OnFileChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var image = d as Image;

            var storageFile = e.NewValue as StorageFile;
            
            if (image != null && storageFile != null)
            {
                var bitmapImage = new BitmapImage();

                using (var stream = await storageFile.OpenReadAsync())
                {
                    bitmapImage.SetSource(stream);
                }

                image.Source = bitmapImage;

                image.Width = bitmapImage.PixelWidth;

                image.Height = bitmapImage.PixelHeight;
            }
        }
    }
}
