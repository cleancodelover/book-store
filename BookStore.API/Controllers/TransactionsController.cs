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
    [Authorize]
    [Route("transactions")]
    [ApiController]
    //[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class TransactionsController : ControllerBase
    {
        private readonly ILogger<TransactionsController> _logger;
        private readonly ITransactionBL _transactionBl;
        private readonly IMapper _mapper;

        public TransactionsController(ILogger<TransactionsController> logger, ITransactionBL transactionBl, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _transactionBl = transactionBl;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult GetAllTransactions(string? cartId = null, string? userId = null)
        {
            if (!string.IsNullOrEmpty(cartId))
            {
                var cartTransaction = _transactionBl.GeTransactionsByCartIdAsync(cartId);
                if (cartTransaction.Status == (int)Statuses.Failed)
                    return BadRequest(new ApiResponse<TransactionVM>(true, new List<string>() { cartTransaction.Message ?? "No transactions found." }, null));
                return Ok(new ApiResponse<TransactionVM>(true, new List<string>() { "Success." }, _mapper.Map<TransactionVM>(cartTransaction.Data)));
            }

            if (!string.IsNullOrEmpty(userId))
            {
                var userTransaction = _transactionBl.GeTransactionsByCartIdAsync(userId);
                if (userTransaction.Status == (int)Statuses.Failed)
                    return BadRequest(new ApiResponse<TransactionVM>(true, new List<string>() { userTransaction.Message ?? "No transactions found." }, null));
                return Ok(new ApiResponse<List<TransactionVM>>(true, new List<string>() { "Success." }, _mapper.Map<List<TransactionVM>>(userTransaction.Data)));
            }

            var transactions = _transactionBl.GetTransactionsAsync();
            if (transactions.Status == (int)Statuses.Failed)
                return BadRequest(new ApiResponse<TransactionVM>(false, new List<string>() { transactions.Message ?? "No records found" }, null));
            return Ok(new ApiResponse<List<TransactionVM>>(true, new List<string>() { "Success." }, _mapper.Map<List<TransactionVM>>(transactions.Data)));
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult GetTransactionById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound(new ApiResponse<TransactionVM>(false, new List<string>() { "Transaction not found" }, null));

            var result = _transactionBl.GetTransactionByTransactionIdAsync(id);

            if (result.Status == (int)Statuses.Failed)
                return BadRequest(new ApiResponse<TransactionVM>(false, new List<string>() { result.Message ?? "Cart item not found" }, null)); 

            return Ok(new ApiResponse<TransactionVM>(true, new List<string>() { "Success." }, _mapper.Map<TransactionVM>(result.Data)));
        }

        
        [HttpPost]
        [Route("checkout")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Post([FromBody]CreateTransactionVM vm)
        {
            if(!ModelState.IsValid)
                return NotFound(new ApiResponse<TransactionVM>(false, "Book not found in cart", null));

            var model = _mapper.Map<Transaction>(vm);

            var result = await _transactionBl.AddTransactionAsync(model);
            if (result.Status == (int)Statuses.Failed)
                return BadRequest(new ApiResponse<TransactionVM>(false, result.Message ?? "Unable to add book to cart", null));

            return Ok(new ApiResponse<TransactionVM>(true, "Book added to cart successful.", _mapper.Map<TransactionVM>(result.Data)));
        }
        
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Put(string id, [FromBody] EditTransactionVM vm)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                return NotFound(new ApiResponse<TransactionVM>(false, new List<string>() { "Book not found" }, null));

            var model = _mapper.Map<Transaction>(vm);

            var result = _transactionBl.UpdateTransactionAsync(model);
            if (result.Status == (int)Statuses.Failed)
                return BadRequest(new ApiResponse<TransactionVM>(false, new List<string>() {result.Message ?? "Error updating cart"}, null));
            return Ok(new ApiResponse<TransactionVM>(true, new List<string>() { "Cart updated successful." }, null));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Delete(string id)
        {
            if (!string.IsNullOrEmpty(id))
                return NotFound(new ApiResponse<TransactionVM>(false, new List<string>() { "Transaction not found" }, null));


            var result = _transactionBl.DeleteTransactionAsync(id);

            if (result.Status == (int)Statuses.Failed)
                return BadRequest(new ApiResponse<TransactionVM>(false, new List<string>() { result.Message ?? "Book not found" }, null));

            return Ok(new ApiResponse<TransactionVM>(true, new List<string>() { "Transaction deleted." }, null));
        }
    }
}