using Application.Common.Interfaces;
using Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authentication.Commands.LoginCommand;

public record LoginCommand(string UserName, string Email, string Password) : IRequest<AuthResponse>;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
{
    private readonly IIdentityService _identity;
    private readonly ITokenService _tokenService;
    public async Task<AuthResponse> Handle(LoginCommand command, CancellationToken cancellationToken)
    {

    }
}

