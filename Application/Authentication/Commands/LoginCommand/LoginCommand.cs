using Application.Common.Interfaces;
using Application.Common.Models;


namespace Application.Authentication.Commands.LoginCommand;

public record LoginCommand(string Email, string Password) : IRequest<AuthResponse>;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
{
    private readonly IIdentityService _identity;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(IIdentityService identity, ITokenService tokenService)
    {
        _identity = identity;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var (result, userId) = await _identity.VerifyUserPasswordAsync(command.Password, command.Email);
        if(!result.Succeeded || string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("Invalid email or password.");

        var pair = await _tokenService.GenerateTokenPairAsync(userId, cancellationToken);

        return new AuthResponse
        { 
            Tokens = pair,
            UserId = userId
        };
    }
}

