using Caliburn.Micro;
using Job.Notification.Contracts;
using System;

namespace JobNotificationsClient.Models
{
    public class JobNotification: PropertyChangedBase
    {
        public JobNotification()
        {
            Timestamp = DateTime.Now;
        }
        public int JobId { get; set; }
        public JobStatus JobStatus { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
