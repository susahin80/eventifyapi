using Eventify.Core;
using Eventify.Core.Domain;
using Eventify.Core.Repositories;
using Eventify.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventify.Persistence.Repositories
{
    public class AttendanceRepository : Repository<Attendance>, IAttendanceRepository
    {
        public AttendanceRepository(EventifyDbContext context) : base(context)
        {
        }

        public EventifyDbContext EventifyDbContext
        {
            get { return Context as EventifyDbContext; }
        }

        public async Task<IEnumerable<Attendance>> GetUserAttendanceEvents(Guid userId)
        {
            return await EventifyDbContext.Attendances.Where(a => a.AttendeeId == userId && a.Event.IsActive).Include(a => a.Event).ToListAsync();
        }

        public async Task<QueryResult<Attendance>> GetUserAttendances(Guid userId, EventQuery filter)
        {
            var query = EventifyDbContext.Attendances.Where(a => a.AttendeeId == userId).Include(a => a.Event).ThenInclude(e => e.Category).AsQueryable();


            //apply sorting
            query = query.ApplySorting(filter.SortBy, filter.IsSortAscending);

            //apply paging

            var totalItems = await query.CountAsync();
            query = query.ApplyPaging(filter.Page, filter.PageSize);

            var result = new QueryResult<Attendance>();
            result.Items = await query.ToListAsync();
            result.TotalItems = totalItems;

            return result;
        }
    }
}


