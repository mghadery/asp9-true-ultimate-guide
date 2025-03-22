using OrderManager.Core.Domain.RepositoryContracts;
using OrderManager.Core.DTOs;
using OrderManager.Core.ServiceContracts;

namespace OrderManager.Core.Services;

public class IdentityService(IIdentityRepository identityRepository) : IIdentityService
{
    public async Task<IdentityResponse> CreateUserAsync(RegisterDTO registerDTO)
    {
        var user = await identityRepository.FindUserAsync(registerDTO.UserName);
        if (user is not null)
            return new IdentityResponse()
            {
                Succeeded = false,
                Errors = ["User exists"]
            };
        user = registerDTO.ToUserDTO();
        var res = await identityRepository.CreateUserAsync(user);
        return res;
    }
    public async Task SignInAsync(string userName, bool isPersistent)
    {
        await identityRepository.SignInAsync(userName, isPersistent);
    }
    public async Task<IdentityResponse> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
    {
        return await identityRepository.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);
    }
    public async Task SignOut()
    {
        await identityRepository.SignOut();
    }
    public async Task UpdateUserAsync(UserDTO userDTO)
    {
        await identityRepository.UpdateUserAsync(userDTO);
    }

    public async Task<UserDTO> FindUserAsync(string userName)
    {
        return await identityRepository.FindUserAsync(userName);
    }
}
