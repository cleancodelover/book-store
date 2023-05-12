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
    public interface ICartItemBL
    {
        Task<ResponseDTO> AddCartItemAsync(CartItem model);
        ResponseDTO UpdateCartItemAsync(CartItem model);
        ResponseDTO GetCartItemByCartItemIdAsync(string? Id);
        ResponseDTO GetCartItemsByCartIdAsync(string? Id);
        ResponseDTO GetCartItemsByBookIdAsync(string? Id);
        ResponseDTO GetCartItemsAsync();
        ResponseDTO DeleteCartItemAsync(string? Id);
        ResponseDTO GetCartItemsByBookIdAndCartIdAsync(string? CartId, string BookId);
    }
}
