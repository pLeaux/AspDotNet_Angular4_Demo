using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VEGA.Models;
using AutoMapper; 
using VEGA.Models_API;
using VEGA.Persistance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using VEGA.Mapping;

namespace VEGA.Controllers
{
    [Produces("application/json")]
    [Route("api/Vehicles")]
    public class VehiclesController : Controller
    {
        private readonly IMapper mapper;
        private readonly IVehicleRepository vehicleRepository;
        private readonly IUnitOfWork unitOfWork;
        private IConfiguration configuration { get; } 
        private String admin_role;
        private IHttpUtils httpUtils; 


        public VehiclesController(IMapper mapper, IVehicleRepository vehicleRepository, IUnitOfWork unitOfWork, IConfiguration configuration, IHttpUtils httpUtils)
        {
            this.mapper = mapper;
            this.vehicleRepository = vehicleRepository;
            this.unitOfWork = unitOfWork;
            this.configuration = configuration;
            this.admin_role = configuration["AuthSettings:adminRole_PolicyName"];
            this.httpUtils = httpUtils;
        }

        // GET: api/Vehicles?brandId=1&modelId=2
        [HttpGet]
        public async Task<IActionResult> GetVehicles(VehicleFilterResource vehicleFilterResource) // IEnumerable<VehicleGetResource>
        {
            var vehicleFilter = mapper.Map<VehicleFilterResource, VehicleFilter>(vehicleFilterResource);
            var queryResult_DB = await vehicleRepository.GetVehiclesFromDb(vehicleFilter);
            var queryResult_API = mapper.Map<VehiclesQueryResult, VehiclesQueryResultResource>(queryResult_DB); 
            return Ok(queryResult_API);
        }

        // GET: api/Vehicles/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicle([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var vehicle = await vehicleRepository.GetVehicleFromDb(id); // or " = vehicleRepository.GetVehicleFromDb(id).Result;"

            if (vehicle == null)
            {
                return NotFound();
            } 
            VehicleGetResource vehicleGetResource = new VehicleGetResource();
            vehicleGetResource = mapper.Map<Vehicle, VehicleGetResource>(vehicle, vehicleGetResource);
            return Ok(vehicleGetResource);
        }

        // GET: api/Vehicles/brands
        [HttpGet("brands")]
        [Authorize("AdminRole")]
        public async Task<IActionResult> GetBrandsVehicleCount()  
        { 
            var BrandsVehicleCount_DB = await vehicleRepository.GetBrandsVehicleCountFromDb();
            var BrandsVehicleCount_API = mapper.Map<List<BrandVehicleCount>, List<BrandVehicleCountResource>>(BrandsVehicleCount_DB);
            return Ok(BrandsVehicleCount_API);
        }

        // POST: api/Vehicles
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostVehicle([FromBody] VehicleSaveResource vehicleResource) // public async Task<IActionResult> PostVehicle([FromBody] Vehicle vehicle)
        {
            Vehicle vehicle;

            // throw new Exception("TEST EXCEPTION IN CONTROLLER METHOD PostVehicle() !"); 
            // return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected server error");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // LP: because of change from Vehicle to Vehicle_PostModel, FK constraints are not checked - custom model check example bellow  
            var modelExists = await vehicleRepository.CheckModelExistsInDb(vehicleResource.ModelId);
            if (! modelExists)
            {
                ModelState.AddModelError("ModelId", "Invalid ModelId");
                return BadRequest(ModelState);
            }
            var featuresExist = await vehicleRepository.CheckFeaturesExistInDb(vehicleResource.FeatureIds);
            if (!featuresExist)
            {
                ModelState.AddModelError("FeatureId", "Invalid FeatureId");
                return BadRequest(ModelState);
            }
            vehicle = mapper.Map<VehicleSaveResource, Vehicle>(vehicleResource);
            vehicle.UserID = httpUtils.GetUserID();
            vehicleRepository.AddVehicleToDb(vehicle);
 
            await unitOfWork.Complete();  

            // load detail tables Models/Brands/Features for VehicleGetResource, otherwise null
            vehicle = await vehicleRepository.GetVehicleFromDb(vehicle.Id); // or "= vehicleRepository.GetVehicleFromDb(vehicle.Id).Result;"

            VehicleGetResource vehicleGetResource = mapper.Map<Vehicle, VehicleGetResource>(vehicle);
            return CreatedAtAction("GetVehicle", new { id = vehicle.Id }, vehicleGetResource);
        }

        // PUT: api/Vehicles/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutVehicle([FromRoute] int id, [FromBody] VehicleSaveResource vehicleResource)
        {
            // Check
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != vehicleResource.Id)
            {
                return BadRequest();
            }
            // LP: because of change from Vehicle to Vehicle_PostModel, FK constraints are not checked - custom model check example bellow  
            var modelExists = await vehicleRepository.CheckModelExistsInDb(vehicleResource.ModelId);
            if (!modelExists)
            {
                ModelState.AddModelError("ModelId", "Invalid ModelId");
                return BadRequest(ModelState);
            }
            var featuresExist = await vehicleRepository.CheckFeaturesExistInDb(vehicleResource.FeatureIds);
            if (! featuresExist)
            {
                ModelState.AddModelError("FeatureId", "Invalid FeatureId");
                return BadRequest(ModelState);
            }

            var vehicle = await vehicleRepository.GetVehicleFromDb(id);
            if (vehicle == null)
            {
                return NotFound();
            } 

            if (! vehicle.UserID.Equals(httpUtils.GetUserID()))
            {
                return Unauthorized();
            }

            vehicle = mapper.Map<VehicleSaveResource, Vehicle>(vehicleResource, vehicle);
            vehicleRepository.SetVehicleModified(vehicle); 

            await unitOfWork.Complete();
 
            return NoContent();
        }

        
        // DELETE: api/Vehicles/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteVehicle([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vehicle = await vehicleRepository.GetVehicleFromDb(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            if (!vehicle.UserID.Equals(httpUtils.GetUserID()))
            {
                return Unauthorized();
            }

            vehicleRepository.RemoveVehicleFromDb(vehicle); 
            await unitOfWork.Complete();

            return Ok(id);
        }
         
 
    }
}