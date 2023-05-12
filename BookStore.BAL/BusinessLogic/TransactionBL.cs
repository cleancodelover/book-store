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
    public class TransactionBL : ITransactionBL
    {
        public readonly IRepository<Transaction> _repository;
        public readonly IRepository<Cart> _cartRepository;
        public readonly IRepository<Wallet> _walletRepository;
        public TransactionBL(IRepository<Transaction> repository, IRepository<Cart> cartRepository, IRepository<Wallet> walletRepository)
        {
            _repository = repository;
            _cartRepository = cartRepository;
            _walletRepository = walletRepository;
        }
        //Create Method
        public async Task<ResponseDTO> AddTransactionAsync(Transaction model)
        {
            try
            {
                if (model == null || string.IsNullOrEmpty(model.CartId))
                {
                    throw new ArgumentNullException(nameof(model));
                }
                else
                {
                    var cart = _cartRepository.GetById(model.CartId);
                    if (cart == null)
                        return new ResponseDTO { Data = null, Message="Cart not found", Status= (int)Statuses.Failed};

                    var wallet = _walletRepository.GetAll().Where(x => x.UserId == cart.UserId).FirstOrDefault();

                    if(wallet == null)
                        return new ResponseDTO { Data = null, Message = "Unable to identify your wallet", Status = (int)Statuses.Failed };

                    var items = cart.CartItems.ToList();
                    if(items == null)
                        return new ResponseDTO { Data = null, Message = "Select one or more books to make payment.", Status = (int)Statuses.Failed };

                    var totalAmount = items != null ? items.Sum(x => x.Quantity * x.Book.UnitCost) : 0;

                    if (totalAmount <= 0)
                        return new ResponseDTO { Data = null, Message = "Total cost cannot be less than or equal to zero", Status = (int)Statuses.Failed };

                    if (totalAmount > wallet.Balance)
                        return new ResponseDTO { Data = null, Message = "You have insufficient balance", Status = (int)Statuses.Failed };

                    model.Amount = totalAmount;

                    var result = await _repository.Create(model);

                    if (string.IsNullOrEmpty(result.Id))
                        return new ResponseDTO { Data = null, Message = "Unable to complete transactions. Please try again.", Status = (int)Statuses.Failed };

                    wallet.Balance = wallet.Balance - totalAmount;

                    _walletRepository.Update(wallet);

                    return new ResponseDTO { Data = result, Message = "Success", Status = (int)Statuses.Success };
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ResponseDTO UpdateTransactionAsync(Transaction model)
        {
            try
            {
                var transaction = _repository.GetById(model.Id);
                if (transaction == null)
                    return new ResponseDTO { Data = null, Message = "Transaction not found.", Status = (int)Statuses.Failed };

                transaction.Status = model.Status ?? transaction.Status;
                transaction.CartId = model.CartId ?? transaction.CartId;
                transaction.TransactionReference = model.TransactionReference ?? transaction.TransactionReference;
                transaction.Amount = model.Amount ?? transaction.Amount;
                

                _repository.Update(transaction);
                return new ResponseDTO { Data = null, Message = "Success", Status = (int)Statuses.Success };
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ResponseDTO DeleteTransactionAsync(string? Id)
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
                return new ResponseDTO { Data = null, Message = "Success", Status = (int)Statuses.Failed };
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ResponseDTO GetTransactionsAsync()
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
        public ResponseDTO GetTransactionByTransactionIdAsync(string? Id)
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
        public ResponseDTO GetUserTransactionsByUserIdAsync(string? Id)
        {
            try
            {
                var result = _repository.GetAll().Where(x=>x.CreatedBy==Id);
                return new ResponseDTO { Data = result, Message = "Success", Status = (int)Statuses.Success };
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ResponseDTO GeTransactionsByCartIdAsync(string? Id)
        {
            try
            {
                var result = _repository.GetAll()?.Where(x => x.CartId == Id).FirstOrDefault();
                return new ResponseDTO { Data = result, Message = "Success", Status = (int)Statuses.Success };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
