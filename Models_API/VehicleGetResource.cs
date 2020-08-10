using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using VEGA.Models;

namespace VEGA.Models_API
{
    public class VehicleGetResource
    {
        [JsonPropertyAttribute (Order = 1)] // LP: without that, JSON returns fields in random order
        public int Id { get; set; }

        [JsonPropertyAttribute(Order = 2)]
        public IdNameResourceType Brand { get; set; }

        [JsonPropertyAttribute(Order = 3)]
        public IdNameResourceType Model { get; set; }

        [JsonPropertyAttribute(Order = 4)]
        public Boolean IsRegistered { get; set; }

        [JsonPropertyAttribute(Order = 5)]
        public Contact Contact;

        [JsonPropertyAttribute(Order = 6)]
        public string UserID { get; set; }

        [JsonPropertyAttribute(Order = 7)]
        public IEnumerable<IdNameResourceType> Features { get; set; }

        public VehicleGetResource()
        {
            Brand = new IdNameResourceType();
            Model = new IdNameResourceType();
            Contact = new Contact(); 
            Features = new List<IdNameResourceType>();
        }
    }
}
