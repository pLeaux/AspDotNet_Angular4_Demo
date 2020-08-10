using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VEGA.Models
{
    public class VehicleFeature
    {
        [Key]
        public int VehicleId { get; set; }

        [JsonIgnore]
        public Vehicle Vehicle { get; set; }

        [Key]
        public int FeatureId { get; set; }

        [JsonIgnore]
        public Feature Feature { get; set;  }

    }
}
