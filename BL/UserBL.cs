
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using DAL;
using DTO;
using System.Reflection;

namespace BL
{
    public class UserBL
    {
        public static volunteersEntities db = new volunteersEntities();
        public static UserDTO create(UserDTO user)
        {
            if (user == null)
                return null;
            user userFromDB = db.users.FirstOrDefault(u => user.email == u.email);

            if (userFromDB == null) 
                userFromDB = db.users.Add(Convert.UserConverter.ConvertToUser(user));
            else
            {
                userFromDB.password = user.password;
                userFromDB.name = user.name;
                userFromDB.phone = user.phone;
            }
            try
            {
                db.SaveChanges();
                return Convert.UserConverter.ConvertToUserDTO(userFromDB);
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public static void addListOfVolunteers(List<UserDTO> list, GroupDTO group)
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
                sendEmail(user1);
            });

        }

        public static UserDTO SignIn(string email, string password)
        {
            user user = db.users.Where(u => u.email == email).FirstOrDefault();
            if (user == null ||(user.password!=null && !user.password.Equals(password)) ) return null;
            return Convert.UserConverter.ConvertToUserDTO(user);
        }

        private static void sendEmail(user user)
        {
            string from = "ostrovruti@gmail.com";
            string to = "mo9080755@gmail.com";
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("ostrovruti@gmail.com", "ridi0556783963"),
                EnableSsl = true,
            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress(from),
                Subject = "subject",
                Body = "<h1>Hello</h1>",
                IsBodyHtml = true,
            };
            mailMessage.To.Add(to);
            //natasmtpClient.Send(mailMessage);
        }
    }
}
