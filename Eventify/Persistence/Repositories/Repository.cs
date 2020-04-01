using Eventify.Core.Domain;
using Eventify.Core.Repositories;
using Eventify.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Eventify.Persistence.Repositories
{

    public class Repository<TEntity> : IRepository<TEntity> where TEntity: Common
    {
        protected readonly EventifyDbContext Context;

        private readonly DbSet<TEntity> _entities;

        public Repository(EventifyDbContext context)
        {
            Context = context;
            // Here we are working with a DbContext, not or DbContext. So we don't have DbSets 
            // such as Users or Events, and we need to use the generic Set() method to access them.

            _entities = Context.Set<TEntity>();
        }

        public async Task<TEntity> Get(Guid id)
        {
            return await _entities.FindAsync(id);
        }

        //https://stackoverflow.com/questions/10001061/dynamic-include-statements-for-eager-loading-in-a-query-ef-4-3-1
        public async Task<TEntity> GetWithRelated(Guid id,  params Expression<Func<TEntity, object>>[] includes)
        {
            //  return await _entities.FindAsync(id);

            var query = includes
           .Aggregate(
               _entities.AsQueryable(),
               (current, include) => current.Include(include)
           );

            return await query.SingleOrDefaultAsync(e => e.Id == id);

        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _entities.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> FindWithRelated(Expression<Func<TEntity, bool>> predicate, string sortBy, bool isSortAscending = true, params Expression<Func<TEntity, object>>[] includes )
        {
            var query = includes
                    .Aggregate(
                        _entities.Where(predicate).AsQueryable(),
                        (current, include) => current.Include(include)
                    );

            query = query.ApplySorting(sortBy, isSortAscending);

            return await query.ToListAsync();
        }


        public async Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return await _entities.Where(predicate).ToListAsync();
        }

        //public async Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate)
        //{
        //    return Context.Set<TEntity>().Where(predicate);
        //}

        public async Task<TEntity> SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return await _entities.SingleOrDefaultAsync(predicate);
        }

        public void Add(TEntity entity)
        {
            _entities.Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _entities.AddRange(entities);
        }

        public void Remove(TEntity entity)
        {
            _entities.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _entities.RemoveRange(entities);
        }



    }
}
