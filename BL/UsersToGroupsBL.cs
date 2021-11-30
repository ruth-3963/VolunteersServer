using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
namespace BL
{
    public class UsersToGroupsBL
    {
        static volunteersEntities db = new volunteersEntities();
        public static UsersToGroupsDTO getByUserAndGroup(int groupId, int userId)
        {
            user_to_group user_To_Group =
                db.user_to_group.FirstOrDefault(ug => ug.group_id == groupId && ug.user_id == userId);
            return Convert.UsersToGroupsConverter.ConvertToUserToGroupDTO(user_To_Group);
        }

        public static UsersToGroupsDTO UpdateColor(UsersToGroupsDTO userToGroup)
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

        public static List<OwnerData> getOwnerData(int groupId)
        {
            List<OwnerData> ownerData = new List<OwnerData>();
            group currGroup = db.groups.FirstOrDefault(g => g.id == groupId);
            if(currGroup != null)
            {
                currGroup.user_to_group.ToList().ForEach(ug =>
                {
                    if (ug.color != null)
                    {
                        ownerData.Add(new OwnerData
                        {
                            OwnerColor = ug.color,
                            Id = ug.user_id,
                            OwnerText = ug.user.name + "  " + ug.user.email
                        });
                    }
                });
               
            }
            return ownerData;

        }
    }
}
