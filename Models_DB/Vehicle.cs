using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;



namespace VEGA.Models
{
    public class Vehicle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Brand not needed, model holds info on Brand
        // [Required]
        // public Brand brand { get; set; }

        [Required]
        public int ModelId { get; set; }

        public Model Model { get; set; }
        
        public Boolean IsRegistered { get; set; }

        [StringLength(64)]
        public String ContactName { get; set; }

        [StringLength(64)]
        public String ContactPhone { get; set; }

        [StringLength(64)]
        public String ContactEmail { get; set; }

        [StringLength(64)]
        [Required]
        public String UserID { get; set; }

        public ICollection<VehicleFeature> Features { get; set; }

        public ICollection<Photo> Photos { get; set; }


        public Vehicle()
        {
            // Model = new Model();
            Features = new List<VehicleFeature>();
            Photos = new List<Photo>();

        }

    }
}
