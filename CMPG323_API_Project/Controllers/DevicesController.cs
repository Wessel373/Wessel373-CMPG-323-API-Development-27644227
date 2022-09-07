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
using System.Security.Cryptography.X509Certificates;

namespace CMPG323_API_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class DevicesController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public DevicesController(ApplicationDBContext context)
        {
            _context = context;
        }

        private Device FindDevice(Guid id)
        {
            return _context.Device.Find(id);
        }

        // GET: api/Devices
        [HttpGet("getDevices")]
        public async Task<ActionResult<IEnumerable<Device>>> GetDevice()
        {
            return await _context.Device.ToListAsync();
        }

        // GET: api/Devices/5
        [HttpGet("getDeviceById/{id}")]
        public async Task<ActionResult<Device>> GetDevice(Guid id)
        {
            var device = await _context.Device.FindAsync(id);

            if (device == null)
            {
                return NotFound();
            }

            return device;
        }

        [HttpGet("getByZoneId/{zoneId}")]
        public async Task<ActionResult<IEnumerable<Device>>> GetDeviceByZoneId(Guid zoneId)
        {
            var device = await _context.Device.Where(x => x.ZoneId == zoneId).ToListAsync();

            if (device == null)
            {
                return NotFound();
            }

            return device;
        }

        // PUT: api/Devices/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPatch("patchDeviceById/{id}")]
        public async Task<IActionResult> PatchDevice(Guid id, string deviceName, string status)
        {
            var device = FindDevice(id);
            device.Status = status;
            device.DeviceName = deviceName;

            _context.Entry(device).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceExists(id))
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

        // POST: api/Devices
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("createDevice")]
        public async Task<ActionResult<Device>> PostDevice(Device device)
        {
            _context.Device.Add(device);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DeviceExists(device.DeviceId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDevice", new { id = device.DeviceId }, device);
        }

        // DELETE: api/Devices/5
        [HttpDelete("deleteDevice/{id}")]
        public async Task<ActionResult<Device>> DeleteDevice(Guid id)
        {
            var device = FindDevice(id);
            if (device == null)
            {
                return NotFound();
            }

            _context.Device.Remove(device);
            await _context.SaveChangesAsync();

            return device;
        }
        /*
        [HttpPatch("{id}")]
        public async Task<Action<Device>> PatchDevice(Guid id)
        { 
            var device = _context.Device.FindAsync(id);
        }*/

        private bool DeviceExists(Guid id)
        {
            return _context.Device.Any(e => e.DeviceId == id);
        }
    }
}
