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
    public class ReviewBL : IReviewBL
    {
        public readonly IRepository<Review> _repository;
        public ReviewBL(IRepository<Review> repository)
        {
            _repository = repository;
        }
        //Create Method
        public async Task<ResponseDTO> AddReviewAsync(Review model)
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

        public ResponseDTO UpdateReviewAsync(Review model)
        {
            try
            {
                var review = _repository.GetById(model.Id);
                if (review == null)
                    return new ResponseDTO { Data = null, Message = "Review not found.", Status = (int)Statuses.Failed };

                review.Status = model.Status ?? review.Status;
                review.BookId = model.BookId ?? review.BookId;
                review.UserId = model.UserId ?? review.UserId;
                review.Description = model.Description ?? review.Description;


                _repository.Update(review);
                return new ResponseDTO { Data = null, Message = "Success", Status = (int)Statuses.Success };
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ResponseDTO DeleteReviewAsync(string? Id)
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
                return new ResponseDTO { Data = null, Message = "Unable to delete review", Status = (int)Statuses.Failed };
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ResponseDTO GetReviewsAsync()
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
        public ResponseDTO GetReviewByReviewIdAsync(string? Id)
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
        public ResponseDTO GetBookReviewsByBookIdAsync(string? Id)
        {
            try
            {
                var result = _repository.GetAll().Where(x=>x.BookId==Id);
                return new ResponseDTO { Data = result, Message = "Success", Status = (int)Statuses.Success };
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ResponseDTO GetUserReviewsByUserIdAsync(string? Id)
        {
            try
            {
                var result = _repository.GetAll().Where(x => x.UserId == Id);
                return new ResponseDTO { Data = result, Message = "Success", Status = (int)Statuses.Success };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
