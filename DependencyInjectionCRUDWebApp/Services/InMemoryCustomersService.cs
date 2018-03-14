using DependencyInjectionCRUDWebApp.Abstractions.Services;
using DependencyInjectionCRUDWebApp.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DependencyInjectionCRUDWebApp.Services
{
    public class InMemoryCustomersService : ICustomersService
    {
        private readonly ILogger<InMemoryCustomersService> _logger;
        private readonly List<Customer> _customers;

        public InMemoryCustomersService(ILogger<InMemoryCustomersService> logger)
        {
            _logger = logger;
            _customers = new List<Customer>();
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return _customers;
        }

        public Customer GetCustomer(int id)
        {
            return _customers.SingleOrDefault(c => c.Id == id);
        }

        public Customer AddCustomer(Customer customer)
        {
            if(customer.Id.HasValue && _customers.Any(c => c.Id == customer.Id))
                throw new ArgumentException("Given Id already exists.");

            if (!customer.Id.HasValue)
                customer.Id = (_customers.Max(c => c.Id) ?? 0) + 1;

            customer.CreatedOn = DateTime.UtcNow;

            _customers.Add(customer);

            return customer;
        }

        public Customer DeleteCustomer(int id)
        {
            var result = GetCustomer(id);
            if (result == null)
                throw new ArgumentException("Given id does not exist.");

            _customers.Remove(result);

            return result;
        }

        public Customer ReplaceCustomer(Customer customer)
        {
            if(!customer.Id.HasValue)
                throw new ArgumentException("Cannot update a customer without an Id.");

            DeleteCustomer(customer.Id.Value);
            AddCustomer(customer);

            return customer;
        }
    }
}
