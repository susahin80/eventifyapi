using Eventify.Core;
using Eventify.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Eventify.Extensions
{
    public static class IQueryableExtensions
    {


        //https://stackoverflow.com/a/40572006/11717458
        //public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, IQueryObject sort)
        //{
        //    if (sort == null || string.IsNullOrWhiteSpace(sort.SortBy))
        //    {
        //        return query;
        //    }

        //    var lambda = (dynamic)CreateExpression(typeof(T), sort.SortBy);

        //    return sort.IsSortAscending
        //        ? Queryable.OrderBy(query, lambda)
        //        : Queryable.OrderByDescending(query, lambda);
        //}

        public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, string key, bool ascending = true)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return query;
            }

            var lambda = (dynamic)CreateExpression(typeof(T), key);

            return ascending
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


        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, int? page, int? pageSize)
        {
            int calculatedPage = 1;
            int calculatedPageSize = 10;

            if (page.HasValue)
            {
                calculatedPage = page.Value < 1 ? 1 : page.Value;
            }

            if (pageSize.HasValue)
            {
                calculatedPageSize = pageSize.Value < 1 ? 10 : pageSize.Value;
            }


            return query.Skip((calculatedPage - 1) * calculatedPageSize).Take(calculatedPageSize);
        }

        //public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, IQueryObject queryObj)
        //{
        //    if (queryObj.Page <= 0)
        //        queryObj.Page = 1;

        //    if (queryObj.PageSize <= 0)
        //        queryObj.PageSize = 10;

        //    return query.Skip((queryObj.Page - 1) * queryObj.PageSize).Take(queryObj.PageSize);
        //}
    }
}
