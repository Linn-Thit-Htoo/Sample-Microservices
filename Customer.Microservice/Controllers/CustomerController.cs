using Customer.Microservice.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Customer.Microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public CustomerController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region Get Customers
        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            try
            {
                var lst = await _dbContext.Customers.Where(customer => customer.IsDeleted == false).ToListAsync();
                return Ok(lst);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Customer
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCutomer(int id)
        {
            try
            {
                if (id == 0) return BadRequest();

                var item = await _dbContext.Customers.Where(customer => customer.CustomerId == id && customer.IsDeleted == false).FirstOrDefaultAsync();

                return item is null ? NotFound("No data found.") : Ok(item);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerModel customerModel)
        {
            try
            {
                if (customerModel is null || string.IsNullOrEmpty(customerModel.CustomerName) || customerModel.Age == 0) return BadRequest();

                customerModel.CreateDate = DateTime.Now;
                //customerModel.IsDeleted = false;

                await _dbContext.Customers.AddAsync(customerModel);

                int result = await _dbContext.SaveChangesAsync();

                return result > 0 ? StatusCode(StatusCodes.Status201Created, "Customer Created!") : BadRequest();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, CustomerModel customerModel)
        {
            try
            {
                if (id == 0 || string.IsNullOrEmpty(customerModel.CustomerName) || customerModel.Age == 0) return BadRequest();

                CustomerModel? item = await _dbContext.Customers.Where(customer => customer.CustomerId == id).FirstOrDefaultAsync();

                if (item is null) return NotFound("No data found.");

                item.CustomerName = customerModel.CustomerName;
                item.Age = customerModel.Age;

                int result = await _dbContext.SaveChangesAsync();

                return result > 0 ? StatusCode(StatusCodes.Status202Accepted, "Customer Updated!") : BadRequest();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id == 0) return BadRequest();

                CustomerModel? item = await _dbContext.Customers.Where(customer => customer.CustomerId == id).FirstOrDefaultAsync();

                if (item is null) return NotFound("No data found.");

                item.IsDeleted = true;

                int result = await _dbContext.SaveChangesAsync();

                return result > 0 ? StatusCode(StatusCodes.Status202Accepted, "Customer Deleted!") : BadRequest();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
