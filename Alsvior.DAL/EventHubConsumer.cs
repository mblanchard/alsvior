using Alsvior.Representations.Config;
using Microsoft.ServiceBus.Messaging;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Alsvior.DAL
{
    public class EventHubConsumer : IDisposable
    {
        #region Properties
        //IDisposable
        private bool disposed = false;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        //EventHub
        private EventProcessorHost _eventProcessorHost;
        private EventHubNamespaceConfig _config;
        #endregion Properties

        #region Constructor
        public EventHubConsumer(EventHubNamespaceConfig config, string hubName)
        {
            _config = config;
            string storageConnectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", _config.StorageAccountName, _config.StorageAccountKey);
            string eventProcessorHostName = Guid.NewGuid().ToString();
            var hub = _config.Hubs.FirstOrDefault(x => x.Name == hubName);
            if (hub == null) throw new ArgumentException("Hub Name Not Found");
            _eventProcessorHost = new EventProcessorHost(eventProcessorHostName, hub.Name, EventHubConsumerGroup.DefaultGroupName, _config.ConnectionString, storageConnectionString);

        }
        #endregion Constructor


        #region Methods
        public void RegisterEventProcessor<T>() where T: IEventProcessor
        {
            var options = new EventProcessorOptions();
            options.ExceptionReceived += (sender, e) => { Console.WriteLine(e.Exception); };
            _eventProcessorHost.RegisterEventProcessorAsync<T>(options).Wait();
        }
        #endregion Methods


        #region Dispose
        public void Dispose() { Dispose(true); GC.SuppressFinalize(this); }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                _eventProcessorHost.UnregisterEventProcessorAsync().Wait();
            }
            disposed = true;
        }
        #endregion Dispose
    }
}
