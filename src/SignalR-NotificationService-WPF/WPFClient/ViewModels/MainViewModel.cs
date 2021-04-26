using Caliburn.Micro;
using System;
using JobNotificationsClient.Infrastructure;
using System.Windows;
using JobNotificationsClient.Messages;

namespace JobNotificationsClient.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private int _selectedIndex;
        private readonly IWindowManager _windowManager;
        private readonly IViewModelFactory _viewModelFactory;

        public MainViewModel(IViewModelFactory viewModelFactory, IWindowManager windowManager)
        {
            if (viewModelFactory == null) throw new ArgumentNullException("viewModelFactory");
            if (windowManager == null) throw new ArgumentNullException("windowManager");
            _viewModelFactory = viewModelFactory;
            _windowManager = windowManager;

            TabItems = new BindableCollection<IScreen>();
            InitializeView();
        }

        public BindableCollection<IScreen> TabItems { get; set; }
        public string Title { get; set; }
        public bool Busy { get; set; }
        public bool NotBusy { get { return !Busy; } }      
        public int SelectedTabIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (_selectedIndex == value) return;
                _selectedIndex = value;

                NotifyOfPropertyChange(() => SelectedTabIndex);

                if (_selectedIndex == -1) return;

                TabSelected();
            }
        }


        private void InitializeView()
        {
            AddTabItem(_viewModelFactory.Create<JobViewModel>());
        }

        protected override void OnLoadedCore()
        {
            SubscribeToMessages();
        }

        protected string GetApplicationVersion()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version;
            return string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build);
        }

        public void ExitApplication()
        {
            Application.Current.Shutdown();
        }

        private void TabSelected()
        {

            EventAggregator.PublishOnBackgroundThread(new TabActivated
            {
                ActivatedTab = TabItems[_selectedIndex].DisplayName
            });
        }

        private void AddTabItem(IScreen screen)
        {
            TabItems.Add(screen);
        }
    }
}
