using System.ComponentModel.DataAnnotations;

namespace OrderManager.Core.DTOs;

public class RegisterDTO
{
    [Required(ErrorMessage = "UserName is required")]
    //[Remote("IsUserNameAlreadyRegistered")]
    public string UserName { get; set; }
    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Not confirmed")]
    public string PasswordConfirm { get; set; }
    [Required(ErrorMessage = "PersonName is required")]
    public string PersonName { get; set; }

    public UserDTO ToUserDTO()
    {
        UserDTO userDTO = new UserDTO()
        {
            UserName = UserName,
            Password = Password,
            PersonName = PersonName
        };
        return userDTO;
    }
}
