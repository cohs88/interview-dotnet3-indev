using GroceryStoreAPI.Core.Entities;
using GroceryStoreAPI.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;

namespace GroceryStoreAPI.Infrastructure.Data
{
    public class CustomerRepository : ICustomerRepository
    {
        private const string FILENAME = "database.json";
        public CustomerRepository()
        {

        }
        public async Task<Customer> Create(Customer customer)
        {
            var customers = await this.Get();

            var list = customers.ToList();
            list.Add(customer);

            var customersList = new CustomersList() { Customers = list };

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            string jsonString = JsonSerializer.Serialize(customersList, options);

            await File.WriteAllTextAsync(FILENAME, jsonString);

            return customer;
        }

        public async Task<IEnumerable<Customer>> Get()
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            using FileStream openStream = File.OpenRead(FILENAME);
            var jsonObj = await JsonSerializer.DeserializeAsync<CustomersList>(openStream, options);

            return jsonObj.Customers;
        }

        public async Task<Customer> Get(int customerId)
        {
            var customers = await this.Get();

            return customers.FirstOrDefault(c => c.Id == customerId);
        }

        public async Task<Customer> Update(Customer customer)
        {
            var customers = await this.Get();

            var list = customers.ToList();

            var customerFromDB = list.FirstOrDefault(c => c.Id == customer.Id);
            customerFromDB.Name = customer.Name;

            var customersList = new CustomersList() { Customers = list };

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            string jsonString = JsonSerializer.Serialize(customersList, options);

            await File.WriteAllTextAsync(FILENAME, jsonString);

            return customerFromDB;
        }
    }
}
