using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Scanner.Windows.Views.DocumentView;
using Scanner.Windows.Views.MainView;
using Scanner.Windows.Views.PhotoView;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Scanner.Windows.Views.CurrentCategoryView;
using GalaSoft.MvvmLight.Messaging;
using Scanner.Windows.Views.PostProccesView;
using Scanner.Core.Enums;
using Scanner.Core.Helpers;
using Scanner.Windows.ViewModels;

namespace Scanner.Windows
{
    sealed partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
//#if DEBUG
//            if (System.Diagnostics.Debugger.IsAttached)
//            {
//                this.DebugSettings.EnableFrameRateCounter = true;
//            }
//#endif
            SimpleIoc.Default.Register<INavigationServiceEx, NavigationServiceEx>();

            SimpleIoc.Default.Register<IMessenger, Messenger>();

            var service = SimpleIoc.Default.GetInstance<INavigationServiceEx>();

            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();

                this.ConfigureNavigationService((NavigationServiceEx)service);
                
                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    
                }

                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                service.NavigateTo(NavigationSource.MainView.ToString());
            }

            if (e != null && !string.IsNullOrEmpty(e.Arguments))
            {
                ArguementHelper.LaunchAppArguements launchArgs = ArguementHelper.ParseArguementString(e.Arguments);

                service.NavigateTo(launchArgs.PageDestinationType, launchArgs.TileId);
            }

            Window.Current.Activate();
        }

        /// <summary>
        /// Вызывается при приостановке выполнения приложения.  Состояние приложения сохраняется
        /// без учета информации о том, будет ли оно завершено или возобновлено с неизменным
        /// содержимым памяти.
        /// </summary>
        /// <param name="sender">Источник запроса приостановки.</param>
        /// <param name="e">Сведения о запросе приостановки.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Сохранить состояние приложения и остановить все фоновые операции
            deferral.Complete();
        }

        private void ConfigureNavigationService(NavigationServiceEx navigationService)
        {
            navigationService.Configure(NavigationSource.MainView.ToString(), typeof(MainView));
          
            navigationService.Configure(NavigationSource.PhotoView.ToString(), typeof(PhotoView));

            navigationService.Configure(NavigationSource.DocumentView.ToString(), typeof(DocumentView));

            navigationService.Configure(NavigationSource.CurrentCategoryView.ToString(), typeof(CurrentCategoryView));
            
            navigationService.Configure(NavigationSource.PostProccesView.ToString(), typeof(PostProccesView));
        }
    }
}
