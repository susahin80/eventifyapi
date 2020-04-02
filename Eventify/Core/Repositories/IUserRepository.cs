using Eventify.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventify.Core.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        //Task<User> GetUser(string username);
    }
}
