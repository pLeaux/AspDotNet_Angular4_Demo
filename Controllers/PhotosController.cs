using System;
using System.Collections.Generic; 
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; 
using VEGA.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using VEGA.Persistance;
using AutoMapper;
using VEGA.Models_API;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;

namespace VEGA.Controllers
{


    [Produces("application/json")]
    [Route("/api/vehicles/{vehicleId}/photos")]
    public class PhotosController : Controller
    {
        private readonly VegaDbContext _context; 
        private readonly IHostingEnvironment hostEnv;
        private readonly IVehicleRepository vehicleRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IOptions<PhotoSettings> photoSettings;
        private readonly IPhotoRepository photoRepository; 

        public PhotosController(
            VegaDbContext context,
            IHostingEnvironment hostEnv,
            IVehicleRepository vehicleRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IOptions<PhotoSettings> photoSettings,
            IPhotoRepository photoRepository 
            )
        {
            _context = context;
            this.hostEnv = hostEnv;
            this.vehicleRepository = vehicleRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.photoSettings = photoSettings;
            this.photoRepository = photoRepository; 
        }

        // GET: api/vehicles/2/photos 
        [HttpGet]
        public async Task<IActionResult> GetPhotos(int vehicleId)
        {
            var photos_DB = await photoRepository.GetVehiclePhotosFromDb(vehicleId);
            var photos_API = mapper.Map<List<Photo>, List<PhotoResource>>(photos_DB);
            return Ok(photos_API);
        }

        /* 
        // GET: api/vehicles/2/photos/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPhoto([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var photo = await _context.Photos.SingleOrDefaultAsync(m => m.Id == id);

            if (photo == null)
            {
                return NotFound();
            }

            return Ok(photo);
        }
        */

        // POST: api/vehicles/{vehicleId}/Photos
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadPhoto(int vehicleId, [FromForm] IFormFile file)
        {
            long fileSize = 100000;// TEST uploadFile.Length;
            string destFilePath;
            string uploadDirPath;
            string fileExt;
            string destFileName;
            Vehicle vehicle;

            vehicle = await vehicleRepository.GetVehicleFromDb(vehicleId);
            // checks
            if (vehicle == null)
            {
                return BadRequest("Vehicle does not exist.");
            };

            if (file == null)
            {
                return BadRequest("Null file.");
            }
            fileSize = file.Length;
            if (fileSize > photoSettings.Value.MaxBytes)
            {
                return BadRequest("File is too large.");
            }
            fileExt = Path.GetExtension(file.FileName).ToLower();
            if (!photoSettings.Value.IsFileExtSupported(fileExt))
            {
                return BadRequest("File type not allowed.");
            }
            // save file to uploads folder
            uploadDirPath = Path.Combine(hostEnv.WebRootPath, "uploads");
            if (!Directory.Exists(uploadDirPath))
            {
                Directory.CreateDirectory(uploadDirPath);
            };
            destFileName = Guid.NewGuid().ToString() + fileExt;
            destFilePath = Path.Combine(uploadDirPath, destFileName);
            Console.WriteLine("Uploaded file path: " + destFilePath);
            using (var stream = new FileStream(destFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            };
            // save photo text data to vehicle
            var photo = new Photo { FileName = destFileName, VehicleId = vehicleId };
            vehicle.Photos.Add(photo);
            await unitOfWork.Complete();

            //return photo text data
            return Ok(mapper.Map<Photo, PhotoResource>(photo));
        }

        /* 
        // PUT: api/Photos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPhoto([FromRoute] int id, [FromBody] Photo photo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            } 
            if (id != photo.Id)
            {
                return BadRequest();
            }
            _context.Entry(photo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhotoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        */


        /*
        // DELETE: api/Photos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var photo = await _context.Photos.SingleOrDefaultAsync(m => m.Id == id);
            if (photo == null)
            {
                return NotFound();
            }

            _context.Photos.Remove(photo);
            await _context.SaveChangesAsync();

            return Ok(photo);
        }

        private bool PhotoExists(int id)
        {
            return _context.Photos.Any(e => e.Id == id);
        }
        */
    }
}