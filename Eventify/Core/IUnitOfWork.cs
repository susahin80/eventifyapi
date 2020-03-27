using Eventify.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventify.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }

        IEventRepository Events { get; }

        ICategoryRepository Categories { get; }

        IAttendanceRepository Attendances { get; }

        Task CompleteAsync();
    }
}
