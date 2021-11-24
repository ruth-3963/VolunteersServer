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
        public static EventDTO ConvertToEventDTO(@event _event)
        {
            return new EventDTO
            {
                id = _event.id,
                StartTime = _event.StartTime,
                EndTime = _event.EndTime,
                OwnerId = _event.OwnerId,
                GroupId = _event.GroupId,
                Subject = _event.Subject,
                Description = _event.Description,
                Guid = _event.Guid
            };
        }
        public static @event ConvertToEvent(EventDTO eventDTO)
        {
            return new @event
            {
                id = eventDTO.id,
                StartTime = eventDTO.StartTime,
                EndTime = eventDTO.EndTime,
                OwnerId = eventDTO.OwnerId,
                GroupId = eventDTO.GroupId,
                Subject = eventDTO.Subject,
                Description = eventDTO.Description,
                Guid = eventDTO.Guid
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
