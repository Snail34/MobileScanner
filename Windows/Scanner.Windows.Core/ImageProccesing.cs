using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Math.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using WF = Windows.Foundation;
using OpenCVRT;

namespace Scanner.Windows.Core
{
    public class ImageProccesing
    {
        public async void Quadraliteral(StorageFile file, IList<WF.Point> points)
        {
            var data = await FileIO.ReadBufferAsync(file);

            OpencvImageProcess opencv = new OpencvImageProcess();
            
            // create a stream from the file
            var ms = new InMemoryRandomAccessStream();
            var dw = new DataWriter(ms);
            dw.WriteBuffer(data);
            await dw.StoreAsync();
            ms.Seek(0);

            // find out how big the image is, don't need this if you already know
            var bm = new BitmapImage();
            await bm.SetSourceAsync(ms);

            // create a writable bitmap of the right size
            var wb = new WriteableBitmap(bm.PixelWidth, bm.PixelHeight);
            ms.Seek(0);

            // load the writable bitpamp from the stream
            await wb.SetSourceAsync(ms);

            Bitmap bmp = (Bitmap)wb;

            var wb1 = opencv.GetImageCorners(wb);

            wb1.Invalidate();
            //wb.Invalidate();
            // define quadrilateral's corners
            //List<IntPoint> corners = new List<IntPoint>();
            
            //foreach (var point in points)
            //{
            //    corners.Add(new IntPoint((int)point.X, (int)point.Y));
            //}
            
            //// create filter

            //var filter =
            //    new SimpleQuadrilateralTransformation(corners);
            
            //// apply the filter
            //Bitmap newImage = filter.Apply(bmp);

            //wb = (WriteableBitmap)newImage;

            var f = await this.WriteableBitmapToStorageFile(wb1, FileFormat.Jpeg);
        }

        public async void ProccesImage(StorageFile imageFile)
        {
            var data = await FileIO.ReadBufferAsync(imageFile);

            // create a stream from the file
            var ms = new InMemoryRandomAccessStream();
            var dw = new DataWriter(ms);
            dw.WriteBuffer(data);
            await dw.StoreAsync();
            ms.Seek(0);

            // find out how big the image is, don't need this if you already know
            var bm = new BitmapImage();
            await bm.SetSourceAsync(ms);

            // create a writable bitmap of the right size
            var wb = new WriteableBitmap(bm.PixelWidth, bm.PixelHeight);
            ms.Seek(0);

            // load the writable bitpamp from the stream
            await wb.SetSourceAsync(ms);

            Bitmap bmp = (Bitmap)wb;

            //var filter1 = Grayscale.CommonAlgorithms.BT709;

            //bmp = filter1.Apply(bmp);

            wb = (WriteableBitmap)bmp;

            var file = await this.WriteableBitmapToStorageFile(wb, FileFormat.Jpeg);
        }

        private async Task<StorageFile> WriteableBitmapToStorageFile(WriteableBitmap WB, FileFormat fileFormat)
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

            var file = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(FileName, CreationCollisionOption.GenerateUniqueName);
            using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoderGuid, stream);
                Stream pixelStream = WB.PixelBuffer.AsStream();
                byte[] pixels = new byte[pixelStream.Length];
                await pixelStream.ReadAsync(pixels, 0, pixels.Length);

                encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore,
                                    (uint)WB.PixelWidth,
                                    (uint)WB.PixelHeight,
                                    96.0,
                                    96.0,
                                    pixels);
                await encoder.FlushAsync();
            }
            return file;
        }

        private enum FileFormat
        {
            Jpeg,
            Png,
            Bmp,
            Tiff,
            Gif
        }
    }
}
