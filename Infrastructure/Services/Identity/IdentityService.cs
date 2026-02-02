using Application.Common.Interfaces;
using Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services.Identity;

public class IdentityService : IIdentityService
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentityService(IAuthorizationService service, IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory, UserManager<ApplicationUser> user)
    {
        _authorizationService = service;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _userManager = user;
    }

    public async Task<string?> GetUserNameAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user?.UserName;
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user != null && await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password)
    {
        var user = new ApplicationUser
        { 
            UserName = userName,
            Email = userName
        };
        var result = await _userManager.CreateAsync(user, password);
        return (result.ToApplicationResult(), user.Id);
    }

    public async Task<bool> AuthorizeAsync(string userId, string policyName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) 
        { 
            return false; 
        }
        var principal = await _userClaimsPrincipalFactory.CreateAsync(user);
        var result  = await _authorizationService.AuthorizeAsync(principal, policyName);
        return result.Succeeded;
    }

    public async Task<Result> DeleteUserAsync(string userId) 
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user != null ? await DeleteUserAsync(user) : Result.Success();
    }

    public async Task<Result> DeleteUserAsync(ApplicationUser user) 
    {
        var result = await _userManager.DeleteAsync(user);
        return result.ToApplicationResult();
    }
}
