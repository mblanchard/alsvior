using Alsvior.Utility.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alsvior.Utility
{
    /// <summary>
    /// As of 6/16: Used to associate a Time Series Dataset with a Geospatial Dataset. Can be self-referential.
    /// </summary>
    public class AssociatedGeospatialModelAttribute: Attribute
    {
        private Type AssociatedModel { get; set; }

        public AssociatedGeospatialModelAttribute(Type boundModel)
        {
            AssociatedModel = boundModel;
        }
    }
}
