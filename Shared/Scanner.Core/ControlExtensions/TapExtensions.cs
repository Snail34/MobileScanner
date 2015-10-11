using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Scanner.Core.ControlExtensions
{
    public class TapExtensions : DependencyObject
    {
        public static readonly DependencyProperty TappedCommandProperty =
            DependencyProperty.RegisterAttached("TappedCommand",
            typeof(ICommand),
            typeof(TapExtensions),
            new PropertyMetadata(null, TappedCommandChanged));

        public static ICommand GetTappedCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(TappedCommandProperty);
        }

        public static void SetTappedCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(TappedCommandProperty, value);
        }

        private static void TappedCommandChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var uiElement = d as UIElement;

            if (uiElement != null)
            {
                if (e.OldValue != null)
                {
                    uiElement.Tapped -= OnTapped;
                }

                if (e.NewValue != null)
                {
                    uiElement.Tapped += OnTapped;
                }
            }
        }

        private static void OnTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var tappedCommand = GetTappedCommand((DependencyObject)sender);

            if (tappedCommand != null)
            {
                if (tappedCommand.CanExecute((TextBlock)sender))
                {
                    tappedCommand.Execute((TextBlock)sender);
                }
            }
        }

    }
}
