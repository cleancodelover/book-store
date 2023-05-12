using BookStore.DAL.Contracts;
using BookStore.DAL.Data;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DAL.Repositories
{
    public class UserRepository: IRepository<AspNetUser>
    {
        private readonly UserManager<AspNetUser> _userManager;
        private readonly ILogger _logger;
        private readonly BOOK_STOREContext _appDbContext;
        public UserRepository(ILogger<AspNetUser> logger, UserManager<AspNetUser> userManager, BOOK_STOREContext appDbContext)
        {
            _logger = logger;
            _appDbContext = appDbContext;
            _userManager = userManager;
        }
        public async Task<AspNetUser> Create(AspNetUser appuser)
        {
            try
            {
                if (appuser != null)
                {
                    var obj = _appDbContext.Add<AspNetUser>(appuser);
                    await _appDbContext.SaveChangesAsync();
                    return obj.Entity;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Delete(AspNetUser appuser)
        {
            try
            {
                if (appuser != null)
                {
                    var obj = _appDbContext.Remove(appuser);
                    if (obj != null)
                    {
                        _appDbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<AspNetUser> GetAll()
        {
            try
            {
                var obj =  _appDbContext.Users.ToList();
                if (obj != null) return obj;
                else return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<AspNetUser> GetByIdAsync(string Id)
        {
            try
            {
                if (Id != null)
                {
                    var Obj = await _userManager.FindByIdAsync(Id);
                    if (Obj != null) return Obj;
                    return null;
                }
                    return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public AspNetUser  GetById(string Id)
        {
            try
            {
                if (Id != null)
                {
                    var Obj =  _userManager.FindByIdAsync(Id).Result;
                    if (Obj != null) return Obj;
                    return null;
                }
                    return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Update(AspNetUser appuser)
        {
            try
            {
                if (appuser != null)
                {
                    var obj = _appDbContext.Update(appuser);
                    if (obj != null) _appDbContext.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
