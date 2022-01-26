using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System.Data.Entity.Validation;

namespace BL
{
    public class UsersToGroupsBL
    {
        public static UsersToGroupsDTO getByUserAndGroup(int groupId, int userId)
        {
            using (volunteersEntities db = new volunteersEntities())
            {
                user_to_group user_To_Group =
                db.user_to_group.FirstOrDefault(ug => ug.group_id == groupId && ug.user_id == userId);
                return Convert.UsersToGroupsConverter.ConvertToUserToGroupDTO(user_To_Group);
            }
        }

        public static UsersToGroupsDTO UpdateColor(UsersToGroupsDTO userToGroup)
        {
            using (volunteersEntities db = new volunteersEntities())
            {
                user_to_group utg =
                db.user_to_group.FirstOrDefault(ug => ug.user_id == userToGroup.user_id && ug.group_id == userToGroup.group_id);
                try
                {
                    utg.color = userToGroup.color;
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                return Convert.UsersToGroupsConverter.ConvertToUserToGroupDTO(utg);
            }
        }

        public static void RemoveUser(UsersToGroupsDTO userToGroupDto)
        {
            using (volunteersEntities db = new volunteersEntities())
            {
                user_to_group userToGroup =
                    db.user_to_group.FirstOrDefault(ug => ug.user_id == userToGroupDto.user_id &&
                                                          ug.group_id == userToGroupDto.group_id);
                if (userToGroup == null)
                {
                    return;
                }
                userToGroup.isDeleted = true;
                try
                {
                    db.SaveChanges();
                    removeFromEvents(userToGroup);
                }
                catch (DbEntityValidationException ex)
                {
                    throw new DbEntityValidationException(ex.Message);
                }
            }
        }
        private static void removeFromEvents(user_to_group userToGroup)
        {
            //send email to manager with this events
            using(volunteersEntities db = new volunteersEntities())
            {
                List<@event> userEvents =
                    db.events.Where(e => e.OwnerId == userToGroup.user_id &&
                                    e.GroupId == userToGroup.group_id &&
                                    DateTime.Compare(e.StartTime,DateTime.Now)  > 0)
                             .ToList();

                userEvents.ForEach(e =>
                {
                    e.OwnerId = null;
                });
                try
                {
                    db.SaveChanges();
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

        }
        public static List<OwnerData> getOwnerData(int groupId)
        {
            using (volunteersEntities db = new volunteersEntities())
            {
                List<OwnerData> ownerData = new List<OwnerData>();
                group currGroup = db.groups.FirstOrDefault(g => g.id == groupId);
                if (currGroup != null)
                {
                    currGroup.user_to_group.ToList().ForEach(ug =>
                    {
                        if (ug.color != null)
                        {
                            ownerData.Add(new OwnerData
                            {
                                OwnerColor = ug.color,
                                Id = ug.user_id,
                                OwnerText = ug.user.name + "  " + ug.user.email,
                                IsDeleted = ug.isDeleted
                            });
                        }
                    });

                }
                return ownerData;
            }

        }
    }
}
