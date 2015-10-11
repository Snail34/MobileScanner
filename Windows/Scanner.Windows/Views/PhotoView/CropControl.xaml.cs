using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using Windows.Storage;
using Scanner.Core.Extensions;

namespace Scanner.Windows.Views.PhotoView
{
    public sealed partial class CropControl : UserControl
    { 
        public CropControl()
        {
            this.InitializeComponent();

            this.Loaded += CropControl_Loaded;
        }

        private void CropControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.LeftTop.CurrentPosition = new Point(this.LeftTopPosition.X + this.LeftTop.Width / 2,
                this.LeftTopPosition.Y + this.LeftTop.Height / 2);
            
            this.RightTop.CurrentPosition = new Point(this.RightTopPosition.X + this.RightTop.Width / 2,
                this.RightTopPosition.Y + this.RightTop.Height / 2);

            this.RightBottom.CurrentPosition = new Point(this.RightBottomPosition.X + this.RightBottom.Width / 2,
                this.RightBottomPosition.Y + this.RightBottom.Height / 2);

            this.LeftBottom.CurrentPosition = new Point(this.LeftBottomPosition.X + this.LeftBottom.Width / 2,
                this.LeftBottomPosition.Y + this.LeftBottom.Height / 2);
        }

        #region Dependency Properties



        public double ScaleX
        {
            get { return (double)GetValue(ScaleXProperty); }
            set { SetValue(ScaleXProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ScaleX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScaleXProperty =
            DependencyProperty.Register("ScaleX", typeof(double), typeof(CropControl), new PropertyMetadata(0));

        public double ScaleY
        {
            get { return (double)GetValue(ScaleYProperty); }
            set { SetValue(ScaleYProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScaleYProperty =
            DependencyProperty.Register("ScaleY", typeof(double), typeof(CropControl), new PropertyMetadata(0));



        public StorageFile ImageFile
        {
            get { return (StorageFile)GetValue(ImageFileProperty); }
            set { SetValue(ImageFileProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageFile.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageFileProperty =
            DependencyProperty.Register("ImageFile", typeof(StorageFile), typeof(CropControl), new PropertyMetadata(0));
        

        public string ImageFilePath
        {
            get { return (string)GetValue(ImageFilePathProperty); }
            set { SetValue(ImageFilePathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageFilePath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageFilePathProperty =
            DependencyProperty.Register("ImageFilePath", typeof(string), typeof(CropControl), new PropertyMetadata(0));

        public Point LeftTopPosition
        {
            get { return (Point)GetValue(LeftTopPositionProperty); }
            set { SetValue(LeftTopPositionProperty, value); }
        }

        public static readonly DependencyProperty LeftTopPositionProperty =
            DependencyProperty.Register("LeftTopPosition", typeof(Point), typeof(CropControl), new PropertyMetadata(0));

        public Point RightTopPosition
        {
            get { return (Point)GetValue(RightTopPositionProperty); }
            set { SetValue(RightTopPositionProperty, value); }
        }

        public static readonly DependencyProperty RightTopPositionProperty =
            DependencyProperty.Register("RightTopPosition", typeof(Point), typeof(CropControl), new PropertyMetadata(0));

        public Point LeftBottomPosition
        {
            get { return (Point)GetValue(LeftBottomPositionProperty); }
            set { SetValue(LeftBottomPositionProperty, value); }
        }

        public static readonly DependencyProperty LeftBottomPositionProperty =
            DependencyProperty.Register("LeftBottomPosition", typeof(Point), typeof(CropControl), new PropertyMetadata(0));

        public Point RightBottomPosition
        {
            get { return (Point)GetValue(RightBottomPositionProperty); }
            set { SetValue(RightBottomPositionProperty, value); }
        }

        public static readonly DependencyProperty RightBottomPositionProperty =
            DependencyProperty.Register("RightBottomPosition", typeof(Point), typeof(CropControl), new PropertyMetadata(0));

        public double LeftTopX
        {
            get { return (double)GetValue(LeftTopXProperty); }
            set { SetValue(LeftTopXProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LeftTopX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LeftTopXProperty =
            DependencyProperty.Register("LeftTopX", typeof(double), typeof(CropControl), new PropertyMetadata(0));

        public double LeftTopY
        {
            get { return (double)GetValue(LeftTopYProperty); }
            set { SetValue(LeftTopYProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LeftTopY.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LeftTopYProperty =
            DependencyProperty.Register("LeftTopY", typeof(double), typeof(CropControl), new PropertyMetadata(0));

        public double RightTopX
        {
            get { return (double)GetValue(RightTopXProperty); }
            set { SetValue(RightTopXProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RightTopX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RightTopXProperty =
            DependencyProperty.Register("RightTopX", typeof(double), typeof(CropControl), new PropertyMetadata(0));

        public double RightTopY
        {
            get { return (double)GetValue(RightTopYProperty); }
            set { SetValue(RightTopYProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RightTopY.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RightTopYProperty =
            DependencyProperty.Register("RightTopY", typeof(double), typeof(CropControl), new PropertyMetadata(0));


        public double LeftBottomX
        {
            get { return (double)GetValue(LeftBottomXProperty); }
            set { SetValue(LeftBottomXProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LeftBottomX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LeftBottomXProperty =
            DependencyProperty.Register("LeftBottomX", typeof(double), typeof(CropControl), new PropertyMetadata(0));


        public double LeftBottomY
        {
            get { return (double)GetValue(LeftBottomYProperty); }
            set { SetValue(LeftBottomYProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LeftBottomY.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LeftBottomYProperty =
            DependencyProperty.Register("LeftBottomY", typeof(double), typeof(CropControl), new PropertyMetadata(0));

        public double RightBottomY
        {
            get { return (double)GetValue(RightBottomYProperty); }
            set { SetValue(RightBottomYProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RightBottomY.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RightBottomYProperty =
            DependencyProperty.Register("RightBottomY", typeof(double), typeof(CropControl), new PropertyMetadata(0));

        public double RightBottomX
        {
            get { return (double)GetValue(RightBottomXProperty); }
            set { SetValue(RightBottomXProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RightBottomX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RightBottomXProperty =
            DependencyProperty.Register("RightBottomX", typeof(double), typeof(CropControl), new PropertyMetadata(0));

        public double CropActualWidth
        {
            get { return (double)GetValue(CropActualWidthProperty); }
            set { SetValue(CropActualWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CropActualWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CropActualWidthProperty =
            DependencyProperty.Register("CropActualWidth", typeof(double), typeof(CropControl), new PropertyMetadata(0));

        public double CropActualHeight
        {
            get { return (double)GetValue(CropActualHeightProperty); }
            set { SetValue(CropActualHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CropActualHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CropActualHeightProperty =
            DependencyProperty.Register("CropActualHeight", typeof(double), typeof(CropControl), new PropertyMetadata(0));

        
        #endregion

        private void Control_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            var control = sender as CustomControl;

            //Point currentPosition = new Point();

            var transform = this.ImageField.RenderTransform as CompositeTransform;

            if (transform != null && control != null)
            {
                switch ((int)transform.Rotation)
                {
                    case 0:
                    case -360:
                        control.CurrentPosition = this.MoveWithZeroRotation(e.Delta.Translation, control); break;
                    case -90: control.CurrentPosition = this.MoveWithNegative90Rotation(e.Delta.Translation, control); break;
                    case -180: control.CurrentPosition = this.MoveWithNegative180Rotation(e.Delta.Translation, control); break;
                    case -270: control.CurrentPosition = this.MoveWithNegative270Rotation(e.Delta.Translation, control); break;
                }
            }
        }

        private void CheckControlBounds(CustomControl control)
        {
            if ((Canvas.GetLeft(control) + control.Width / 2) < 0) Canvas.SetLeft(control, -control.Width / 2);

            if (Canvas.GetLeft(control) + control.Width / 2 > this.vb.ActualWidth)
                Canvas.SetLeft(control, this.vb.ActualWidth - control.Width / 2);

            if ((Canvas.GetTop(control) + control.Height / 2) < 0) Canvas.SetTop(control, -control.Height / 2);

            if (Canvas.GetTop(control) + control.Height / 2 > this.vb.ActualHeight)
                Canvas.SetTop(control, this.vb.ActualHeight - control.Height / 2);  
        }

        private Point MoveWithZeroRotation(Point translation, CustomControl control)
        {
            double dx = Canvas.GetLeft(control) + translation.X;

            double dy = Canvas.GetTop(control) + translation.Y;

            Canvas.SetLeft(control, dx);

            Canvas.SetTop(control, dy);
            
            this.CheckControlBounds(control);

            Point point = new Point(Canvas.GetLeft(control) + control.Width / 2,
                Canvas.GetTop(control) + control.Height / 2);

            return point;
        }

        private Point MoveWithNegative90Rotation(Point translation, CustomControl control)
        {
            double dx = Canvas.GetLeft(control) - translation.Y;

            double dy = Canvas.GetTop(control) + translation.X;

            Canvas.SetTop(control, dy);

            Canvas.SetLeft(control, dx);

            this.CheckControlBounds(control);

            Point point = new Point(Canvas.GetLeft(control) + control.Width / 2,
                Canvas.GetTop(control) + control.Height / 2);

            return point;
        }

        private Point MoveWithNegative180Rotation(Point translation, CustomControl control)
        {
            double dx = Canvas.GetLeft(control) - translation.X;

            double dy = Canvas.GetTop(control) - translation.Y;

            Canvas.SetTop(control, dy);

            Canvas.SetLeft(control, dx);

            this.CheckControlBounds(control);

            Point point = new Point(Canvas.GetLeft(control) + control.Width / 2,
                Canvas.GetTop(control) + control.Height / 2);

            return point;
        }

        private Point MoveWithNegative270Rotation(Point translation, CustomControl control)
        {
            double dx = Canvas.GetLeft(control) + translation.Y;

            double dy = Canvas.GetTop(control) - translation.X;

            Canvas.SetTop(control, dy);

            Canvas.SetLeft(control, dx);

            this.CheckControlBounds(control);

            Point point = new Point(Canvas.GetLeft(control) + control.Width / 2,
                Canvas.GetTop(control) + control.Height / 2);

            return point;
        }

        private void Viewbox_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Viewbox viewBox = (Viewbox)sender;

            this.CropActualWidth = vb.ActualWidth;

            this.CropActualHeight = vb.ActualHeight;

            //this.Image.Width = vb.ActualWidth;

            //this.Image.Height = vb.ActualHeight;

            this.ScaleX = vb.GetChildScaleX();

            //if (this.ScaleX >= 1) this.ScaleX = 1 / this.ScaleX;

            this.ScaleY = vb.GetChildScaleY();

            //if (this.ScaleY >= 1) this.ScaleY = 1 / this.ScaleY;
        }
    }
}
