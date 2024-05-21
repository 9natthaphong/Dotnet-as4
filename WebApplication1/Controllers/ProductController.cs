using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DataAccess;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationContext _context;
        public ProductController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet("AllProducts")]
        [Authorize(Roles = "User,Admin")]
        public IActionResult Get()
        {
            var products = _context.Products.ToList();
            return Ok(products);
        }

        [HttpGet("Product/{id}")]
        [Authorize(Roles = "User,Admin")]
        public IActionResult GetProduct([FromRoute] int id)
        {
            var product = _context.Products.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost("Productsave")]
        [Authorize(Roles = "Admin")]
        public IActionResult Save([FromBody] Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut("Productupdate")]
        [Authorize(Roles = "Admin")]
        public IActionResult Update([FromBody] Product product)
        {
            var result = _context.Products.AsNoTracking().FirstOrDefault(x => x.Id == product.Id);
            if (result == null)
            {
                return NotFound();
            }
            _context.Products.Update(product);
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("Productdelete")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete([FromQuery] int id)
        {
            var deleteproduct = _context.Products.AsNoTracking().FirstOrDefault(x => x.Id == id);
            if (deleteproduct == null)
            {
                return NotFound();
            }
            _context.Products.Remove(deleteproduct);
            _context.SaveChanges();
            return Ok();
        }
    }
}
