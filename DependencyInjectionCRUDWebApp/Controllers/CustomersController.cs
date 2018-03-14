using DependencyInjectionCRUDWebApp.Abstractions.Services;
using DependencyInjectionCRUDWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DependencyInjectionCRUDWebApp.Controllers
{
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        private readonly ICustomersService _customersService;

        public CustomersController(ICustomersService customersService)
        {
            _customersService = customersService;
        }

        // GET
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_customersService.GetAllCustomers());
        }

        // GET 
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(_customersService.GetCustomer(id));
        }

        // POST 
        [HttpPost]
        public IActionResult Post([FromBody]Customer customer)
        {
            try
            {
                var result = _customersService.AddCustomer(customer);
                return CreatedAtAction(nameof(Get), new {id = result.Id}, result);
            }
            catch (ArgumentException)
            {
                return BadRequest("Given Id already exists. Use PUT if you want to overwrite this record.");
            }
        }

        // PUT
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Customer customer)
        {
            try
            {
                customer.Id = id;
                _customersService.ReplaceCustomer(customer);
                return NoContent();
            }
            catch (ArgumentException)
            {
                return BadRequest("Given Id does not exist.");
            }
        }

        // DELETE
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _customersService.DeleteCustomer(id);
                return NoContent();
            }
            catch (ArgumentException)
            {
                return BadRequest("Given Id does not exist.");
            }
        }
    }
}
