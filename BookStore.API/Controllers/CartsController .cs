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
using System.Security.Claims;

namespace BookStore.API.Controllers
{
    [Authorize]
    [Route("carts")]
    [ApiController]
    //[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class CartsController : ControllerBase
    {
        private readonly ILogger<CartsController> _logger;
        private readonly ICartBL _cartsBl;
        private readonly ICartItemBL _cartItemBL;
        private readonly IMapper _mapper;

        public CartsController(ILogger<CartsController> logger, ICartBL cartsBl, ICartItemBL cartItemBL, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _cartsBl = cartsBl;
            _cartItemBL = cartItemBL;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult GetAllCarts(string? userId = null, bool? isActive = false)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                if (isActive == true)
                {
                    var activeCart = _cartsBl.GetUserActiveCartByUserIdAsync(userId);
                    if (activeCart.Status == (int)Statuses.Failed)
                        return BadRequest(new ApiResponse<CartVM>(true, new List<string>() { activeCart.Message ?? "User has no active carts." }, null));
                    return Ok(new ApiResponse<CartVM>(true, new List<string>() { "Success." }, _mapper.Map<CartVM>(activeCart.Data)));
                }

                var userCarts = _cartsBl.GetUserCartsByUserIdAsync(userId);
                if (userCarts.Status == (int)Statuses.Failed)
                    return BadRequest(new ApiResponse<CartVM>(true, new List<string>() { userCarts.Message ?? "User has no carts." }, null));
                return Ok(new ApiResponse<List<CartVM>>(true, new List<string>() { "Success." }, _mapper.Map<List<CartVM>>(userCarts.Data)));
            }
            
            var carts = _cartsBl.GetCartsAsync();
            if(carts == null)
                return BadRequest(new ApiResponse<CartVM>(false, new List<string>() { "No records found" }, null));
            return Ok(new ApiResponse<List<CartVM>>(true, new List<string>() { "Success." }, _mapper.Map<List<CartVM>>(carts)));
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult GetCartById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound(new ApiResponse<CartVM>(false, new List<string>() { "Cart not found" }, null));

            var result = _cartsBl.GetCartByCartIdAsync(id);

            if (result.Status == (int)Statuses.Failed)
                return BadRequest(new ApiResponse<CartVM>(false, new List<string>() {result.Message ?? "Cart not found" }, null)); 

            return Ok(new ApiResponse<CartVM>(true, new List<string>() { "Success." }, _mapper.Map<CartVM>(result.Data)));
        }

        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Post([FromBody]CreateCartVM vm)
        {
            var authUser_id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;

            if (!ModelState.IsValid)
                return NotFound(new ApiResponse<CartVM>(false, "Cart not found", null));

            var cart = _cartsBl.GetUserActiveCartByUserIdAsync(authUser_id);
            if(cart.Status== (int)Statuses.Success)
            {
                var existingCart = (Cart)cart.Data;
                if (vm.Items.Count() > 0)
                {
                    foreach (var item in vm.Items)
                    {
                        var cartItem = _cartItemBL.GetCartItemsByBookIdAndCartIdAsync(existingCart?.Id, item.BookId);
                        if(cartItem.Status == (int)Statuses.Success && cartItem.Data != null)
                        {
                            CartItem book = (CartItem)cartItem.Data;
                            book.Book = null;
                            book.Cart = null;
                            book.Quantity = item.Quantity;
                            _cartItemBL.UpdateCartItemAsync(book);
                            continue;
                        }
                        item.CartId = vm.Id;
                        await _cartItemBL.AddCartItemAsync(_mapper.Map<CartItem>(item));
                    }
                }
                return Ok(new ApiResponse<CartVM>(true, "Cart updated successful.", _mapper.Map<CartVM>(cart.Data)));
            }

            var model = _mapper.Map<Cart>(vm);
            model.UserId = authUser_id;
            var result = await _cartsBl.AddCartAsync(model);
            if (result.Status == (int)Statuses.Failed)
                return BadRequest(new ApiResponse<CartVM>(false, result.Message ?? "Unable to create cart", null));

            if (vm.Items.Count() > 0)
            {
                foreach (var item in vm.Items)
                {
                    item.CartId = vm.Id;
                    await _cartItemBL.AddCartItemAsync(_mapper.Map<CartItem>(item));
                }
            }


            return Ok(new ApiResponse<CartVM>(true, "Cart created successful.", _mapper.Map<CartVM>(result.Data)));
        }
        
        
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Put(string id, [FromBody] EditCartVM vm)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                return NotFound(new ApiResponse<CartVM>(false, new List<string>() { "Cart not found" }, null));

            var model = _mapper.Map<Cart>(vm);

            var result = _cartsBl.UpdateCartAsync(model);
            if (result.Status == (int)Statuses.Failed)
                return BadRequest(new ApiResponse<CartVM>(false, new List<string>() {result.Message ?? "Error updating cart"}, null));
            return Ok(new ApiResponse<CartVM>(true, new List<string>() { "Cart updated successful." }, _mapper.Map<CartVM>(result.Data)));
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Delete(string id)
        {
            if (!string.IsNullOrEmpty(id))
                return NotFound(new ApiResponse<CartVM>(false, new List<string>() { "Cart not found" }, null));


            var result = _cartsBl.DeleteCartAsync(id);

            if (result.Status == (int)Statuses.Failed)
                return BadRequest(new ApiResponse<CartVM>(false, new List<string>() { result.Message ?? "Cart not found" }, null));

            return Ok(new ApiResponse<CartVM>(true, new List<string>() { "Cart Deleted." }, null));
        }
    }
}