using Eventify.Core;
using Eventify.Core.Repositories;
using Eventify.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventify.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EventifyDbContext _context;

        public UnitOfWork(EventifyDbContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
            Events = new EventRepository(_context);
            Categories = new CategoryRepository(_context);
            Attendances = new AttendanceRepository(context);
        }

        public IUserRepository Users { get; private set; }

        public IEventRepository Events { get; private set; }

        public ICategoryRepository Categories { get; private set; }

        public IAttendanceRepository Attendances { get; private set; }

        public async Task  CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
