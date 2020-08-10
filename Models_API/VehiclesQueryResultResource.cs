using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VEGA.Models_API
{
    public class VehiclesQueryResultResource
    {
        public int TotalCount_allPages { get; set; }
        public IEnumerable<VehicleGetResource> Vehicles { get; set; } 
    }
}
