using Alsvior.Representations.Interfaces;
using Alsvior.Representations.Models;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Alsvior.Api.Controllers.Geospatial
{
    [RoutePrefix("api/weatherDaily")]
    public class WeatherDailyController : GeospatialTimeSeriesDatesetController<WeatherDaily>
    {
        public WeatherDailyController(ICassandraClient client) : base(client) { }

    }
}
