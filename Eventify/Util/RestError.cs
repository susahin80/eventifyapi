using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Eventify.Util
{
    public class RestError : Exception
    {
        public readonly HttpStatusCode Code;

        public object Errors { get; }

        public RestError(HttpStatusCode code, object errors = null)
        {
            Code = code;
            Errors = errors;
        }

    }
}
