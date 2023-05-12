using BookStore.BLL.BusinessLogic.Interfaces;
using BookStore.DAL.Contracts;
using BookStore.DAL.Helpers;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.BLL.BusinessLogic
{
    public class UserBL: IUserBL
    {
        public readonly IRepository<AspNetUser> _repository;
        private readonly UserManager<AspNetUser> _userManager;
        public UserBL(IRepository<AspNetUser> repository, UserManager<AspNetUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }
        //Create Method
        public ResponseDTO AddUserAsync(AspNetUser appUser, string password)
        {
            try
            {
                if (appUser == null)
                {
                    throw new ArgumentNullException(nameof(appUser));
                }
                else
                {
                    var result = _userManager.CreateAsync(appUser, password);
                    if(!result.Result.Succeeded) return new ResponseDTO { Data = result.Result, Message = result.Result.Errors.Select(x => x.Description).ToString(), Status = (int)Statuses.Failed };
                    return new ResponseDTO { Data = result.Result, Message = "Success", Status = (int)Statuses.Success };
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ResponseDTO UpdateUserAsync(AspNetUser model)
        {
            try
            {
                
                var result = _userManager.UpdateAsync(model);
                return new ResponseDTO { Data = result.Result, Message = "Success", Status = (int)Statuses.Success };
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ResponseDTO DeleteUserAsync(string? Id)
        {
            try
            {
                if (!string.IsNullOrEmpty(Id))
                {
                    var user = _userManager.FindByIdAsync(Id);
                    if (user.IsCompleted)
                    {
                        var obj = _userManager.DeleteAsync(user.Result);
                        if(!obj.Result.Succeeded) return new ResponseDTO { Data = obj.Result.Succeeded, Message = obj.Result.Errors.Select(x => x.Description).ToString(), Status = (int)Statuses.Success };
                        return new ResponseDTO { Data = obj.Result.Succeeded, Message = "Success", Status = (int)Statuses.Success };
                    }
                }
                return new ResponseDTO { Data = null, Message = "Unable to delete user", Status = (int)Statuses.Failed };
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ResponseDTO GetUsersAsync()
        {
            try
            {
                var result = _userManager.Users.ToList();
                return new ResponseDTO { Data = result, Message = "Success", Status = (int)Statuses.Success };
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<ResponseDTO> GetUserByUserIdAsync(string? Id)
        {
            try
            {
                var result = await _userManager.FindByIdAsync(Id);
                return new ResponseDTO { Data = result, Message = "Success", Status = (int)Statuses.Success };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
