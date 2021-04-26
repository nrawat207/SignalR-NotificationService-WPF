using Owin;

namespace JobService
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            appBuilder.MapSignalR();
        }
    }
}
