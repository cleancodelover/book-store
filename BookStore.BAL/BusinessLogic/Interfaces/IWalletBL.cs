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
    public interface IWalletBL
    {
        Task<ResponseDTO> AddWalletAsync(Wallet model);
        ResponseDTO UpdateWalletAsync(Wallet model);
        ResponseDTO GetWalletByWalletIdAsync(string? Id);
        ResponseDTO GetUserWalletByUserIdAsync(string? Id);
        ResponseDTO GetWalletsAsync();
        ResponseDTO DeleteWalletAsync(string? Id);
    }
}
