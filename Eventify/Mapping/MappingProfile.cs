using AutoMapper;
using Eventify.Controllers.Resources;
using Eventify.Controllers.Resources.Attendance;
using Eventify.Controllers.Resources.Event;
using Eventify.Core;
using Eventify.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventify.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateUserResource, User>();
            CreateMap<User, ReadUserResource>();
            CreateMap<CreateEventResource, Event>()
                .ForMember(e => e.EndDate, opt => opt.MapFrom(c => c.StartDate.AddHours(c.DurationInHours)));

            CreateMap<Event, ReadEventResource>()
                    .ForMember(e => e.Category, opt => opt.MapFrom(c => c.Category.Name))
                    .ForMember(e => e.Host, opt => opt.MapFrom(v => v.Host.Username));

            CreateMap<Attendance, ReadAttendanceResource>()
                .ForMember(e => e.JoinedDate, opt => opt.MapFrom(v => v.CreatedAt))
                .ForMember(e => e.Event, opt => opt.MapFrom(v => v.Event));


            CreateMap<FilterEventResource, EventQuery>();
            CreateMap<QueryResult<Event>, QueryResultResource<ReadEventResource>>();

        }
    }
}
