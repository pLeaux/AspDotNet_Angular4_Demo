using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VEGA.Controllers.Resources
{
    public class BrandResource
    {
        // [Key]
        // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // [Required]
        [StringLength(64)]
        public string Name { get; set; }

        // [JsonIgnore] 
        // [IgnoreDataMember]
        public ICollection<ModelResource> Models { get; set; }

        public BrandResource()
        {
            Models = new Collection<ModelResource>();
        }
    }
}
