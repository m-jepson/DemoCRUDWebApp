using DependencyInjectionCRUDWebApp.Abstractions.Services;
using DependencyInjectionCRUDWebApp.Models;
using DependencyInjectionCRUDWebApp.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DependencyInjectionCRUDWebApp.Services
{
    public class MongoDbCustomersService : ICustomersService
    {
        private readonly IMongoCollection<Customer> _collection;

        public MongoDbCustomersService(IOptions<MongoDbOptions> mongoDbOptions)
        {
            var client = new MongoClient(mongoDbOptions.Value.Url);
            _collection = client.GetDatabase("customers").GetCollection<Customer>("customers");
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return _collection.Find(c => true).ToList();
        }

        public Customer GetCustomer(int id)
        {
            return _collection.Find(c => c.Id == id).SingleOrDefault();
        }

        public Customer AddCustomer(Customer customer)
        {
            if (customer.Id.HasValue && _collection.Find(c => c.Id == customer.Id).Any())
                throw new ArgumentException("Given Id already exists.");

            if (!customer.Id.HasValue)
            {
                if (_collection.AsQueryable().Any())
                    customer.Id = (_collection.AsQueryable().Max(c => c.Id) ?? 0) + 1;
                else customer.Id = 1;
            }

            customer.CreatedOn = DateTime.UtcNow;

            _collection.InsertOne(customer);

            return customer;
        }

        public Customer DeleteCustomer(int id)
        {
            if (!_collection.Find(c => c.Id == id).Any())
                throw new ArgumentException("Given Id already exists.");

            return _collection.FindOneAndDelete(c => c.Id == id);
        }

        public Customer ReplaceCustomer(Customer customer)
        {
            if (!customer.Id.HasValue)
                throw new ArgumentException("Cannot update a customer without an Id.");

            _collection.ReplaceOne(c => c.Id == customer.Id, customer);

            return customer;
        }
    }
}
