using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DTO;
using BL;
using System.Web.Http.Cors;

namespace API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class GroupController : ApiController
    {
        [Route("GetByManager")]
        [HttpGet]
        public List<ManagerGroup> GetByManager(string id)
        {
            return GroupBL.getGroupsByUserWithManeger(int.Parse(id));
        }
        public GroupDTO Post(string email, string name, string description)
        {
            return GroupBL.createGroup(email,name,description);
        }
        public GroupDTO Get(string id)
        {
            return GroupBL.getGroupByID(id);
        }
    }
}
