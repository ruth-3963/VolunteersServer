using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class EventToUserDTO
    {
        public int id { get; set; }
        public int userId { get; set; }
        public UserDTO user { get; set; }
        public int eventId { get; set; }
        public GroupDTO group { get; set; }
        public int groupId { get; set; }
        public EventDTO _event { get; set; }
    }
}
