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
using BookStore.BLL.BusinessLogic;

namespace BookStore.API.Controllers
{
    [Authorize]
    [Route("cart/items")]
    [ApiController]
    //[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class CartItemsController : ControllerBase
    {
        private readonly ILogger<CartItemsController> _logger;
        private readonly ICartItemBL _cartItemBl;
        private readonly ICartBL _cartBl;
        private readonly IMapper _mapper;

        public CartItemsController(ILogger<CartItemsController> logger, ICartItemBL cartItemBl, ICartBL cartBl, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _cartItemBl = cartItemBl;
            _cartBl = cartBl;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult GetAllCartItems(string? cartId = null)
        {
            if (!string.IsNullOrEmpty(cartId))
            {
                var cartItems = _cartItemBl.GetCartItemsByCartIdAsync(cartId);
                if (cartItems.Status == (int)Statuses.Failed)
                    return BadRequest(new ApiResponse<CartItemVM>(true, new List<string>() { cartItems.Message ?? "Cart has no items." }, null));
                return Ok(new ApiResponse<List<CartItemVM>>(true, new List<string>() { cartItems.Message ?? "Success." }, _mapper.Map<List<CartItemVM>>(cartItems.Data)));
            }
            
            var allCartItems = _cartItemBl.GetCartItemsAsync();
            if (allCartItems.Status == (int)Statuses.Failed)
                return BadRequest(new ApiResponse<CartItemVM>(false, new List<string>() { allCartItems.Message ?? "No records found" }, null));
            return Ok(new ApiResponse<List<CartItemVM>>(true, new List<string>() { "Success." }, _mapper.Map<List<CartItemVM>>(allCartItems.Data)));
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult GetCartById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound(new ApiResponse<CartItemVM>(false, new List<string>() { "Cart item not found" }, null));

            var result = _cartItemBl.GetCartItemByCartItemIdAsync(id);

            if (result.Status == (int)Statuses.Failed)
                return BadRequest(new ApiResponse<CartItemVM>(false, new List<string>() { result.Message ?? "Cart item not found" }, null)); 

            return Ok(new ApiResponse<CartItemVM>(true, new List<string>() { "Success." }, _mapper.Map<CartItemVM>(result.Data)));
        }

        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Post([FromBody]CreateCartItemVM vm)
        {
            if(!ModelState.IsValid)
                return NotFound(new ApiResponse<CartItemVM>(false, "Book not found in cart", null));

            var model = _mapper.Map<CartItem>(vm);

            var result = await _cartItemBl.AddCartItemAsync(model);
            if (result.Status == (int)Statuses.Failed)
                return BadRequest(new ApiResponse<CartItemVM>(false, result.Message ?? "Unable to add book to cart", null));

            return Ok(new ApiResponse<CartItemVM>(true, "Book added to cart successful.", _mapper.Map<CartItemVM>(result.Data)));
        }
        
        
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Put(string id, [FromBody] EditCartItemVM vm)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                return NotFound(new ApiResponse<CartItemVM>(false, new List<string>() { "Book not found" }, null));

            var model = _mapper.Map<CartItem>(vm);

            var result = _cartItemBl.UpdateCartItemAsync(model);
            if (result.Status == (int)Statuses.Failed)
                return BadRequest(new ApiResponse<CartItemVM>(false, new List<string>() {result.Message ?? "Error updating cart"}, null));
            return Ok(new ApiResponse<CartItemVM>(true, new List<string>() { "Cart updated successful." }, null));
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Delete(string id)
        {
            if (!string.IsNullOrEmpty(id))
                return NotFound(new ApiResponse<CartItemVM>(false, new List<string>() { "Book not found in cart" }, null));


            var result = _cartItemBl.DeleteCartItemAsync(id);

            if (result.Status == (int)Statuses.Failed)
                return BadRequest(new ApiResponse<CartItemVM>(false, new List<string>() { result.Message ?? "Book not found in cart" }, null));

            return Ok(new ApiResponse<CartItemVM>(true, new List<string>() { "Book Deleted from cart." }, null));
        }
    }
}