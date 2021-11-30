using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BL.Convert
{
    public class EventConverter
    {
        public static EventDTO ConvertToEventDTO(@event _event,bool needDeep=true)
        {
            if (_event == null) return null;
            return new EventDTO
            {
                id = _event.id,
                StartTime = _event.StartTime,
                EndTime = _event.EndTime,
                OwnerId = _event.OwnerId != null ? _event.OwnerId : null,
                GroupId = _event.GroupId,
                Subject = _event.Subject != null ? _event.Subject : "" ,
                Description = _event.Description != null ? _event.Description : "",
                Guid = _event.Guid != null ? _event.Description : "",
                eventToUserDTO = needDeep? EventToUserConverter.ConvertToListOfEventToUserDTO(_event.event_to_user.ToList()):null
            };
        }
        public static @event ConvertToEvent(EventDTO eventDTO)
        {
            if (eventDTO == null) return null;

            return new @event
            {
                id = eventDTO.id,
                StartTime = eventDTO.StartTime,
                EndTime = eventDTO.EndTime,
                OwnerId = eventDTO.OwnerId != null ? eventDTO.OwnerId : null,
                GroupId = eventDTO.GroupId,
                Subject = eventDTO.Subject != null ? eventDTO.Subject : null,
                Description = eventDTO.Description != null ? eventDTO.Description : null,
                Guid = eventDTO.Guid != null ? eventDTO.Guid : null,
                event_to_user = eventDTO.eventToUserDTO != null ?EventToUserConverter.ConvertToListOfEventToUser(eventDTO.eventToUserDTO):null
            };
        }

        public static List<@event> ConvertToListOfEvents(List<EventDTO> eventsDTO)
        {
            List<@event> groups = eventsDTO.Select(e => ConvertToEvent(e)).ToList();
            return groups;
        }
        public static List<EventDTO> ConvertToListOfEventDTO(List<@event> events)
        {
            List<EventDTO> eventsDTO = events.Select(e => ConvertToEventDTO(e)).ToList();
            return eventsDTO;
        }
    }
}
