using BL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;
using System.Web.Http.Cors;
using BL;
namespace API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EmailController : ApiController
    {
        // POST api/<controller>
        public void Post([FromBody] EmailFromClient message)
        {
            Email.SendEmail(BL.Convert.EmailConverter.ConvertToMailMessage(message));
        }

          
    }
}