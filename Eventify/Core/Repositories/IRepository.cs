using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Eventify.Core.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> Get(Guid id);

        Task<TEntity> GetWithRelated(Guid id, params Expression<Func<TEntity, object>>[] includes);

        Task<IEnumerable<TEntity>> GetAll();

        Task<IEnumerable<TEntity>> FindWithRelated(Expression<Func<TEntity, bool>> predicate, string sortBy, bool isSortAscending = true, params Expression<Func<TEntity, object>>[] includes);

        Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> SingleOrDefaultWithRelated(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);

        void Add(TEntity entity);

        void AddRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);

        void RemoveRange(IEnumerable<TEntity> entities);


    }
}
