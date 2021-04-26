using Caliburn.Micro;
using Job.Notification.Contracts;
using JobNotificationsClient.Infrastructure.TestData;
using JobNotificationsClient.Messages;
using JobNotificationsClient.Models;
using JobNotificationsClient.Services;
using NServiceBus;
using System;

namespace JobNotificationsClient.ViewModels
{
    public class JobViewModel : ViewModelBase
    , IHandle<TabActivated>
    , IHandle<JobNotificationEvent>
    , IHandle<HubConnected>
    {
        private readonly IBus _bus;
        private readonly IConfiguration _config;

        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(JobViewModel));
        public JobViewModel(IBus bus, IConfiguration config)
        {
            if (bus == null) throw new ArgumentNullException("bus");
            _bus = bus;
            _config = config;
            JobNotifications = new BindableCollection<Models.JobNotification>();
            DisplayName = "Job Notifications";
        }

        public bool CanStart
        {
            get
            {
                return string.IsNullOrEmpty(_config.SignalRId) ? false : true;
            }
        }

        public IObservableCollection<JobNotification> JobNotifications { get; set; }

        public void Handle(TabActivated message)
        {
            JobNotifications.Refresh();
        }

        public void StartJobs()
        {
            //send test job tasks 
            JobTaskFake.Send(_bus, _config);
        }

        public void Handle(JobNotificationEvent message)
        {
            JobNotifications.Add(new JobNotification {
                 JobId = message.JobId
                ,JobStatus = message.JobStatus 
            });
            JobNotifications.Refresh();
        }

        protected override void OnLoadedCore()
        {
            SubscribeToMessages();
        }

        public void Handle(HubConnected message)
        {
            NotifyOfPropertyChange(() => CanStart);
        }
    }
}
