using OrderManager.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Core.Domain.RepositoryContracts;

public interface IIdentityRepository
{
    Task<IdentityResponse> CreateUserAsync(UserDTO userDTO);
    Task<UserDTO?> FindUserAsync(string userName);
    Task SignInAsync(string userName, bool isPersistent);
    Task<IdentityResponse> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure);
    Task SignOut();
    Task UpdateUserAsync(UserDTO userDTO);
}
