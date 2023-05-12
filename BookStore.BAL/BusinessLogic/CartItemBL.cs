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
    public class CartItemBL : ICartItemBL
    {
        public readonly IRepository<CartItem> _repository;
        public CartItemBL(IRepository<CartItem> repository)
        {
            _repository = repository;
        }
        //Create Method
        public async Task<ResponseDTO> AddCartItemAsync(CartItem model)
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

        public ResponseDTO UpdateCartItemAsync(CartItem model)
        {
            try
            {
                var cartItem = _repository.GetById(model.Id);
                if (cartItem == null)
                    return new ResponseDTO { Data = null, Message = "Cart not found.", Status = (int)Statuses.Failed };

                cartItem.Status = model.Status ?? cartItem.Status;
                cartItem.BookId = model.BookId ?? cartItem.BookId;
                cartItem.Quantity = model.Quantity ?? cartItem.Quantity;
                cartItem.CartId = model.CartId ?? cartItem.CartId;

                _repository.Update(cartItem);
                return new ResponseDTO { Data = null, Message = "Success", Status = (int)Statuses.Success };
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ResponseDTO DeleteCartItemAsync(string? Id)
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
                return new ResponseDTO { Data = null, Message = "Unable to delete item", Status = (int)Statuses.Failed };
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ResponseDTO GetCartItemsAsync()
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
        public ResponseDTO GetCartItemByCartItemIdAsync(string? Id)
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
        public ResponseDTO GetCartItemsByCartIdAsync(string? Id)
        {
            try
            {
                var result = _repository.GetAll().Where(x=>x.CartId==Id);
                return new ResponseDTO { Data = result, Message = "Success", Status = (int)Statuses.Success };
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ResponseDTO GetCartItemsByBookIdAsync(string? Id)
        {
            try
            {
                var result = _repository.GetAll().Where(x => x.BookId == Id);
                return new ResponseDTO { Data = result, Message = "Success", Status = (int)Statuses.Success };
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ResponseDTO GetCartItemsByBookIdAndCartIdAsync(string? CartId, string BookId)
        {
            try
            {
                var result = _repository.GetAll().Where(x => x.BookId == BookId && x.CartId==CartId)?.FirstOrDefault();
                return new ResponseDTO { Data = result, Message = "Success", Status = (int)Statuses.Success };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
