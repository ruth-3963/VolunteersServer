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
        private static volunteersEntities db = new volunteersEntities();

        public static List<GroupDTO> getGroupsByUser(int id)
        {

            List<group> groups = db.groups.Where(g => db.user_to_group.ToList().Any(ug => ug.group_id == g.id && ug.user_id == id)).ToList();
            return GroupConverter.ConvertToListOfGroupDTO(groups);
        }
        [HttpPut]
        public static void AddEventsToGroup(GroupDTO group, List<EventDTO> events)
        {
            group group1 = db.groups.Where(g => g.id == group.id).FirstOrDefault();
            events.ForEach(e =>
            {
                db.events.Add(Convert.EventConverter.ConvertToEvent(e));
            });
            db.SaveChanges();
        }

        public static GroupDTO getGroupByID(string id)
        {
            group group1 = db.groups.Where(g => g.id.ToString() == id).FirstOrDefault();
            return GroupConverter.ConvertToGroupDTO(group1);
        }

        public static GroupDTO createGroup(string email, string name, string description)
        {

            //group g = new group() { name = data.name, description = data.description };
            //string email = data.email.ToString();
            //user u = db.users.Where(uu => uu.email == email).FirstOrDefault();
            //string name = data.name.ToString();
            //if (db.groups.Any(gr => gr.name == name && gr.id_manager == u.id))
            //    return null;
            //else
            //{
            //    g.id_manager = u.id;
            //    db.groups.Add(g);
            //    db.SaveChanges();
            //    //g = db.groups.Last();
            //    db.user_to_group.Add(new user_to_group { user_id = u.id, group_id = g.id, is_manager = true });
            //    db.SaveChanges();
            //    return GroupConverter.ConvertToGroupDTO(g);
            //}
            return null;
        }
        public static List<ManagerGroup> getGroupsByUserWithManeger(int id)
        {
            var lo = db.groups.Join(db.users, g => g.id_manager, u => u.id,
                        (g, u) => new ManagerGroup { id = g.id, name = g.name, mName = u.name, mEmail = u.email })
                        .Where(groupWithManager =>
                        db.user_to_group.Any(ug => ug.group_id == groupWithManager.id && ug.user_id == id));
            return lo.ToList();

        }
    }
}
