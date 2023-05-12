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
    public class CartBL : ICartBL
    {
        public readonly IRepository<Cart> _repository;
        public CartBL(IRepository<Cart> repository)
        {
            _repository = repository;
        }
        //Create Method
        public async Task<ResponseDTO> AddCartAsync(Cart model)
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

        public ResponseDTO UpdateCartAsync(Cart model)
        {
            try
            {
                var cart = _repository.GetById(model.Id);
                if (cart == null)
                    return new ResponseDTO { Data = null, Message = "Cart not found.", Status = (int)Statuses.Failed };

                cart.Status = model.Status?? cart.Status;
                cart.UserId = model.UserId ?? cart.UserId;
                cart.TotalAmount = model.TotalAmount ?? cart.TotalAmount;

                _repository.Update(cart);
                return new ResponseDTO { Data = null, Message = "Success", Status = (int)Statuses.Success };
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ResponseDTO DeleteCartAsync(string? Id)
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
                return new ResponseDTO { Data = null, Message = "Unable to delete cart", Status = (int)Statuses.Failed };
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ResponseDTO GetCartsAsync()
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
        public ResponseDTO GetCartByCartIdAsync(string? Id)
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
        public ResponseDTO GetUserCartsByUserIdAsync(string? Id)
        {
            try
            {
                var result = _repository.GetAll().Where(x=>x.UserId==Id);
                return new ResponseDTO { Data = result, Message = "Success", Status = (int)Statuses.Success };
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ResponseDTO GetUserActiveCartByUserIdAsync(string? Id)
        {
            try
            {
                var result = _repository.GetAll().FirstOrDefault(x => x.UserId == Id & x.Status=="Active");
                return new ResponseDTO { Data = result, Message = "Success", Status = (int)Statuses.Success };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
