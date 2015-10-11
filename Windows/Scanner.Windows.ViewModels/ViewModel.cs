using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanner.Windows.ViewModels
{
    public class ViewModel : ViewModelBase
    {
        public ViewModel()
        {
            this.OnNavigatedToCommand = new RelayCommand<object>(this.OnNavigatedToCommandExecute);
        }

        protected virtual void OnNavigatedToCommandExecute(object obj)
        {
            
        }

        protected virtual void InitializeCommands()
        {

        }

        public RelayCommand<object> OnNavigatedToCommand { get; set; }
    }
}
