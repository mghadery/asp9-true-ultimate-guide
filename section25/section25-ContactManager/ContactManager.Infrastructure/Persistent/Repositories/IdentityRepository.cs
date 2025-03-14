using ContactManager.Core.Domain.RepositorieContracts;
using ContactManager.Core.DTOs;
using ContactManager.Infrastructure.Persistent.Entities;
using ContactManager.Infrastructure.Persistent.Helper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager.Infrastructure.Persistent.Repositories;

public class IdentityRepository(UserManager<User> userManager, RoleManager<Role> roleManager, SignInManager<User> signInManager) : IIdentityRepository
{
    public async Task<IdentityResponse> CreateUserAsync(UserDTO userDTO)
    {
        var user = userDTO.ToUser();
        IdentityResult identityResult = await userManager.CreateAsync(user, userDTO.Password);
        IdentityResponse identityResponse = new()
        {
            Succeeded = identityResult.Succeeded,
            Errors = identityResult.Errors.Select(x => x.Description).ToList()
        };
        return identityResponse;
    }

    public async Task<UserDTO?> FindUserAsync(string userName)
    {
        var user = await userManager.FindByNameAsync(userName);
        return user?.ToUserDTO();
    }

    public async Task SignInAsync(string userName, bool isPersistent)
    {
        var user = await userManager.FindByNameAsync(userName);
        if (user is not null)
            await signInManager.SignInAsync(user, isPersistent);
    }
    public async Task<bool> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
    {
        var res = await signInManager.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);
        return res.Succeeded;
    }
}
