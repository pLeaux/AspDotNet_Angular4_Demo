using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VEGA.Models
{
    public class Brand
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set;  }

        [Required]
        [StringLength(64)]
        public string Name { get; set;  }

        // [JsonIgnore] 
        // [IgnoreDataMember]
        public ICollection<Model> Models { get; set;  }

        public Brand()
        {
            Models = new Collection<Model>();
        }
    }
}
