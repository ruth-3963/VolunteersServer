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
            try {
                utg.color = userToGroup.color;
                db.SaveChanges();
            }
            catch(Exception e){
                Console.WriteLine(e);
            }
            return Convert.UsersToGroupsConverter.ConvertToUserToGroupDTO(utg);
        }
    }
}
