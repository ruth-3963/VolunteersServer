using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class GroupDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public int id_manager { get; set; }
        public string description { get; set; }
        public string events { get; set; }
    }
}
