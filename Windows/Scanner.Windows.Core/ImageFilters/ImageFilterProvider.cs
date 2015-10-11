using Lumia.Imaging;
using Lumia.Imaging.Transforms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Foundation;
using System.IO;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;
using Lumia.Imaging.Artistic;
using Lumia.Imaging.Adjustments;

namespace Scanner.Windows.Core.ImageFilters
{
    public class ImageFilterProvider
    {
        private enum FileFormat
        {
            Jpeg,
            Png,
            Bmp,
            Tiff,
            Gif
        }

        private async Task<StorageFile> WriteableBitmapToStorageFile(WriteableBitmap wb, FileFormat fileFormat)
        {
            string FileName = "MyFile.";
            Guid BitmapEncoderGuid = BitmapEncoder.JpegEncoderId;
            switch (fileFormat)
            {
                case FileFormat.Jpeg:
                    FileName += "jpeg";
                    BitmapEncoderGuid = BitmapEncoder.JpegEncoderId;
                    break;

                case FileFormat.Png:
                    FileName += "png";
                    BitmapEncoderGuid = BitmapEncoder.PngEncoderId;
                    break;

                case FileFormat.Bmp:
                    FileName += "bmp";
                    BitmapEncoderGuid = BitmapEncoder.BmpEncoderId;
                    break;

                case FileFormat.Tiff:
                    FileName += "tiff";
                    BitmapEncoderGuid = BitmapEncoder.TiffEncoderId;
                    break;

                case FileFormat.Gif:
                    FileName += "gif";
                    BitmapEncoderGuid = BitmapEncoder.GifEncoderId;
                    break;
            }

            var file = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
            using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoderGuid, stream);
                Stream pixelStream = wb.PixelBuffer.AsStream();
                byte[] pixels = new byte[pixelStream.Length];
                await pixelStream.ReadAsync(pixels, 0, pixels.Length);

                encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore,
                                    (uint)wb.PixelWidth,
                                    (uint)wb.PixelHeight,
                                    96.0,
                                    96.0,
                                    pixels);
                await encoder.FlushAsync();
            }
            return file;
        }

        private void SelectFilter(FilterOption selectedFilterOption)
        {
            switch(selectedFilterOption)
            {
                case FilterOption.Cartoon: this.SelectedFilter = new CartoonFilter(true); break;
            }
        }

        public IFilter SelectedFilter { get; private set; }

        public async Task<StorageFile> ApplyFilter(StorageFile file, FilterOption filterOption)
        {
            IImageProvider source = null;

            var properties = await file.Properties.GetImagePropertiesAsync();

            this.SelectFilter(filterOption);

            using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                source = new StreamImageSource(stream.AsStream(), ImageFormat.Jpeg);

                using (var filterEffect = new FilterEffect(source))
                {
                    // Initialize the filter and add the filter to the FilterEffect collection
                    //var filter = new SketchFilter(SketchMode.Gray);

                    filterEffect.Filters = new IFilter[] { this.SelectedFilter };

                    // Create a target where the filtered image will be rendered to
                    var target = new WriteableBitmap((int)properties.Width, (int)properties.Height);
                    
                    // Create a new renderer which outputs WriteableBitmaps
                    using (var renderer = new WriteableBitmapRenderer(filterEffect, target))
                    {
                        // Render the image with the filter(s)
                        await renderer.RenderAsync();
                        
                        return await this.WriteableBitmapToStorageFile(target, FileFormat.Jpeg);
                    }
                }
            }
        }
    }
}
