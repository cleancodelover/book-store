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
    public class BookBL : IBookBL
    {
        public readonly IRepository<Book> _repository;
        public BookBL(IRepository<Book> repository)
        {
            _repository = repository;
        }
        //Create Method
        public async Task<ResponseDTO> AddBookAsync(Book model)
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

        public ResponseDTO UpdateBookAsync(Book model)
        {
            try
            {
                var book = _repository.GetById(model.Id);
                if (book == null)
                    return new ResponseDTO { Data = null, Message = "Book not found.", Status = (int)Statuses.Failed };

                book.Title = model.Title ?? book.Title;
                book.UserId = model.UserId ?? book.UserId;
                book.ImageUrl = model.ImageUrl ?? book.ImageUrl;
                book.Description = model.Description ?? book.Description;
                book.Author = model.Author ?? book.Author;
                book.UnitCost = model.UnitCost ?? book.UnitCost;

                _repository.Update(model);
                return new ResponseDTO { Data = null, Message = "Success", Status = (int)Statuses.Success };
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ResponseDTO DeleteBookAsync(string? Id)
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
                return new ResponseDTO { Data = null, Message = "Cannot get result", Status = (int)Statuses.Failed };
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ResponseDTO GetBooksAsync()
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
        public ResponseDTO GetBookByBookIdAsync(string? Id)
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
        public ResponseDTO GetUserBooksByUserIdAsync(string? Id)
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
    }
}
