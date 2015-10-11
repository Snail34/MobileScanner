using Scanner.DataAccess;
using Scanner.Models;
using Scanner.Models.Extensions;
using Scanner.Models.Arguements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace Scanner.Windows.Core
{
    public static class ImageService
    {
        static ImageService()
        {

        }

        public static string CategoryName { get; set; }

        public async static Task<PhotoPageArguements> DoPhotoFromCamera(bool isNewDocument = true)
        {
            var capturedImage = await CameraProvider.Instance.GetPhotoFromCamera();

            PhotoPageArguements arguments = null;

            if (capturedImage != null)
            {
                arguments = await ImageService.GetPhotoPageArguements(capturedImage, isNewDocument);
            }

            return arguments;
        }

        public async static Task<PhotoPageArguements> GetImageFromImport(bool isNewDocument = true)
        {
            var picker = new FileOpenPicker();

            picker.ViewMode = PickerViewMode.Thumbnail;

            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;

            picker.FileTypeFilter.Add(".jpg");

            var file = await picker.PickSingleFileAsync();

            PhotoPageArguements arguments = null;

            if (file != null)
            {
                var temporaryFolder = ApplicationData.Current.TemporaryFolder;

                await file.CopyAsync(temporaryFolder,
                    file.Name, NameCollisionOption.ReplaceExisting);

                file = await temporaryFolder.TryGetFileAsync(file.Name);

                if (file != null)
                {
                    arguments = await ImageService.GetPhotoPageArguements(file, isNewDocument);
                }
            }

            return arguments;
        }

        private static async Task<PhotoPageArguements> GetPhotoPageArguements(StorageFile imageFile, bool isNewDocument = true)
        {
            var photoData = await PhotoCapturedData.CreatePhotoCapturedDataAsync(imageFile, ImageService.CategoryName);

            photoData.IsFromCamera = true;

            Page page = null;

            if (isNewDocument)
            {
                var createdDocument = await Document.CreateDocumentAsync(photoData);

                await SerializationProvider.Instance.AddDocument(createdDocument);

                page = createdDocument.Pages[0];
            }
            else
            {
                page = await Page.CreatePageAsync(photoData);
            }

            await LocalStorageHelper.CreatePageFiles(imageFile, page.ID);

            var photoPageArguements = new PhotoPageArguements()
            {
                PageToProcces = page,
                IsNew = true,
            };

            return photoPageArguements;
        }
    }
}
