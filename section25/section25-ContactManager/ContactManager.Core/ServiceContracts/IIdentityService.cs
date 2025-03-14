using ContactManager.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager.Core.ServiceContracts;

public interface IIdentityService
{
    Task<IdentityResponse> CreateUserAsync(RegisterDTO registerDTO);
    Task SignInAsync(string userName, bool isPersistent);
    Task<bool> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure);
}
