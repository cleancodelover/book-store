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
    public interface IBookBL
    {
        Task<ResponseDTO> AddBookAsync(Book model);
        ResponseDTO UpdateBookAsync(Book model);
        ResponseDTO GetBookByBookIdAsync(string? Id);
        ResponseDTO GetUserBooksByUserIdAsync(string? Id);
        ResponseDTO GetBooksAsync();
        ResponseDTO DeleteBookAsync(string? Id);
    }
}
