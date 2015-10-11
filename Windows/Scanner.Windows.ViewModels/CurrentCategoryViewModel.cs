using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Scanner.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Input;
using Scanner.DataAccess;
using Scanner.Windows.Core;
using Windows.Storage.Pickers;
using Windows.Storage;
using Scanner.Models.Arguements;
using Scanner.Core.Enums;

namespace Scanner.Windows.ViewModels
{
    public class CurrentCategoryViewModel : ViewModel
    {
        private bool isActivated;

        private bool isSticky;

        private bool isCategoryEmpty;

        private bool isEnabled;

        private Category category;

        public CurrentCategoryViewModel()
        {
            this.NavigationContext = SimpleIoc.Default.GetInstance<NavigationContext>();

            Messenger.Default.Register<Document>(this,
                MessengerToken.DocumentAddedUserCategory, this.HandleDocument);

            this.IsTopAppBarVisible = true;

            this.InitializeCommands();
        }

        #region Properties
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

        public bool IsCategoryEmpty
        {
            get
            {
                return this.isCategoryEmpty;
            }

            set
            {
                this.isCategoryEmpty = value;
                this.RaisePropertyChanged();
            }
        }

        public string CurrentCategoryName 
        { 
            get
            {
                return this.CurrentCategory.CategoryName;
            }
            set
            {
                this.CurrentCategory.CategoryName = value;
                this.RaisePropertyChanged();
            }
        }

        public NavigationContext NavigationContext { get; private set; }

        public IList<object> SelectedItems { get; set; }

        public Category CurrentCategory 
        {
            get
            {
                return this.category;
            }
            set
            {
                this.category = value;
                this.RaisePropertyChanged();
            }
        }

        public bool IsTopAppBarVisible
        {
            get
            {
                return this.isEnabled;
            }

            set
            {
                this.isEnabled = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion

        #region Commands
        public ICommand BackCommand { get; private set; }

        public ICommand DeleteDocumentCommand { get; private set; }

        public ICommand DeleteCategoryCommand { get; private set; }

        public ICommand MergeDocumentsCommand { get; private set; }

        public ICommand RenameCategoryCommand { get; private set; }

        public ICommand AddDocumentCommand { get; private set; }

        public ICommand SelectionChangedCommand { get; private set; }

        public ICommand NewScanCommand { get; private set; }

        public ICommand PinCategoryCommand { get; private set; }

        public ICommand DocumentItemClickCommand { get; private set; }
        #endregion

        protected async override void OnNavigatedToCommandExecute(object obj)
        {
            if (obj != null)
            {
                Guid guid = Guid.Empty;

                if (obj is Guid)
                {
                    guid = (Guid)obj;
                }

                var currentCategory = await SerializationProvider.Instance.GetCategoryById(guid);

                if (currentCategory != null)
                {
                    this.CurrentCategory = currentCategory;

                    await this.CheckMainCategory();
                }

                //if (currentCategory.CategoryDocuments.Count == 0)
                //{
                //    this.IsCategoryEmpty = true;
                //}
                //else
                //{
                //    this.IsCategoryEmpty = false;
                //}
            }
        }

        protected override void InitializeCommands()
        {
            this.BackCommand = new RelayCommand(this.Back);

            this.DeleteDocumentCommand = new RelayCommand(async () => { await this.DeleteDocument(); });

            this.DeleteCategoryCommand = new RelayCommand(this.DeleteCategory);

            this.RenameCategoryCommand = new RelayCommand<string>(this.RenameCategory);

            this.AddDocumentCommand = new RelayCommand(async () => { await this.AddDocument(); });

            this.SelectionChangedCommand = new RelayCommand(this.SelectionChanged);

            this.MergeDocumentsCommand = new RelayCommand(async () => { await this.MergeDocuments(); });

            this.NewScanCommand = new RelayCommand(async () => { await this.NewScan(); });

            this.PinCategoryCommand = new RelayCommand(this.PinCategory);

            this.DocumentItemClickCommand = new RelayCommand<Document>(this.DocumentItemClick);
        }

        private async Task CheckMainCategory()
        {
            if (this.CurrentCategory.CategoryName.Equals("All documents"))
            {
                this.IsTopAppBarVisible = false;
                this.CurrentCategoryName = "None";
                var allDocs = await SerializationProvider.Instance.DeserializeDocuments();

                foreach (var doc in allDocs)
                {
                    this.CurrentCategory.CategoryDocuments.Add(doc);
                }
            }
        }

        private void DocumentItemClick(Document clickedItem)
        {
            var arguements = new DocumentPageArguements()
            {
                DocumentId = clickedItem.ID,
                PreviousPageName = "CurrentCategoryView",
            };

            this.NavigationContext.NavigationService.NavigateTo("DocumentView", arguements);
        }

        private async void PinCategory()
        {
            string tileId = this.CurrentCategory.ID.ToString();

            if (!TileProvider.Instance.TileExist(tileId))
            {
                this.CurrentCategory.IsPinned = await TileProvider.Instance.PinCategoryTile(this.CurrentCategory);
            }
            else
            {
                await TileProvider.Instance.UnPinCategoryTile(tileId);

                this.CurrentCategory.IsPinned = false;
            }
        }

        private async Task NewScan()
        {
            ImageService.CategoryName = this.CurrentCategory.CategoryName;

            var imageFromImport = await ImageService.GetImageFromImport();

            this.NavigationContext.NavigationService.NavigateTo(NavigationSource.PhotoView.ToString(),
                    imageFromImport);
        }

        private async Task MergeDocuments()
        {
            if (this.SelectedItems.Count < 2)
            {
                return;
            }

            var documentList = new List<Document>();

            var documentListId = new List<Guid>();

            foreach (var item in this.SelectedItems)
            {
                documentList.Add((Document)item);
            }

            var doc = Document.CreateDefaultDocument(this.CurrentCategory.CategoryName);

            foreach (var document in documentList)
            {
                foreach (var page in document.Pages)
                {
                    doc.Pages.Add(page);
                }

                documentListId.Add(document.ID);

                this.CurrentCategory.CategoryDocuments.Remove(document);
            }

            this.CurrentCategory.CategoryDocuments.Add(doc);

            await SerializationProvider.Instance.DeleteDocuments(documentListId);

            await SerializationProvider.Instance.AddDocument(doc);
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

        private void HandleDocument(Document d)
        {
            if (this.CurrentCategory.CategoryDocuments == null)
            {
                this.CurrentCategory.CategoryDocuments = new ObservableCollection<Document>();
            }

            this.CurrentCategory.CategoryDocuments.Add(d);
        }

        private async Task AddDocument()
        {
            ImageService.CategoryName = this.CurrentCategory.CategoryName;

            var photo = await ImageService.DoPhotoFromCamera();

            this.NavigationContext.NavigationService.NavigateTo(NavigationSource.PhotoView.ToString(), photo);
        }

        private async void RenameCategory(string categoryNewName)
        {
            await SerializationProvider.Instance.UpdateDocumentsCategoryName(this.CurrentCategory.CategoryName, categoryNewName);

            await SerializationProvider.Instance.UpdateCategoryName(this.CurrentCategory.ID, categoryNewName);

            this.CurrentCategoryName = categoryNewName;
        }

        private async void DeleteCategory()
        {
            if (this.CurrentCategory.CategoryDocuments.Count == 0)
            {
                await SerializationProvider.Instance.DeleteCategory(this.CurrentCategory.CategoryName);

                this.NavigationContext.NavigationService.NavigateTo("MainView");
            }
        }

        private void Back()
        {
            this.NavigationContext.NavigationService.NavigateTo("MainView");
        }

        private async Task DeleteDocument()
        {
            var documentListId = new List<Guid>();

            var documentList = new List<Document>();

            foreach (var o in this.SelectedItems)
            {
                documentList.Add((Document)o);

                documentListId.Add(((Document)o).ID);
            }

            await SerializationProvider.Instance.DeleteDocuments(documentListId);

            foreach (var doc in documentList)
            {
                this.CurrentCategory.CategoryDocuments.Remove(doc);
            }
        }
    }
}
