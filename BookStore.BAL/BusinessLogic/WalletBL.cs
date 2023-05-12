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
    public class WalletBL : IWalletBL
    {
        public readonly IRepository<Wallet> _repository;
        public WalletBL(IRepository<Wallet> repository)
        {
            _repository = repository;
        }
        //Create Method
        public async Task<ResponseDTO> AddWalletAsync(Wallet model)
        {
            try
            {
                if (model == null)
                {
                    throw new ArgumentNullException(nameof(model));
                }
                else
                {
                    var result = await _repository.Create(model);
                    return new ResponseDTO { Data = result, Message = "Success", Status = (int)Statuses.Success };
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ResponseDTO UpdateWalletAsync(Wallet model)
        {
            try
            {
                var wallet = _repository.GetById(model.Id);
                if(wallet == null)
                    return new ResponseDTO { Data = null, Message = "Wallet not found.", Status = (int)Statuses.Failed };

                wallet.Balance = model.Balance + wallet.Balance;
                wallet.UserId = model.UserId ?? wallet.UserId;
                wallet.Status = model.Status ?? wallet.Status;
                
                _repository.Update(wallet);
                return new ResponseDTO { Data = null, Message = "Success", Status = (int)Statuses.Success };
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ResponseDTO DeleteWalletAsync(string? Id)
        {
            try
            {
                if (!string.IsNullOrEmpty(Id))
                {
                    var model = _repository.GetById(Id);
                    if (!string.IsNullOrEmpty(model.Id))
                    {
                        _repository.Delete(model);
                        return new ResponseDTO { Data = null, Message = "Success", Status = (int)Statuses.Success };
                    }
                }
                return new ResponseDTO { Data = null, Message = "Unable to delete wallet.", Status = (int)Statuses.Success };
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ResponseDTO GetWalletsAsync()
        {
            try
            {
                var result = _repository.GetAll();
                return new ResponseDTO { Data = result, Message = "Success", Status = (int)Statuses.Success };
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ResponseDTO GetWalletByWalletIdAsync(string? Id)
        {
            try
            {
                var result = _repository.GetById(Id);
                return new ResponseDTO { Data = result, Message = "Success", Status = (int)Statuses.Success };
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ResponseDTO GetUserWalletByUserIdAsync(string? Id)
        {
            try
            {
                var result = _repository.GetAll().FirstOrDefault(x=>x.UserId==Id);
                return new ResponseDTO { Data = result, Message = "Success", Status = (int)Statuses.Success };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
