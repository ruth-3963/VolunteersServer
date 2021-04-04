using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;
namespace BL
{
    public class UserBL
    {
        public static volunteersEntities db = new volunteersEntities();
        public static void create(user user)
        {
            if (user == null)
                return;
            db.users.Add(user);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public static void addListOfVolunteers(List<user> list, group group)
        {
            list.ForEach(item =>
            {
                //user u = new user() { name = item.name, email = item.email };
                user user1 = db.users.FirstOrDefault(u => u.email == item.email);
                if (user1 == null)
                {
                    user1 = new user() { name = item.name, email = item.email };
                    db.users.Add(user1);
                    db.SaveChanges();
                }
                db.user_to_group.Add(new user_to_group() { group_id = group.id, user_id = user1.id, is_manager = false });
                db.SaveChanges();
            });

        }

        public static UserDTO SignIn(string email, string password)
        {
            user user = db.users.Where(u => u.email == email && u.password == password).FirstOrDefault();
            if (user == null) return null;
            return Convert.UserConverter.ConvertToUserDTO(user);
        }
    }
}
