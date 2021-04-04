using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTO;
using DAL;
using BL;
namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        public void create(user user)
        {
            UserBL.create(user);
        }
        [HttpGet]
        [Route("/get")]
        public user SignIn(string email,string password)
        {
            return UserBL.SignIn(email, password);
        }
        [HttpGet]
        public List<user> Get()
        {
            DAL.volunteersEntities db = new volunteersEntities();
            return db.users.Select(x => x).ToList();
            
        }
        
    }
}
