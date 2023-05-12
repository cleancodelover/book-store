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
    public interface IReviewBL
    {
        Task<ResponseDTO> AddReviewAsync(Review model);
        ResponseDTO UpdateReviewAsync(Review model);
        ResponseDTO GetReviewByReviewIdAsync(string? Id);
        ResponseDTO GetBookReviewsByBookIdAsync(string? Id);
        ResponseDTO GetUserReviewsByUserIdAsync(string? Id);
        ResponseDTO GetReviewsAsync();
        ResponseDTO DeleteReviewAsync(string? Id);
    }
}
