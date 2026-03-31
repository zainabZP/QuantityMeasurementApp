// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Mvc;
// using QM.Models.DTOs;
// using QM.Models.Entities;
// // using QuantityMeasurementApi.Services;
// using QM.BusinessLogic.Service;
// using Swashbuckle.AspNetCore.Annotations;

// namespace QuantityMeasurementApi.Controllers;

// [ApiController]
// [Route("api/v1/auth")]
// [Tags("Authentication")]
// public class AuthController : ControllerBase
// {
//     private readonly UserManager<ApplicationUser>   _userManager;
//     private readonly SignInManager<ApplicationUser> _signInManager;
//     private readonly IJwtTokenService               _jwtService;
//     private readonly ITokenBlacklistService         _blacklist;

//     public AuthController(
//         UserManager<ApplicationUser>   userManager,
//         SignInManager<ApplicationUser> signInManager,
//         IJwtTokenService               jwtService,
//         ITokenBlacklistService         blacklist)
//     {
//         _userManager   = userManager;
//         _signInManager = signInManager;
//         _jwtService    = jwtService;
//         _blacklist     = blacklist;
//     }

//     /// <summary>Register a new user with username, email and password</summary>
//     [HttpPost("register")]
//     [SwaggerOperation(Summary = "Register a new user")]
//     [ProducesResponseType(typeof(AuthResponseDTO), StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status400BadRequest)]
//     public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
//     {
//         var user   = new ApplicationUser { UserName = dto.Username, Email = dto.Email };
//         var result = await _userManager.CreateAsync(user, dto.Password);

//         if (!result.Succeeded)
//             return BadRequest(result.Errors);

//         return Ok(new AuthResponseDTO(_jwtService.GenerateToken(user)));
//     }

//     /// <summary>Login with email and password — returns a JWT Bearer token</summary>
//     [HttpPost("login")]
//     [SwaggerOperation(Summary = "Login and receive JWT token")]
//     [ProducesResponseType(typeof(AuthResponseDTO), StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//     public async Task<IActionResult> Login([FromBody] LoginDTO dto)
//     {
//         var user = await _userManager.FindByEmailAsync(dto.Email);
//         if (user == null) return Unauthorized("Invalid credentials");

//         var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
//         if (!result.Succeeded) return Unauthorized("Invalid credentials");

//         return Ok(new AuthResponseDTO(_jwtService.GenerateToken(user)));
//     }

//     /// <summary>Logout — revokes the current JWT token</summary>
//     [HttpPost("logout")]
//     [Authorize]
//     [SwaggerOperation(Summary = "Logout and revoke current JWT token")]
//     [ProducesResponseType(StatusCodes.Status200OK)]
//     public IActionResult Logout()
//     {
//         var token = Request.Headers.Authorization.FirstOrDefault()?["Bearer ".Length..]?.Trim();
//         if (!string.IsNullOrEmpty(token))
//             _blacklist.Revoke(token);

//         return Ok(new { message = "Logged out successfully. Token revoked." });
//     }

//     /// <summary>Initiate Google OAuth2 login flow</summary>
//     // [HttpGet("google-login")]
//     // [SwaggerOperation(Summary = "Initiate Google OAuth2 login")]
//     // public IActionResult GoogleLogin()
//     // {
//     //     var props = _signInManager.ConfigureExternalAuthenticationProperties(
//     //         "Google", Url.Action(nameof(GoogleCallback))!);
//     //     return Challenge(props, "Google");
//     // }

//     [HttpGet("google-login")]
//     [SwaggerOperation(Summary = "Initiate Google OAuth2 login")]
//     public IActionResult GoogleLogin()
//     {
//         var callbackUrl = Url.Action(nameof(GoogleCallback), "Auth", null, Request.Scheme)!;
//         var props = _signInManager.ConfigureExternalAuthenticationProperties("Google", callbackUrl);
//         return Challenge(props, "Google");
//     }

//     /// <summary>Google OAuth2 callback — auto-creates user if first login</summary>
//     [HttpGet("google-callback")]
//     [SwaggerOperation(Summary = "Google OAuth2 callback")]
//     public async Task<IActionResult> GoogleCallback()
//     {
//         var info = await _signInManager.GetExternalLoginInfoAsync();
//         if (info == null) return Unauthorized("Google login failed");

//         var email = info.Principal.FindFirst(System.Security.Claims.ClaimTypes.Email)!.Value;
//         var user  = await _userManager.FindByEmailAsync(email);

//         if (user == null)
//         {
//             user = new ApplicationUser
//             {
//                 UserName = info.Principal.FindFirst(System.Security.Claims.ClaimTypes.Name)!.Value,
//                 Email    = email
//             };
//             await _userManager.CreateAsync(user);
//             await _userManager.AddLoginAsync(user, info);
//         }

//         return Ok(new AuthResponseDTO(_jwtService.GenerateToken(user)));
//     }
// }



using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QM.Models.DTOs;
using QM.Models.Entities;
using QM.BusinessLogic.Service;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
// using QM.BusinessLogic.Interface;
using QM.Repository.Interface;

namespace QuantityMeasurementApi.Controllers;

[ApiController]
[Route("api/v1/auth")]
[Tags("Authentication")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser>   _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtTokenService               _jwtService;
    private readonly ITokenBlacklistService         _blacklist;

    public AuthController(
        UserManager<ApplicationUser>   userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtTokenService               jwtService,
        ITokenBlacklistService         blacklist)
    {
        _userManager   = userManager;
        _signInManager = signInManager;
        _jwtService    = jwtService;
        _blacklist     = blacklist;
    }

    /// <summary>Register a new user with username, email and password</summary>
    [HttpPost("register")]
    [SwaggerOperation(Summary = "Register a new user")]
    [ProducesResponseType(typeof(AuthResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
    {
        var user   = new ApplicationUser { UserName = dto.Username, Email = dto.Email };
        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok(new AuthResponseDTO(_jwtService.GenerateToken(user)));
    }

    /// <summary>Login with email and password — returns a JWT Bearer token</summary>
    [HttpPost("login")]
    [SwaggerOperation(Summary = "Login and receive JWT token")]
    [ProducesResponseType(typeof(AuthResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null) return Unauthorized("Invalid credentials");

        var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
        if (!result.Succeeded) return Unauthorized("Invalid credentials");

        return Ok(new AuthResponseDTO(_jwtService.GenerateToken(user)));
    }

    /// <summary>Logout — revokes the current JWT token</summary>
    [HttpPost("logout")]
    [Authorize]
    [SwaggerOperation(Summary = "Logout and revoke current JWT token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Logout()
    {
        var authHeader = Request.Headers.Authorization.FirstOrDefault();
        var token = authHeader?.StartsWith("Bearer ") == true
            ? authHeader["Bearer ".Length..].Trim()
            : null;

        if (!string.IsNullOrEmpty(token))
            _blacklist.Revoke(token);

        return Ok(new { message = "Logged out successfully. Token revoked." });
    }

    /// <summary>Initiate Google OAuth2 login flow</summary>
    [HttpGet("google-login")]
    [SwaggerOperation(Summary = "Initiate Google OAuth2 login")]
    public IActionResult GoogleLogin()
    {
        var callbackUrl = Url.Action(nameof(GoogleCallback), "Auth", null, Request.Scheme)!;
        var props = _signInManager.ConfigureExternalAuthenticationProperties("Google", callbackUrl);
        return Challenge(props, "Google");
    }

    /// <summary>Google OAuth2 callback — auto-creates user if first login</summary>
    [HttpGet("google-callback")]
    [SwaggerOperation(Summary = "Google OAuth2 callback")]
    public async Task<IActionResult> GoogleCallback()
    {
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
            return Unauthorized("Google login failed — could not retrieve external login info.");

        // Try to sign in with existing external login first
        var signInResult = await _signInManager.ExternalLoginSignInAsync(
            info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

        if (signInResult.Succeeded)
        {
            var existingEmail = info.Principal.FindFirstValue(ClaimTypes.Email);
            var existingUser  = await _userManager.FindByEmailAsync(existingEmail!);
            return Ok(new AuthResponseDTO(_jwtService.GenerateToken(existingUser!)));
        }

        // New user — create account
        var userEmail = info.Principal.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(userEmail))
            return BadRequest("Could not retrieve email from Google account.");

        // Use email prefix as username — avoids spaces that Identity rejects
        var userName = userEmail.Split('@')[0];

        var user = await _userManager.FindByEmailAsync(userEmail);
        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName       = userName,
                Email          = userEmail,
                EmailConfirmed = true   // Google already verified the email
            };

            var createResult = await _userManager.CreateAsync(user);
            if (!createResult.Succeeded)
                return BadRequest(createResult.Errors);
        }

        var addLoginResult = await _userManager.AddLoginAsync(user, info);
        if (!addLoginResult.Succeeded)
            return BadRequest(addLoginResult.Errors);

        return Ok(new AuthResponseDTO(_jwtService.GenerateToken(user)));
    }
}