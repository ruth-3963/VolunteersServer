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
                startTime = _event.StartTime,
                endTime = _event.EndTime,
                ownerId = _event.OwnerId,
                groupId = _event.GroupId
            };
        }
        public static @event ConvertToEvent(EventDTO eventDTO)
        {
            return new @event
            {
                id = eventDTO.id,
                StartTime = eventDTO.startTime,
                EndTime = eventDTO.endTime,
                OwnerId = eventDTO.ownerId,
                GroupId = eventDTO.groupId
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
