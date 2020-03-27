using Eventify.Core;
using Eventify.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Eventify.Extensions
{
    public static class IQueryableExtensions
    {

        public static IQueryable<Event> ApplyFiltering(this IQueryable<Event> query, EventQuery filter)
        {
            if (filter.IsActive.HasValue)
            {
                query = query.Where(e => e.IsActive == filter.IsActive.Value);
            }

            if (filter.IsFree.HasValue)
            {
                if (filter.IsFree.Value)
                {
                    query = query.Where(e => !e.Price.HasValue);
                }
                else
                {
                    query = query.Where(e => e.Price.HasValue);
                }
            }

            if (filter.CategoryId.HasValue)
            {
                query = query.Where(e => e.CategoryId == filter.CategoryId.Value);
            }

            if (filter.HostId.HasValue)
            {
                query = query.Where(e => e.HostId == filter.HostId.Value);
            }

            if (filter.StartDate.HasValue)
            {
                query = query.Where(e => e.StartDate >= filter.StartDate.Value);
            }

            if (filter.EndDate.HasValue)
            {
                query = query.Where(e => e.EndDate <= filter.EndDate.Value);
            }

            return query;
        }


        //https://stackoverflow.com/a/40572006/11717458
        public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, IQueryObject queryObj)
        {
            if (string.IsNullOrWhiteSpace(queryObj.SortBy))
            {
                return query;
            }

            var lambda = (dynamic)CreateExpression(typeof(T), queryObj.SortBy);

            return queryObj.IsSortAscending
                ? Queryable.OrderBy(query, lambda)
                : Queryable.OrderByDescending(query, lambda);
        }

        private static LambdaExpression CreateExpression(Type type, string propertyName)
        {
            var param = Expression.Parameter(type, "x");

            Expression body = param;
            foreach (var member in propertyName.Split('.'))
            {
                body = Expression.PropertyOrField(body, member);
            }

            return Expression.Lambda(body, param);
        }

        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, IQueryObject queryObj)
        {
            if (queryObj.Page <= 0)
                queryObj.Page = 1;

            if (queryObj.PageSize <= 0)
                queryObj.PageSize = 10;

            return query.Skip((queryObj.Page - 1) * queryObj.PageSize).Take(queryObj.PageSize);
        }
    }
}
