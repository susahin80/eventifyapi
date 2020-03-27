using Eventify.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eventify.Core.Repositories;
namespace Eventify.Persistence.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(EventifyDbContext context) : base(context)
        {
        }

        public User GetUserWithEvents(int id)
        {
            throw new NotImplementedException();
        }

        public EventifyDbContext EventifyDbContext
        {
            get { return Context as EventifyDbContext; }
        }
    }
}
