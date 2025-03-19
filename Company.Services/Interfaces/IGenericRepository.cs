using Company.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Services.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        IEnumerable<TEntity> GetAll();
        TEntity? Get(int Id);
        void Add(TEntity model);
        void Update(TEntity model);
        void Delete(TEntity model);
    }
}
