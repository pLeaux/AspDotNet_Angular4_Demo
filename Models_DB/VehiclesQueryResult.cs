using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VEGA.Models;

namespace VEGA.Models
{
    public class VehiclesQueryResult
    {
        public int TotalCount_allPages { get; set; }
        public IEnumerable<Vehicle> Vehicles { get; set; }
    }
}
