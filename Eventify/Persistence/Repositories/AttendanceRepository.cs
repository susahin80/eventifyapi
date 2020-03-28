using Eventify.Core.Domain;
using Eventify.Core.Repositories;
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
    }
}
