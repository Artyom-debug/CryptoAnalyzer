using Application.Common.Interfaces;
using Application.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authentication.Commands.RefreshCommand;

public record RefreshCommand(string RefreshToken) : IRequest<TokenPair>;

public class RefreshCommandHandler : IRequestHandler<RefreshCommand, TokenPair>
{
    private readonly ITokenService _tokenService;
    private readonly IIdentityService _identityService;

    public RefreshCommandHandler(IIdentityService identityService, ITokenService tokenService)
    {
        _identityService = identityService;
        _tokenService = tokenService;
    }

    public async Task<TokenPair> Handle(RefreshCommand refreshCommand, CancellationToken cancellationToken)
    {
        Guard.Against.NullOrEmpty(refreshCommand.RefreshToken);
        var result = await _tokenService.RefreshAsync(refreshCommand.RefreshToken, cancellationToken);
        return result;
    }
}


