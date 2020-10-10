using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{
    [Route("api/[products]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Product>>> Get(
            [FromServices] DataContext context)
        {
            var product = await context
                .Products
              .Include(x => x.Category)
              .AsNoTracking()
              .ToListAsync();

            return Ok(product);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<List<Product>>> GetById(
            int id,
            [FromServices]DataContext context )
        {
            var product = await context
                .Products
                .Include(x => x.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return Ok(product);
        }

        [HttpGet]
        [Route("categories/{id:int}")]
        public async Task<ActionResult<List<Product>>> GetByIdCategory(
            int id,
            [FromServices]DataContext context
            )
        {
            var products = await context
                .Products
                .Include(x => x.Category)
                .AsNoTracking()
                .Where(x => x.CategoryId == id)
                .ToListAsync(); 
             
            return Ok(products);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<List<Product>>> Post(
            [FromServices]DataContext context,
            [FromBody]Product model
            )
        {
            if (ModelState.IsValid)
            {
                context.Products.Add(model);
                await context.SaveChangesAsync();
                return Ok(model);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
