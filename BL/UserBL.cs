
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
using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Web.Helpers;
namespace BL
{
    public class UserBL
    {
        public static UserDTO create(UserDTO user)
        {
            using (volunteersEntities db = new volunteersEntities())
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
        }

        public static void ConvertToString()
        {
            using(volunteersEntities db = new volunteersEntities()) 
            {
                List<user_to_group> list = db.user_to_group.ToList();
                list.ForEach(e =>
                {
                    if(e.color != null)
                    {
                        e.color = "#" + e.color;
                    }
                    
                 });
                db.SaveChanges();
            }
        }

        public static UserDTO Update(UserDTO userDto, int id)
        {
            using (volunteersEntities db = new volunteersEntities())
            {
                try
                {
                    user userToUpdate = db.users.FirstOrDefault(u => u.id == id);
                    if (userToUpdate == null)
                        return null;
                    userToUpdate.email = userDto.email;
                    userToUpdate.name = userDto.name;
                    userToUpdate.password = userDto.password;
                    userToUpdate.phone = userDto.phone;
                    db.SaveChanges();
                    return Convert.UserConverter.ConvertToUserDTO(userToUpdate);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        public static List<string> addListOfVolunteers(EmailsGroup emailsGroup)
        {
            using (volunteersEntities db = new volunteersEntities())
            {
                if (emailsGroup.emails == null || emailsGroup.group == null)
                    return null;
                List<string> emails = new List<string>();
                emailsGroup.emails.ForEach(item =>
                {
                    //user u = new user() { name = item.name, email = item.email };
                    user user1 = db.users.FirstOrDefault(u => u.email == item);
                    if (user1 == null)
                    {
                        user1 = new user() { email = item };
                        db.users.Add(user1);
                        db.SaveChanges();
                    }
                    user_to_group newUserToGroup = new user_to_group() { group_id = emailsGroup.group.id, user_id = user1.id, is_manager = false };
                    db.user_to_group.Add(newUserToGroup);
                    emailsGroup.html = emailsGroup.html.Replace("http://localhost:3000/signup", "http://localhost:3000/signup?email=" + user1.email + "/");
                    emailsGroup.html = emailsGroup.html.Replace("http://localhost:3000/signin", "http://localhost:3000/signin?email=" + user1.email + "/");
                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("ostrovruti@gmail.com"),
                        Subject = emailsGroup.subject,
                        Body = emailsGroup.html,
                        IsBodyHtml = true,
                    };
                    mailMessage.To.Add(user1.email);
                    if (Email.SendEmail(mailMessage))
                    {
                        emails.Add(user1.email);
                    }


                });
                db.SaveChanges();
                return emails;
            }

        }

        public static UserDTO SignIn(string email, string password)
        {
            using (volunteersEntities db = new volunteersEntities())
            {
                user user = db.users.Where(u => u.email == email).FirstOrDefault();
                if (user == null || (user.password != null && !user.password.Equals(password))) return null;
                return Convert.UserConverter.ConvertToUserDTO(user);
            }
        }

     
    }
}
