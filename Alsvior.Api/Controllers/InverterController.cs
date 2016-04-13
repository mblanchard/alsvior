using Alsvior.Representations.Interfaces;
using Alsvior.Representations.Models;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Alsvior.Api.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/inverter")]
    [Authorize]
    public class InverterController : ApiController
    {
        private ICassandraClient _cassandra;

        public InverterController(ICassandraClient cassandra)
        {
            _cassandra = cassandra;
        }

        [Route("nodes")]
        public IHttpActionResult GetNodes()
        {
            var nodes = _cassandra.Get<InverterNode>();
            return Ok(nodes);
        }

        [Route("{latitude}/{longitude}")]
        public IHttpActionResult GetMostRecentDaily(int latitude, int longitude)
        {
            var dailyResult = _cassandra.Get<InverterData>(x => x.Latitude == latitude
            && x.Longitude == longitude).FirstOrDefault();
            return Ok(dailyResult);
        }

    }
}
