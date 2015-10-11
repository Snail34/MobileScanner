using Scanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Scanner.Models.Extensions;
using Windows.Storage.Streams;

namespace Scanner.DataAccess
{
    public static class LocalStorageHelper
    {
        static LocalStorageHelper()
        {

        }

        public async static Task CreatePageFiles(StorageFile fileToCopy, Guid pageId)
        {
            var localFolder = ApplicationData.Current.LocalFolder;

            await fileToCopy.CopyAsync(localFolder, pageId.ToString() + ".jpg");

            await fileToCopy.CopyAsync(localFolder, pageId.ToString() + "_backup" + ".jpg");
        }

        public async static void DeleteDocumentPagesFiles(Document document)
        {
            var localFolder = ApplicationData.Current.LocalFolder;

            foreach (var page in document.Pages)
            {
                var file = await localFolder.TryGetFileAsync(page.FileName);

                if (file != null)
                {
                    await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
                }

                var backupFile = await localFolder.TryGetFileAsync(page.BackupFileName);

                if (backupFile != null)
                {
                    await backupFile.DeleteAsync(StorageDeleteOption.PermanentDelete);
                }

                var processingFile = await localFolder.TryGetFileAsync(page.ProcessingFileName);

                if (processingFile != null)
                {
                    await processingFile.DeleteAsync(StorageDeleteOption.PermanentDelete);
                }
            }
        }

        public async static Task RefreshPageFiles(Page page)
        {
            var localFolder = ApplicationData.Current.LocalFolder;

            var file = await localFolder.TryGetFileAsync(page.FileName);
   
            var processingFile = await localFolder.TryGetFileAsync(page.ProcessingFileName);
            
            var filteredFile = await localFolder.TryGetFileAsync(page.FilteredFileName);
            
            if (file != null && processingFile != null && filteredFile != null)
            {
                await file.DeleteAsync(StorageDeleteOption.PermanentDelete);

                await processingFile.DeleteAsync(StorageDeleteOption.PermanentDelete);

                await filteredFile.RenameAsync(page.FileName);
            }
        }

        public async static Task RewritePageFiles(Page page)
        {
            var localFolder = ApplicationData.Current.LocalFolder;

            var file = await localFolder.TryGetFileAsync(page.FileName);

            var backupFile = await localFolder.TryGetFileAsync(page.BackupFileName);

            if (file != null && backupFile != null)
            {

                using (var stream = await backupFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    IInputStream inputStream = stream.GetInputStreamAt(0);

                    using (var reader = new DataReader(inputStream))
                    {
                        await reader.LoadAsync((uint)stream.Size);
                        var buffer = new byte[(int)stream.Size];
                        reader.ReadBytes(buffer);
                        await FileIO.WriteBytesAsync(file, buffer);
                    }
                }
            }
        }
        
    }
}
