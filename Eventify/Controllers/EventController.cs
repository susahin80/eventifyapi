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
using Eventify.Infrastructure.Photos;

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

            var userId = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            //check if host has any active events whose StartDate-EndDate conflicts

            var hostEvents = await UnitOfWork.Events.GetEventsWithoutSortingAndPaging(new EventQuery() { HostId = userId, StartDate = DateTime.Now, IsActive = true });

            foreach (var evt in hostEvents)
            {
                if (resource.StartDate.IsInRange(evt.StartDate, evt.EndDate) ||
                    resource.StartDate.AddHours(resource.DurationInHours).IsInRange(evt.StartDate, evt.EndDate))
                {
                    throw new RestError(HttpStatusCode.BadRequest, new { StartDate = $"Event conflict's with one of your active future events: {evt.Title}" });
                }
            }

            //check if user joined another event 

            var attendances = await UnitOfWork.Attendances.GetUserAttendanceEvents(userId);

            foreach (var att in attendances)
            {
                if (resource.StartDate.IsInRange(att.Event.StartDate, att.Event.EndDate) ||
                  resource.StartDate.AddHours(resource.DurationInHours).IsInRange(att.Event.StartDate, att.Event.EndDate))
                {
                    throw new RestError(HttpStatusCode.BadRequest, new { StartDate = $"You already joined at an event at that time: {att.Event.Title}" });
                }
            }

            Event eventEntity = Mapper.Map<CreateEventResource, Event>(resource);

            eventEntity.HostId = userId;
            eventEntity.CreatedAt = DateTime.Now;

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


        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ReadEventResource>> Update(Guid id, CreateEventResource resource)
        {

            var eventEntity = await UnitOfWork.Events.GetEventWithAttenders(id);

            if (eventEntity == null) throw new RestError(HttpStatusCode.NotFound, new { Event = "Event not found." });

            var userId = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            if (eventEntity.HostId != userId) throw new RestError(HttpStatusCode.NotFound, new { Event = "Event not found." });

            if (eventEntity.Attendances.Count() > 0) throw new RestError(HttpStatusCode.NotFound, new { Event = "You can't update an event which has attenders." });

            Mapper.Map<CreateEventResource, Event>(resource, eventEntity);

            await UnitOfWork.CompleteAsync();

            eventEntity.Id = id;

            var result = Mapper.Map<Event, ReadEventResource>(eventEntity);

            return Ok(result);

        }


        [HttpPost("photo")]
        [Authorize]
        public async Task<ActionResult<AddEventPhotoResponseResource>> AddEventPhoto([FromForm]AddEventPhotoResource resource)
        {
            var evt = await UnitOfWork.Events.GetWithRelated(resource.EventId, e => e.Photos);

            if (evt == null) throw new RestError(HttpStatusCode.NotFound, new { Event = "Event not found" });

            var userId = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            if (evt.HostId != userId) throw new RestError(HttpStatusCode.NotFound, new { Event = "Event not found..." });

            PhotoUploadResult result = PhotoAccessor.AddPhoto(resource.File);

            bool isMain = !evt.Photos.Any(p => p.IsMain == true);

            Photo photo = new Photo()
            {
                CreatedAt = DateTime.Now,
                IsMain = isMain ,
                PublicId = result.PublicId,
                Url = result.Url                
            };

            evt.Photos.Add(photo);

            await UnitOfWork.CompleteAsync();

            var response = new AddEventPhotoResponseResource()
            {
                IsMain = isMain,
                PublicId = photo.PublicId,
                Url = photo.Url,
                EventId = evt.Id                
            };

            return Ok(response);
        }
    }
}