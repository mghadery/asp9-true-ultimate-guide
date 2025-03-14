using ContactManager.Core.DTOs;
using ContactManager.Infrastructure.Persistent.Entities;

namespace ContactManager.Infrastructure.Persistent.Helper;

public static class ConverterHelper
{
    public static UserDTO ToUserDTO(this User user)
    {
        var userDTO = new UserDTO()
        {
            Id = user.Id,
            PersonName = user.PersonName
        };
        return userDTO;
    }

    public static User ToUser(this UserDTO userDTO)
    {
        var user = new User()
        {
            Id = userDTO.Id ?? Guid.Empty,
            UserName = userDTO.UserName,
            PersonName = userDTO.PersonName
        };
        return user;
    }
}
