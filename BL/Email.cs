using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace BL
{
    public class Email
    {
        public static bool SendEmail(MailMessage message)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("explane@gmail.com", "password"),
                EnableSsl = true,
            };
            try
            {
                smtpClient.Send(message);
                return true;
            }
            catch (SmtpException smtpExeption)
            {
                Console.WriteLine(smtpExeption.Message);
                return false;
            }
        }
    }
}
