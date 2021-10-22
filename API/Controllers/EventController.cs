using DTO;
using BL;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Newtonsoft.Json;

namespace API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EventController : ApiController
    {
        public List<EventDTO> Get(int id)
        {
            return BL.EventBL.getEventsByGroup(id);
        }
        public void Put(GroupEventDTO groupEvent)
        {
            BL.GroupBL.AddEventsToGroup(groupEvent.group, groupEvent.events);
        }
        public void Post([FromBody] List<EventDTO> data)
        {
            EventBL.addEvents(data);
        }
        [HttpPut]
        [Route("UpdatetUserArr")]
        public void UpdatetUserArr(List<EventDTO> events, int ownerId)
        { 
            //EventBL.UpdateUsersInEvents(events,ownerId);
        }
    }
}
