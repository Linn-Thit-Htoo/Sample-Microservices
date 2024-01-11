using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Tree;
using Microsoft.EntityFrameworkCore;
using Product.Microservice.Models;
using System.Security.Principal;

namespace Product.Microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public ProductController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region Get Products
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var lst = await _dbContext.Products.Where(product => product.IsDeleted == false).ToListAsync();
                return Ok(lst);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Product
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            try
            {
                if (id == 0) return BadRequest();

                var item = await _dbContext.Products.Where(product => product.ProductId == id && product.IsDeleted == false).FirstOrDefaultAsync();

                if (item is null) return NotFound("No data found.");

                return Ok(item);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductModel productModel)
        {
            try
            {
                if (productModel is null || string.IsNullOrEmpty(productModel.ProductName) || productModel.Quantity == 0 || productModel.Price == 0)
                {
                    return BadRequest();
                }

                productModel.Create_Date = DateTime.Now;

                await _dbContext.Products.AddAsync(productModel);

                int result = await _dbContext.SaveChangesAsync();

                return result > 0 ? StatusCode(StatusCodes.Status201Created, "Product Created!") : BadRequest();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Update Product
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductModel productModel)
        {
            try
            {
                if (id == 0 || productModel is null || string.IsNullOrEmpty(productModel.ProductName) || productModel.Quantity == 0 || productModel.Price == 0)
                {
                    return BadRequest();
                }

                var item = await _dbContext.Products.Where(x => x.ProductId == id && x.IsDeleted == false).FirstOrDefaultAsync();

                if (item is null) return NotFound("No data found.");

                item.ProductName = productModel.ProductName;
                item.Quantity = productModel.Quantity;
                item.Price = productModel.Price;

                int result = await _dbContext.SaveChangesAsync();

                return result > 0 ? StatusCode(StatusCodes.Status202Accepted, "Product Updated!") : BadRequest();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Delete Product
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id == 0) return BadRequest();

                var item = await _dbContext.Products.Where(x => x.ProductId == id && x.IsDeleted == false).FirstOrDefaultAsync();

                if (item is null) return NotFound("No data found.");

                item.IsDeleted = true;

                int result = await _dbContext.SaveChangesAsync();

                return result > 0 ? StatusCode(StatusCodes.Status202Accepted, "Product Deleted!") : BadRequest();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
