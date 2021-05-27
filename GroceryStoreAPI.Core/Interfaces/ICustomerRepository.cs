using GroceryStoreAPI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Core.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer> Create(Customer customer);
        Task<Customer> Update(Customer customer);
        Task<IEnumerable<Customer>> Get();
        Task<Customer> Get(int customerId);
    }
}
