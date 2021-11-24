using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class UsersToGroupsDTO
    {
        public int id { get; set; }
        public int? user_id { get; set; }
        public int? group_id { get; set; }
        public bool? is_manager { get; set; } = false;
        public string color { get; set; }
    }
}
