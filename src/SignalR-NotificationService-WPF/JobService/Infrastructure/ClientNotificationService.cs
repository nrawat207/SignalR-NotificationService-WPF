using Job.Notification.Contracts;
using Microsoft.AspNet.SignalR;

namespace JobService
{
    public class ClientNotificationService : IClientNotificationService
    {

        /// <summary>
        /// Job Notification method to notify client
        /// </summary>
        /// <param name="clienId"></param>
        /// <param name="jobId"></param>
        /// <param name="status"></param>
        public void JobNotification(string clientId, int jobId, JobStatus status)
        {
            var hub = GlobalHost.ConnectionManager.GetHubContext<SingalRHub>();

            if (string.IsNullOrEmpty(clientId)) return;

            hub.Clients.Client(clientId).JobNotification(new
            {
                JobId = jobId,
                JobStatus = status,                
            });
        }
    }
}
