using System;
using System.ComponentModel.DataAnnotations;


namespace VEGA.Models_API
{
    public class PhotoResource
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(255)]
        public string FileName { get; set; }

        [Required]
        public int VehicleId { get; set; }

     }
}
