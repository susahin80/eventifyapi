﻿using Eventify.Controllers.Resources.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventify.Controllers.Resources.Attendance
{
    public class ReadAttendanceEventResource
    {


        public DateTime JoinedDate { get; set; }

        public ReadEventResource Event { get; set; }
    }
}
