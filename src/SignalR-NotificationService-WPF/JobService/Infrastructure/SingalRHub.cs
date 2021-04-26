using log4net;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin.Hosting;
using NServiceBus;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace JobService
{
    [HubName("JobNotificationHub")]
    public class SingalRHub:Hub
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SingalRHub));

        public override Task OnConnected()
        {
            Log.DebugFormat("{0} connected", Context.ConnectionId);
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            Log.DebugFormat("{0} disconnected", Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }

        public class HubRunner : IWantToRunAtStartup
        {
            private readonly ILog log = LogManager.GetLogger(typeof(HubRunner));
            public void Run()
            {
                var url = ConfigurationManager.AppSettings["SignalRUrl"];
                if (string.IsNullOrEmpty(url))
                {
                    log.WarnFormat("SignalR url missing");
                    return;
                }
                log.DebugFormat("SignalR url {0}", url);

                try
                {
                    WebApp.Start(url);
                }
                catch (Exception ex)
                {
                    log.DebugFormat("{0} : {1}", ex.Message, ex.StackTrace);

                }
            }

            public void Stop()
            {

            }
        }
    }
}
