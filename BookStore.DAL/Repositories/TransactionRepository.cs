using BookStore.DAL.Contracts;
using BookStore.DAL.Data;
using BookStore.DAL.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DAL.Repositories
{
    public class TransactionRepository : IRepository<Transaction>
    {
        private readonly BOOK_STOREContext _appDbContext;
        private readonly ILogger _logger;
        public TransactionRepository (ILogger<Transaction> logger, BOOK_STOREContext appDbContext)
        {
            _logger = logger;
            _appDbContext = appDbContext;
        }
        public async Task<Transaction> Create(Transaction model)
        {
            try
            {
                if (model != null)
                {
                    var obj = _appDbContext.Add<Transaction>(model);
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
        public void Delete(Transaction model)
        {
            try
            {
                if (model != null)
                {
                    var obj = _appDbContext.Remove(model);
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

        public IEnumerable<Transaction> GetAll()
        {
            try
            {
                var obj = _appDbContext.Transactions.ToList();
                if (obj != null) return obj;
                else return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Transaction GetById(string? Id)
        {
            try
            {
                if (Id != null)
                {
                    var Obj = _appDbContext.Transactions.FirstOrDefault(x => x.Id == Id);
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

        public Task<Transaction> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public void Update(Transaction model)
        {
            try
            {
                if (model != null)
                {
                    var obj = _appDbContext.Update(model);
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
