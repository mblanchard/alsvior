using Alsvior.Representations.Models;
using System;
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
            int count = 0;
            while (true)
            {
                Thread.Sleep(1000);
                var notification = new Notification(41879751, -87634685, DateTime.Now.Ticks, "Notification #" + count++);
                ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);
                if (socket.State == WebSocketState.Open)
                {                 
                    buffer = new ArraySegment<byte>( Encoding.UTF8.GetBytes(notification.ToString()));
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
