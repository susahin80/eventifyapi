using Eventify.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventify.Core.Repositories
{
    public interface IAttendanceRepository : IRepository<Attendance>
    {
        Task<IEnumerable<Attendance>> GetUserAttendanceEvents(Guid userId);

        Task<QueryResult<Attendance>> GetUserAttendances(Guid userId, EventQuery filter);
    }
}
