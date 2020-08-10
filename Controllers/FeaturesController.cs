using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VEGA.Models;
using Microsoft.AspNetCore.Authorization;

namespace VEGA.Controllers
{
    [Produces("application/json")]
    [Route("api/Features")]
    public class FeaturesController : Controller
    {
        private readonly VegaDbContext _context;

        public FeaturesController(VegaDbContext context)
        {
            _context = context;
        }

        // GET: api/Features
        [HttpGet]  
        public IEnumerable<Feature> GetFeatures()
        {
            return _context.Features;
        }

        // GET: api/Features/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFeature([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var feature = await _context.Features.SingleOrDefaultAsync(m => m.Id == id);

            if (feature == null)
            {
                return NotFound();
            }

            return Ok(feature);
        }

        // PUT: api/Features/5
        [HttpPut("{id}")]
        [Authorize("AdminRole")]
        public async Task<IActionResult> PutFeature([FromRoute] int id, [FromBody] Feature feature)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != feature.Id)
            {
                return BadRequest();
            }

            _context.Entry(feature).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeatureExists(id))
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

        // POST: api/Features
        [HttpPost]
        [Authorize("AdminRole")]
        public async Task<IActionResult> PostFeature([FromBody] Feature feature)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Features.Add(feature);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFeature", new { id = feature.Id }, feature);
        }

        // DELETE: api/Features/5
        [HttpDelete("{id}")]
        [Authorize("AdminRole")]
        public async Task<IActionResult> DeleteFeature([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var feature = await _context.Features.SingleOrDefaultAsync(m => m.Id == id);
            if (feature == null)
            {
                return NotFound();
            }

            _context.Features.Remove(feature);
            await _context.SaveChangesAsync();

            return Ok(feature);
        }

        private bool FeatureExists(int id)
        {
            return _context.Features.Any(e => e.Id == id);
        }
    }
}