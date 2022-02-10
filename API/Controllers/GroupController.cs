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
        public UsersToGroupsDTO Post([FromBody]GroupDTO newGroup)
        {
            return GroupBL.createGroup(newGroup);
        }
        public GroupDTO Get(string id)
        {
            return GroupBL.getGroupByID(id);
        }
        [HttpPost]
        [Route("calcEvents/{groupId}")]
        public List<EventDTO> CalcEvents(int groupId ,[FromBody]List<EventDTO> events)
        {
            return GroupBL.CalcEvents(groupId,events);
        }
        [HttpGet]
        [Route("getAllUsersColors")]
        public List<string> getAllUsersColors(int groupId)
        {
            return GroupBL.getAllUsersColors(groupId);
        }
        [HttpPost]
        [Route("group/setManager/{id}")]
        public void SetManager(int id,[FromBody]ManagerGroup group)
        {
            GroupBL.SetManager(id, group);
        }
        [HttpPost]
        [Route("group/newManagerAnswer/{isAgree}")]
        public void NewManagerAnswer(bool isAgree, [FromBody] UsersToGroupsDTO userToGroup)
        {
            GroupBL.SetManager(isAgree, userToGroup);
        }
    }
}
