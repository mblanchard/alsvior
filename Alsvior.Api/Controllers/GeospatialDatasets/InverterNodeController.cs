using Alsvior.Representations.Interfaces;
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

namespace Alsvior.Api.Controllers.GeospatialDatasets
{
    [RoutePrefix("api/inverterNode")]
    public class InverterNodeController : GeospatialDatasetController<InverterNode>
    {
        public InverterNodeController(ICassandraClient client) : base(client) {
        }

        #region Extensions
        [Route("{latitude}/{longitude}/latest")]
        [Authorize]
        public  IHttpActionResult GetLatest(int latitude, int longitude)
        {
            var rand = new Random();
            var mockPerf = generateMockInverterPerformance(latitude, longitude, rand);
            var dailyResult = new InverterData() { Latitude = latitude, Longitude = longitude, Performance = mockPerf };
            return Ok(dailyResult);
        }
        [HttpGet]
        [Route("{token}/connect")]
        public HttpResponseMessage Connect(string token)
        {
            if (HttpContext.Current.IsWebSocketRequest) { HttpContext.Current.AcceptWebSocketRequest(ProcessNotification); }
            return new HttpResponseMessage(HttpStatusCode.SwitchingProtocols);
        }

        private async Task ProcessNotification(AspNetWebSocketContext context)
        {
            WebSocket socket = context.WebSocket;
            int count = _session.Get<InverterNode>().Count();
            var randomGen = new Random();
            while (true)
            {
                var sleepDuration = (int)(randomGen.NextDouble() * 1000);

                int id = (int)(randomGen.NextDouble() * count);
                var node = _session.Get<InverterNode>().Skip(id).FirstOrDefault();
                if (node == null) continue;

                var status = generateMockInverterPerformance(node.Latitude, node.Longitude, randomGen).ToString();
                var notification = new Notification(node.Latitude, node.Longitude, DateTime.Now.Ticks, status);
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
                Thread.Sleep(sleepDuration);
            }

        }

        private double generateMockInverterPerformance(int latitude, int longitude, Random rand) {  return (Math.Abs((double)((latitude + longitude) % 10000)) / 10000) + (rand.NextDouble() / 20); }
        #endregion Extensions

    }
}
