using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VEGA.Models
{
    public class Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        // [JsonIgnore] // LP: without that, JSON parser reports "Self referencing loop" (Model references Brands and vice versa)
        public Brand Brand { get; set; }

        [Required]
        public int BrandId { get; set; }

        public Model()
        {
            // this.Brand = new Brand();
        }
    }
}
