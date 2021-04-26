using Job.Notification.Contracts;

namespace JobService
{
    public interface IClientNotificationService
    {
        void JobNotification(string clientId, int jobId, JobStatus status);
        

    }
}
