using Eventify.Core.Domain;
using Eventify.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventify.Persistence.Repositories
{
    public class FollowerRepository: Repository<Following>, IFollowerRepository
    {
        public FollowerRepository(EventifyDbContext context) : base(context)
        {
        }

        public EventifyDbContext EventifyDbContext
        {
            get { return Context as EventifyDbContext; }
        }


    }
}
