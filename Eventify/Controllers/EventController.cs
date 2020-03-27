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

namespace Eventify.Controllers
{

    public class EventController : BaseController
    {

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ReadEventResource>> CreateEvent(CreateEventResource createEventResource)
        {
            var category = await UnitOfWork.Categories.Get(createEventResource.CategoryId);

            if (category == null) throw new RestError(HttpStatusCode.BadRequest, new { Category = "Category doesn't exist" });

            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            Event eventEntity = Mapper.Map<CreateEventResource, Event>(createEventResource);

            eventEntity.HostId = Guid.Parse(userId);

            UnitOfWork.Events.Add(eventEntity);
            await UnitOfWork.CompleteAsync();

            //var createdEvent = await UnitOfWork.Events.Get(eventEntity.Id, new string[2] { "Category", "Host" });

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

    }
}