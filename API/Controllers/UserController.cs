using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DAL;
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
        public UserDTO Post(user user)
        {
            return UserBL.create(user);
        }
        
       [Route("AddUsers")]
       [HttpPost]
        public void Post(JObject data)
        {
            List<user> list = data["list"].ToObject<List<user>>();
            group group = data["group"].ToObject<group>();
            UserBL.addListOfVolunteers(list,group);
            
        }
        //GET api/User
        public UserDTO Get(string email,string password)
        {
            return UserBL.SignIn(email,password);
        }
    }
}
