using Scanner.Windows.Core;
using GalaSoft.MvvmLight.Command;
using Windows.Storage;
using Windows.Storage.Pickers;
using System;
using System.Linq;
using GalaSoft.MvvmLight.Ioc;
using System.Collections.ObjectModel;
using Scanner.Models;
using Scanner.DataAccess;
using System.Windows.Input;
using System.Threading.Tasks;
using Scanner.Models.Arguements;
using Scanner.Core.Enums;

namespace Scanner.Windows.ViewModels
{
    public class MainViewModel : ViewModel
    {
        private bool isOpen;

        public MainViewModel()
        {
            this.Categories = new ObservableCollection<Category>();

            this.NewCategoryString = string.Empty;
            
            this.NavigationContext = SimpleIoc.Default.GetInstance<NavigationContext>();

            this.ComboBoxSelectedValue = SortSettings.Size.ToString();

            this.InitializeCommands();
        }

        #region Commands

        public ICommand NavigateCommand { get; private set; }

        public ICommand PhotoCommand { get; private set; }

        public ICommand ImageProcessCommand { get; private set; }

        public ICommand ImportCommand { get; private set; }

        public ICommand GetAllDocumentsCommand { get; private set; }

        public ICommand ShowNewCategoryDialogCommand { get; private set; }

        public ICommand AddNewCategoryCommand { get; private set; }

        public ICommand BatchModeCommand { get; private set; }

        public ICommand AllCategoryDocumentsCommand { get; private set; }

        public ICommand AllDocumentsCommand { get; private set; }

        public ICommand DocumentItemClickCommand { get; private set; }

        public ICommand LoadDataCommand { get; private set; }

        public ICommand SortItemsCommand { get; private set; }

        #endregion

        #region Properties

        public string NewCategoryString { get; set; }

        public ObservableCollection<Category> Categories { get; set; }

        public NavigationContext NavigationContext { get; private set; }

        public string ComboBoxSelectedValue { get; set; }

        public bool IsFlyoutOpen 
        { 
            get
            {
                return this.isOpen;
            }
            set
            {
                this.isOpen = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        protected override void OnNavigatedToCommandExecute(object obj)
        {

        }

        protected override void InitializeCommands()
        {
            this.PhotoCommand = new RelayCommand(async () => { await this.DoPhoto(); });

            this.ImportCommand = new RelayCommand(async () => { await this.DoImport(); });

            this.AddNewCategoryCommand = new RelayCommand(async () => { await this.AddNewCategory(); });

            //this.BatchModeCommand = new RelayCommand(this.AddNewCategory);

            this.AllCategoryDocumentsCommand = new RelayCommand<Guid>(this.AllCategoryDocuments);

            this.DocumentItemClickCommand = new RelayCommand<Document>(this.DocumentItemClick);

            this.LoadDataCommand = new RelayCommand(async () => { await this.LoadData(d => d.DocumentName); });

            this.SortItemsCommand = new RelayCommand<string>(async (s) => { await this.SortItems(s); });
        }

        private async Task SortItems(string selectedValue)
        {
            if (selectedValue.Equals(SortSettings.Name.ToString()))
            {
                await this.LoadData(d => d.DocumentName);
            }

            if (selectedValue.Equals(SortSettings.Size.ToString()))
            {
                await this.LoadData(d => d.DocumentSize);
            }

            if (selectedValue.Equals(SortSettings.Number.ToString()))
            {
                await this.LoadData(d => d.DocumentPageCounter);
            }

            if (selectedValue.Equals(SortSettings.Date.ToString()))
            {
                await this.LoadData(d => d.DateCreated);
            }
        }

        private async Task LoadData(Func<Document, object> keySelector)
        {
            this.Categories.Clear();

            var categories = await this.InitializeUserCategories(keySelector);

            foreach (var category in categories)
            {
                this.Categories.Add(category);
            }
        }

        private async Task<ObservableCollection<Category>> InitializeUserCategories(Func<Document, object> keySelector)
        {
            var deserializedDocuments = await SerializationProvider.Instance.DeserializeDocuments();

            var deserializedCategories = await SerializationProvider.Instance.DeserializeCategories();

            var query = from item in deserializedDocuments 
                        where !item.DocumentCategory.Equals("None")
                        group item by ((Document)item).DocumentCategory into g
                        select new { CategoryName = g.Key, Items = g };

            foreach (var g in query)
            {
                var currentCategory =
                    deserializedCategories.FirstOrDefault(category => category.CategoryName.Equals(g.CategoryName));

                foreach (var item in g.Items.OrderBy(keySelector))
                {
                    currentCategory.CategoryDocuments.Add(item);
                }
            }

            var sortedDocuments = new ObservableCollection<Document>();

            foreach (var doc in deserializedDocuments.OrderBy(keySelector))
            {
                sortedDocuments.Add(doc);
            }

            deserializedCategories[0].CategoryDocuments = sortedDocuments;

            return deserializedCategories;
        }

        private void DocumentItemClick(Document clickedItem)
        {
            var arguements = new DocumentPageArguements()
            {
                DocumentId = clickedItem.ID,
                PreviousPageName = "MainView",
            };

            this.NavigationContext.NavigationService.NavigateTo(NavigationSource.DocumentView.ToString(),
                arguements);
        }

        private void AllCategoryDocuments(Guid categoryId)
        {
            if (categoryId != Guid.Empty)
            {
                var arguements = new CurrentCategoryPageArguements()
                {
                    CategoryId = categoryId,
                };

                this.NavigationContext.NavigationService.NavigateTo(NavigationSource.CurrentCategoryView.ToString(),
                    categoryId);
            }
        }

        private async Task AddNewCategory()
        {
            if (string.IsNullOrEmpty(this.NewCategoryString))
            {
                this.IsFlyoutOpen = true;

                return;
            }

            var newCategory = new Category(this.NewCategoryString);

            this.Categories.Add(newCategory);

            await SerializationProvider.Instance.SerializeCategories(this.Categories);

            this.NewCategoryString = string.Empty;

            this.IsFlyoutOpen = false;
        }

        private async Task DoPhoto()
        {
            ImageService.CategoryName = "None";

            var photo = await ImageService.DoPhotoFromCamera();

            if (photo != null)
                this.NavigationContext.NavigationService.NavigateTo(NavigationSource.PhotoView.ToString(), photo);
        }

        private async Task DoImport()
        {
            ImageService.CategoryName = "None";

            var imageFromImport = await ImageService.GetImageFromImport();

            if (imageFromImport != null)
                this.NavigationContext.NavigationService.NavigateTo(NavigationSource.PhotoView.ToString(),
                    imageFromImport);
        }
    }
}
