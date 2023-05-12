using BookStore.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DAL.Contracts
{
    public interface IRepository<T>
    {
         Task<T> Create(T _object);
         void Update(T _object);
         void Delete(T _object);
         Task<T> GetByIdAsync(string id);
         T GetById(string id);
         IEnumerable<T> GetAll();
    }
}
