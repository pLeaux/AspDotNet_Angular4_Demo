using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VEGA.Models_API
{
    public class VehicleFilterResource
    {
        public int? BrandId { get; set; }
        public int? ModelId { get; set; }
        public String SortKey { get; set; }
        public bool? IsSortAsc { get; set; }
        public int? PageSize { get; set; }
        public int? pageNo { get; set; }
    }
}
