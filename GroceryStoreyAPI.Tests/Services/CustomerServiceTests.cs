using AutoMapper;
using GroceryStoreAPI.Controllers;
using GroceryStoreAPI.Core.Entities;
using GroceryStoreAPI.Core.Interfaces;
using GroceryStoreAPI.Dtos;
using GroceryStoreAPI.Helpers;
using GroceryStoreAPI.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using System.Linq;

namespace GroceryStoreyAPI.Tests.Controllers
{
    public class CustomerServiceTests : IDisposable
    {
        private Mock<ICustomerRepository> _mockRepo;

        public CustomerServiceTests()
        {
            _mockRepo = new Mock<ICustomerRepository>();


        }

        [Fact]
        public async Task GetCustomers_Returns_Success()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.Get())
                .ReturnsAsync(value: new List<Customer>() { new Customer { } });

            var service = new CustomerService(_mockRepo.Object);
            //Act
            var result = await service.Get();

            //Assert
            Assert.True(result.Any());
        }

        [Fact]
        public async Task GetCustomerById_ReturnsNull()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.Get(0))
                .ReturnsAsync(value: null);

            var service = new CustomerService(_mockRepo.Object);
            //Act
            var result = await service.Get(0);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetCustomerById_ReturnsCustomer()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.Get(It.IsAny<int>()))
                .ReturnsAsync(value: new Customer { Name = "c1" });

            var service = new CustomerService(_mockRepo.Object);
            //Act
            var result = await service.Get(1);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Name == "c1");
        }

        [Fact]
        public async Task CreateCustomer_Success()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.Get()).ReturnsAsync(value: new List<Customer> { new Customer { Id = 10, Name = "Test" } });
            _mockRepo.Setup(repo => repo.Create(It.IsAny<Customer>()))
                .ReturnsAsync(value: new Customer { Id = 11, Name = "c1" });

            var service = new CustomerService(_mockRepo.Object);
            //Act
            var result = await service.Create(new Customer { Name = "c1"  });

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Name == "c1", "Created Name did not match");
            Assert.True(result.Id == 11, "Created Id did not match");
        }

        [Fact]
        public async Task UpdateCustomer_MissingId()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.Get()).ReturnsAsync(value: new List<Customer> { new Customer { Id = 10, Name = "Test" } });
            _mockRepo.Setup(repo => repo.Update(It.IsAny<Customer>()));

            var service = new CustomerService(_mockRepo.Object);

            //assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => service.Update(new Customer { Name = "Test" }));

            //assert
            Assert.Equal("Customer id is required", exception.Message);
        }

        [Fact]
        public async Task UpdateCustomer_MissingName()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.Get()).ReturnsAsync(value: new List<Customer> { new Customer { Id = 10, Name = "Test" } });
            _mockRepo.Setup(repo => repo.Update(It.IsAny<Customer>()));

            var service = new CustomerService(_mockRepo.Object);

            //assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => service.Update(new Customer { Id = 1 }));

            //assert
            Assert.Equal("Customer name is required", exception.Message);
        }

        [Fact]
        public async Task UpdateCustomer_Success()
        {
            var customerToUpdate = new Customer { Id = 1, Name = "test" };
            // Arrange
            _mockRepo.Setup(repo => repo.Get()).ReturnsAsync(value: new List<Customer> { new Customer { Id = 10, Name = "Test" } });
            _mockRepo.Setup(repo => repo.Update(It.IsAny<Customer>())).ReturnsAsync(value: customerToUpdate);

            var service = new CustomerService(_mockRepo.Object);

            //assert
            var result = await service.Update(customerToUpdate);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Id == 1);
            Assert.True(result.Name == "test");
        }

        public void Dispose()
        {
            _mockRepo = null;
        }
    }
}
