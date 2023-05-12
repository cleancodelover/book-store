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
    public interface ITransactionBL
    {
        Task<ResponseDTO> AddTransactionAsync(Transaction model);
        ResponseDTO UpdateTransactionAsync(Transaction model);
        ResponseDTO GetTransactionByTransactionIdAsync(string? Id);
        ResponseDTO GetUserTransactionsByUserIdAsync(string? Id);
        ResponseDTO GeTransactionsByCartIdAsync(string? Id);
        ResponseDTO GetTransactionsAsync();
        ResponseDTO DeleteTransactionAsync(string? Id);
    }
}
