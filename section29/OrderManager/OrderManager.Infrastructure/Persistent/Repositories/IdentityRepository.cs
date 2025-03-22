using Microsoft.AspNetCore.Identity;
using OrderManager.Core.Domain.RepositoryContracts;
using OrderManager.Core.DTOs;
using OrderManager.Infrastructure.Persistent.IdentityEntities;
using OrderManager.Infrastructure.Persistent.Helper;

namespace OrderManager.Infrastructure.Persistent.Repositories;

public class IdentityRepository(UserManager<User> userManager, RoleManager<Role> roleManager, SignInManager<User> signInManager) : IIdentityRepository
{
    public async Task<IdentityResponse> CreateUserAsync(UserDTO userDTO)
    {
        //just demo!
        var userRole = await roleManager.FindByNameAsync("User");
        if (userRole is null)
            await roleManager.CreateAsync(new Role() { Id = Guid.NewGuid(), Name = "User", NormalizedName = "USER" });

        var user = userDTO.ToUser();
        IdentityResult identityResult = await userManager.CreateAsync(user, userDTO.Password);
        if (identityResult.Succeeded)
            identityResult = await userManager.AddToRoleAsync(user, "User");
        IdentityResponse identityResponse = new()
        {
            Succeeded = identityResult.Succeeded,
            Errors = identityResult.Errors.Select(x => x.Description).ToList(),
            UserDTO = user.ToUserDTO()
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
    public async Task<IdentityResponse> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
    {
        var res = await signInManager.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);

        if (res.Succeeded)
        {
            var user = await userManager.FindByNameAsync(userName);
            var userDTO = user?.ToUserDTO();
            return new IdentityResponse()
            {
                Succeeded = true,
                Errors = [],  //TODO
                UserDTO = userDTO
            };
        }
        return new IdentityResponse()
        {
            Succeeded = false,
            Errors = ["Error"],  //TODO
            UserDTO = null
        };
    }
    public async Task SignOut()
    {
        await signInManager.SignOutAsync();
    }

    public async Task UpdateUserAsync(UserDTO userDTO)
    {
        var user = await userManager.FindByNameAsync(userDTO.UserName);
        if (user is null) return;
        user.PersonName = userDTO.PersonName;
        user.RefreshExpiration= userDTO.RefreshExpiration;
        user.RefreshToken= userDTO.RefreshToken;
        await userManager.UpdateAsync(user);
    }
}
