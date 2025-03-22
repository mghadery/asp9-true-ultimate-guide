using OrderManager.Core.DTOs;

namespace OrderManager.Core.ServiceContracts;

public interface IIdentityService
{
    Task<IdentityResponse> CreateUserAsync(RegisterDTO registerDTO);
    Task SignInAsync(string userName, bool isPersistent);
    Task<IdentityResponse> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure);
    Task SignOut();
    Task UpdateUserAsync(UserDTO userDTO);
    Task<UserDTO> FindUserAsync(string userName);
}
