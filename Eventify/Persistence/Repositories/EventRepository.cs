﻿using Eventify.Controllers.Resources.Event;
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
    public class EventRepository: Repository<Event>, IEventRepository
    {
        public EventRepository(EventifyDbContext context) : base(context)
        {

        }

        public EventifyDbContext EventifyDbContext
        {
            get { return Context as EventifyDbContext; }
        }

        public async Task<QueryResult<Event>> GetEvents(EventQuery filter)
        {          
            var query = EventifyDbContext.Events.Include(e => e.Host).Include(e => e.Category).AsQueryable();

            //apply filter
            query = query.ApplyFiltering(filter);

            //apply sorting
            query = query.ApplySorting(filter);

            var totalItems = await query.CountAsync();

            //apply paging
            query = query.ApplyPaging(filter);

            var result = new QueryResult<Event>();
            result.Items = await query.ToListAsync();
            result.TotalItems = totalItems;

            return result;

        }

        //public async Task<Event> GetEventWithHostAttendances(int id)
        //{

        //    return await EventifyDbContext.Events
        //         .Include(e => e.Attendances)
        //         .Include(e => e.Host)
        //         .SingleOrDefaultAsync(e => e.Id == id);
        //}

        //public async Task<Event> GetEventWithCategoryAndHost(int id)
        //{
        //    return await EventifyDbContext.Events
        //        .Include(e => e.Category)
        //        .Include(e => e.Host)
        //        .SingleOrDefaultAsync(e => e.Id == id);

        //}


    }
}
