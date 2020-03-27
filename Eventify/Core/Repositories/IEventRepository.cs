using Eventify.Controllers.Resources.Event;
using Eventify.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventify.Core.Repositories
{
    public interface IEventRepository: IRepository<Event>
    {
        Task<QueryResult<Event>> GetEvents(EventQuery filter);
        // Task<Event> GetEventWithCategoryAndHost(int id);

        //Task<Event> GetEventWithHostAttendances(int id);
    }
}
