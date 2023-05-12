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
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BookStore.API.Controllers
{
    //[Authorize]
    [Route("users")]
    [ApiController]
    //[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserBL _userBl;
        private readonly IMapper _mapper;

        public UsersController(ILogger<UsersController> logger, IUserBL userBl, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _userBl = userBl;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult GetAllUsers()
        {
            var result = _userBl.GetUsersAsync();

            if (result.Status == (int)Statuses.Failed)
                return BadRequest(new ApiResponse<AspNetUserVM>(false, new List<string>() { "No records found" }, null));
            return Ok(new ApiResponse<List<AspNetUserVM>>(true, new List<string>() { "Success." }, _mapper.Map<List<AspNetUserVM>>(result.Data)));
        }


        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult GetUserById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound(new ApiResponse<AspNetUserVM>(false, new List<string>() { "User not found" }, null));

            var result = _userBl.GetUserByUserIdAsync(id).Result;

            if (result.Status == (int)Statuses.Failed)
                return BadRequest(new ApiResponse<AspNetUserVM>(false, new List<string>() { result.Message ?? "User not found" }, null));

            return Ok(new ApiResponse<AspNetUserVM>(true, new List<string>() { "Success." }, _mapper.Map<AspNetUserVM>(result.Data)));
        }

        [HttpGet]
        [Route("authorized")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult GetAuthorizedUser()
        {

            var authUser_id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = _userBl.GetUserByUserIdAsync(authUser_id).Result;

            if (result.Status == (int)Statuses.Failed)
                return BadRequest(new ApiResponse<AspNetUserVM>(false, new List<string>() { result.Message ?? "User not found" }, null));

            return Ok(new ApiResponse<AspNetUserVM>(true, new List<string>() { "Success." }, _mapper.Map<AspNetUserVM>(result.Data)));
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Post([FromBody]CreateAspNetUserVM user)
        {
            if(!ModelState.IsValid)
                return NotFound(new ApiResponse<AspNetUserVM>(false, "User not found", null));

            var model = _mapper.Map<AspNetUser>(user);
            model.UserName = user.Email;

            var result = _userBl.AddUserAsync(model, user.Password);
            if (result.Status == (int)Statuses.Failed)
                return BadRequest(new ApiResponse<AspNetUserVM>(false, result.Message ?? "", null));

            return Ok(new ApiResponse<AspNetUserVM>(true, "Account created successful.", null));
        }
        
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Put(string id, [FromBody] EditAspNetUserVM user)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                return NotFound(new ApiResponse<AspNetUserVM>(false, new List<string>() { "User not found" }, null));

            var model = _mapper.Map<AspNetUser>(user);

            var result = _userBl.UpdateUserAsync(model);
            if (result.Status == (int)Statuses.Failed)
                return BadRequest(new ApiResponse<AspNetUserVM>(false, result.Message ?? "", null));

            return Ok(new ApiResponse<AspNetUserVM>(true, new List<string>() { "Account updated successful." }, null));
        }


        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Delete(string id)
        {
            if (!string.IsNullOrEmpty(id))
                return NotFound(new ApiResponse<AspNetUserVM>(false, new List<string>() { "User not found" }, null));


            var result = _userBl.DeleteUserAsync(id);

            if (result.Status == (int)Statuses.Failed)
                return BadRequest(new ApiResponse<AspNetUserVM>(false, new List<string>() { result.Message ?? "User not found" }, null));

            return Ok(new ApiResponse<AspNetUserVM>(true, new List<string>() { "User Deleted." }, null));
        }
    }
}