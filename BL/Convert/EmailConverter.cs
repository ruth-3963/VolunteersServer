using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BL.Convert
{
    public class EmailConverter
    {
        public static MailMessage ConvertToMailMessage(EmailFromClient emailFromClient)
        {
            //IEnumerable<MailAddress> addresses = emailFromClient.To.Select(e => new MailAddress(e));
            MailMessage message = new MailMessage()
            {
                From = new MailAddress("ostrovruti@gmail.com"),
                Subject = emailFromClient.Subject,
                Body = emailFromClient.Body
            };
            emailFromClient.To.ForEach(email =>
            {
                message.To.Add(email);
            });
            return message;            
        }
    }
}
