using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BL.Convert
{
    class UserConverter
    {
        public static UserDTO ConvertToUserDTO(user user)
        {
            if (user == null) return null;
            return new UserDTO
            {
                id = user.id,
                name = user.name,
                email = user.email,
                password = user.password,
                phone = user.phone,
                resetPasswordToken = user.resetPasswordToken,
                reserPasswordExpired = user.reserPasswordExpired
            };
        }
        public static user ConvertToUser(UserDTO userDTO)
        {
            if (userDTO == null) return null;
            return new user
            {
                id = userDTO.id,
                name = userDTO.name,
                email = userDTO.email,
                password = userDTO.password,
                phone = userDTO.phone,
                reserPasswordExpired = userDTO.reserPasswordExpired,
                resetPasswordToken = userDTO.resetPasswordToken
            };
        }
        public static List<user> ConvertToListOfUsers(List<UserDTO> usersDTO)
        {
            List<user> users = usersDTO.Select(ud => ConvertToUser(ud)).ToList();
            return users;
        }
        public static List<UserDTO> ConvertToListOfGroupDTO(List<user> users)
        {
            List<UserDTO> usersDTO = users.Select(ud => ConvertToUserDTO(ud)).ToList();
            return usersDTO;
        }
    }
}
