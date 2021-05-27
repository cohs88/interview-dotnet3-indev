using AutoMapper;
using GroceryStoreAPI.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Helpers
{
    public class CustomerMapping : Profile
    {
        public CustomerMapping()
        {
            CreateMap<CreateOrUpdateCustomerDto, Core.Entities.Customer>();
            CreateMap<CustomerDto, Core.Entities.Customer>().ReverseMap();
        }
    }
}
