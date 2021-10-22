using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class UserGroupDTO
    {
        public List<UserDTO> users { get; set; }
        public GroupDTO group { get; set; }

    }
}
