using JobNotificationsClient.Services;
using StructureMap.Configuration.DSL;

namespace JobNotificationsClient.Infrastructure.IoC
{
    public class ServiceRegistry : Registry
    {
        public ServiceRegistry()
        {
            For<IConfiguration>().Singleton().Use<Configuration>();
            For<IApplicationService>().Singleton().Use<SignalRService>();
        }
    }
}
