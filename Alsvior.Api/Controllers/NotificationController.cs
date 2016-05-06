using Alsvior.Representations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.WebSockets;

namespace Alsvior.Api.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/notification")]
    public class NotificationController : ApiController
    {
        private static List<string> statusTemplates = new List<string>() { "Storm to reach #{0} in {1} minutes", "Inverter #{0} operating at {1}% capacity", "Inverter #{0} unresponsive for {1} minutes", "Inverter #{0}{1} back online", "Updated weather for location #{0}{1}" };
        public HttpResponseMessage Get()
        {
            if (HttpContext.Current.IsWebSocketRequest)
            {
                HttpContext.Current.AcceptWebSocketRequest(ProcessNotification);
            }
            return new HttpResponseMessage(HttpStatusCode.SwitchingProtocols);
        }

        private async Task ProcessNotification(AspNetWebSocketContext context)
        {

            WebSocket socket = context.WebSocket;
            var randomGen = new Random();
            while (true)
            {
                var sleepDuration = (int)(randomGen.NextDouble() * 6000);
                Thread.Sleep(sleepDuration);
                var statusTemplate = statusTemplates[(int)(randomGen.NextDouble()* statusTemplates.Count)];
                var node = (int)(randomGen.NextDouble() * 100); var value = (int)(randomGen.NextDouble() * 30);
                var notification = new Notification(41879751, -87634685, DateTime.Now.Ticks, String.Format(statusTemplate,node,value));
                ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);
                if (socket.State == WebSocketState.Open)
                {
                    buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(notification.ToString()));
                    await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                }
                else
                {
                    break;
                }
            }
        }
    }
}
