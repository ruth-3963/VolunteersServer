using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Convert;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Mail;
using System.Net;

namespace BL
{
    public class GroupBL
    {
        public static List<GroupDTO> getGroupsByUser(int id)
        {
            using (volunteersEntities db = new volunteersEntities())
            {
                List<group> groups = db.groups.Where(g => db.user_to_group.ToList().Any(ug => ug.group_id == g.id && ug.user_id == id)).ToList();
                return GroupConverter.ConvertToListOfGroupDTO(groups);
            }
        }
        [HttpPut]
        public static void AddEventsToGroup(GroupDTO group, List<EventDTO> events)
        {
            using (volunteersEntities db = new volunteersEntities())
            {
                group group1 = db.groups.Where(g => g.id == group.id).FirstOrDefault();
                events.ForEach(e =>
                {
                    db.events.Add(Convert.EventConverter.ConvertToEvent(e));
                });
                db.SaveChanges();
            }
        }

        public static List<string> getAllUsersColors(int groupId)
        {
            using (volunteersEntities db = new volunteersEntities())
            {
                group _group = db.groups.FirstOrDefault(g => g.id == groupId);
                if (_group == null) return null;
                List<string> colors = new List<string>();
                _group.user_to_group.ToList().ForEach(utg =>
                {
                    if (utg.color != null)
                    {
                        colors.Add(utg.color);
                    }
                });
                return colors;
            }

        }

        public static void SetManager(bool isAgree, UsersToGroupsDTO userToGroup)
        {
            using (volunteersEntities db = new volunteersEntities())
            {
                string body = "";
                string subject = "";
                user_to_group newManager =
                       db.user_to_group.FirstOrDefault(ug => ug.group_id == userToGroup.group_id && ug.user_id == userToGroup.user_id);
                user_to_group oldManager =
                    db.user_to_group.FirstOrDefault(ug => ug.is_manager == true);
                group currGroup = db.groups.FirstOrDefault(g => g.id == userToGroup.group_id);
                string userName = newManager.user.name;
                string userEmail = newManager.user.email;
                string userValue = userName != null ? userName :
                                   "user with email" + userEmail;
                if (isAgree)
                {
                    newManager.is_manager = true;
                    oldManager.is_manager = false;
                    currGroup.id_manager = newManager.user.id;
                    subject = userValue + " agree to manage " + currGroup.name + " group";
                    body = subject + "/n now you are regular user in group/nand"
                        + newManager.user.name + " is the manager";
                }
                else
                {
                    subject = userValue + " not agree to manage " + currGroup.name + " group";
                    body = subject + "/n so you left the manager";
                }
                newManager.confirm_manage = false;
                db.SaveChanges();

                MailMessage message = new MailMessage()
                {
                    From = new MailAddress("ostrovruti@gmail.com"),
                    Subject = subject,
                    Body = body
                };
                message.To.Add(oldManager.user.email);
                try
                {
                   // Email.SendEmail(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public static void SetManager(int newManagerId, ManagerGroup Mgroup)
        {
            using (volunteersEntities db = new volunteersEntities())
            {
                user newManager = db.users.FirstOrDefault(u => u.id == newManagerId);
                group DBgroup = db.groups.FirstOrDefault(g => g.id == Mgroup.id);
                if (newManager == null || DBgroup == null)
                    return;


                var mailMessage = new MailMessage
                {
                    From = new MailAddress("ostrovruti@gmail.com"),
                    Subject = "manage " + DBgroup.name + " group",
                    Body = DBgroup.user.name + "accept that you manage " + DBgroup.name +
                    " group please <a href='https://localhost:3000'>enter to everyOneToOne</a> " +
                    "to send your decide",
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(newManager.email);
                try
                {
                    //Email.SendEmail(mailMessage);
                    user_to_group managerToGroup =
                        db.user_to_group.FirstOrDefault(ug => ug.user_id == newManager.id && ug.group_id == DBgroup.id);
                    managerToGroup.confirm_manage = true;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public static List<EventDTO> CalcEvents(int groupId, List<EventDTO> events)
        {
            using (volunteersEntities db = new volunteersEntities())
            {
                group currGroup = db.groups.Find(groupId);
                if (currGroup == null) return null;
                List<event_to_user> eventsToUsers = currGroup.event_to_user.ToList();
                List<int> eventsIds = events.Select(e => e.id).ToList();
                List<@event> groupEvents = db.events.Where(e => eventsIds.Contains(e.id))?.ToList();
                if (eventsToUsers == null || groupEvents == null)
                    return null;
                return Inlay.InlayEvents.Calc(currGroup, eventsToUsers, groupEvents);
            }
        }

        public static GroupDTO getGroupByID(string id)
        {
            using (volunteersEntities db = new volunteersEntities())
            {
                group group1 = db.groups.Where(g => g.id.ToString() == id).FirstOrDefault();
                if (group1 != null)
                {
                    GroupDTO groupDto = GroupConverter.ConvertToGroupDTO(group1);
                    if (group1.events1 != null)
                    {
                        groupDto.events = EventConverter.ConvertToListOfEventDTO(group1.events1.ToList());
                    }
                    return groupDto;
                }
                return null;
            }
        }

        public static UsersToGroupsDTO createGroup(GroupDTO newGroup)
        {
            using (volunteersEntities db = new volunteersEntities())
            {
                user manager = db.users.Where(user => user.id == newGroup.id_manager).FirstOrDefault();
                if (manager == null) return null;
                if (db.groups.Any(gr => gr.name == newGroup.name && gr.id_manager == manager.id))
                    return null;
                group groupDB = new group() { name = newGroup.name, description = newGroup.description, id_manager = newGroup.id_manager };
                db.groups.Add(groupDB);
                db.SaveChanges();
                user_to_group userToGroup = db.user_to_group.Add(new user_to_group
                {
                    user_id = manager.id,
                    group_id = groupDB.id,
                    is_manager = true
                });
                db.SaveChanges();
                return UsersToGroupsConverter.ConvertToUserToGroupDTO(userToGroup);
            }
        }
        public static List<ManagerGroup> getGroupsByUserWithManeger(int id)
        {
            using (volunteersEntities db = new volunteersEntities())
            {
                var lo = db.groups.Join(db.users, g => g.id_manager, u => u.id,
                        (g, u) => new ManagerGroup
                        {
                            id = g.id,
                            name = g.name,
                            mName = u.name,
                            mEmail = u.email,
                            color = g.user_to_group.FirstOrDefault(utg => utg.user_id == id).color
                        })
                        .Where(groupWithManager =>
                        db.user_to_group.Any(ug => ug.group_id == groupWithManager.id && ug.user_id == id && ug.isDeleted != true));
                return lo.ToList();
            }

        }
       
    }
}
