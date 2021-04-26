using NServiceBus;
using StructureMap.Configuration.DSL;

namespace JobNotificationsClient.Infrastructure.IoC
{
    public class BusRegistry : Registry
    {
        public BusRegistry()
        {
            For<IBus>().Singleton().Use(StartBus());
        }

        private static IBus StartBus()
        {
            return NServiceBus.Configure
                .With()
                .DefiningCommandsAs(type =>
                {
                    return type.Namespace == "Job.Notification.Contracts";
                })

                .DefaultBuilder()
                .XmlSerializer()
                .MsmqTransport()
                .UnicastBus()
                .SendOnly();
        }
    }
}
