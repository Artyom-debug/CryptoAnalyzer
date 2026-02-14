using Application.Common.Models;
namespace Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string?> GetUserNameAsync(string userId);


    Task<bool> IsInRoleAsync(string userId, string role);

    Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password, string email);

    Task<Result> DeleteUserAsync(string userId);

    Task<(Result Result, string UserId)> VerifyUserPassword(string password, string email);
}
