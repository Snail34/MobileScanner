using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanner.Windows.ViewModels
{
    public class NavigationContext
    {
        private INavigationServiceEx navigationService;

        public NavigationContext()
        {
            this.navigationService = SimpleIoc.Default.GetInstance<INavigationServiceEx>();
        }

        public INavigationServiceEx NavigationService
        {
            get
            {
                return this.navigationService;
            }
            set
            {
                this.navigationService = value;
            }
        }
    }
}
