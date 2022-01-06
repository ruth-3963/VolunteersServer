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
                    if(utg.color != null)
                    {
                        colors.Add(utg.color);
                    }
                });
                return colors;
            }

        }

        public static List<EventDTO> CalcEvents(int groupId,List<EventDTO> events)
        {
            using (volunteersEntities db = new volunteersEntities())
            {
                group currGroup = db.groups.Find(groupId);
                if (currGroup == null) return null;
                List<event_to_user> eventsToUsers = currGroup.event_to_user.ToList();
                List<@event> groupEvents = EventConverter.ConvertToListOfEvents(events);
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
                return Convert.UsersToGroupsConverter.ConvertToUserToGroupDTO(userToGroup);
            }
        }
        public static List<ManagerGroup> getGroupsByUserWithManeger(int id)
        {
            using (volunteersEntities db = new volunteersEntities())
            {
                var lo = db.groups.Join(db.users, g => g.id_manager, u => u.id,
                        (g, u) => new ManagerGroup { id = g.id, name = g.name, mName = u.name, mEmail = u.email ,
                            color = g.user_to_group.FirstOrDefault(utg => utg.user_id == id).color})
                        .Where(groupWithManager =>       
                        db.user_to_group.Any(ug => ug.group_id == groupWithManager.id && ug.user_id == id));
                return lo.ToList();
            }

        }
    }
}
