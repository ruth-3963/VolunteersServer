using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BL.Convert
{
    class EventToUserConverter
    {
        public static EventToUserDTO ConvertToEventToUserDTO(event_to_user eventToUser)
        {
            if (eventToUser == null) return null;
            return new EventToUserDTO
            {
                id = eventToUser.id,
                userId = eventToUser.userId,
                user = UserConverter.ConvertToUserDTO(eventToUser.user),
                eventId = eventToUser.eventId,
                _event = EventConverter.ConvertToEventDTO(eventToUser.@event,false),
                groupId = eventToUser.groupId,
                group = GroupConverter.ConvertToGroupDTO(eventToUser.group)
            };
        }
        public static event_to_user ConvertToEventToUser(EventToUserDTO eventToUserDTO)
        {
            if (eventToUserDTO == null) return null;
            return new event_to_user
            {
                id = eventToUserDTO.id,
                userId = eventToUserDTO.userId,
                user = UserConverter.ConvertToUser(eventToUserDTO.user),
                eventId = eventToUserDTO.eventId,
                @event = EventConverter.ConvertToEvent(eventToUserDTO._event),
                groupId = eventToUserDTO.groupId,
                group = GroupConverter.ConvertToGroup(eventToUserDTO.group),
            };
        }

        public static List<event_to_user> ConvertToListOfEventToUser(List<EventToUserDTO> eventToUserDTO)
        {
            List<event_to_user> groups = eventToUserDTO.Select(eu => ConvertToEventToUser(eu)).ToList();
            return groups;
        }
        public static List<EventToUserDTO> ConvertToListOfEventToUserDTO(List<event_to_user> eventToUser)
        {
            List<EventToUserDTO> eventToUserDTO = eventToUser.Select(eu => ConvertToEventToUserDTO(eu)).ToList();
            return eventToUserDTO;
        }
    }
}
