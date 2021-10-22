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
        public static void addEvents(List<EventDTO> events)
        {

            events.ForEach(e => db.events.Add(new @event { 
                StartTime = e.startTime,
                EndTime = e.endTime, 
                GroupId = e.groupId
            }));
            db.SaveChanges();
        }
        public static List<EventDTO> getEventsByGroup(int GroupId)
        {
            List<@event> events = db.events.Where(e => e.GroupId == GroupId).ToList();
            return BL.Convert.EventConverter.ConvertToListOfEventDTO(events);
        }

        //public static void UpdateUsersInEvents(List<EventDTO> events, int ownerId)
        //{
        //    List<int> eventsID = events.Select(e => e.id).ToList();
        //    List<@event> eventsToUpdate = db.events.Where(e => eventsID.Contains(e.id)).ToList();
        //    eventsToUpdate.ForEach(e =>
        //    {
        //        if(e.users == null)
        //        {
        //            int[] arr = new int[] { ownerId };
        //            e.users = String.Join(",", arr);
        //        }
        //        else
        //        {
        //            List<int> newArray = new List<int>();
        //            int[] array = e.users.Split(',').Select(int.Parse).ToArray();
        //            if (array.Contains(ownerId))
        //            {

        //            }
        //        }
        //    });
               
        //}

    }
}
