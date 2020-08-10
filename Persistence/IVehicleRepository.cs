using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VEGA.Models; 

namespace VEGA.Persistance
{
    public interface IVehicleRepository
    {
        Task<Vehicle> GetVehicleFromDb(int id);
 
        Task<VehiclesQueryResult> GetVehiclesFromDb(VehicleFilter vehicleFilter);

        Task<List<BrandVehicleCount>> GetBrandsVehicleCountFromDb();

        void AddVehicleToDb(Vehicle vehicle);

        void SetVehicleModified(Vehicle vehicle);

        void RemoveVehicleFromDb(Vehicle vehicle);

        Task<bool> CheckModelExistsInDb(int id);

        Task<bool> CheckFeaturesExistInDb(int[] featureIdsSelected);

        bool CheckVehicleExists(int id);


    }
}
