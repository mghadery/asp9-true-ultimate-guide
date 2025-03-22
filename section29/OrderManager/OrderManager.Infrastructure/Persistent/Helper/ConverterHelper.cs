using OrderManager.Core.DTOs;
using OrderManager.Infrastructure.Persistent.IdentityEntities;

namespace OrderManager.Infrastructure.Persistent.Helper;

public static class ConverterHelper
{
    public static UserDTO ToUserDTO(this User user)
    {
        var userDTO = new UserDTO()
        {
            Id = user.Id,
            PersonName = user.PersonName,
            UserName = user.UserName,
            RefreshExpiration = user.RefreshExpiration,
            RefreshToken = user.RefreshToken
        };
        return userDTO;
    }

    public static User ToUser(this UserDTO userDTO)
    {
        var user = new User()
        {
            Id = userDTO.Id ?? Guid.Empty,
            UserName = userDTO.UserName,
            PersonName = userDTO.PersonName,
            RefreshExpiration = userDTO.RefreshExpiration,
            RefreshToken = userDTO.RefreshToken
        };
        return user;
    }
}
