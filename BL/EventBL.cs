using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public static class EventBL
    {
        public static void addEvents(int groupId, List<EventDTO> events)
        {
            using (volunteersEntities db = new volunteersEntities())
            {
                events.ForEach(e =>
                {
                    e.OwnerId = null;
                    e.GroupId = groupId;
                    db.events.Add(Convert.EventConverter.ConvertToEvent(e));
                });
                db.SaveChanges();
            }
        }

        public static List<EventDTO> getEventsByGroup(int GroupId)
        {
            using (volunteersEntities db = new volunteersEntities())
            {
                List<@event> events = db.events.Where(e => e.GroupId == GroupId)?.ToList();
                return BL.Convert.EventConverter.ConvertToListOfEventDTO(events);
            }
        }

        public static void updateEvents(List<EventDTO> events)
        {
            using (volunteersEntities db = new volunteersEntities())
            {
                events.ForEach(e =>
                {
                    @event currEvent = db.events.Find(e.id);
                    if (currEvent != null)
                    {
                        currEvent.Guid = e.Guid; currEvent.OwnerId = e.OwnerId;
                        currEvent.Subject = e.Subject; currEvent.Description = e.Description;
                        currEvent.StartTime = e.StartTime; currEvent.EndTime = e.EndTime;
                        try
                        {
                            db.SaveChanges();
                        }
                        catch (DbEntityValidationException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                });
            }
        }

        public static bool SendAppropriateEmail(string buttonId, GroupDTO group)
        {
            using (volunteersEntities db = new volunteersEntities())
            {
                group groupFromDb = db.groups.FirstOrDefault(g => g.id == group.id);
                List<string> emailsOfUsersGroup =
                    groupFromDb.user_to_group.Where(u => u.isDeleted != true && u.is_manager != true).
                                              Select(u => u.user.email).ToList();
                if (groupFromDb == null)
                    return false;
                string subject = "";
                string body = "";
                string nicknameManager = groupFromDb.user.name == null ?
                                         "email: " + groupFromDb.user.email :
                                         groupFromDb.user.name + " - " + groupFromDb.user.email;
                switch (buttonId)
                {
                    case "send":
                        subject = "The new schedule of " + group.name + "group is ready to watch";
                        body = "The manager of " + group.name + " (" + nicknameManager + ") finish to inlay shedule" +
                            "you can <a href='https://localhost:3000/schedule/'" + group.id + ">enter to this link</a> and wath the new schedule";
                        break;
                    case "requestInlay":
                        subject = "The new schedule of " + group.name + "group ready";
                        body = "The manager of " + group.name + " (" + nicknameManager + ") create shifts "
                            + "we invite you to <a href='https://localhost:3000/chooseEvents/'" + group.id + ">enter to this link </a>and choose shifts that you can to volunteer";
                        break;
                    default:
                        break;
                }

                MailMessage mailMessage = new MailMessage()
                {
                    From = new MailAddress("ostrovruti@gmail.com"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                emailsOfUsersGroup.ForEach(email =>
                {
                    mailMessage.To.Add(new MailAddress(email)); ;
                });
                return GroupBL.SendEmail(mailMessage);
            }
        }

        public static bool InlayUser(int userId, EventDTO data)
        {
            using (volunteersEntities db = new volunteersEntities())
            {
                user volunteer = db.users.FirstOrDefault(u => u.id == userId);
                @event eventFromDB = db.events.FirstOrDefault(e => e.id == data.id);
                if (eventFromDB == null || volunteer == null)
                {
                    return false;
                }
                string subject = "";
                if (eventFromDB.OwnerId == userId)
                {
                    eventFromDB.OwnerId = null;
                    subject = volunteer.name + "inlay in" + eventFromDB.Subject + "shift";
                    event_to_user eventToUser = db.event_to_user.FirstOrDefault(e => e.eventId == eventFromDB.id);
                    if(eventToUser != null)
                    {
                        db.event_to_user.Remove(eventToUser);
                    }
                }
                else
                {
                    eventFromDB.OwnerId = userId;
                    subject = volunteer.name + "cancel inlay in" + eventFromDB.Subject + "shift";
                    List<int> usersEventIds = eventFromDB.event_to_user.Select(etu => etu.userId)?.ToList();
                    if (usersEventIds == null || usersEventIds.Count == 0 || !usersEventIds.Contains(userId))
                    {
                        db.event_to_user.Add(new event_to_user()
                        {
                            userId = userId,eventId = eventFromDB.id,groupId = eventFromDB.GroupId
                        });
                    }
                }
                try
                {
                    db.SaveChanges();
                    //send email to manager
                    MailMessage message = new MailMessage()
                    {
                        From = new MailAddress("ostrovruti@gmail.com"),
                        Subject = subject,
                        Body = "In date " + eventFromDB.StartTime
                    };
                    //GroupBL.SendEmail(message);
                    return true;
                }
                catch
                {
                    return false;
                }

            }
        }

        public static void SetDescription(int eventId, string description)
        {
            using (volunteersEntities db = new volunteersEntities())
            {
                @event eventToUpdate = db.events.FirstOrDefault(e => e.id == eventId);
                if (eventToUpdate == null)
                {
                    return;
                }
                eventToUpdate.Description = description;
                db.SaveChanges();
            }
        }

        public static void deleteEvents(List<EventDTO> events)
        {
            using (volunteersEntities db = new volunteersEntities())
            {
                events.ForEach(e =>
                {
                    @event eventToDelete = db.events.Find(e.id);
                    db.events.Remove(eventToDelete);
                    db.SaveChanges();
                });
            }
        }

        public static void Reminder()
        {
            //get events in Next 24 hours
            using(volunteersEntities db = new volunteersEntities())
            {
                List<@event> eventIn24Hours =
                    db.events.AsEnumerable().Where(e => {
                        if (e.OwnerId == null) return false;
                        user_to_group utg = db.user_to_group.FirstOrDefault(item => item.group_id == e.GroupId && item.user_id == e.OwnerId);
                        if (utg == null || utg.reminder == null || utg.reminder == 0) return false;
                        DateTime timeToReminder = TrimSeconds(e.StartTime.AddHours((double)(utg.reminder * -1)));
                        return timeToReminder == TrimSeconds(DateTime.Now);
                        }).ToList();
                eventIn24Hours.ForEach(x =>
                {
                    //  if (dtOrig.TrimMilliseconds() == dtNew.TrimMilliseconds())

                    //send Reminder email
                    string body = "Remainder about volunteer in " + x.group.name +
                        " In Date " + x.StartTime.Date + "in Hours " +
                        x.StartTime.TimeOfDay + "-" + x.EndTime.TimeOfDay + " Good Luck";
                    MailMessage message = new MailMessage()
                    {
                        From = new MailAddress("ostrovruti@gmail.com"),
                        Subject = "Volunteer Remainder",
                        Body = body
                    };
                    message.To.Add(x.user.email);
                   // GroupBL.SendEmail(message);
                    Debug.WriteLine(body);
                });
            }
        }
        public static DateTime TrimSeconds(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0, 0, 0);
        }

    }
}
