using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Scanner.Core.Enums;
using Scanner.DataAccess;
using Scanner.Models;
using Scanner.Models.Arguements;
using Scanner.Windows.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace Scanner.Windows.ViewModels
{
    public class DocumentViewModel : ViewModel
    {
        private StorageFile imageSource;

        private bool isActivated;

        private bool isSticky;

        public DocumentViewModel()
        {
            this.CategoryNames = new ObservableCollection<string>();

            this.NavigationContext = SimpleIoc.Default.GetInstance<NavigationContext>();

            this.InitializeCommands();

            this.CurrentDocument = new Document();

            this.CurrentDocument.Pages = new ObservableCollection<Page>();
        }

        #region Properties
        public Models.Document CurrentDocument { get; set; }

        public StorageFile CapturedImage
        {
            get
            {
                return this.imageSource;
            }

            set
            {
                this.imageSource = value;
                this.RaisePropertyChanged();
            }
        }

        public NavigationContext NavigationContext { get; set; }

        public ObservableCollection<string> CategoryNames { get; set; }

        public string SelectedCategoryName { get; set; }

        public IList<object> SelectedItems { get; set; }

        public bool IsBottomAppBarOpen
        {
            get
            {
                return this.isActivated;
            }

            set
            {
                this.isActivated = value;
                this.RaisePropertyChanged();
            }
        }

        public bool IsBottomAppBarSticky
        {
            get
            {
                return this.isSticky;
            }

            set
            {
                this.isSticky = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion

        #region Commands
        public ICommand DeleteCurrentDocumentCommand { get; private set; }

        public ICommand BackCommand { get; private set; }

        public ICommand AddPageCommand { get; private set; }

        public ICommand PageItemClickCommand { get; private set; }

        public ICommand AddFromGalleryCommand { get; private set; }

        public ICommand RenameDocumentCommand { get; private set; }

        public ICommand LoadCategoryNamesCommand { get; private set; }

        public ICommand MoveToCategoryCommand { get; private set; }

        public ICommand PinDocumentCommand { get; private set; }

        public ICommand MergePagesCommand { get; private set; }

        public ICommand DeletePagesCommand { get; private set; }

        public ICommand SelectionChangedCommand { get; private set; }

        #endregion

        protected async override void OnNavigatedToCommandExecute(object obj)
        {
            var arguements = obj as DocumentPageArguements;

            if (arguements != null)
            {
                var currentDocument = await SerializationProvider.Instance.GetDocumentById(arguements.DocumentId);

                Document.CopyDocument(currentDocument, this.CurrentDocument);

                //if (arguements.PreviousPageName.Equals("PhotoView"))
                //{
                //    this.NavigationContext.NavigationService.RemoveBackEntry();
                //    this.NavigationContext.NavigationService.RemoveBackEntry();
                //    this.NavigationContext.NavigationService.RemoveBackEntry();
                //}
            }
        }

        protected override void InitializeCommands()
        {
            this.BackCommand = new RelayCommand(this.Back);

            this.AddPageCommand = new RelayCommand(async () => { await this.AddPage(); });

            this.AddFromGalleryCommand = new RelayCommand(async () => { await this.AddFromGallery(); });

            this.DeleteCurrentDocumentCommand = new RelayCommand(this.DeleteCurrentDocument);

            this.PageItemClickCommand = new RelayCommand<Models.Page>(this.PageItemClick);

            this.RenameDocumentCommand = new RelayCommand<string>(this.RenameDocument);

            this.LoadCategoryNamesCommand = new RelayCommand(this.LoadCategoryNames);

            this.MoveToCategoryCommand = new RelayCommand<string>(this.MoveToCategory);

            this.PinDocumentCommand = new RelayCommand(this.PinDocument);

            this.MergePagesCommand = new RelayCommand(this.MergePages);

            this.DeletePagesCommand = new RelayCommand(this.DeletePages);

            this.SelectionChangedCommand = new RelayCommand(this.SelectionChanged);
        }

        private async void DeletePages() 
        {
            IList<Page> selectedPages = this.CastSelectedItems();

            foreach (var page in selectedPages)
            {
                this.CurrentDocument.DocumentSize -= page.MbSize;

                this.CurrentDocument.Pages.Remove(page);
            }

            this.UpdatePageNumbers();

            await SerializationProvider.Instance.UpdateDocument(this.CurrentDocument);
        }

        private void UpdatePageNumbers()
        {
            for (int i = 0; i < this.CurrentDocument.Pages.Count; i++)
            {
                this.CurrentDocument.Pages[i].Number = i + 1;

                this.CurrentDocument.Pages[i].PageName = "Page" + this.CurrentDocument.Pages[i].Number.ToString();
            }
        }

        private List<Page> CastSelectedItems()
        {
            var pageList = new List<Page>();

            foreach (var item in this.SelectedItems)
            {
                pageList.Add((Page)item);
            }

            return pageList;
        }

        private void SelectionChanged()
        {
            if (this.SelectedItems.Count >= 1)
            {
                this.IsBottomAppBarOpen = true;
                this.IsBottomAppBarSticky = true;
            }
            else
            {
                this.IsBottomAppBarOpen = false;
                //this.IsBottomAppBarSticky = false;
            }
        }

        private async void MergePages()
        {
            if (this.SelectedItems.Count < 2)
            {
                return;
            }

            IList<Page> selectedPages = this.CastSelectedItems();

            var docWithMergedPages = new Document(this.CurrentDocument.DocumentCategory);

            foreach (var page in selectedPages)
            {
                docWithMergedPages.Pages.Add(page);

                this.CurrentDocument.DocumentSize -= page.MbSize;

                this.CurrentDocument.Pages.Remove(page);
            }

            await SerializationProvider.Instance.AddDocument(docWithMergedPages);

            await SerializationProvider.Instance.UpdateDocument(this.CurrentDocument);

            var arguements = new DocumentPageArguements()
            {
                DocumentId = docWithMergedPages.ID,
                PreviousPageName = "DocumentView",
            };

            this.UpdatePageNumbers();

            this.NavigationContext.NavigationService.NavigateTo("DocumentView", arguements);
        }

        private async void PinDocument()
        {
            string tileId = this.CurrentDocument.ID.ToString();

            if (!TileProvider.Instance.TileExist(tileId))
            {
                this.CurrentDocument.IsPinned = await TileProvider.Instance.PinDocumentTile(this.CurrentDocument);
            }
            else
            {
                await TileProvider.Instance.UnPinDocumentTile(tileId);

                this.CurrentDocument.IsPinned = false;
            }
        }

        private async void MoveToCategory(string selectedCategoryName)
        {
            await SerializationProvider.Instance.UpdateDocumentsCategoryName(
                this.CurrentDocument.DocumentCategory, selectedCategoryName);

            this.CurrentDocument.DocumentCategory = selectedCategoryName;
        }

        private async void LoadCategoryNames()
        {
            var categories = await SerializationProvider.Instance.DeserializeCategories();

            foreach (var category in categories)
            {
                this.CategoryNames.Add(category.CategoryName);
            }
        }

        private async Task AddFromGallery()
        {
            ImageService.CategoryName = this.CurrentDocument.DocumentCategory;

            var imageFromImport = await ImageService.GetImageFromImport(false);

            if (imageFromImport != null)
            {
                imageFromImport.PageToProcces.DocumentId = this.CurrentDocument.ID;

                imageFromImport.PageToProcces.Number = this.CurrentDocument.DocumentPageCounter + 1;

                imageFromImport.PageToProcces.PageName = "Page" + imageFromImport.PageToProcces.Number.ToString();

                this.CurrentDocument.Pages.Add(imageFromImport.PageToProcces);

                this.CurrentDocument.DocumentSize += imageFromImport.PageToProcces.MbSize;

                await SerializationProvider.Instance.UpdateDocument(this.CurrentDocument);
            }
            
            this.NavigationContext.NavigationService.NavigateTo("PhotoView", imageFromImport);
        }

        private void PageItemClick(Page page)
        {
            var folder = ApplicationData.Current.TemporaryFolder;

            this.NavigationContext.NavigationService.NavigateTo("PostProccesView", this.CurrentDocument);
        }

        private async void DeleteCurrentDocument()
        {
            await SerializationProvider.Instance.DeleteDocuments(new List<Guid>() { this.CurrentDocument.ID });

            this.NavigationContext.NavigationService.GoBack();
        }

        private async Task AddPage()
        {
            ImageService.CategoryName = this.CurrentDocument.DocumentCategory;

            var photo = await ImageService.DoPhotoFromCamera(false);

            if (photo != null)
            {
                photo.PageToProcces.DocumentId = this.CurrentDocument.ID;

                photo.PageToProcces.Number = this.CurrentDocument.DocumentPageCounter + 1;

                photo.PageToProcces.PageName = "Page" + photo.PageToProcces.Number.ToString();

                this.CurrentDocument.Pages.Add(photo.PageToProcces);

                this.CurrentDocument.DocumentSize += photo.PageToProcces.MbSize;

                await SerializationProvider.Instance.UpdateDocument(this.CurrentDocument);
            }

            this.NavigationContext.NavigationService.NavigateTo(NavigationSource.PhotoView.ToString(),
                photo);
        }

        private async void RenameDocument(string documentNewName)
        {
            await SerializationProvider.Instance.UpdateDocumentName(this.CurrentDocument.ID, documentNewName);
        }

        private void Back()
        {
            //this.NavigationContext.NavigationService.RemoveBackEntry();
            this.NavigationContext.NavigationService.GoBack();
        }
    }
}
