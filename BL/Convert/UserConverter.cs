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
                phone = user.phone
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
                phone = userDTO.phone
            };
        }
    }
}
