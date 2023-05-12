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
    [Route("wallets")]
    [ApiController]
    //[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class WalletsController : ControllerBase
    {
        private readonly ILogger<WalletsController> _logger;
        private readonly IWalletBL _walletBl;
        private readonly IMapper _mapper;


        public WalletsController(ILogger<WalletsController> logger, IWalletBL walletBl, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _walletBl = walletBl;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult GetAllWallets(string? userId = null)
        {

            if (!string.IsNullOrEmpty(userId))
            {
                var wallet = _walletBl.GetUserWalletByUserIdAsync(userId);
                if (wallet.Status == (int)Statuses.Failed)
                    return BadRequest(new ApiResponse<WalletVM>(true, new List<string>() { wallet.Message ?? "User has no wallet." }, null));
                return Ok(new ApiResponse<WalletVM>(true, new List<string>() { "Success." }, _mapper.Map<WalletVM>(wallet.Data)));
            }
            
               var wallets = _walletBl.GetWalletsAsync();
            if (wallets.Status == (int)Statuses.Failed)
                return BadRequest(new ApiResponse<WalletVM>(false, new List<string>() { wallets.Message ?? "No records found" }, null));
            return Ok(new ApiResponse<List<WalletVM>>(true, new List<string>() { "Success." }, _mapper.Map<List<WalletVM>>(wallets.Data)));
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult GetWalletById(string id)
        {
            if (!string.IsNullOrEmpty(id))
                return NotFound(new ApiResponse<WalletVM>(false, new List<string>() { "User not found" }, null));

            var result = _walletBl.GetWalletByWalletIdAsync(id);

            if (result.Status == (int)Statuses.Failed)
                return BadRequest(new ApiResponse<WalletVM>(false, new List<string>() { result.Message ?? "User not found" }, null)); 
            return Ok(new ApiResponse<WalletVM>(true, new List<string>() { "Success." }, _mapper.Map<WalletVM>(result.Data)));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Post([FromBody]CreateWalletVM vm)
        {
            var authUser_id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;

            if (!ModelState.IsValid)
                return NotFound(new ApiResponse<WalletVM>(false, "Wallet not found", null));

            var model = _mapper.Map<Wallet>(vm);
            model.CreatedBy = authUser_id;
            var result = await _walletBl.AddWalletAsync(model);
            if (result.Status == (int)Statuses.Failed)
                return BadRequest(new ApiResponse<WalletVM>(false, result.Message ?? "Unable to create wallet", null));
            return Ok(new ApiResponse<WalletVM>(true, "Wallet created successful.", _mapper.Map<WalletVM>(result.Data)));
        }
        
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Put(string id, [FromBody] EditWalletVM vm)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                return NotFound(new ApiResponse<WalletVM>(false, new List<string>() { "Wallet not found" }, null));

            var model = _mapper.Map<Wallet>(vm);

            var result = _walletBl.UpdateWalletAsync(model);
            if (result.Status == (int)Statuses.Failed)
                return BadRequest(new ApiResponse<WalletVM>(false, new List<string>() {result.Message ?? "Error updating wallet"}, null));
            return Ok(new ApiResponse<WalletVM>(true, new List<string>() { "Wallet updated successful." }, null));
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Delete(string id)
        {
            if (!string.IsNullOrEmpty(id))
                return NotFound(new ApiResponse<WalletVM>(false, new List<string>() { "Wallet not found" }, null));

            var result = _walletBl.DeleteWalletAsync(id);

            if (result.Status == (int)Statuses.Failed)
                return BadRequest(new ApiResponse<WalletVM>(false, new List<string>() { result.Message ?? "Wallet not found" }, null));
            return Ok(new ApiResponse<WalletVM>(true, new List<string>() { "Wallet Deleted." }, null));
        }
    }
}