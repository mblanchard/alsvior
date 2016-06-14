using Alsvior.Core;
using Alsvior.Representations.Interfaces;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Alsvior.Api.Controllers
{
    public abstract class GeospatialDatasetController<T> : ApiController where T : class, ILocatable
    {
        private ICassandraClient _cassandra;
        protected ICassandraSession _session;

        public GeospatialDatasetController(ICassandraClient cassandra)
        {
            Console.WriteLine(HttpContext.Current.Request.RawUrl);
            _cassandra = cassandra;
            _session = _cassandra.CreateSession();
        }

        
        [Route("")]
        public virtual IHttpActionResult GetAll()
        {
            var nodes = _session.Get<T>();
            return Ok(nodes);
        }

        [Authorize]
        [Route("{latitude}/{longitude}/nearby")]
        public virtual IHttpActionResult GetNearCoord(int latitude, int longitude)
        {
            var nodes = _session.Get<T>();
            var filter = new DistanceFilter<T>(latitude, longitude, 400 * 1000); //400 km
            return Ok(filter.FilterCoords(nodes));
        }

        [Authorize]
        [Route("{latitude}/{longitude}")]
        public virtual IHttpActionResult GetAtCoord(int latitude, int longitude)
        {
            var node = _session.Get<T>(x=> x.Latitude == latitude && x.Longitude == longitude);
            return Ok(node);
        }
    }
}
