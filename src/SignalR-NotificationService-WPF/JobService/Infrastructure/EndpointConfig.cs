using NServiceBus;
using StructureMap;
using System;

namespace JobService
{
    public class EndpointConfig : IWantCustomInitialization, IConfigureThisEndpoint, AsA_Server
    {
        public void Init()
        {
            var container = ConfigureStructureMap();
            log4net.Config.XmlConfigurator.Configure();
            var assemblies = AllAssemblies
                .Except("Microsoft.AspNet.SignalR.Owin.dll");

            Configure.With()
                .DefiningCommandsAs(t =>
                {
                    return t.Namespace != null
                         && ((t.Namespace.EndsWith("Commands")
                               || (t.Namespace.EndsWith("Contracts"))
                             ));
                })                
                .StructureMapBuilder(container)
                 .XmlSerializer()
                .DisableSecondLevelRetries()
                .DisableTimeoutManager()
                .MsmqSubscriptionStorage();
        }

        private static IContainer ConfigureStructureMap()
        {
            return new Container(x =>
            {
                x.For<IClientNotificationService>().Singleton().Use<ClientNotificationService>();
            });
        }
    }
}
