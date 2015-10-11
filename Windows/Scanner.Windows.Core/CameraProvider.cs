using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Scanner.Windows.Core
{
    public class CameraProvider
    {
        private static readonly CameraProvider instance = new CameraProvider();

        static CameraProvider()
        {

        }

        public CameraProvider()
        {
            
        }

        public static CameraProvider Instance
        {
            get
            {
                return instance;
            }
        }

        public async Task<StorageFile> GetPhotoFromCamera()
        {
            var camera = new CameraCaptureUI();

            camera.PhotoSettings.AllowCropping = false;
            
            var file = await camera.CaptureFileAsync(CameraCaptureUIMode.Photo);

            return file;
        }
    }
}
