using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Eventify.Controllers.Resources;
using Eventify.Controllers.Resources.Event;
using Eventify.Core;
using Eventify.Core.Domain;
using Eventify.Core.Repositories;
using Eventify.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Eventify.Extensions;

namespace Eventify.Controllers
{

    public class EventController : BaseController
    {

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ReadEventResource>> CreateEvent(CreateEventResource resource)
        {

            if (!(resource.StartDate >= DateTime.Now.AddDays(1))) throw new RestError(HttpStatusCode.BadRequest, new { StartDate = "Event start date can start from 1 day away." });

            var category = await UnitOfWork.Categories.Get(resource.CategoryId);

            if (category == null) throw new RestError(HttpStatusCode.BadRequest, new { Category = "Category doesn't exist" });

            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            //check if host has any active events whose StartDate-EndDate conflicts

            var hostEvents = await UnitOfWork.Events.GetEventsWithoutSortingAndPaging(new EventQuery() { HostId = Guid.Parse(userId), StartDate = DateTime.Now, IsActive = true });

            foreach (var evt in hostEvents)
            {
                if (evt.StartDate.IsInRange(resource.StartDate, resource.StartDate.AddHours(resource.DurationInHours)) || 
                    evt.EndDate.IsInRange(resource.StartDate, resource.StartDate.AddHours(resource.DurationInHours)))
                {
                    throw new RestError(HttpStatusCode.BadRequest, new { StartDate = $"Event conflict's with one of your active future events. {evt.Title}" });
                }

            }

            Event eventEntity = Mapper.Map<CreateEventResource, Event>(resource);

            eventEntity.HostId = Guid.Parse(userId);

            UnitOfWork.Events.Add(eventEntity);
            await UnitOfWork.CompleteAsync();

            var createdEvent = await UnitOfWork.Events.GetWithRelated(eventEntity.Id, e => e.Host, e => e.Category);

            var response = Mapper.Map<Event, ReadEventResource>(createdEvent);

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<QueryResultResource<ReadEventResource>>> List([FromQuery]FilterEventResource filter)
        {

            var eventQuery = Mapper.Map<FilterEventResource, EventQuery>(filter);

            QueryResult<Event> events = await UnitOfWork.Events.GetEvents(eventQuery);

            var result = Mapper.Map<QueryResult<Event>, QueryResultResource<ReadEventResource>>(events);

            return Ok(result);

        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ReadEventResource>> Get(Guid id)
        {

            var eventEntity = await UnitOfWork.Events.GetEventWithAttenders(id);

            if (eventEntity == null) throw new RestError(HttpStatusCode.NotFound, new { Event = "Event not found." });

            var result = Mapper.Map<Event, ReadEventResource>(eventEntity);

            return Ok(result);

        }

    }
}