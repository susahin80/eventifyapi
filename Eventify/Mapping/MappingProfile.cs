using AutoMapper;
using Eventify.Controllers.Resources;
using Eventify.Controllers.Resources.Attendance;
using Eventify.Controllers.Resources.Event;
using Eventify.Controllers.Resources.Follower;
using Eventify.Controllers.Resources.User;
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

            CreateMap<User, ReadMinUserResource>();
            CreateMap<Category, CategoryReadResource>();

            CreateMap<CreateEventResource, Event>()
                .ForMember(e => e.EndDate, opt => opt.MapFrom(c => c.StartDate.AddHours(c.DurationInHours)));

            CreateMap<Event, ReadEventResource>()
                    .ForMember(e => e.Category, opt => opt.MapFrom(c => c.Category))
                    .ForMember(e => e.Host, opt => opt.MapFrom(v => v.Host))
                    .ForMember(e => e.Attendees, opt => opt.MapFrom(v => v.Attendances));

            CreateMap<Attendance, ReadAttendanceResource>()
                .ForMember(e => e.JoinedDate, opt => opt.MapFrom(v => v.CreatedAt))
                .ForMember(e => e.UserId, opt => opt.MapFrom(v => v.Attendee.Id))
                .ForMember(e => e.Username, opt => opt.MapFrom(v => v.Attendee.Username));

            CreateMap<FilterEventResource, EventQuery>();
            CreateMap<QueryResult<Event>, QueryResultResource<ReadEventResource>>();

            CreateMap<Following, ReadFollowerResource>()
                .ForMember(e => e.UserId, opt => opt.MapFrom(c => c.FollowerId))
                .ForMember(e => e.Username, opt => opt.MapFrom(c => c.Follower.Username))
                .ForMember(e => e.FollowedDate, opt => opt.MapFrom(c => c.CreatedAt));

            CreateMap<Following, ReadFollowingResource>()
           .ForMember(e => e.UserId, opt => opt.MapFrom(c => c.FollowedId))
           .ForMember(e => e.Username, opt => opt.MapFrom(c => c.Followed.Username))
           .ForMember(e => e.FollowedDate, opt => opt.MapFrom(c => c.CreatedAt));
        }
    }
}
