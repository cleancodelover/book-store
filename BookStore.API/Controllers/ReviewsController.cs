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
using BookStore.BLL.BusinessLogic;
using BookStore.DAL.Helpers;

namespace BookStore.API.Controllers
{
    //[Authorize]
    [Route("books/reviews")]
    [ApiController]
    //[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class ReviewsController : ControllerBase
    {
        private readonly ILogger<ReviewsController> _logger;
        private readonly IReviewBL _reviewBl;
        private readonly IMapper _mapper;

        public ReviewsController(ILogger<ReviewsController> logger, IReviewBL reviewBl, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _reviewBl = reviewBl;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult GetAllReviews(string? userId = null, string? bookId = null)
        {
            if (string.IsNullOrEmpty(userId))
            {
                var userReviews= _reviewBl.GetUserReviewsByUserIdAsync(userId);
                if (userReviews.Status == (int)Statuses.Failed)
                    return BadRequest(new ApiResponse<ReviewVM>(true, new List<string>() { userReviews.Message ?? "Book has no Review." }, null));
                return Ok(new ApiResponse<List<ReviewVM>>(true, new List<string>() { "Success." }, _mapper.Map<List<ReviewVM>>(userReviews.Data)));
            }
            if (string.IsNullOrEmpty(bookId))
            {
                var bookReviews = _reviewBl.GetBookReviewsByBookIdAsync(bookId);
                if (bookReviews.Status == (int)Statuses.Failed)
                    return BadRequest(new ApiResponse<ReviewVM>(true, new List<string>() { bookReviews.Message ?? "User has no Review." }, null));
                return Ok(new ApiResponse<List<ReviewVM>>(true, new List<string>() { "Success." }, _mapper.Map<List<ReviewVM>>(bookReviews.Data)));
            }
            var reviews = _reviewBl.GetReviewsAsync();
            if (reviews.Status == (int)Statuses.Failed)
                return BadRequest(new ApiResponse<ReviewVM>(false, new List<string>() { reviews.Message ?? "No records found" }, null));
            return Ok(new ApiResponse<List<ReviewVM>>(true, new List<string>() { "Success." }, _mapper.Map<List<ReviewVM>>(reviews.Data)));
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult GetReviewById(string id)
        {
            if (!string.IsNullOrEmpty(id))
                return NotFound(new ApiResponse<ReviewVM>(false, new List<string>() { "Review not found" }, null));

            var result = _reviewBl.GetReviewByReviewIdAsync(id);

            if (result.Status == (int)Statuses.Failed)
                return BadRequest(new ApiResponse<ReviewVM>(false, new List<string>() { result.Message ?? "Review not found" }, null)); 

            return Ok(new ApiResponse<ReviewVM>(true, new List<string>() { "Success." }, _mapper.Map<ReviewVM>(result.Data)));
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Post([FromBody]CreateReviewVM vm)
        {
            if(!ModelState.IsValid)
                return NotFound(new ApiResponse<ReviewVM>(false, "Book not found", null));

            var model = _mapper.Map<Review>(vm);
            var result = await _reviewBl.AddReviewAsync(model);
            if (result.Status == (int)Statuses.Failed)
                return BadRequest(new ApiResponse<ReviewVM>(false, result.Message ?? "Unable to create Review", null));

            return Ok(new ApiResponse<ReviewVM>(true, "Book created successful.", _mapper.Map<ReviewVM>(result.Data)));
        }
        
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Put(string id, [FromBody] EditReviewVM vm)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                return NotFound(new ApiResponse<ReviewVM>(false, new List<string>() { "Review not found" }, null));

            var model = _mapper.Map<Review>(vm);

            var result = _reviewBl.UpdateReviewAsync(model);
            if (result.Status == (int)Statuses.Failed)
                return BadRequest(new ApiResponse<ReviewVM>(false, new List<string>() {result.Message ?? "Error updating Review"}, null));
            return Ok(new ApiResponse<ReviewVM>(true, new List<string>() { "Review updated successful." }, _mapper.Map<ReviewVM>(result.Data)));
        }


        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Delete(string id)
        {
            if (!string.IsNullOrEmpty(id))
                return NotFound(new ApiResponse<ReviewVM>(false, new List<string>() { "Book not found" }, null));


            var result = _reviewBl.DeleteReviewAsync(id);

            if (result.Status == (int)Statuses.Failed)
                return BadRequest(new ApiResponse<ReviewVM>(false, new List<string>() { result.Message ?? "Book not found" }, null));

            return Ok(new ApiResponse<ReviewVM>(true, new List<string>() { "Review Deleted." }, null));
        }
    }
}