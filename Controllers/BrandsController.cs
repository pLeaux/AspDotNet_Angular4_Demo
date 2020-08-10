using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VEGA.Models;
using Newtonsoft.Json;
using AutoMapper;
using VEGA.Controllers.Resources;
using VEGA.Models_API;
using Microsoft.AspNetCore.Authorization;

namespace VEGA.Controllers
{
    [Produces("application/json")]
    // [Route("api/Brands")]
    public class BrandsController : Controller
    {
        private readonly VegaDbContext _context;
        private readonly IMapper mapper;

        public BrandsController(VegaDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // GET: api/Brands
        [HttpGet("api/brands")]
        public IEnumerable<BrandResource> GetBrands()
        {
            List<Brand> brandList;
            brandList = _context.Brands.ToList();
            return mapper.Map<List<Brand>, List<BrandResource>>(brandList);
        }

        // GET: api/BrandsWithModels
        [HttpGet("api/brandsWithModels")]
        public IEnumerable<BrandResource> GetBrandsWithModels()
        {
            List<Brand> brandList;
            brandList =  _context.Brands.Include(b => b.Models).ToList(); 
            return mapper.Map<List<Brand>, List<BrandResource>>(brandList);
        }

        // GET: api/Brands/5
        [HttpGet("api/brands/{id}")]
        public async Task<IActionResult> GetBrand([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var brand = await _context.Brands.SingleOrDefaultAsync(m => m.Id == id);

            if (brand == null)
            {
                return NotFound(); 
            }

            return Ok(brand);
        }

        // PUT: api/Brands/5
        [HttpPut("api/brands/{id}")]
        [Authorize("AdminRole")]
        public async Task<IActionResult> PutBrand([FromRoute] int id, [FromBody] Brand brand)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != brand.Id)
            {
                return BadRequest();
            }

            _context.Entry(brand).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BrandExists(id))
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

        // POST: api/Brands
        [HttpPost("api/brands")]
        [Authorize("AdminRole")]
        public async Task<IActionResult> PostBrand([FromBody] Brand brand)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBrand", new { id = brand.Id }, brand);
        }

        // DELETE: api/Brands/5
        [HttpDelete("api/brands/{id}")]
        [Authorize("AdminRole")]
        public async Task<IActionResult> DeleteBrand([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var brand = await _context.Brands.SingleOrDefaultAsync(m => m.Id == id);
            if (brand == null)
            {
                return NotFound();
            }

            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();

            return Ok(brand);
        }

        private bool BrandExists(int id)
        {
            return _context.Brands.Any(e => e.Id == id);
        }
    }
}