using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BL.Convert
{
    public class GroupConverter
    {
        public static GroupDTO ConvertToGroupDTO(group group)
        {
            if (group == null) return null;
            return new GroupDTO
            {
                id = group.id,
                name = group.name,
                id_manager = group.id_manager,
                description = group.description,
                events = group.events
            };
        }
        public static group ConvertToGroup(GroupDTO groupDTO)
        {
            if (groupDTO == null) return null;
            return new group
            {
                id = groupDTO.id,
                name = groupDTO.name,
                id_manager = groupDTO.id_manager,
                description = groupDTO.description,
                events = groupDTO.events
            };
        }
        
        public static List<group> ConvertToListOfGroup(List<GroupDTO> groupsDTO)
        {
            List<group> groups = groupsDTO.Select(gd => ConvertToGroup(gd)).ToList();
            return groups;
        }
        public static List<GroupDTO> ConvertToListOfGroupDTO(List<group> groups)
        {
            List<GroupDTO> groupsDTO = groups.Select(gd => ConvertToGroupDTO(gd)).ToList();
            return groupsDTO;
        }
    }
}
