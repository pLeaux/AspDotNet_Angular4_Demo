using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VEGA.Models;
using VEGA.Models_API;

namespace VEGA.Persistance
{
    public class VehicleRepository: IVehicleRepository
    {
        private VegaDbContext _context;
        private List<Feature> validFeatures;

        public VehicleRepository(VegaDbContext _context)
        {
            this._context = _context;
            this.validFeatures = new List<Feature>();
        }
        public async Task<Vehicle> GetVehicleFromDb(int id)
        { 
            var vehicleQuery = _context.Vehicles.Where(v => v.Id == id).Include(v => v.Model).ThenInclude(m => m.Brand).Include(v => v.Features).ThenInclude(vf => vf.Feature);
            var vehicle = await vehicleQuery.SingleOrDefaultAsync();
            return vehicle;
        }


        public async Task<VehiclesQueryResult> GetVehiclesFromDb(VehicleFilter vehicleFilter)
        {
            var queryResult = new VehiclesQueryResult();

            // prepare vehicleQuery tables, filtered + sorted + paged
            bool isSortAsc;
            var vehicleQuery = _context.Vehicles.Include(v => v.Model).ThenInclude(m => m.Brand).Include(v => v.Features).ThenInclude(vf => vf.Feature).AsQueryable();
            if (vehicleFilter.BrandId != null)
                vehicleQuery = vehicleQuery.Where(v => v.Model.BrandId == vehicleFilter.BrandId);
            if (vehicleFilter.ModelId != null)
                vehicleQuery = vehicleQuery.Where(v => v.ModelId == vehicleFilter.ModelId);
            isSortAsc = (vehicleFilter.IsSortAsc == null) | (vehicleFilter.IsSortAsc == true);
            if (!String.IsNullOrEmpty(vehicleFilter.SortKey))
            {
                if (vehicleFilter.SortKey == "brand")
                {
                    vehicleQuery = isSortAsc ? vehicleQuery.OrderBy(v => v.Model.Brand.Name) : vehicleQuery.OrderByDescending(v => v.Model.Brand.Name);
                }
                else if (vehicleFilter.SortKey == "model")
                {
                    vehicleQuery = isSortAsc ? vehicleQuery.OrderBy(v => v.Model.Name) : vehicleQuery.OrderByDescending(v => v.Model.Name);
                }
                else if (vehicleFilter.SortKey == "contact")
                {
                    vehicleQuery = isSortAsc ? vehicleQuery.OrderBy(v => v.ContactName) : vehicleQuery.OrderByDescending(v => v.ContactName);
                }
            }
            // get count before paging applied
            queryResult.TotalCount_allPages = await vehicleQuery.CountAsync(); 
            // apply paging
            if (((vehicleFilter.PageNo ?? 0) != 0) && ((vehicleFilter.PageSize ?? 0) != 0))
            {
                vehicleQuery = vehicleQuery
                    .Skip(((vehicleFilter.PageNo ?? 0) - 1) * (vehicleFilter.PageSize ?? 0))
                    .Take(vehicleFilter.PageSize ?? 0);
            }
            // execute
            queryResult.Vehicles = await vehicleQuery.ToListAsync();
            return queryResult;

        }

        /* backup old
        public async Task<List<Vehicle>> GetVehiclesFromDb_Old(VehicleFilter vehicleFilter)
        {
            // prepare vehicleQuery tables, filtered + sorted + paged
            bool isSortAsc;  
            var vehicleQuery = _context.Vehicles.Include(v => v.Model).ThenInclude(m => m.Brand).Include(v => v.Features).ThenInclude(vf => vf.Feature).AsQueryable();
            if (vehicleFilter.BrandId != null)
                vehicleQuery = vehicleQuery.Where(v => v.Model.BrandId == vehicleFilter.BrandId);
            if (vehicleFilter.ModelId != null)
                vehicleQuery = vehicleQuery.Where(v => v.ModelId == vehicleFilter.ModelId);
            isSortAsc = (vehicleFilter.IsSortAsc == null) | (vehicleFilter.IsSortAsc == true);  
            if (! String.IsNullOrEmpty(vehicleFilter.SortKey))
            {
                if (vehicleFilter.SortKey == "brand") 
                {
                    vehicleQuery = isSortAsc ? vehicleQuery.OrderBy(v => v.Model.Brand.Name) : vehicleQuery.OrderByDescending(v => v.Model.Brand.Name);
                }
                else if (vehicleFilter.SortKey == "model")
                {
                    vehicleQuery = isSortAsc ? vehicleQuery.OrderBy(v => v.Model.Name) : vehicleQuery.OrderByDescending(v => v.Model.Name);
                }
                else if (vehicleFilter.SortKey == "contact")
                {
                    vehicleQuery = isSortAsc ? vehicleQuery.OrderBy(v => v.ContactName) : vehicleQuery.OrderByDescending(v => v.ContactName);
                }
            }
            if (((vehicleFilter.PageNo ?? 0) != 0) && ((vehicleFilter.PageSize ?? 0) != 0))
            {
                vehicleQuery = vehicleQuery
                    .Skip(((vehicleFilter.PageNo ?? 0) - 1) * (vehicleFilter.PageSize ?? 0))
                    .Take(vehicleFilter.PageSize ?? 0); 
            }
            // execute
            var vehicles = await vehicleQuery.ToListAsync();
            return vehicles; 
 
        }
        */

        public async Task<List<BrandVehicleCount>>GetBrandsVehicleCountFromDb()
        {
            var query = _context.Vehicles
                .Include(v => v.Model)
                    .ThenInclude(m => m.Brand)
                .GroupBy(v => v.Model.Brand.Name)
                .Select(g => new BrandVehicleCount { BrandName = g.Key, VehicleCount = g.Count()})
                .AsQueryable();
            return await query.ToListAsync(); 
        }
  

        public void AddVehicleToDb(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
        }

        public void SetVehicleModified(Vehicle vehicle)
        {
            _context.Entry(vehicle).State = EntityState.Modified;
        }

        public void RemoveVehicleFromDb(Vehicle vehicle)
        {
            _context.Vehicles.Remove(vehicle);
        } 

        public async Task<bool> CheckModelExistsInDb(int id)
        {
           return await _context.Models.Where(m => m.Id == id).AnyAsync(); 
        }

        public async Task<bool> CheckFeaturesExistInDb(int[] featureIdsSelected)
        {
            bool isOk = true;
            if (validFeatures.Count == 0) {
                validFeatures = await _context.Features.ToListAsync();
            };
            foreach (int featureIdSelected in featureIdsSelected)
            {
                if (validFeatures.Find(f => f.Id == featureIdSelected) == null)
                {
                    isOk = false;
                    break;
                } 
            } 
            return isOk; 
        }

        public bool CheckVehicleExists(int id)
        {
            return _context.Vehicles.Any(e => e.Id == id);
        }


    } 
 
}
