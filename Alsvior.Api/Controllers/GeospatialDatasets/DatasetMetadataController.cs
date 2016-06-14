using System.Web.Http;

namespace Alsvior.Api.Controllers.GeospatialDatasets
{
    public class DatasetMetadataController : ApiController
    {
        public IHttpActionResult GetDatasetEndpoints()
        {
            return Ok(DatasetControllerCache.GetDatasetEndpoints());
        }
    }
}
