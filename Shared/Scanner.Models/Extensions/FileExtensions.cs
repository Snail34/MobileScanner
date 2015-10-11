using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Scanner.Models.Extensions
{
    public static class FileExtensions
    {
        public static async Task<StorageFile> TryGetFileAsync(this StorageFolder folder, string fileName)
        {
            StorageFile file = null;

            try
            {
                if (folder != null)
                {
                    file = await folder.GetFileAsync(fileName);
                }
            }
            catch(FileNotFoundException) {}

            return file;
        }
    }
}
