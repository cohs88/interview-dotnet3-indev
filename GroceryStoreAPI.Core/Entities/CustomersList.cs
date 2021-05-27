using System;
using System.Collections.Generic;
using System.Text;

namespace GroceryStoreAPI.Core.Entities
{
    public class CustomersList
    {
        public CustomersList()
        {
            Customers = new List<Customer>();
        }
        public ICollection<Customer> Customers { get; set; }
    }
}
