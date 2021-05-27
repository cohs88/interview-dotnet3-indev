using AutoMapper;
using GroceryStoreAPI.Core.Entities;
using GroceryStoreAPI.Core.Interfaces;
using GroceryStoreAPI.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;
        public CustomersController(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieve all customers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> Get()
        {
            var data = await _customerService.Get();
            var customers = _mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerDto>>(data.ToList());
            return Ok(customers);
        }

        [HttpGet("{id}", Name = "GetCustomer")]
        public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
        {
            var data = await _customerService.Get(id);
            if (data == null)
            {
                return NotFound();
            }
            var customer = _mapper.Map<Customer, CustomerDto>(data);
            return Ok(customer);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> CreateCustomer(CreateOrUpdateCustomerDto createCustomerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(createCustomerDto);
            
            var data = _mapper.Map<CreateOrUpdateCustomerDto, Customer>(createCustomerDto);

            var createdCustomer = await _customerService.Create(data);

            var returnData = _mapper.Map<Customer, CustomerDto>(createdCustomer);

            return CreatedAtRoute(nameof(GetCustomer), new { id = returnData.Id }, returnData);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerDto>> UpdateCustomer(int id, CreateOrUpdateCustomerDto updateCustomerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(updateCustomerDto);

            if (await _customerService.Get(id) == null)
            {
                return NotFound();
            }

            var data = _mapper.Map<CreateOrUpdateCustomerDto, Customer>(updateCustomerDto);

            data.Id = id;

            await _customerService.Update(data);

            var returnData = _mapper.Map<Customer, CustomerDto>(data);

            return Ok(returnData);
        }
    }
}
