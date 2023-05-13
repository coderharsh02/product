using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductApi.DTOs;
using ProductApi.Models;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ExamProductContext _context;
        private readonly IMapper _autoMapper;

        public ProductsController(ExamProductContext context, IMapper autoMapper)
        {
            _context = context;
            _autoMapper = autoMapper;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDetails>>> GetProduct()
        {
          if (_context.Product == null)
          {
              return NotFound();
          }

            return _autoMapper.Map<List<ProductDetails>>(await new ExamProductContextProcedures(_context).USP_GetProductsAsync());
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(string id)
        {
          if (_context.Product == null)
          {
              return NotFound();
          }
            var product = await _context.Product.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(string id, Product product)
        {
            if (id != product.Sku)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(ProductDetails product)
        {
            if (_context.Product == null)
            {
                return Problem("Entity set 'ExamProductContext.Product'  is null.");
            }

            var doesCategoryExist = _context.Category.Any(x => x.CategoryName == product.CategoryName);

            var catId = 0;
            if (!doesCategoryExist)
            {
                var cat = _context.Category.Add(new Category
                {
                    CategoryName = product.CategoryName
                });
                await _context.SaveChangesAsync();
                catId = 1;
            }

            var prod = _autoMapper.Map<Product>(product);
            prod.CategoryId = catId;
            _context.Product.Add(prod);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProductExists(prod.Sku))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProduct", new { id = product.Sku }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            if (_context.Product == null)
            {
                return NotFound();
            }
            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(string id)
        {
            return (_context.Product?.Any(e => e.Sku == id)).GetValueOrDefault();
        }
    }
}
