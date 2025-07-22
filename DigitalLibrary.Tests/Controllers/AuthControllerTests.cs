using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DigitalLibraryApi.Controllers;
using DigitalLibraryApi.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Xunit;

namespace DigitalLibrary.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly AuthController _controller;
        private readonly IConfiguration _mockConfig;

        public AuthControllerTests()
        {
            // Manually build mock config
            var configDict = new Dictionary<string, string>
            {
                { "Jwt:Key", "ThisIsASecretKeyForJwt1234" },
                { "Jwt:Issuer", "DigitalLibrary" },
                { "Jwt:Audience", "DigitalLibraryUsers" }
            };

            _mockConfig = new ConfigurationBuilder()
                .AddInMemoryCollection(configDict!)
                .Build();

            _controller = new AuthController(_mockConfig);
        }

        [Fact]
        public void Login_AdminCredentials_ReturnsOkWithToken()
        {
            // Arrange
            var dto = new LoginDto
            {
                Username = "admin",
                Password = "password",
                Age = 30
            };

            // Act
            var result = _controller.Login(dto);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);

            var tokenResult = okResult.Value?.GetType().GetProperty("token")?.GetValue(okResult.Value)?.ToString();
            var roleResult = okResult.Value?.GetType().GetProperty("role")?.GetValue(okResult.Value)?.ToString();

            tokenResult.Should().NotBeNullOrEmpty();
            roleResult.Should().Be("Admin");
        }

        [Fact]
        public void Login_UserCredentials_ReturnsOkWithToken()
        {
            // Arrange
            var dto = new LoginDto
            {
                Username = "user",
                Password = "password",
                Age = 22
            };

            // Act
            var result = _controller.Login(dto);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);

            var token = okResult.Value?.GetType().GetProperty("token")?.GetValue(okResult.Value)?.ToString();
            var role = okResult.Value?.GetType().GetProperty("role")?.GetValue(okResult.Value)?.ToString();

            token.Should().NotBeNullOrEmpty();
            role.Should().Be("User");
        }

        [Fact]
        public void Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var dto = new LoginDto
            {
                Username = "hacker",
                Password = "wrongpass",
                Age = 25
            };

            // Act
            var result = _controller.Login(dto);

            // Assert
            result.Should().BeOfType<UnauthorizedResult>();
        }
    }
}
