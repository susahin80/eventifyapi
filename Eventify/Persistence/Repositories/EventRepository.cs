using Eventify.Controllers.Resources.Event;
using Eventify.Core.Domain;
using Eventify.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Eventify.Extensions;
using Eventify.Core;

namespace Eventify.Persistence.Repositories
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        public EventRepository(EventifyDbContext context) : base(context)
        {

        }

        public EventifyDbContext EventifyDbContext
        {
            get { return Context as EventifyDbContext; }
        }

        public async Task<QueryResult<Event>> GetEvents(EventQuery filter,  params Expression<Func<Event, object>>[] includes)
        {

            var query = includes
               .Aggregate(
                   EventifyDbContext.Events.AsQueryable(),
                   (current, include) => current.Include(include)
               );            

            //apply filter
            query = query.ApplyFiltering(filter);

            //apply sorting
            query = query.ApplySorting(filter.SortBy, filter.IsSortAscending);

            //apply paging

            var totalItems = await query.CountAsync();
            query = query.ApplyPaging(filter.Page, filter.PageSize);

            var result = new QueryResult<Event>();
            result.Items = await query.ToListAsync();
            result.TotalItems = totalItems;

            return result;

        }

        //public async Task<QueryResult<Event>> GetEventsWithRelated(EventQuery filter, Expression<Func<Event, bool>> predicate, params Expression<Func<Event, object>>[] includes)
        //{
        //  //  var query = EventifyDbContext.Events.Include(e => e.Host).Include(e => e.Category).AsQueryable();


        //    var query = includes
        //        .Aggregate(
        //            EventifyDbContext.Events.Where(predicate).AsQueryable(),
        //            (current, include) => current.Include(include)
        //        );

        //    //apply filter
        //    query = query.ApplyFiltering(filter);

        //    //apply sorting
        //    query = query.ApplySorting(filter.SortBy, filter.IsSortAscending);

        //    //apply paging

        //    var totalItems = await query.CountAsync();
        //    query = query.ApplyPaging(filter.Page, filter.PageSize);

        //    var result = new QueryResult<Event>();
        //    result.Items = await query.ToListAsync();
        //    result.TotalItems = totalItems;

        //    return result;

        //}

        public async Task<IEnumerable<Event>> GetEventsWithoutSortingAndPaging(EventQuery filter)
        {
            var query = EventifyDbContext.Events.AsQueryable();

            query = query.ApplyFiltering(filter);

            var result = await query.ToListAsync();

            return result;
        }

        public async Task<Event> GetEventWithAttenders(Guid id)
        {
            return await EventifyDbContext.Events
                 .Include(e => e.Category)
                 .Include(e => e.Host)
                 .Include(e => e.Attendances)
                 .ThenInclude(a => a.Attendee)
                 .SingleOrDefaultAsync(e => e.Id == id);
        }


    }
}
