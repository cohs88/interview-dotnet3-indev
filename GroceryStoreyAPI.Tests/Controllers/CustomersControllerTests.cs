using AutoMapper;
using GroceryStoreAPI.Controllers;
using GroceryStoreAPI.Core.Entities;
using GroceryStoreAPI.Core.Interfaces;
using GroceryStoreAPI.Dtos;
using GroceryStoreAPI.Helpers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace GroceryStoreyAPI.Tests.Controllers
{
    public class CustomersControllerTests : IDisposable
    {
        private Mock<ICustomerService> _mockService;
        private CustomerMapping _realProfile;
        private MapperConfiguration _configuration;
        private IMapper _mapper;

        public CustomersControllerTests()
        {
            _mockService = new Mock<ICustomerService>();
            _realProfile = new CustomerMapping();
            _configuration = new MapperConfiguration(cfg => cfg.
                AddProfile(_realProfile));
            _mapper = new Mapper(_configuration);
        }

        [Fact]
        public void GetCustomers_Returns200OK_WhenDBIsEmpty()
        {
            // Arrange
            _mockService.Setup(repo => repo.Get())
                .ReturnsAsync(value: new List<Customer>() { });

            var controller = new CustomersController(_mockService.Object, _mapper);
            //Act
            var result = controller.Get();

            //Assert
            Assert.IsType<OkObjectResult>(result.Result.Result);
        }

        [Fact]
        public void GetCustomerById_Returns404NotFound_WhenNonExistentIdProvided()
        {
            // Arrange
            _mockService.Setup(repo => repo.Get(0))
                .ReturnsAsync(value: null);

            var controller = new CustomersController(_mockService.Object, _mapper);
            //Act
            var result = controller.GetCustomer(0);

            //Assert
            Assert.IsType<NotFoundResult>(result.Result.Result);
        }

        [Fact]
        public void GetCustomerById_Returns200OK_WhenValidIDProvided()
        {
            // Arrange
            _mockService.Setup(repo => repo.Get(1))
                .ReturnsAsync(value: new Customer());

            var controller = new CustomersController(_mockService.Object, _mapper);
            //Act
            var result = controller.GetCustomer(1);

            //Assert
            Assert.IsType<OkObjectResult>(result.Result.Result);
        }

        [Fact]
        public void CreateCustomer_Returns201Created_WhenValidObjectSubmitted()
        {
            // Arrange
            _mockService.Setup(repo => repo.Create(It.IsAny<Customer>()))
                .ReturnsAsync(value: new Customer() { Id = 1 });

            var controller = new CustomersController(_mockService.Object, _mapper);
            //Act
            var result = controller.CreateCustomer(new CreateOrUpdateCustomerDto());

            //Assert
            Assert.IsType<CreatedAtRouteResult>(result.Result.Result);
        }

        [Fact]
        public void UpdateCustomer_Returns404NotFound_WhenNonExistentIdProvided()
        {
            // Arrange
            _mockService.Setup(repo => repo.Get(0))
                .ReturnsAsync(value: null);

            var controller = new CustomersController(_mockService.Object, _mapper);
            //Act
            var result = controller.UpdateCustomer(0, new CreateOrUpdateCustomerDto { });

            //Assert
            Assert.IsType<NotFoundResult>(result.Result.Result);
        }

        [Fact]
        public void UpdateCustomer_Returns200OK_WhenValidIDProvided()
        {
            // Arrange
            _mockService.Setup(repo => repo.Get(1))
                .ReturnsAsync(value: new Customer());

            var controller = new CustomersController(_mockService.Object, _mapper);
            //Act
            var result = controller.GetCustomer(1);

            //Assert
            Assert.IsType<OkObjectResult>(result.Result.Result);
        }

        public void Dispose()
        {
            _mockService = null;
            _mapper = null;
            _configuration = null;
            _realProfile = null;
        }
    }
}
