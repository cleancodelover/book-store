using AutoMapper;
using BookStore.API.ViewModels.Post;
using BookStore.API.ViewModels.Get;
using BookStore.BLL.BusinessLogic.Interfaces;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using BookStore.API.Helpers;
using BookStore.API.ViewModels.Put;
using Microsoft.AspNetCore.Mvc.Routing;
using BookStore.DAL.Helpers;

namespace BookStore.API.Controllers
{
    //[Authorize]
    [Route("books")]
    [ApiController]
    //[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class BooksController : ControllerBase
    {
        private readonly ILogger<BooksController> _logger;
        private readonly IBookBL _bookBl;
        private readonly IMapper _mapper;

        public BooksController(ILogger<BooksController> logger, IBookBL bookBl, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _bookBl = bookBl;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult GetAllBooks(string? userId = null)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var userBooks= _bookBl.GetUserBooksByUserIdAsync(userId);
                if (userBooks.Status == (int)Statuses.Failed)
                    return BadRequest(new ApiResponse<BookVM>(true, new List<string>() { userBooks.Message ?? "Unknown Error" }, null));
                return Ok(new ApiResponse<List<BookVM>>(true, new List<string>() { userBooks.Message ?? "Success." }, _mapper.Map<List<BookVM>>(userBooks.Data)));
            }
            
               var books = _bookBl.GetBooksAsync();
            if (books.Status == (int)Statuses.Failed)
                return BadRequest(new ApiResponse<BookVM>(false, new List<string>() { books.Message ?? "No records found" }, null));
            return Ok(new ApiResponse<List<BookVM>>(true, new List<string>() { books.Message ?? "Success" }, _mapper.Map<List<BookVM>>(books.Data)));
        }

        [HttpGet]
        [Route("explore")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult ExploreBooks()
        {
            var books = _bookBl.GetBooksAsync();
            if (books.Status == (int)Statuses.Failed)
                return BadRequest(new ApiResponse<BookVM>(false, new List<string>() { books.Message ?? "No records found" }, null));
            return Ok(new ApiResponse<List<BookVM>>(true, new List<string>() { books.Message ?? "Success." }, _mapper.Map<List<BookVM>>(books.Data)));
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult GetBookById(string id)
        {
            if (!string.IsNullOrEmpty(id))
                return NotFound(new ApiResponse<BookVM>(false, new List<string>() { "Book not found" }, null));

            var result = _bookBl.GetBookByBookIdAsync(id);

            if (result.Status == (int)Statuses.Failed)
                return BadRequest(new ApiResponse<BookVM>(false, new List<string>() { result.Message ?? "No records found" }, null));

            return Ok(new ApiResponse<BookVM>(true, new List<string>() { "Success." }, _mapper.Map<BookVM>(result.Data)));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Post([FromBody]CreateBookVM vm)
        {
            if(!ModelState.IsValid)
                return NotFound(new ApiResponse<BookVM>(false, "Book not found", null));

            var model = _mapper.Map<Book>(vm);

            var result = await _bookBl.AddBookAsync(model);
            if (result.Status == (int)Statuses.Failed)
                return BadRequest(new ApiResponse<BookVM>(false, result.Message ?? "Unable to create Book", null));

            return Ok(new ApiResponse<BookVM>(true, "Book created successful.", _mapper.Map<BookVM>(result.Data)));
        }
        
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Put(string id, [FromBody] EditBookVM vm)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                return NotFound(new ApiResponse<BookVM>(false, new List<string>() { "Book not found" }, null));

            var model = _mapper.Map<Book>(vm);

            var result = _bookBl.UpdateBookAsync(model);
            if (result.Status == (int)Statuses.Failed)
                return BadRequest(new ApiResponse<BookVM>(false, new List<string>() {result.Message ?? "Error updating Book"}, null));
            return Ok(new ApiResponse<BookVM>(true, new List<string>() { "Book updated successful." }, null));
        }


        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Delete(string id)
        {
            if (!string.IsNullOrEmpty(id))
                return NotFound(new ApiResponse<BookVM>(false, new List<string>() { "Book not found" }, null));


            var result = _bookBl.DeleteBookAsync(id);

            if (result.Status == (int)Statuses.Failed)
                return BadRequest(new ApiResponse<BookVM>(false, new List<string>() { result.Message ?? "Book not found" }, null));

            return Ok(new ApiResponse<BookVM>(true, new List<string>() { "Book Deleted." }, null));
        }
    }
}