using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager.Core.DTOs;

public class LoginDTO
{
    [Required(ErrorMessage = "UserName is required")]
    public string UserName { get; set; }
    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

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
