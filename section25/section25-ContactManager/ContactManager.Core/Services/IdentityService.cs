using ContactManager.Core.Domain.RepositorieContracts;
using ContactManager.Core.DTOs;
using ContactManager.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager.Core.Services;

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
    public async Task<bool> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
    {
        return await identityRepository.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);
    }

}
