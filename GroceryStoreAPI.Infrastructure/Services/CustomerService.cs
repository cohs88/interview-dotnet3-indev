using GroceryStoreAPI.Core.Entities;
using GroceryStoreAPI.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace GroceryStoreAPI.Infrastructure.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        private void RunValidations(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("Customer is required");
            
            if (string.IsNullOrEmpty(customer.Name))
                throw new ArgumentException("Customer name is required");
        }

        public async Task<Customer> Create(Customer customer)
        {
            RunValidations(customer);

            var data = await this.Get();
            var lastCustomerId = data.Max(d => d.Id);

            customer.Id = lastCustomerId + 1;

            return await _customerRepository.Create(customer);
        }

        public async Task<IEnumerable<Customer>> Get()
        {
            return await _customerRepository.Get();
        }

        public async Task<Customer> Get(int customerId)
        {
            return await _customerRepository.Get(customerId);
        }

        public async Task<Customer> Update(Customer customer)
        {
            RunValidations(customer);
            if (customer.Id <= 0)
                throw new ArgumentException("Customer id is required");

            return await _customerRepository.Update(customer);
        }
    }
}
