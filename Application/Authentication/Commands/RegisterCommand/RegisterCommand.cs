using Application.Common.Interfaces;

namespace Application.Authentication.Commands.RegisterCommand;

public record RegisterCommand(string Email, string UserName, string Password) : IRequest<string?>;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, string?>
{
    private readonly IIdentityService _identityService;
    private readonly IEmailSender _emailSender;

    public RegisterCommandHandler(IIdentityService identityService, IEmailSender emailSender)
    {
        _identityService = identityService;
        _emailSender = emailSender;
    }

    public async Task<string?> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var result = await _identityService.CreateUserAsync(command.UserName, command.Password, command.Email);
        if (!result.Result.Succeeded)
        {
            return null;
        }

        await _emailSender.SendEmailAsync(
            command.Email,
            "Welcome to CryptoAnalyzer",
            $"Hello, {command.UserName}! Your registration was completed successfully.",
            cancellationToken: cancellationToken);

        return result.UserId;
    }
}
