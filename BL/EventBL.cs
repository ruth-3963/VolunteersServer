using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class EventBL
    {
        public static volunteersEntities db = new volunteersEntities();
        public static void addEvents(GroupEventDTO groupEvent)
        {
            groupEvent.events.ForEach(e =>
                {
                    e.OwnerId = null;
                    e.GroupId = groupEvent.group.id;
                    db.events.Add(Convert.EventConverter.ConvertToEvent(e));
                }
            );
            db.SaveChanges();
        }

        public static List<EventDTO> getEventsByGroup(int GroupId)
        {
            List<@event> events = db.events.Where(e => e.GroupId == GroupId).ToList();
            return BL.Convert.EventConverter.ConvertToListOfEventDTO(events);
        }

        public static void updateEvents(List<EventDTO> events)
        {
            events.ForEach(e =>
            {
                @event currEvent = db.events.Find(e.id);
                if (currEvent != null)
                {
                    currEvent.Guid = e.Guid; currEvent.OwnerId = e.OwnerId;
                    currEvent.Subject = e.Subject; currEvent.Description = e.Description;
                    currEvent.StartTime = e.StartTime; currEvent.EndTime = e.EndTime;
                    db.SaveChanges();
                }
            });
        }

        public static void deleteEvents(List<EventDTO> events)
        {

            events.ForEach(e =>
            {
                @event eventToDelete = db.events.Find(e.id);
                db.events.Remove(eventToDelete);
                db.SaveChanges();
            });
        }

        public static List<EventToUserDTO> ListOfEvent(EventsByUserAndGroup data)
        {
            List<event_to_user> listToConvert = db.event_to_user.Where(val => val.groupId == data.groupId && val.userId == data.userId).ToList();
            return Convert.EventToUserConverter.ConvertToListOfEventToUserDTO(listToConvert);
        }

        public static List<EventToUserDTO> updateUserInEvent(EventsByUserAndGroup data)
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
}
