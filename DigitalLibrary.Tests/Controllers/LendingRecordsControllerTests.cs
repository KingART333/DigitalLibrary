using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using DigitalLibraryApi.Controllers;
using DigitalLibraryConsole.Interfaces;
using DigitalLibraryConsole.Models;
using DigitalLibraryApi.DTOs;
using System.Collections.Generic;
using System;

namespace DigitalLibrary.Tests.Controllers
{
    public class LendingRecordsControllerTests
    {
        private readonly Mock<ILibraryService> _mockService;
        private readonly LendingRecordsController _controller;

        public LendingRecordsControllerTests()
        {
            _mockService = new Mock<ILibraryService>();
            _controller = new LendingRecordsController(_mockService.Object);
        }

        [Fact]
        public void GetAllLendingRecords_ShouldReturnOk_WithRecords()
        {
            var records = new List<LendingRecord> { new LendingRecord(), new LendingRecord() };
            _mockService.Setup(s => s.GetAllLendingRecords(1, 10)).Returns(records);

            var result = _controller.GetAllLendingRecords();

            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(records);
        }

        [Fact]
        public void GetById_ShouldReturnOk_WhenRecordExists()
        {
            var record = new LendingRecord { Id = 1 };
            _mockService.Setup(s => s.GetLendingRecordById(1)).Returns(record);

            var result = _controller.GetById(1);

            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(record);
        }

        [Fact]
        public void GetById_ShouldReturnNotFound_WhenRecordNotExists()
        {
            _mockService.Setup(s => s.GetLendingRecordById(1)).Returns((LendingRecord)null);

            var result = _controller.GetById(1);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void GetByUserId_ShouldReturnOk_WhenRecordsFound()
        {
            var records = new List<LendingRecord> { new LendingRecord() };
            _mockService.Setup(s => s.GetLendingRecordsByUser(1, 1, 10)).Returns(records);

            var result = _controller.GetByUserId(1, 1, 10);

            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(records);
        }

        [Fact]
        public void GetByUserId_ShouldReturnNotFound_WhenNoneFound()
        {
            _mockService.Setup(s => s.GetLendingRecordsByUser(1, 1, 10)).Returns(new List<LendingRecord>());

            var result = _controller.GetByUserId(1);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void BorrowBook_ShouldReturnOk_WhenSuccessful()
        {
            var dto = new CreateLendingRecordDto { UserId = 1, BookId = 2 };
            _mockService.Setup(s => s.BorrowBook(dto.UserId, dto.BookId)).Returns(true);

            var result = _controller.BorrowBook(dto);

            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().Be("Book borrowed successfully.");
        }

        [Fact]
        public void BorrowBook_ShouldReturnBadRequest_WhenFails()
        {
            var dto = new CreateLendingRecordDto { UserId = 1, BookId = 2 };
            _mockService.Setup(s => s.BorrowBook(dto.UserId, dto.BookId)).Returns(false);

            var result = _controller.BorrowBook(dto);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void ReturnBook_ShouldReturnOk_WhenSuccessful()
        {
            var record = new LendingRecord
            {
                Id = 1,
                BorrowDate = DateTime.UtcNow.AddDays(-5),
                DueDate = DateTime.UtcNow.AddDays(5),
                ReturnDate = DateTime.UtcNow,
                Book = new Book("Book", "Author", "123", 1),
                User = new User { Name = "John", Surname = "Smith" }
            };

            _mockService.Setup(s => s.ReturnBook(1)).Returns(true);
            _mockService.Setup(s => s.GetLendingRecordById(1)).Returns(record);

            var result = _controller.ReturnBook(1);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void ReturnBook_ShouldReturnBadRequest_WhenReturnFails()
        {
            _mockService.Setup(s => s.ReturnBook(1)).Returns(false);

            var result = _controller.ReturnBook(1);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void ReturnBook_ShouldReturnNotFound_WhenRecordMissingAfterReturn()
        {
            _mockService.Setup(s => s.ReturnBook(1)).Returns(true);
            _mockService.Setup(s => s.GetLendingRecordById(1)).Returns((LendingRecord)null);

            var result = _controller.ReturnBook(1);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void DeleteLendingRecord_ShouldReturnNoContent()
        {
            var result = _controller.DeleteLendingRecord(1);

            result.Should().BeOfType<NoContentResult>();
        }
    }
}
