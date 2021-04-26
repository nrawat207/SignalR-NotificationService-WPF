using Caliburn.Micro;
using log4net.Config;
using MahApps.Metro.Controls.Dialogs;
using StructureMap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using JobNotificationsClient.Infrastructure.IoC;
using JobNotificationsClient.ViewModels;

namespace JobNotificationsClient.Infrastructure
{
    public class AppBootstrapper: BootstrapperBase
    {
        private IContainer container;
        private readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(AppBootstrapper));
        private Mutex mutex;

        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            XmlConfigurator.Configure();
            container = new Container(x =>
            {

                x.For<IWindowManager>().Singleton().Use<WindowManager>();
                x.For<IEventAggregator>().Singleton().Use<EventAggregator>();
                x.For<IViewModelFactory>().Singleton().Use<ViewModelFactory>();

                x.AddRegistry<BusRegistry>();
                x.AddRegistry<ServiceRegistry>();
                x.AddRegistry<ViewModelRegistry>();

                x.Scan(s =>
                {

                    s.With(new SingletonConvention<ViewModelBase>());                    
                    s.AddAllTypesOf<IScreen>().NameBy(t => t.Name.Replace("ViewModel", ""));
                    s.TheCallingAssembly();
                });
            });

            Log.Info("Bootstrapper completed");            
           
        }
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            Log.Info("OnStartup 0");
            bool onlyApplicationInstance;
            mutex = new Mutex(true, "JobNotificationsClient", out onlyApplicationInstance);
            if (!onlyApplicationInstance)
            {
                MessageBox.Show("An instance of Job Notification client is already running", "Job Notifications", MessageBoxButton.OK, MessageBoxImage.Information);
                Application.Current.Shutdown();
                Log.Info("OnStartup. Shutdown. An existing instance already running");
                return;
            }
            EventManager.RegisterClassHandler(typeof(TextBox), UIElement.GotFocusEvent, new RoutedEventHandler(TextBoxGotFocus));
            base.OnStartup(sender, e);
            Application.DispatcherUnhandledException += OnDispatcherUnhandledExceptionHandler;
            AppDomain.CurrentDomain.UnhandledException += OnCurrentDomainUnhandledException;
            TaskScheduler.UnobservedTaskException += OnTaskSchedulerUnobservedTaskException;
            DisplayRootViewFor<MainViewModel>();

            StartApplicationServices();
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            return string.IsNullOrEmpty(key) ? container.GetInstance(serviceType) : container.GetInstance(serviceType, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return container.GetAllInstances(serviceType).Cast<object>();
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }

        protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var result = DialogService.ShowMessage(GetExceptionMessages(e.Exception), MessageDialogStyle.Affirmative, "Error");
            e.Handled = true;
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            Log.Debug("Application exit");
            base.OnExit(sender, e);
        }

        #region Private Methods
        private void StartApplicationServices()
        {
            var services = container.GetAllInstances<IApplicationService>().ToList();
            services.ForEach(x => x.Start());
        }

        private void TextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null) textBox.SelectAll();
        }

        private void OnTaskSchedulerUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Log.Error("General exception (task)", e.Exception);
            if (e.Observed) return;
            if (e.Exception.InnerExceptions.Any(x => x is System.Net.Http.HttpRequestException))
                return;
            Execute.OnUIThread(() => MessageBox.Show(GetExceptionMessages(e.Exception), "Handle Error", MessageBoxButton.OK, MessageBoxImage.Error));
        }

        private void OnCurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.IsTerminating) return;
            var exception = e.ExceptionObject as Exception;
            Log.Error("General exception (current domain)", exception);
            Execute.OnUIThread(() => MessageBox.Show(GetExceptionMessages(exception), "Handle Error", MessageBoxButton.OK, MessageBoxImage.Error));
        }

        private void OnDispatcherUnhandledExceptionHandler(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Log.Error("General exception", e.Exception);
            if (e.Handled) return;
            MessageBox.Show(GetExceptionMessages(e.Exception), "Handle Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private string GetExceptionMessages(Exception e)
        {
            if (e == null) return "Unexpected error (no exception found)";
            var sb = new StringBuilder();
            sb.AppendLine(e.Message);
            var current = e.InnerException;
            while (current != null)
            {
                sb.Insert(0, string.Format("{0}{1}", current.Message, Environment.NewLine));
                current = current.InnerException;
            }
            return sb.ToString();
        }

        #endregion
    }
}
