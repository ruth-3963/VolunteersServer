using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class EventsByUserAndGroup
    {
        public int groupId { get; set; }
        public int userId { get; set; }
        public List<int> events { get; set; }
    }
}
