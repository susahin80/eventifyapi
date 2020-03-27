using Eventify.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventify.Core.Domain
{
    public class Category: Common
    { 

        public string Name { get; set; }
        
        public ICollection<Event> Events { get; set; }

    }
}
