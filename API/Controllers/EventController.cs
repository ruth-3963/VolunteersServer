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
            return EventBL.getEventsByGroup(id);
        }
        public void Put([FromBody] GroupEventDTO groupEvent)
        {
            GroupBL.AddEventsToGroup(groupEvent.group, groupEvent.events);
        }
        [HttpPost]
        [Route("SaveEvents/{groupId}")]
        public void SaveEvents(int groupId,[FromBody] List<EventDTO> updateNewEvents)
        {
            EventBL.addEvents(groupId, updateNewEvents);
        }
        [HttpPut]
        [Route("UpdatetUserArr")]
        public void UpdatetUserArr(List<EventDTO> events, int ownerId)
        { 
            //EventBL.UpdateUsersInEvents(events,ownerId);
        }
        [HttpPut]
        [Route("UpdateEvents")]
        public void PUT(List<EventDTO> newUpdate)
        {
            if(newUpdate != null)
            {
                BL.EventBL.updateEvents(newUpdate);
            }
        }
        [HttpDelete]
        [Route("deleteEvents")]
        public void Delete(List<EventDTO> newDel)
        {
            BL.EventBL.deleteEvents(newDel);
        }
        [HttpPost]
        [Route("api/Event/specialSave/{buttonId}")]
        public bool specialSave(string buttonId,[FromBody] GroupDTO group)
        {
            return EventBL.SendAppropriateEmail(buttonId,group);
            
        }
        [HttpPut]
        [Route("api/Event")]
        public void SetDescription(int eventId,string description)
        {
            EventBL.SetDescription(eventId,description);
        }
        [HttpPost]
        [Route("inlay/{userId}")]
        public bool InlayUser(int userId,[FromBody] EventDTO data)
        {
            return EventBL.InlayUser(userId, data);
        }

    }
}

