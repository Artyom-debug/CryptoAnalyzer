using Application.Common.Interfaces;

namespace Application.Authentication.Commands.LogoutCommand;

public record LogoutCommand(string UserId) : IRequest;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand>
{
    private readonly ITokenService _tokenService;

    public LogoutCommandHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public async Task Handle(LogoutCommand logoutCommand, CancellationToken cancellationToken)
    {
        var userId = logoutCommand.UserId;
        await _tokenService.RevokeAllTokensAsync(userId, cancellationToken);
    }
}


