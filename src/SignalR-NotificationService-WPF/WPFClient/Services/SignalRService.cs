using Caliburn.Micro;
using JobNotificationsClient.Infrastructure;
using JobNotificationsClient.Messages;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobNotificationsClient.Services
{
    public class SignalRService : IApplicationService
    {
        private readonly IEventAggregator _eventAggregator;
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(SignalRService));
        private HubConnection hubConnection;
        private IConfiguration _config;
        private IHubProxy hubProxy;

        public SignalRService(IEventAggregator eventAggregator, IConfiguration config)
        {
            _eventAggregator = eventAggregator;
            _config = config;
        }

        public void Start()
        {
            try
            {
                var url = ConfigurationManager.AppSettings["SignalRUrl"];
                hubConnection = new HubConnection(url);
                hubConnection.Closed += OnHubConnectionClosed;
                hubConnection.Error += OnHubConnectionError;
                hubConnection.StateChanged += OnStateChanged;

                hubProxy = hubConnection.CreateHubProxy("JobNotificationHub");
                hubProxy.On<JobNotificationEvent>("JobNotification", OnJobNotification);
                            }
            catch (Exception ex)
            {
                Log.Error("Create signalR hub failed..", ex);
            }
            Connect();
        }

        private void OnJobNotification(object obj)
        {
            _eventAggregator.PublishOnBackgroundThread(obj);
        }

        private void OnStateChanged(StateChange obj)
        {
            if (obj.NewState == ConnectionState.Connected)
            {
                _config.SignalRId = hubConnection.ConnectionId;
                _eventAggregator.PublishOnBackgroundThread(new HubConnected());
            }
        }

        public void Stop()
        {
            if (hubConnection == null) return;
            hubConnection.Closed -= OnHubConnectionClosed;
            hubConnection.Error -= OnHubConnectionError;
            hubConnection.Stop();
        }

        

        private void Connect()
        {
            try
            {
                hubConnection.Start();
            }
            catch (Exception ex)
            {
                Log.Error("Connect to SignalR server failed..", ex);
            }
        }

        

        private void OnHubConnectionError(Exception obj)
        {
            Log.WarnFormat("OnHubConnectionError: {0}", obj.Message);
        }

        private void OnHubConnectionClosed()
        {
            Log.WarnFormat("OnHubConnectionClosed");
            Connect();
        }

       
    }
}
