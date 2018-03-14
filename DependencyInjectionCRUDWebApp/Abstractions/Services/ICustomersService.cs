using DependencyInjectionCRUDWebApp.Models;
using System.Collections.Generic;

namespace DependencyInjectionCRUDWebApp.Abstractions.Services
{
    public interface ICustomersService
    {
        IEnumerable<Customer> GetAllCustomers();
        Customer GetCustomer(int id);
        Customer AddCustomer(Customer customer);
        Customer DeleteCustomer(int id);
        Customer ReplaceCustomer(Customer customer);
    }
}
