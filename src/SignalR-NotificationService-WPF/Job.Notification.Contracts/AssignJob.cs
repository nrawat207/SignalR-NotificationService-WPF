namespace Job.Notification.Contracts
{
    public class AssignJob
    {
        public string ClientId { get; set; }
        public int JobId { get; set; }
        public JobStatus JobStatus { get; set; }

    }
}
