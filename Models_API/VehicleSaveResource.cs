using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace VEGA.Models
{
    /**
     *  Vehicle_PostModel structure is ment to be used for saving new vehicle with HttpPost method.
     *  It has only data, needed for saving and no redundant details from detail tables (Model/Features)
     * */

    public class Contact
    {
        public String Name { get; set; }
        public String Phone { get; set; }
        public String Email { get; set; }
    }

    public class VehicleSaveResource
    {
        public int Id { get; set; } 
        public int ModelId { get; set; }  
        public Boolean IsRegistered { get; set; }
        public Contact Contact { get; set; } 
        public int[] FeatureIds { get; set; }  

        public VehicleSaveResource()
        {
            Contact = new Contact();
        }
    }
}
