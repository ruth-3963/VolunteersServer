using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using BL;
using DTO;

namespace API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LoginController : ApiController
    {
        [Route("forgetPassword")]
        [HttpGet]
        public string ForgetPassword(string email)
        {
            return LoginBL.ForgetPassword(email);
        }
        [Route("checkToken")]
        [HttpGet]
        public UserDTO CheckToken(int token, string email)
        {
            return LoginBL.CheckToken(token, email);
        }
        [Route("reset")]
        [HttpGet]
        public UserDTO ResetPassword(string resetPasswordToken)
        {
            return LoginBL.ResetPassword(resetPasswordToken);
        }

        [Route("updatePasswordViaEmail")]
        [HttpPut]
        public string UpdatePasswordViaEmail([FromBody] UserDTO data)
        {
            return LoginBL.UpdatePasswordViaEmail(data);
        }
    }
}
