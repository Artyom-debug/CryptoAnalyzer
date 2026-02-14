using Application.Common.Interfaces;

namespace Application.Authentication.Commands.RegisterCommand;

public record RegisterCommand(string Email, string UserName, string Password) : IRequest<string?>;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, string?>
{
    private readonly IIdentityService _identityService;
    
    public RegisterCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<string?> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var result = await _identityService.CreateUserAsync(command.UserName, command.Password, command.Email);
        return result.Result.Succeeded ? result.UserId : null;
    }
}

