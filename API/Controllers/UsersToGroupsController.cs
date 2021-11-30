using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using DTO;
namespace API.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UsersToGroupsController : ApiController
    {
        public UsersToGroupsDTO Get(int groupId,int userId)
        {
            return BL.UsersToGroupsBL.getByUserAndGroup(groupId,userId);
        }
       //get user by group
       //update color
       public UsersToGroupsDTO PUT([FromBody] UsersToGroupsDTO data)
        {
            return BL.UsersToGroupsBL.UpdateColor(data);
        }
        [Route("getOwnerData")]
        [HttpGet]
        public List<OwnerData> GetOwnerData(int groupId)
        {
            return BL.UsersToGroupsBL.getOwnerData(groupId);
        }
        
    }
}
