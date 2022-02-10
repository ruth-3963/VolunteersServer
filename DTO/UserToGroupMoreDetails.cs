using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class UserToGroupMoreDetails
    {
        public UserDTO user { get; set; }
        public string color{ get; set; }
        public bool isDeleted { get; set; }
    }
}
