using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Scanner.Core.ControlExtensions
{
    public class RotateExtensions : DependencyObject
    {
        public static readonly DependencyProperty RotateProperty = DependencyProperty.RegisterAttached(
          "RotateRight",
          typeof(CompositeTransform),
          typeof(RotateExtensions),
          new PropertyMetadata(null, OnRotateRightChanged));

        public static readonly DependencyProperty RotateLeftProperty = DependencyProperty.RegisterAttached(
            "RotateLeft",
          typeof(CompositeTransform),
          typeof(RotateExtensions),
          new PropertyMetadata(null, OnRotateLeftChanged));

        public static void SetRotateRight(UIElement element, CompositeTransform value)
        {
            element.SetValue(RotateProperty, value);
        }

        public static CompositeTransform GetRotateRight(UIElement element)
        {
            return (CompositeTransform)element.GetValue(RotateProperty);
        }

        public static void SetRotateLeft(UIElement element, CompositeTransform value)
        {
            element.SetValue(RotateProperty, value);
        }

        public static CompositeTransform GetRotateLeft(UIElement element)
        {
            return (CompositeTransform)element.GetValue(RotateProperty);
        }

        private static void OnRotateLeftChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uiElement = d as FrameworkElement;

            var compositeTransform = e.NewValue as CompositeTransform;

            if (uiElement != null && compositeTransform != null)
            {
                var transform = uiElement.RenderTransform as CompositeTransform;

                if (transform != null)
                {
                    transform.Rotation -= compositeTransform.Rotation;

                    uiElement = ScalingFrameworkElement(uiElement);
                }
            }
        }

        private static void OnRotateRightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uiElement = d as FrameworkElement;

            var compositeTransform = e.NewValue as CompositeTransform;

            if (uiElement != null && compositeTransform != null)
            {
                var transform = uiElement.RenderTransform as CompositeTransform;

                if (transform != null)
                {
                    transform.Rotation += compositeTransform.Rotation;

                    uiElement = ScalingFrameworkElement(uiElement);
                }
            }
        }

        private static FrameworkElement ScalingFrameworkElement(FrameworkElement element)
        {
            var compositeTransform = element.RenderTransform as CompositeTransform;

            switch ((int)compositeTransform.Rotation)
            {
                case -90:
                case 90:
                case 270:
                case -270:
                    {
                        element.Width = element.ActualWidth / 2;
                        element.Height = element.ActualHeight / 2;
                    } break;
                case 0:
                case 180:
                case -180:
                    {
                        element.Width = element.ActualWidth * 2;
                        element.Height = element.ActualHeight * 2;
                    } break;
                case -360:
                case 360:
                    {
                        compositeTransform.Rotation = 0;
                        element.Width = element.ActualWidth * 2;
                        element.Height = element.ActualHeight * 2;
                    } break;
            }

            return element;
        }
    }
}
