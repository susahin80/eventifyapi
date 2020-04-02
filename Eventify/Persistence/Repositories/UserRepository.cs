using Eventify.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eventify.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Eventify.Persistence.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(EventifyDbContext context) : base(context)
        {
        }


        public EventifyDbContext EventifyDbContext
        {
            get { return Context as EventifyDbContext; }
        }

        //public async Task<User> GetUser(string username)
        //{
        //    var user = await EventifyDbContext.Users.Include(u => u.Followers).Include(u => u.Followings).FirstOrDefaultAsync(u => u.Username == username);

        //    return user;
        //}
    }
}
