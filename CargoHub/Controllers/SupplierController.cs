using CargoHub.Models;
using CargoHub.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CargoHub.Controllers
{
    [Route("api/v1/suppliers")]
    public class SupplierController(AppDbContext appDbContext, SupplierService storage) : Controller
    {
        AppDbContext appDbContext = appDbContext;
        SupplierService storage = storage;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSupplier(int id)
        {
            var supplier = await storage.GetSupplier((int)id);

            if (supplier == null)
                return NotFound("Supplier doesn't exist with id: " + id);

            return Ok(supplier);
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllSuppliers([FromQuery] int page = 0)
        {
            var suppliers = await storage.GetAllSuppliers();
            var paginatedSuppliers = suppliers.Skip(page * 100).Take(100).ToList();
            return Ok(paginatedSuppliers);
        }

        [HttpPost()]
        public async Task<IActionResult> PostSupplier([FromBody] Supplier supplier)
        {
            if (supplier == null)
                return BadRequest("Supplier cannot be null");

            var createdSupplierId = await storage.PostSupplier(supplier);
            return CreatedAtAction(nameof(GetSupplier), new { id = createdSupplierId }, createdSupplierId);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSupplier(int id, [FromBody] Supplier supplier)
        {
            if (supplier == null)
                return BadRequest("Supplier cannot be null");

            var updatedSupplier = await storage.PutSupplier(id, supplier);
            if (updatedSupplier == null)
                return NotFound("Supplier doesn't exist with id: " + id);

            return Ok(updatedSupplier);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var supplier = await storage.GetSupplier(id);
            if (supplier == null)
                return NotFound("Supplier doesn't exist with id: " + id);

            return Ok(await storage.DeleteSupplier(id));
        }
    }
}