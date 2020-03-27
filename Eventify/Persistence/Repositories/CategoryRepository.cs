using Eventify.Core.Domain;
using Eventify.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventify.Persistence.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(EventifyDbContext context) : base(context)
        {
        }

        public EventifyDbContext EventifyDbContext
        {
            get { return Context as EventifyDbContext; }
        }
    }
}
