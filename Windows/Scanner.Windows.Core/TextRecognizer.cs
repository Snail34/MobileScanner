using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;
using WindowsPreview.Media.Ocr;

namespace Scanner.Windows.Core
{
    public class TextRecognizer
    {
        private OcrEngine ocrEngine;

        public TextRecognizer()
        {
            ocrEngine = new OcrEngine(OcrLanguage.English);
        }

        public async Task<string> RecognizeFromImage(StorageFile file)
        {
            string recognizedText = string.Empty;

            if (file != null)
            {
                Byte[] imageBytes = await this.ImageFileToByteArray(file);

                ImageProperties imageProperties = await file.Properties.GetImagePropertiesAsync();

                OcrResult ocrResult = await ocrEngine.RecognizeAsync(imageProperties.Height, imageProperties.Width, imageBytes);

                foreach (var line in ocrResult.Lines)
                {
                    foreach (var word in line.Words)
                    {
                        recognizedText += word.Text + " ";
                    }

                    recognizedText += Environment.NewLine;
                }
            }

            return recognizedText;
        }

        public async Task<Byte[]> ImageFileToByteArray(StorageFile file)
        {
            IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read);

            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);

            PixelDataProvider pixelData = await decoder.GetPixelDataAsync();

            return pixelData.DetachPixelData();
        }
    }
}
