using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DTO;
using BL;
using System.Web.Http.Cors;
using Newtonsoft.Json.Linq;

namespace API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserController : ApiController
    {
        //Post api/User
        public UserDTO Post(UserDTO user)
        {
            return UserBL.create(user);
        }
        
       [Route("AddUsers")]
       [HttpPost]
        public List<string> Post([FromBody] EmailsGroup emailsGroup)
        {
            return UserBL.addListOfVolunteers(emailsGroup);
        }
        //GET api/User
        public UserDTO Get(string email,string password)
        {
            return UserBL.SignIn(email,password);
        }
        [HttpPut]
        public UserDTO PUT(int id,[FromBody]UserDTO userDto)
        {
            return UserBL.Update(userDto,id);
        }
      
    }

}
