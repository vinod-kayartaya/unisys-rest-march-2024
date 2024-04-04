using AspNetCoreCustomersWebApi.Models;
using AspNetCoreCustomersWebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreCustomersWebApi.Controllers
{
    // http://localhost:5030/api/customers
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerService _service;
        
        // this constructor is called automatically during the bootstrap
        // by the asp.net web api engine, and the dependency injector is
        // supposed to inject an object of the required class
        public CustomersController(CustomerService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult HandleGetAll()
        {
            return Ok(_service.GetAllCustomers());
        }

        [HttpGet("{id}")]
        public IActionResult HandleGetOne(Guid id) {
            var customer = _service.GetCustomerById(id);
            if(customer==null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpPost]
        public IActionResult HandlePost(Customer customer)
        {
            try
            {
                _service.AddCustomer(customer);
                return CreatedAtAction(nameof(HandleGetOne), new { id = customer.Id }, customer);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult HandlePut(Guid id, Customer customer)
        {
            customer.Id = id;

            try
            {
                _service.UpdateCustomer(customer);
                return Ok(customer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{id}")]
        public IActionResult HandleDelete(Guid id)
        {
            try
            {
                var deletedCustomer = _service.DeleteCustomer(id);
                return Ok(deletedCustomer);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
