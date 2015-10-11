using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Scanner.Models;
using Scanner.Core;
using Scanner.Windows.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Scanner.DataAccess;
using System.Windows.Input;
using Windows.UI.Xaml.Media;
using Scanner.Models.Arguements;

namespace Scanner.Windows.ViewModels
{
    public class PhotoViewModel : ViewModel
    {
        private StorageFile imageSource;

        private double angle;

        private CropControlPointsData pointsData;

        public PhotoViewModel()
        {
            this.NavigationContext = SimpleIoc.Default.GetInstance<NavigationContext>();

            this.PointsData = new CropControlPointsData() 
            { 
                LeftTopPoint = new Point(200, 320),
                LeftTopLinePoint = new Point(200, 320),
                RightTopPoint = new Point(500, 500),
                RightTopLinePoint = new Point(500, 500),
                LeftBottomPoint = new Point(100, 300),
                LeftBottomLinePoint = new Point(100, 300),
                RightBottomLinePoint = new Point(500, 600),
                RightBottomPoint = new Point(500, 600)
            };

            this.InitializeCommands();
        }

        #region Properties
        public NavigationContext NavigationContext { get; set; }

        public PhotoCapturedData CurrentCapturedData { get; set; }

        public Page PageToProcces { get; set; }

        public StorageFile CapturedImage
        {
            get
            {
                return imageSource;
            }

            set
            {
                imageSource = value;
                this.RaisePropertyChanged();
            }
        }

        public double RotationAngle 
        { 
            get
            {
                return angle;
            }
            set
            {
                angle = value;
                this.RaisePropertyChanged();
            }
        }

        public ObservableCollection<double> Points { get; set; }

        public CropControlPointsData PointsData 
        {
            get
            {
                return this.pointsData;
            }

            set
            {
                this.pointsData = value;
                this.RaisePropertyChanged();
            }
        }

        public double CropControlWidth { get; set; }

        public double CropControlHeight { get; set; }

        public string ImageFilePath { get; set; }

        bool IsNew { get; set; }

        public double ScaleX { get; set; }

        public double ScaleY { get; set; }

        #endregion

        #region Commands
        public ICommand RotateRightCommand { get; private set; }

        public ICommand RotateLeftCommand { get; private set; }

        public ICommand ProccesCommand { get; private set; }

        public ICommand BackCommand { get; private set; }

        public ICommand DeletePageCommand { get; private set; }

        public ICommand ResetEdgesCommand { get; private set; }

        public ICommand LoadPointPresetsCommand { get; private set; } 
        #endregion

        protected async override void OnNavigatedToCommandExecute(object obj)
        {
            if (obj != null)
            {
                var arguements = obj as PhotoPageArguements;

                if (arguements != null)
                {
                    this.IsNew = arguements.IsNew;

                    this.PageToProcces = arguements.PageToProcces;

                    this.CapturedImage = await StorageFile.GetFileFromApplicationUriAsync(new Uri(this.PageToProcces.FilePath));
                }
            }
        }

        protected override void InitializeCommands()
        {
            this.RotateRightCommand = new RelayCommand(this.DoRotateRight);

            this.RotateLeftCommand = new RelayCommand(this.DoRotateLeft);

            this.ProccesCommand = new RelayCommand(async () => { await this.Procces(); });

            this.BackCommand = new RelayCommand(async () => { await this.Back(); });

            this.DeletePageCommand = new RelayCommand(this.DeletePage);

            this.ResetEdgesCommand = new RelayCommand(this.ResetEdges);
        }

        private void ResetEdges()
        {
            this.PointsData = new CropControlPointsData()
            {
                LeftTopPoint = new Point(-15, -15),
                LeftTopLinePoint = new Point(0, 0),
                RightTopPoint = new Point(this.CropControlWidth - 15, -15),
                RightTopLinePoint = new Point(this.CropControlWidth, 0),
                LeftBottomPoint = new Point(-15, this.CropControlHeight - 15),
                LeftBottomLinePoint = new Point(0, this.CropControlHeight),
                RightBottomLinePoint = new Point(this.CropControlWidth, this.CropControlHeight),
                RightBottomPoint = new Point(this.CropControlWidth - 15, this.CropControlHeight - 15)
            };
        }

        private void DoRotateRight()
        {
            this.RotationAngle += 90;
        }

        private void DoRotateLeft()
        {
            this.RotationAngle -= 90;
        }

        private async Task Back()
        {
            if (this.IsNew)
            {
                await this.Cancel();
            }

            this.NavigationContext.NavigationService.GoBack();
        }

        private async void DeletePage()
        {
            await SerializationProvider.Instance.DeletePages(new List<Guid>() { this.PageToProcces.ID}, 
                this.PageToProcces.DocumentId);

            var arguements = new DocumentPageArguements()
            {
                DocumentId = this.PageToProcces.DocumentId,
                PreviousPageName = "PhotoView",
            };

            this.NavigationContext.NavigationService.NavigateTo("DocumentView", arguements);
        }

        private async Task Procces()
        {
            if (!this.IsNew)
            {
                await LocalStorageHelper.RewritePageFiles(this.PageToProcces);
            }

            var pr = new ImageProccesing();

            //var f = await ApplicationData.Current.LocalFolder.GetFileAsync(this.PageToProcces.FilePath);

            IList<Point> pnts = new List<Point>();

            pnts.Add(new Point(this.PointsData.LeftTopLinePoint.X * this.ScaleX,
    this.PointsData.LeftTopLinePoint.Y * this.ScaleY));

            pnts.Add(new Point(this.PointsData.LeftBottomLinePoint.X * this.ScaleX, 
                this.PointsData.LeftBottomLinePoint.Y * this.ScaleY));

            pnts.Add(new Point(this.PointsData.RightBottomLinePoint.X * this.ScaleX,
                this.PointsData.RightBottomLinePoint.Y * this.ScaleY));

            pnts.Add(new Point(this.PointsData.RightTopLinePoint.X * this.ScaleX,
                this.PointsData.RightTopLinePoint.Y * this.ScaleY));

            pr.Quadraliteral(this.CapturedImage, pnts);

            await ApplicationData.Current.LocalFolder.CreateFileAsync(
                this.PageToProcces.ID.ToString() + "_processing" + ".jpg", CreationCollisionOption.ReplaceExisting);

            this.NavigationContext.NavigationService.GoBack();
        }

        private async Task Cancel() // debug
        {
            await SerializationProvider.Instance.DeleteDocuments(new List<Guid>() { this.PageToProcces.DocumentId });
        }
    }
}
