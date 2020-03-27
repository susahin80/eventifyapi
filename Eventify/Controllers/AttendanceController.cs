﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Eventify.Controllers.Resources.Attendance;
using Eventify.Core;
using Eventify.Core.Domain;
using Eventify.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eventify.Controllers
{

    [Authorize]
    public class AttendanceController : BaseController
    {

        [HttpPost()]
        public async Task<ActionResult<ReadAttendanceResource>> CreateEvent(CreateAttendanceResource resource)
        {

            Event eventEntity = await UnitOfWork.Events.GetWithRelated(resource.EventId, e => e.Category, e => e.Attendances);
            if (eventEntity == null) throw new RestError(HttpStatusCode.BadRequest, new { Event = "Event doesn't exist." });

            if (!eventEntity.IsActive) throw new RestError(HttpStatusCode.BadRequest, new { Event = "Event isn't active." });

            if (eventEntity.StartDate < DateTime.Now.AddHours(-1)) throw new RestError(HttpStatusCode.BadRequest, new { Event = "Too late to join this event." });

            Guid attendanceId = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            User user = await UnitOfWork.Users.Get(attendanceId);

            //check if user already attended
            if (eventEntity.Attendances.FirstOrDefault(a => a.AttendeeId == attendanceId) != null)
                throw new RestError(HttpStatusCode.BadRequest, new { Event = "You are already attending this event." });

            //check if user is host
            if (eventEntity.HostId == attendanceId)
                throw new RestError(HttpStatusCode.BadRequest, new { Event = "As a host, you don't need the join your event." });

            //check if event has age limit, and user is eligible
            if (eventEntity.MinAgeLimit.HasValue)
            {
                int age = DateUtil.CalculateAge(user.BirthDate);

                if (age < eventEntity.MinAgeLimit.Value) 
                    throw new RestError(HttpStatusCode.BadRequest, new { Event = $"This event has minimum age limit {eventEntity.MinAgeLimit.Value}, and you are {age} years old." });
            }

            if (eventEntity.MaxAgeLimit.HasValue)
            {
                int age = DateUtil.CalculateAge(user.BirthDate);

                if (age > eventEntity.MaxAgeLimit.Value)
                    throw new RestError(HttpStatusCode.BadRequest, new { Event = $"This event has maximum age limit {eventEntity.MaxAgeLimit.Value}, and you are {age} years old." });
            }

            //check if event has already max number of people
            if (eventEntity.MaxNumberOfPeople.HasValue)
            {
                //check how many attendances for the event
                if (!(eventEntity.Attendances.Count < eventEntity.MaxNumberOfPeople.Value))
                    throw new RestError(HttpStatusCode.BadRequest, new { Event = $"This event has already {eventEntity.MaxNumberOfPeople.Value} people." });
            }

            Attendance attendance = Mapper.Map<CreateAttendanceResource, Attendance>(resource);
            attendance.AttendeeId = attendanceId;

            UnitOfWork.Attendances.Add(attendance);

            await UnitOfWork.CompleteAsync();

            attendance = await UnitOfWork.Attendances.SingleOrDefault(a => a.AttendeeId == attendanceId && a.EventId == eventEntity.Id);

            ReadAttendanceResource response = Mapper.Map<Attendance, ReadAttendanceResource>(attendance);

            return Ok(response);
        }

    }
}