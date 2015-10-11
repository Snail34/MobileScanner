using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Scanner.DataAccess;
using Scanner.Models;
using Scanner.Models.Arguements;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Scanner.Windows.Core.ImageFilters;
using System.Linq;
using Scanner.Windows.Core;
using Scanner.Core.Enums;

namespace Scanner.Windows.ViewModels
{
    public class PostProccesViewModel : ViewModel
    {
        private StorageFile imageSource;

        private Page currentPage;

        public PostProccesViewModel()
        {
            this.NavigationContext = SimpleIoc.Default.GetInstance<NavigationContext>();

            this.InitializeCommands();
        }

        public Document CurrentDocument { get; set; }

        public NavigationContext NavigationContext { get; private set; }

        public string ImagePath { get; set; }

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

        public Page SelectedPage 
        { 
            get
            {
                return this.currentPage;
            }
            set
            {
                this.currentPage = value;
                this.RaisePropertyChanged();
            }
        }

        #region Commands
        public ICommand BackCommand { get; private set; }

        public ICommand RescanCommand { get; private set; }

        public ICommand FlipViewSelectionChangedCommand { get; private set; }

        public ICommand DeletePageCommand { get; private set; }

        public ICommand SelectFilterCommand { get; private set; }

        public ICommand UndoRedoCommand { get; private set; }

        public ICommand SaveCommand { get; private set; }
        #endregion

        protected async override void OnNavigatedToCommandExecute(object obj)
        {
            var arguements = obj as PostProccesPageArguements;

            var document = obj as Document;

            if (document != null)
            {
                this.CurrentDocument = document;

                string path = this.CurrentDocument.Pages[0].FilePath;

                this.SelectedPage = this.CurrentDocument.Pages[0];

                this.CapturedImage = await StorageFile.GetFileFromApplicationUriAsync(new Uri(path));
            }
        }

        protected override void InitializeCommands()
        {
            this.BackCommand = new RelayCommand(this.Back);

            this.RescanCommand = new RelayCommand(this.Rescan);

            this.FlipViewSelectionChangedCommand = new RelayCommand(this.FlipViewSelectionChanged);

            this.DeletePageCommand = new RelayCommand(async () => { await this.DeletePage(); });

            this.SelectFilterCommand = new RelayCommand<string>(async (s) => { await this.SelectFilter(s); });

            this.UndoRedoCommand = new RelayCommand(this.UndoRedo);

            this.SaveCommand = new RelayCommand(async () => { await this.Save(); });
        }

        private async Task Save()
        {
            await LocalStorageHelper.RefreshPageFiles(this.SelectedPage);

            this.NavigationContext.NavigationService.GoBack();
        }

        private void UndoRedo()
        {
            //throw new NotImplementedException();
        }

        private async Task SelectFilter(string filterName)
        {
            ImageFilterProvider imageProvider = new ImageFilterProvider();

            string path = this.SelectedPage.FilePath;

            FilterOption filterOption = this.SelectFilterOption(filterName);

            this.CapturedImage = await imageProvider.ApplyFilter(this.CapturedImage, filterOption);

            await this.CapturedImage.CopyAsync(ApplicationData.Current.LocalFolder, 
                this.SelectedPage.ID.ToString() + "_filtered" + ".jpg", NameCollisionOption.ReplaceExisting);
        }

        private FilterOption SelectFilterOption(string filterName)
        {
            FilterOption option = FilterOption.None;

            switch(filterName)
            {
                case "Cartoon filter": option = FilterOption.Cartoon; break;
                default: option = FilterOption.None; break;
            }

            return option;
        }

        private async Task DeletePage() // debug
        {
            await SerializationProvider.Instance.DeletePages(new List<Guid>() { this.SelectedPage.ID },
                this.SelectedPage.DocumentId);

            this.CurrentDocument.DocumentSize -= this.SelectedPage.MbSize;

            this.CurrentDocument.Pages.Remove(this.SelectedPage);

            await SerializationProvider.Instance.UpdateDocument(this.CurrentDocument);

            this.NavigationContext.NavigationService.GoBack();
        }

        private async void FlipViewSelectionChanged() //originalfilepath to proccesed file path
        {
            if (this.SelectedPage != null)
            {
                string path = this.SelectedPage.FilePath;

                this.CapturedImage = await StorageFile.GetFileFromApplicationUriAsync(new Uri(path));
            }
        }

        private void Back()
        {
            this.NavigationContext.NavigationService.GoBack();
        }

        private void Rescan()
        {
            var localFolder = ApplicationData.Current.LocalFolder;

            this.SelectedPage.FilePath = this.SelectedPage.BackUpFilePath;

            var photoPageArguements = new PhotoPageArguements()
            {
                PageToProcces = this.SelectedPage,
                IsNew = false,
            };

            this.NavigationContext.NavigationService.NavigateTo("PhotoView", photoPageArguements);
        }
    }
}
