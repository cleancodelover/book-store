using BookStore.DAL.Helpers;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.BLL.BusinessLogic.Interfaces
{
    public interface IUserBL
    {
        ResponseDTO AddUserAsync(AspNetUser model, string password);
        ResponseDTO UpdateUserAsync(AspNetUser model);
        Task<ResponseDTO> GetUserByUserIdAsync(string? Id);
        ResponseDTO GetUsersAsync();
        ResponseDTO DeleteUserAsync(string? Id);
    }
}
