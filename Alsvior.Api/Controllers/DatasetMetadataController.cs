using System.Web.Http;
using System.Web.Http.Cors;

namespace Alsvior.Api.Controllers.GeospatialDatasets
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/datasetMetadata")]
    [Authorize]
    public class DatasetMetadataController : ApiController
    {

        public DatasetMetadataController()
        {

        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok(DatasetControllerCache.GetDatasetEndpoints());
        }
    }
}
