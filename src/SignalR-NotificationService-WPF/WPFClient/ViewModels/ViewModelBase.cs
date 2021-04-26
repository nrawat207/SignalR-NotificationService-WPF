using Caliburn.Micro;
using StructureMap.Attributes;
using System.Windows;

namespace JobNotificationsClient.ViewModels
{
    public abstract class ViewModelBase : Screen
    {
        [SetterProperty]
        public IEventAggregator EventAggregator { get; set; }

        protected override void OnViewLoaded(object view)
        {
            OnLoadedCore();
        }

        protected abstract void OnLoadedCore();

        public void SubscribeToMessages()
        {
            EventAggregator.Subscribe(this);
        }
        public void Focus()
        {
            var window = GetView() as Window;
            if (window != null) window.Activate();
        }
        internal bool CanUpdateView()
        {
            if (IsBusy) return false;
            IsBusy = true;
            return true;
        }

        internal void ViewUpdateCompleted()
        {
            IsBusy = false;
        }
        internal bool IsBusy { get; private set; }

        public void ShowErrorMessage(string displayname, string message)
        {
            Execute.OnUIThread(() => MessageBox.Show(message, displayname, MessageBoxButton.OK, MessageBoxImage.Error));
        }

    }
}
