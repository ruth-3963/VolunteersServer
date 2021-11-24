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
            return new EventToUserDTO
            {
                id = eventToUser.id,
                userId = eventToUser.userId,
                eventId = eventToUser.eventId,
                groupId = eventToUser.groupId
            };
        }
        public static event_to_user ConvertToEventToUser(EventToUserDTO eventToUserDTO)
        {
            return new event_to_user
            {
                id = eventToUserDTO.id,
                userId = eventToUserDTO.userId,
                eventId = eventToUserDTO.eventId,
                groupId = eventToUserDTO.groupId
            };
        }

        public static List<event_to_user> ConvertToListOfEventToUser(List<EventToUserDTO> eventToUserDTO)
        {
            List<event_to_user> groups = eventToUserDTO.Select(eu => ConvertToEventToUser(eu)).ToList();
            return groups;
        }
        public static List<EventToUserDTO> ConvertToListOfEventToUserDTO(List<event_to_user> groups)
        {
            List<EventToUserDTO> eventToUserDTO = groups.Select(eu => ConvertToEventToUserDTO(eu)).ToList();
            return eventToUserDTO;
        }
    }
}
