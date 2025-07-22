using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using DigitalLibraryApi.Controllers;
using DigitalLibraryConsole.Interfaces;
using DigitalLibraryConsole.Models;
using System.Collections.Generic;

namespace DigitalLibrary.Tests.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<ILibraryService> _mockService;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _mockService = new Mock<ILibraryService>();
            _controller = new UsersController(_mockService.Object);
        }

        [Fact]
        public void GetAllUsers_ShouldReturnOk_WithUserList()
        {
            var users = new List<User>
            {
                new User { Id = 1, Name = "John" },
                new User { Id = 2, Name = "Jane" }
            };

            _mockService.Setup(s => s.GetAllUsers(1, 10)).Returns(users);

            var result = _controller.GetAllUsers();

            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(users);
        }

        [Fact]
        public void GetById_ShouldReturnOk_WhenUserExists()
        {
            var user = new User { Id = 1, Name = "John" };

            _mockService.Setup(s => s.GetUserById(1)).Returns(user);

            var result = _controller.GetById(1);

            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().Be(user);
        }

        [Fact]
        public void GetById_ShouldReturnNotFound_WhenUserNotExists()
        {
            _mockService.Setup(s => s.GetUserById(1)).Returns((User)null);

            var result = _controller.GetById(1);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void GetByName_ShouldReturnOk_WhenUserExists()
        {
            var user = new User { Id = 1, Name = "John" };
            _mockService.Setup(s => s.GetUserByName("John")).Returns(user);

            var result = _controller.GetByName("John");

            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().Be(user);
        }

        [Fact]
        public void GetByName_ShouldReturnNotFound_WhenUserNotExists()
        {
            _mockService.Setup(s => s.GetUserByName("John")).Returns((User)null);

            var result = _controller.GetByName("John");

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void GetBySurname_ShouldReturnOk_WhenUserExists()
        {
            var user = new User { Id = 1, Surname = "Smith" };
            _mockService.Setup(s => s.GetUserBySurname("Smith")).Returns(user);

            var result = _controller.GetBySurname("Smith");

            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().Be(user);
        }

        [Fact]
        public void GetBySurname_ShouldReturnNotFound_WhenUserNotExists()
        {
            _mockService.Setup(s => s.GetUserBySurname("Smith")).Returns((User)null);

            var result = _controller.GetBySurname("Smith");

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void GetByPhoneNumber_ShouldReturnOk_WhenUserExists()
        {
            var user = new User { Id = 1, PhoneNumber = "1234567890" };
            _mockService.Setup(s => s.GetUserByPhoneNumber("1234567890")).Returns(user);

            var result = _controller.GetByPhoneNumber("1234567890");

            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().Be(user);
        }

        [Fact]
        public void GetByPhoneNumber_ShouldReturnNotFound_WhenUserNotExists()
        {
            _mockService.Setup(s => s.GetUserByPhoneNumber("1234567890")).Returns((User)null);

            var result = _controller.GetByPhoneNumber("1234567890");

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void Create_ShouldReturnCreatedAtAction_WithUser()
        {
            var user = new User { Id = 1, Name = "Alice" };

            var result = _controller.Create(user);

            result.Should().BeOfType<CreatedAtActionResult>()
                .Which.Value.Should().Be(user);
        }

        [Fact]
        public void Delete_ShouldReturnNoContent()
        {
            var result = _controller.Delete(1);

            result.Should().BeOfType<NoContentResult>();
        }
    }
}
