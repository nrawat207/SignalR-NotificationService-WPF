using Job.Notification.Contracts;
using log4net;
using NServiceBus;
using StructureMap.Attributes;

namespace JobService.Handler
{
    public class JobHandler : IHandleMessages<AssignJob>
    ,IHandleMessages<RunJob>
    ,IHandleMessages<AbortJob>
    ,IHandleMessages<JobCompleted>
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(JobHandler));

        [SetterProperty]
        public IClientNotificationService NotificationService { get; set; }

        public void Handle(AssignJob message)
        {
            _log.DebugFormat("Assign Job : Job ID {0}", message.JobId);
            NotificationService.JobNotification(message.ClientId, message.JobId, message.JobStatus);
        }

        public void Handle(RunJob message)
        {
            _log.DebugFormat("RunJob Job : Job ID {0}", message.JobId);
            NotificationService.JobNotification(message.ClientId, message.JobId, message.JobStatus);
        }

        public void Handle(AbortJob message)
        {
            _log.DebugFormat("Abort Job : Job ID {0}", message.JobId);
            NotificationService.JobNotification(message.ClientId, message.JobId, message.JobStatus);
        }

        public void Handle(JobCompleted message)
        {
            _log.DebugFormat("Job Completed : Job ID {0}", message.JobId);
            NotificationService.JobNotification(message.ClientId, message.JobId, message.JobStatus);
        }
    }
}
