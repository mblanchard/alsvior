using Alsvior.Core;
using Alsvior.Representations.Interfaces;
using Alsvior.Representations.Models;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Alsvior.Api.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/weather")]
    [Authorize]
    public class WeatherController : ApiController
    {
        private ICassandraClient _cassandra;
        private IWeatherClientWrapper _weather;

        public WeatherController(ICassandraClient cassandra, IWeatherClientWrapper weather)
        {
            _cassandra = cassandra;
            _weather = weather;
        }

        [Route("nodes")]
        public IHttpActionResult GetNodes()
        {
            var nodes = _cassandra.Get<WeatherNode>();
            return Ok(nodes);
        }

        [Route("{latitude}/{longitude}/nodes")]
        public IHttpActionResult GetNodesForCoord(int latitude, int longitude)
        {
            var nodes = _cassandra.Get<WeatherNode>();
            var filter = new DistanceFilter<WeatherNode>(latitude, longitude, 40000); //40 km
            return Ok(filter.FilterCoords(nodes));
        }

        [Route("{latitude}/{longitude}/{time}/hourly")]
        public IHttpActionResult GetHourly(int latitude, int longitude, long time)
        {
            var hourlyResult = _cassandra.Get<WeatherHourly>(x => x.Latitude == latitude
            && x.Longitude == longitude && x.Time == time).ToList();
            return Ok(hourlyResult);
        }

        [Route("{latitude}/{longitude}/{time}/daily")]
        public IHttpActionResult GetDaily(int latitude, int longitude, long time)
        {
            var dailyResult = _cassandra.Get<WeatherDaily>(x => x.Latitude == latitude 
            && x.Longitude == longitude && x.Time == time).ToList();
            return Ok(dailyResult);
        }

        [Route("{latitude}/{longitude}/daily")]
        public IHttpActionResult GetMostRecentDaily(int latitude, int longitude)
        {
            var dailyResult = _cassandra.Get<WeatherDaily>(x => x.Latitude == latitude
            && x.Longitude == longitude).OrderByDescending(x=> x.Time).FirstOrDefault();
            return Ok(dailyResult);
        }


        [Route("{latitude}/{longitude}/{timeStart}/{timeEnd}/hourly")]
        public IHttpActionResult GetHourlyRange(int latitude, int longitude, long timeStart, long timeEnd)
        {
            var hourlyResult = _cassandra.Get<WeatherHourly>(x => x.Latitude == latitude
            && x.Longitude == longitude && x.Time >= timeStart && x.Time <= timeEnd);
            return Ok(hourlyResult);
        }

        [Route("{latitude}/{longitude}/{timeStart}/{timeEnd}/daily")]
        public IHttpActionResult GetDailyRange(int latitude, int longitude, long timeStart, long timeEnd)
        {
            var dailyResult = _cassandra.Get<WeatherDaily>(x => x.Latitude == latitude
            && x.Longitude == longitude && x.Time >= timeStart && x.Time <= timeEnd).ToList();
            return Ok(dailyResult);
        }

    }
}
