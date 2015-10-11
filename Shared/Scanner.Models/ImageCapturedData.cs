using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Scanner.Models
{
    public class PhotoCapturedData
    {
        private PhotoCapturedData(StorageFile imageFile, string categoryName, ulong size)
        {
            this.ImageFile = imageFile;
            this.CategoryName = categoryName;
            this.ImageFileSize = size;
        }

        public PhotoCapturedData(string categoryName)
        {
            this.CategoryName = categoryName;
        }

        public static async Task<PhotoCapturedData> CreatePhotoCapturedDataAsync(StorageFile imageFile, string categoryName)
        {
            var photoData = new PhotoCapturedData(categoryName);

            photoData.ImageFile = imageFile;

            photoData.ImageFileSize = (await imageFile.GetBasicPropertiesAsync()).Size;

            return photoData;
        }

        public ulong ImageFileSize { get; set; }

        public StorageFile ImageFile { get; set; }

        public string CategoryName { get; set; }

        public bool IsFromCamera { get; set; }
    }
}
