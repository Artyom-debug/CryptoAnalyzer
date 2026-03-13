using Application.Authentication.Commands.LoginCommand;
using Application.Authentication.Commands.LogoutCommand;
using Application.Authentication.Commands.RefreshCommand;
using Application.Authentication.Commands.RegisterCommand;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Principal;
using Web.Server.Constants;

namespace Web.Server.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IIdentityService _identityService;
    public AuthController(IMediator mediator, IIdentityService identityService)
    {
        _mediator = mediator;
        _identityService = identityService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken token)
    {
        var result = await _mediator.Send(new RegisterCommand(request.Email, request.UserName, request.Password), token);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken token)
    {
        var response = await _mediator.Send(new LoginCommand(request.Email, request.Password), token);
        SetAuthCookie(response.Tokens!);
        return Ok(new 
        { 
            userId = response.UserId,
            accessTokenExpiresAt = response.Tokens?.AccessTokenExpiresAt,
            refreshTokenExpiresAt = response.Tokens?.RefreshTokenExpiresAt
        });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(CancellationToken token)
    {
        if(!Request.Cookies.TryGetValue(AuthCookies.RefreshToken, out var refreshToken) || string.IsNullOrEmpty(refreshToken))
        {
            return Unauthorized();
        }
        var response = await _mediator.Send(new RefreshCommand(refreshToken), token);
        SetAuthCookie(response);
        return Ok(new 
        {
            accessTokenExpiresAt = response.AccessTokenExpiresAt,
            refreshTokenExpiresAt = response.RefreshTokenExpiresAt
        });
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout(CancellationToken token)
    {
        var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        if(string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }
        await _mediator.Send(new LogoutCommand(userId), token);
        DeleteAuthCookie();
        return NoContent();
    }

    [HttpPost("confirmEmail")]
    public async Task<IActionResult> EmailConfirm([FromQuery] string userId, [FromQuery] string token)
    {
        var response = await _identityService.ConfirmEmailAsync(userId, token);
        if(!response.Succeeded)
        {
            return BadRequest("Invalid emailConfirmation token.");
        }
        return Ok("Email successfully confirmed.");
    }

    private void SetAuthCookie(TokenPair tokens)
    {
        Response.Cookies.Append(AuthCookies.AccessToken, tokens.AccessToken!, new CookieOptions 
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            Secure = true,
            Expires = tokens.AccessTokenExpiresAt,
            Path = "/"
        });

        Response.Cookies.Append(AuthCookies.RefreshToken, tokens.RefreshToken!, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            Secure = true,
            Expires = tokens.RefreshTokenExpiresAt,
            Path = "/auth/refresh"
        });
    }

    private void DeleteAuthCookie()
    {
        Response.Cookies.Delete("access_token", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Path = "/"
        });

        Response.Cookies.Delete("refresh_token", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Path = "/auth/refresh"
        });
    }
}

public record RegisterRequest(string Email, string UserName, string Password);
public record LoginRequest(string Email, string Password);