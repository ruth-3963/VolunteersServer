using DTO;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;


namespace BL
{
    public class EventToUserBL
    {
        public static List<EventDTO> GetEventToChooseEvents(int userId, int groupId)
        {
            using(volunteersEntities db = new volunteersEntities())
            {
                List<int> eventsToUserIds =
                    db.event_to_user.Where(etu => etu.userId == userId && etu.groupId == etu.groupId)
                    .Select(etu => etu.eventId).ToList();
                List<@event> groupEvents = db.events.Where(e => e.GroupId == groupId).ToList();
                List<EventDTO> groupEventDto =
                    Convert.EventConverter.ConvertToListOfEventDTO(groupEvents);
                groupEventDto.ForEach(ge =>
                {
                    if (eventsToUserIds.Contains(ge.id))
                    {
                        ge.OwnerId = userId;
                    }
                });
                return groupEventDto;
             }
        }
        public static List<EventToUserDTO> updateUserInEvent(EventsByUserAndGroup data)
        {
            using (volunteersEntities db = new volunteersEntities())
            {
                List<event_to_user> etu = db.event_to_user.Where(val => val.groupId == data.groupId && val.userId == data.userId).ToList();
                etu.ForEach(e =>
                {
                    if (!data.events.Contains(e.eventId))
                    {
                        //remove this event
                        db.event_to_user.Remove(e);
                    }
                });
                //items to add
                foreach (int id in data.events)
                {
                    if (etu.FirstOrDefault(e => e.eventId == id) == null)
                    {
                        db.event_to_user.Add(new event_to_user
                        {
                            eventId = id,
                            groupId = data.groupId,
                            userId = data.userId
                        });
                    }
                }
                db.SaveChanges();
                return ListOfEvent(data);
            }
        }
        public static List<EventToUserDTO> ListOfEvent(EventsByUserAndGroup data)
        {
            using (volunteersEntities db = new volunteersEntities())
            {
                List<event_to_user> listToConvert = db.event_to_user.Where(val => val.groupId == data.groupId && val.userId == data.userId).ToList();
                return Convert.EventToUserConverter.ConvertToListOfEventToUserDTO(listToConvert);
            }
        }

    }
}