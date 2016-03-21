using Alsvior.DAL;
using Alsvior.Representations;
using Alsvior.Representations.Interfaces;
using Alsvior.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Alsvior.Api.Controllers
{
    [RoutePrefix("api/weather")]
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
            var nodes = _weather.GetNodes();
            return Ok(nodes);
        }


        [Route("{latitude}/{longitude}/{time}/hourly")]
        public IHttpActionResult GetHourly(int latitude, int longitude, long time)
        {
            var hourlyResult = _cassandra.Get<WeatherHourly>(x => x.Latitude == latitude 
            && x.Longitude == longitude && x.Time == time);
            return Ok(hourlyResult);
        }

        [Route("{latitude}/{longitude}/{time}/daily")]
        public IHttpActionResult GetDaily(int latitude, int longitude, long time)
        {
            var dailyResult = _cassandra.Get<WeatherDaily>(x => x.Latitude == latitude 
            && x.Longitude == longitude && x.Time == time).ToList();
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
