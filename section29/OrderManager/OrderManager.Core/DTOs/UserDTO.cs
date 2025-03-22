using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Core.DTOs;

public class UserDTO
{
    public Guid? Id { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? PersonName { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshExpiration { get; set; }
}
