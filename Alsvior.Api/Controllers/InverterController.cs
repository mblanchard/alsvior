using Alsvior.Representations.Interfaces;
using Alsvior.Representations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Alsvior.Api.Controllers
{
    [RoutePrefix("api/inverter")]
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
    }
}
