using BookStore.BLL.BusinessLogic;
using BookStore.BLL.BusinessLogic.Interfaces;
using BookStore.DAL.Contracts;
using BookStore.DAL.Helpers;
using BookStore.DAL.Models;
using BookStore.DAL.Repositories;
using Moq;

namespace BookStore.UnitTest
{
    [TestClass]
    public class BookUnitTest
    {
        [TestMethod]
        public void CreateProduct_ShouldCreateNewProduct()
        {
            // Arrange
            var mockBookRepository = new Mock<IRepository<Book>>();
            var bookService = new BookBL(mockBookRepository.Object);
            string Id = Guid.NewGuid().ToString();
            string UserId = Guid.NewGuid().ToString();
            var newBook = new Book { Id= Id, UserId= UserId, Title = "I have a dream.", Author="Marthin Luther (Jr.)", UnitCost=5000, Description= "From Dr. Martin Luther King, Jr.’s daughter, Dr. Bernice A. King: “My father’s dream continues to live on from generation to generation, and this beautiful and powerful illustrated edition of his world-changing" };

            // Assume that the book creation is successful
            mockBookRepository.Setup(repo => repo.Create(newBook).Result).Returns(new Book { Id= Id, Title = "I have a dream.", Author = "Marthin Luther (Jr.)", UnitCost = 5000, Description = "From Dr. Martin Luther King, Jr.’s daughter, Dr. Bernice A. King: “My father’s dream continues to live on from generation to generation, and this beautiful and powerful illustrated edition of his world-changing" });

            // Act
            ResponseDTO result = bookService.AddBookAsync(newBook).Result;

            // Assert
            Assert.AreEqual(result.Status, (int)Statuses.Success);
        }
    }
}