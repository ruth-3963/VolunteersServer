using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;
namespace BL.Convert
{
    class UsersToGroupsConverter
    {
        public static UsersToGroupsDTO ConvertToUserToGroupDTO(user_to_group userToGroup)
        {
            if (userToGroup == null) return null;
            return new UsersToGroupsDTO
            {
                id = userToGroup.id,
                user_id = userToGroup.user_id,
                group_id = userToGroup.group_id,
                is_manager = userToGroup.is_manager,
                color = userToGroup.color,
                isDeleted = userToGroup.isDeleted,
                confirmManager = userToGroup.confirm_manage,
                reminder = userToGroup.reminder,
                group = GroupConverter.ConvertToGroupDTO(userToGroup.group)
            };
        }
        public static user_to_group ConvertToUserToGroup(UsersToGroupsDTO userToGroupDTO)
        {
            if (userToGroupDTO == null) return null;
            return new user_to_group
            {
                id = userToGroupDTO.id,
                user_id = userToGroupDTO.user_id,
                group_id = userToGroupDTO.group_id,
                is_manager = userToGroupDTO.is_manager,
                color = userToGroupDTO.color,
                isDeleted = userToGroupDTO.isDeleted,
                confirm_manage = userToGroupDTO.confirmManager,
                reminder = userToGroupDTO.reminder,
                group = GroupConverter.ConvertToGroup(userToGroupDTO.group)
            };
        }

        public static List<user_to_group> ConvertToListOfUserToGroup(List<UsersToGroupsDTO> userToGroupDTO)
        {
            List<user_to_group> groups = userToGroupDTO.Select(ug => ConvertToUserToGroup(ug)).ToList();
            return groups;
        }
        public static List<UsersToGroupsDTO> ConvertToListOfUserToGroupDTO(List<user_to_group> groups)
        {
            List<UsersToGroupsDTO> userToGroupDTO = groups.Select(ug => ConvertToUserToGroupDTO(ug)).ToList();
            return userToGroupDTO;
        }
    }
}
