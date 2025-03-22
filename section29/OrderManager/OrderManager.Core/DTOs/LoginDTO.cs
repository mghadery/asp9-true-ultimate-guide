using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Core.DTOs;

public class LoginDTO
{
    [Required(ErrorMessage = "UserName is required")]
    //[MinLength(1)]
    public string? UserName { get; set; }
    [Required(ErrorMessage = "Password is required")]
    //[MinLength(1)]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    public UserDTO ToUserDTO()
    {
        UserDTO userDTO = new UserDTO()
        {
            UserName = UserName,
            Password = Password
        };
        return userDTO;
    }
}
