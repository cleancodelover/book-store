using BookStore.DAL.Models;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System.Net.Http.Headers;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace BookStore.IntegrationTest
{
    [TestClass]
    public class BookIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        string Id = Guid.NewGuid().ToString();
        string UserId = Guid.NewGuid().ToString();

        public BookIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CreateBook_AddToCart_Checkout_Success()
        {
            // Arrange
            var client = _factory.CreateClient();
            var book = new Book { Id=Id, UserId= UserId, Title = "I have a dream.", Author = "Marthin Luther (Jr.)", UnitCost = 5000, Description = "From Dr. Martin Luther King, Jr.’s daughter, Dr. Bernice A. King: “My father’s dream continues to live on from generation to generation, and this beautiful and powerful illustrated edition of his world-changing" };

            // Create a book
            var createResponse = await client.PostAsync("/books", SerializeObject(book));
            createResponse.EnsureSuccessStatusCode();

            // Add the book to the cart
            var addToCartResponse = await client.PostAsync("/carts", SerializeObject(book));
            addToCartResponse.EnsureSuccessStatusCode();

            // Checkout the cart
            var checkoutResponse = await client.PostAsync("/transactions/checkout", null);
            checkoutResponse.EnsureSuccessStatusCode();

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, checkoutResponse.StatusCode);
        }

        private HttpContent SerializeObject<T>(T obj)
        {
            var json = JsonSerializer.Serialize(obj);
            var content = new ByteArrayContent(Encoding.UTF8.GetBytes(json));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return content;
        }
    }
}