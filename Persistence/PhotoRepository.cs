using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VEGA.Models;

namespace VEGA.Persistance
{
    public class PhotoRepository : IPhotoRepository
    {
        VegaDbContext vegaDbContext;

        public PhotoRepository(VegaDbContext vegaDbContext)
        {
            this.vegaDbContext = vegaDbContext; 
        }
        public async Task<List<Photo>> GetVehiclePhotosFromDb(int vehicleId)
        {
            List<Photo> photos;
            photos =  await vegaDbContext.Photos
                .Where(p => p.VehicleId == vehicleId) 
                .ToListAsync();
            return (photos);   
        }
    }
}
