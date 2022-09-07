using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CMPG323_API_Project.Models;
using Microsoft.AspNetCore.Authorization;
using CMPG323_API_Project.Authentication;

namespace CMPG323_API_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class ZonesController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public ZonesController(ApplicationDBContext context)
        {
            _context = context;
        }

        private Zone FindZone(Guid id)
        {
            return _context.Zone.Find(id);
        }

        // GET: api/Zones
        [HttpGet("getAllZones")]
        public async Task<ActionResult<IEnumerable<Zone>>> GetZone()
        {
            return await _context.Zone.ToListAsync();
        }

        // GET: api/Zones/5
        [HttpGet("getZoneById/{id}")]
        public async Task<ActionResult<Zone>> GetZone(Guid id)
        {
            var zone = await _context.Zone.FindAsync(id);

            if (zone == null)
            {
                return NotFound();
            }

            return zone;
        }

        [HttpGet("getNumberOfZonesByCategory/{categoryId}")]
        public async Task<ActionResult<int>> GetNumberOfZonesByCategory(Guid categoryId)
        {
            var devices = await _context.Device.Where(x => x.CategoryId == categoryId).ToListAsync();

            if (devices.Count < 1)
            {
                return NotFound();
            }

            List<Guid> zones = new List<Guid>();

            foreach (var device in devices)
            {
                if (!zones.Contains(device.ZoneId))
                { 
                    zones.Add(device.ZoneId);
                }
            }
            return zones.Count;
        }

        // PUT: api/Zones/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("createZone/{id}")]
        public async Task<IActionResult> PutZone(Guid id, Zone zone)
        {
            if (id != zone.ZoneId)
            {
                return BadRequest();
            }

            _context.Entry(zone).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ZoneExists(id))
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
        [HttpPatch("patchZoneById/{id}")]
        public async Task<IActionResult> PatchZone(Guid id, string zoneName, string zoneDescription)
        {
            var zone = FindZone(id);
            zone.ZoneName = zoneName;
            zone.ZoneDescription = zoneDescription;

            _context.Entry(zone).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ZoneExists(id))
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
        // POST: api/Zones
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("createZone")]
        public async Task<ActionResult<Zone>> PostZone(Zone zone)
        {
            _context.Zone.Add(zone);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ZoneExists(zone.ZoneId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetZone", new { id = zone.ZoneId }, zone);
        }

        // DELETE: api/Zones/5
        [HttpDelete("deleteZone{id}")]
        public async Task<ActionResult<Zone>> DeleteZone(Guid id)
        {
            var zone = FindZone(id);
            if (zone == null)
            {
                return NotFound();
            }

            _context.Zone.Remove(zone);
            await _context.SaveChangesAsync();

            return zone;
        }

        private bool ZoneExists(Guid id)
        {
            return _context.Zone.Any(e => e.ZoneId == id);
        }
    }
}
