using Eventify.Core;
using Eventify.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventify.Extensions
{
    public static class EventExtensions
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
    }
}
