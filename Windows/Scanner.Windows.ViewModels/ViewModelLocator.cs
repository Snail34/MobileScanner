using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using Scanner.Windows.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanner.Windows.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            
            SimpleIoc.Default.Register<NavigationContext>();

            SimpleIoc.Default.Register<MainViewModel>();
        }

        public NavigationContext NavigationContext
        {
            get
            {
                return SimpleIoc.Default.GetInstance<NavigationContext>();
            }
        }
    }
}
