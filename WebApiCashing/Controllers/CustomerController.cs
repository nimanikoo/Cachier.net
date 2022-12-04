using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiCashing.Data;
using WebApiCashing.Models;
using WebApiCashing.Services;

namespace WebApiCaching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICacheService _cachingService;
        private readonly AppDbContext _context;
        public CustomerController(ICacheService cacheService, AppDbContext context)
        {
            _cachingService = cacheService;
            _context = context;
        }

        [HttpGet("customers")]
        public async Task<IActionResult> GetDrivers()
        {
            //Check Cache Data Info
            var cacheData = _cachingService.GetData<IEnumerable<Customer>>("customers");
            if (cacheData != null && cacheData.Count() > 0)
            {
                return Ok(cacheData);
            }

            cacheData = await _context.Customers.ToListAsync();

            //set expireTime 
            var expireTime = DateTimeOffset.Now.AddSeconds(45);
            _cachingService.SetData<IEnumerable<Customer>>("customers", cacheData, expireTime);
            return Ok(cacheData);
        }

        [HttpPost("AddCustomer")]
        public async Task<IActionResult> AddDriver(Customer value)
        {
            var AddedObject = await _context.Customers.AddAsync(value);
            var expireTime = DateTimeOffset.Now.AddSeconds(45);
            _cachingService.SetData<Customer>($"customer{value.Id}", AddedObject.Entity, expireTime);
            await _context.SaveChangesAsync();
            return Ok(AddedObject.Entity);
        }

        [HttpDelete("DeleteCustomer")]
        public async Task<IActionResult> RemoveDriver(int id)
        {
            var existCache = await _context.Customers.FirstOrDefaultAsync(d => d.Id == id);
            if (existCache != null)
            {
                _context.Remove(existCache);
                _cachingService.RemoveData($"customer{id}");
                await _context.SaveChangesAsync();
                return NoContent();
            }
            return NotFound();
        }

    }
}
