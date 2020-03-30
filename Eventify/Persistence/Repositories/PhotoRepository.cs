using Eventify.Core.Domain;
using Eventify.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventify.Persistence.Repositories
{
    public class PhotoRepository: Repository<Photo>, IPhotoRepository
    {
        public PhotoRepository(EventifyDbContext context) : base(context)
        {
        }

        public EventifyDbContext EventifyDbContext
        {
            get { return Context as EventifyDbContext; }
        }
    }
}
