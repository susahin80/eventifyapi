using Eventify.Controllers.Resources.Event;
using Eventify.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Eventify.Core.Repositories
{
    public interface IEventRepository: IRepository<Event>
    {
        Task<QueryResult<Event>> GetEvents(EventQuery filter, params Expression<Func<Event, object>>[] includes);

        Task<IEnumerable<Event>> GetEventsWithoutSortingAndPaging(EventQuery filter);

        Task<Event> GetEventWithAttenders(Guid id);

    }
}
