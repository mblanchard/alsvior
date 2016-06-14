using Alsvior.Representations.Interfaces;
using Alsvior.Representations.Models;
using Alsvior.Utility;
using System;
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
    public abstract class GeospatialTimeSeriesDatesetController<T> : GeospatialDatasetController<T> where T : class, ILocatable, IChronometricable
    {
        public GeospatialTimeSeriesDatesetController(ICassandraClient client): base(client) { }

        [Authorize]
        [Route("{latitude}/{longitude}/{time}")]
        public virtual IHttpActionResult GetAtTime(int latitude, int longitude, long time)
        {
            var range = TimeConversion.GetDailyTimestampRange(time, longitude);
            var dailyResult = _session.Get<T>(x => 
                x.Latitude == latitude && 
                x.Longitude == longitude && 
                x.Time >= range.Item1 && x.Time <= range.Item2
            ).FirstOrDefault();

            return Ok(dailyResult);
        }

        [Authorize]
        [Route("{latitude}/{longitude}/latest")]
        public virtual IHttpActionResult GetLatest(int latitude, int longitude)
        {
            return GetAtTime(latitude, longitude, TimeConversion.ConvertToTimestamp(DateTime.Now));
        }

        [Authorize]
        [Route("{latitude}/{longitude}/{timeStart}/{timeEnd}")]
        public virtual IHttpActionResult GetInTimeRange(int latitude, int longitude, long timeStart, long timeEnd)
        {
            var result = _session.Get<T>(x => 
                x.Latitude == latitude && 
                x.Longitude == longitude && 
                x.Time >= timeStart && x.Time <= timeEnd
            );
            return Ok(result);
        }


        [Route("{token}/connect")]
        public virtual HttpResponseMessage Connect(string token)
        {
            if (HttpContext.Current.IsWebSocketRequest) { HttpContext.Current.AcceptWebSocketRequest(ProcessNotification); }
            return new HttpResponseMessage(HttpStatusCode.SwitchingProtocols);
        }

        private async Task ProcessNotification(AspNetWebSocketContext context)
        {
            WebSocket socket = context.WebSocket;
            int count = _session.Get<T>().Count();
            var randomGen = new Random();
            while (true)
            {
                var sleepDuration = (int)(randomGen.NextDouble() * 1000);

                int id = (int)(randomGen.NextDouble() * count);
                var node = _session.Get<T>().Skip(id).FirstOrDefault();
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
        private double generateMockInverterPerformance(int latitude, int longitude, Random rand) { return (Math.Abs((double)((latitude + longitude) % 10000)) / 10000) + (rand.NextDouble() / 20); }
    }
}
