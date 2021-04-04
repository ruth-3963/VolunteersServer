using DAL;
using BL;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EventController : ApiController
    {
        public void Put(JObject data)
        {
            group group = data["group"].ToObject<group>();
            JArray events = data["events"].ToObject<JArray>();
            BL.GroupBL.AddEventsToGroup(group, events);
        }
    }
}
