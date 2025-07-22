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
    public class BooksControllerTests
    {
        private readonly Mock<ILibraryService> _mockLibraryService;
        private readonly BooksController _controller;

        public BooksControllerTests()
        {
            _mockLibraryService = new Mock<ILibraryService>();
            _controller = new BooksController(_mockLibraryService.Object);
        }

        [Fact]
        public void GetAllBooks_ReturnsOkResult_WithBooks()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book("Title 1", "Author 1", "123", 3),
                new Book("Title 2", "Author 2", "456", 2)
            };

            _mockLibraryService
                .Setup(s => s.GetAvailableBooks(1, 10))
                .Returns(books);

            // Act
            var result = _controller.GetAllBooks();

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(books);
        }

        [Fact]
        public void Create_ReturnsCreatedAtAction_WithBook()
        {
            // Arrange
            var book = new Book("New Book", "Author X", "789", 5) { Id = 101 };

            // Act
            var result = _controller.Create(book);

            // Assert
            var createdAtActionResult = result as CreatedAtActionResult;
            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult!.ActionName.Should().Be(nameof(_controller.GetAllBooks));
            createdAtActionResult.Value.Should().Be(book);
        }

        [Fact]
        public void GetByTitle_ReturnsOk_WhenBookFound()
        {
            // Arrange
            var book = new Book("Clean Code", "Robert Martin", "123456", 4);
            _mockLibraryService.Setup(s => s.GetBookByTitle("Clean Code")).Returns(book);

            // Act
            var result = _controller.GetByTitle("Clean Code");

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().Be(book);
        }

        [Fact]
        public void GetByTitle_ReturnsNotFound_WhenBookNotFound()
        {
            // Arrange
            _mockLibraryService.Setup(s => s.GetBookByTitle("Unknown")).Returns((Book?)null);

            // Act
            var result = _controller.GetByTitle("Unknown");

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void GetByAuthor_ReturnsOk_WhenBooksFound()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book("Book A", "Author A", "111", 2),
                new Book("Book B", "Author A", "222", 1)
            };

            _mockLibraryService.Setup(s => s.GetBooksByAuthor("Author A", 1, 10)).Returns(books);

            // Act
            var result = _controller.GetByAuthor("Author A");

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(books);
        }

        [Fact]
        public void GetByAuthor_ReturnsNotFound_WhenNoBooks()
        {
            // Arrange
            _mockLibraryService.Setup(s => s.GetBooksByAuthor("Author B", 1, 10)).Returns(new List<Book>());

            // Act
            var result = _controller.GetByAuthor("Author B");

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void GetByISBN_ReturnsOk_WhenBookFound()
        {
            // Arrange
            var book = new Book("Book X", "Author X", "isbn123", 3);
            _mockLibraryService.Setup(s => s.GetBookByISBN("isbn123")).Returns(book);

            // Act
            var result = _controller.GetByISBN("isbn123");

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().Be(book);
        }

        [Fact]
        public void GetByISBN_ReturnsNotFound_WhenBookNotFound()
        {
            // Arrange
            _mockLibraryService.Setup(s => s.GetBookByISBN("notfound")).Returns((Book?)null);

            // Act
            var result = _controller.GetByISBN("notfound");

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void Delete_ReturnsNoContent()
        {
            // Arrange
            int bookId = 5;

            // Act
            var result = _controller.Delete(bookId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _mockLibraryService.Verify(s => s.DeleteBook(bookId), Times.Once);
        }
    }
}
