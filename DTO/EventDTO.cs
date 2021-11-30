using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class EventDTO
    {
        public int id { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public int? OwnerId { get; set; } = null;
        public int GroupId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Guid { get; set; }
        public List<EventToUserDTO> eventToUserDTO { get; set; }

    }
}
