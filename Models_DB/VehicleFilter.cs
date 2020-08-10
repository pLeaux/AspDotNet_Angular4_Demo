using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlTypes; 

namespace VEGA.Models
{
    public class VehicleFilter
    {
        public int? BrandId { get; set; }
        public int? ModelId { get; set; }
        public String SortKey { get; set; }
        public bool? IsSortAsc { get; set; }
        public int? PageSize { get; set; }
        public int? PageNo { get; set; }
    }
}
