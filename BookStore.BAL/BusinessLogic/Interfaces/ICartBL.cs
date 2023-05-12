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
    public interface ICartBL
    {
        Task<ResponseDTO> AddCartAsync(Cart model);
        ResponseDTO UpdateCartAsync(Cart model);
        ResponseDTO GetCartByCartIdAsync(string? Id);
        ResponseDTO GetUserCartsByUserIdAsync(string? Id);
        ResponseDTO GetUserActiveCartByUserIdAsync(string? Id);
        ResponseDTO GetCartsAsync();
        ResponseDTO DeleteCartAsync(string? Id);
    }
}
