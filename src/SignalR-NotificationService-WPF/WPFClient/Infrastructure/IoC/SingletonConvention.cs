using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using StructureMap.TypeRules;
using System;
using System.Linq;

namespace JobNotificationsClient.Infrastructure.IoC
{
    internal class SingletonConvention<TPluginFamily> : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (!type.IsConcrete() || !type.CanBeCreated() || !type.AllInterfaces().Contains(typeof(TPluginFamily))) return;

            registry.For(typeof(TPluginFamily)).Singleton().Use(type);
        }
    }
}
