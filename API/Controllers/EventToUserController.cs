using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using BL;
using DTO;

namespace API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EventToUserController : ApiController
    {
        public List<EventToUserDTO> Post([FromBody] EventsByUserAndGroup data)
        {
            return EventBL.updateUserInEvent(data);
        }
    }
}
