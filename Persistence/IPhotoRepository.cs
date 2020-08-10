using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VEGA.Models;

namespace VEGA.Persistance
{
    public interface IPhotoRepository
    {
        Task<List<Photo>> GetVehiclePhotosFromDb(int vehicleId);
    }
}
