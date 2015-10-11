using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Scanner.Windows.Common
{
    public class ViewBase : Page
    {
        public static readonly DependencyProperty OnNavigatedToCommandProperty =
            DependencyProperty.Register("OnNavigatedToCommand", 
            typeof(ICommand), 
            typeof(ViewBase), 
            new PropertyMetadata(null));

        public ICommand OnNavigatedToCommand
        {
            get { return (ICommand)GetValue(OnNavigatedToCommandProperty); }
            set { SetValue(OnNavigatedToCommandProperty, value); }
        }

        protected override void OnNavigatedTo(global::Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (this.OnNavigatedToCommand != null && e.Parameter != null)
            {
                this.OnNavigatedToCommand.Execute(e.Parameter);
            }
        }
    }
}
