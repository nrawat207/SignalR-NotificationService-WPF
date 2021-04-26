using StructureMap;
using System;
using JobNotificationsClient.ViewModels;

namespace JobNotificationsClient.Infrastructure
{
    public class ViewModelFactory : IViewModelFactory
    {
        private readonly IContainer container;

        public ViewModelFactory(IContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");
            this.container = container;
        }

        public T Create<T>()
        {
            return container.GetInstance<T>();
        }

        public T Create<T>(string key)
            where T : ViewModelBase
        {
            return container.GetInstance<T>(key);
        }
    }
}
