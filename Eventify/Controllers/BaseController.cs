using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Eventify.Core;
using Eventify.Infrastructure.Photos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Eventify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private IUnitOfWork _UnitOfWork;

        private IMapper _Mapper;

        private IPhotoAccessor _PhotoAccessor;

        protected IUnitOfWork UnitOfWork
        {
            get
            {
                return _UnitOfWork ?? (_UnitOfWork = HttpContext.RequestServices.GetService<IUnitOfWork>());
            }
        }

        protected IMapper Mapper
        {
            get
            {
                return _Mapper ?? (_Mapper = HttpContext.RequestServices.GetService<IMapper>());
            }
        }

        protected IPhotoAccessor PhotoAccessor
        {
            get
            {
                return _PhotoAccessor ?? (_PhotoAccessor = HttpContext.RequestServices.GetService<IPhotoAccessor>());
            }
        }
    }
}