using Application.Common.Models;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services.Identity;

public static class IdentityResultExtension
{
    public static Result ToApplicationResult(this IdentityResult result)
    {
        return result.Succeeded ? Result.Success() : Result.Failure(result.Errors.Select(e => e.Description));
    }
}
