using Application.Common.Interfaces;
using Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace Infrastructure.Services.Identity;

public class IdentityService : IIdentityService
{
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailSender _emailSender;
    private readonly IConfiguration _configuration;

    public IdentityService(IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory, UserManager<ApplicationUser> user, IEmailSender sender, IConfiguration config)
    {
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _userManager = user;
        _emailSender = sender;
        _configuration = config;
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

    public async Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password, string email)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);
        if (existingUser != null)
        {
            return (Result.Failure(new[] { "User with such email already exist." }), string.Empty);
        }
        var user = new ApplicationUser
        { 
            UserName = userName,
            Email = email,
            EmailConfirmed = false
        };
        var result = await _userManager.CreateAsync(user, password);
        var addToRoleResult = await _userManager.AddToRoleAsync(user, "User");

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        var baseUrl = _configuration["Api:BaseUrl"];
        var confirmationLink = $"{baseUrl}/auth/confirmEmail?userId={user.Id}&token={encodedToken}"; ;
        var body = $"""
            <h2>Confirmation message</h2>
            <p>Hi, {user.UserName}!</p>
            <p>Confirm email to continue:</p>
            <p><a href="{confirmationLink}">Click here</a></p>
            """;

        await _emailSender.SendEmailAsync(email, "Confirm email", body);
        return (result.ToApplicationResult(), user.Id);
    }

    public async Task<Result> ConfirmEmailAsync(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if(user is null)
        {
            return Result.Failure(new[] { "User not found." });
        }
        var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
        var isConfirmed = await _userManager.ConfirmEmailAsync(user, decodedToken);
        return isConfirmed.ToApplicationResult(); 
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

    public async Task<(Result Result, string UserId)> VerifyUserPasswordAsync(string password, string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if(user is null)
        {
            return (Result.Failure(new[] {"User with such email don't exist."}), string.Empty);
        }

        bool isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
        return isPasswordValid ? (Result.Success(), user.Id) : (Result.Failure(new[] {"Entered password is invalid."}), user.Id);
    }
}
