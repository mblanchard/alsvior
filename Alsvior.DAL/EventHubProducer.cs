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
    public class EventHubProducer : IDisposable
    {
        #region Properties
        //IDisposable
        private bool disposed = false;  
        private     
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        //EventHub
        private EventHubClient _client;
        private EventHubNamespaceConfig _config;
        #endregion Properties

        #region Constructor
        public EventHubProducer(EventHubNamespaceConfig config, string hubName)
        {
            _config = config;
            var matchingHub = _config.Hubs.FirstOrDefault(x => x.Name == hubName);
            if (matchingHub == null) throw new ArgumentException("Hub Name Not Found");
            _client = EventHubClient.CreateFromConnectionString(matchingHub.GetConnectionString(_config.ConnectionString));
           
        }
        #endregion Constructor


        #region Methods
        public async Task SendAsync(Byte[] msg)
        {
            await _client.SendAsync(new EventData(msg));
        }

        public async Task SendBatchAsync(Byte[][] msgs)
        {
            await _client.SendBatchAsync(msgs.Select(x => new EventData(x)));
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
                _client.Close();
            }
            disposed = true;
        }
        #endregion Dispose
    }
}
