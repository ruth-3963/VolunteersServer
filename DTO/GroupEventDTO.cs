using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class GroupEventDTO
    {
        public GroupDTO group { get; set; }
        public List<EventDTO> events { get; set; }
    }
}
