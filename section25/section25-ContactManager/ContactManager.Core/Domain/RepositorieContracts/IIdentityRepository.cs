using ContactManager.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager.Core.Domain.RepositorieContracts;

public interface IIdentityRepository
{
    Task<IdentityResponse> CreateUserAsync(UserDTO userDTO);
    Task<UserDTO?> FindUserAsync(string userName);
    Task SignInAsync(string userName, bool isPersistent);
    Task<bool> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure);
}
