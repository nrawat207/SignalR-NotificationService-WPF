using StructureMap.Configuration.DSL;
using JobNotificationsClient.ViewModels;

namespace JobNotificationsClient.Infrastructure.IoC
{
    public class ViewModelRegistry : Registry
    {
        public ViewModelRegistry()
        {
            Scan(s =>
            {
                s.TheCallingAssembly();
                s.With(new SingletonConvention<ViewModelBase>());                
                s.AddAllTypesOf<ViewModelBase>();
            });
        }
    }
}
