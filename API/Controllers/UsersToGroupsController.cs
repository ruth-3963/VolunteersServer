using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using DTO;
using BL;
namespace API.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UsersToGroupsController : ApiController
    {
        public List<UserToGroupMoreDetails> Get(int groupId)
        {
            return UsersToGroupsBL.GetUsersByGroup(groupId);
        }

        [HttpGet]
        [Route("group/getConfirmManagerGroups/{userId}")]
        public ManagerGroup GetConfirmManagerGroups(int userId)
        {
            return UsersToGroupsBL.GetConfirmManagerGroups(userId);
        }
        public UsersToGroupsDTO Get(int groupId,int userId)
        {
            return BL.UsersToGroupsBL.getByUserAndGroup(groupId,userId);
        }
       //get user by group
       //update color
       public UsersToGroupsDTO PUT([FromBody] UsersToGroupsDTO data)
        {
            return BL.UsersToGroupsBL.UpdateSettings(data);
        }
        [Route("getOwnerData")]
        [HttpGet]
        public List<OwnerData> GetOwnerData(int groupId)
        {
            return BL.UsersToGroupsBL.getOwnerData(groupId);
        }
        [HttpPost]
        [Route("removeUserFromGroup")]
        public void RemoveUser(UsersToGroupsDTO userToGroup)
        {
            BL.UsersToGroupsBL.RemoveUser(userToGroup);
        }
    }
}
