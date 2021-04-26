using Job.Notification.Contracts;
using JobNotificationsClient.Services;
using NServiceBus;

namespace JobNotificationsClient.Infrastructure.TestData
{
    public static class JobTaskFake
    {
        
        public static void Send(IBus bus,IConfiguration config)
        {
            bus.Send(new AssignJob
            {
                ClientId = config.SignalRId,
                JobId = 1,
                JobStatus = JobStatus.Assigned,
            });
            bus.Send(new RunJob
            {
                ClientId = config.SignalRId,
                JobId = 1,
                JobStatus = JobStatus.Running,
            });
            bus.Send(new AssignJob
            {
                ClientId = config.SignalRId,
                JobId = 2,
                JobStatus = JobStatus.Assigned,
            });
            bus.Send(new AssignJob
            {
                ClientId = config.SignalRId,
                JobId = 3,
                JobStatus = JobStatus.Assigned,
            });
            bus.Send(new JobCompleted
            {
                ClientId = config.SignalRId,
                JobId = 1,
                JobStatus = JobStatus.Completed,
            });
            bus.Send(new AbortJob
            {
                ClientId = config.SignalRId,
                JobId = 2,
                JobStatus = JobStatus.Failed,
            });
            bus.Send(new RunJob
            {
                ClientId = config.SignalRId,
                JobId = 3,
                JobStatus = JobStatus.Running,
            });
            bus.Send(new JobCompleted
            {
                ClientId = config.SignalRId,
                JobId = 3,
                JobStatus = JobStatus.Completed,
            });
        }
            
    }
}
