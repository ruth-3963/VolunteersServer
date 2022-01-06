using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using WebMatrix.WebData;
using DAL;
using DTO;

namespace BL
{
    public class LoginBL
    {
        public static UserDTO CheckToken(int token, string email)
        {
            using (volunteersEntities db = new volunteersEntities())
            {
                user currUser = db.users.FirstOrDefault(u => u.email == email && u.resetPasswordToken.Equals(token));
                if (currUser == null)
                    return null;
                return Convert.UserConverter.ConvertToUserDTO(currUser);
            }
        }

        public static UserDTO ResetPassword(string token)
        {
            using (volunteersEntities db = new volunteersEntities())
            {
                user currUser = db.users.FirstOrDefault(u => u.resetPasswordToken == token && u.reserPasswordExpired > DateTime.Now);
                if (currUser == null)
                    return null;
                return Convert.UserConverter.ConvertToUserDTO(currUser);
            }
        }

        public static string UpdatePasswordViaEmail(UserDTO values)
        {
            using (volunteersEntities db = new volunteersEntities())
            
           {
                user userFromDB = db.users.FirstOrDefault(u => u.email == values.email);
                if(userFromDB == null)
                {
                    return "no user exist";
                }
                userFromDB.password = values.password;
                userFromDB.reserPasswordExpired = null;
                userFromDB.resetPasswordToken = null;
                db.SaveChanges();
                return "password updated";
            }
        }

        public static string ForgetPassword(string email)
        {
            using (volunteersEntities db = new volunteersEntities())
            {

                user currUser = db.users.FirstOrDefault(u => u.email == email);
                if (currUser == null)
                    return "email not in db";
                currUser.resetPasswordToken = Guid.NewGuid().ToString();
                currUser.reserPasswordExpired = DateTime.Now.AddDays(3);
                db.SaveChanges();
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
                    Subject = "reset password",
                    Body = currUser.resetPasswordToken.ToString(),
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(to);
                //smtpClient.Send(mailMessage);
                return "recovery email sent";
            }
        }
    }
}
