using Job.Notification.Contracts;

namespace JobNotificationsClient.Messages
{
    public class JobNotificationEvent
    {        
        public int JobId { get; set; }
        public JobStatus JobStatus { get; set; }
    }
}
