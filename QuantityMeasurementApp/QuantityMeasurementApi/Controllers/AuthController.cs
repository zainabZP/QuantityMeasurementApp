//-----------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QM.BusinessLogic.Service;
using QM.Models.DTOs;
using QM.Models.Entities;
using QM.Repository.Data;
using QM.Repository.Interface;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace QuantityMeasurementApi.Controllers;

[ApiController]
[Route("api/v1/auth")]
[Tags("Authentication")]
public class AuthController : ControllerBase
{
    private readonly QuantityMeasurementDbContext _db;
    private readonly IJwtTokenService             _jwtService;
    private readonly ITokenBlacklistService       _blacklist;
    private readonly IHashService                 _hash;

    public AuthController(
        QuantityMeasurementDbContext db,
        IJwtTokenService             jwtService,
        ITokenBlacklistService       blacklist,
        IHashService                 hash)
    {
        _db         = db;
        _jwtService = jwtService;
        _blacklist  = blacklist;
        _hash       = hash;
    }

    [HttpPost("register")]
    [SwaggerOperation(Summary = "Register a new user")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
    {
        if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
            return BadRequest("Email already in use.");

        var user = new ApplicationUser
        {
            UserName     = dto.Username,
            Email        = dto.Email,
            PasswordHash = _hash.HashBcrypt(dto.Password)   // BCrypt with salting
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return Ok(new AuthResponseDTO(_jwtService.GenerateToken(user)));
    }

    [HttpPost("login")]
    [SwaggerOperation(Summary = "Login and receive JWT token")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null) return Unauthorized("Invalid credentials.");

        if (!_hash.VerifyBcrypt(dto.Password, user.PasswordHash))
            return Unauthorized("Invalid credentials.");

        return Ok(new AuthResponseDTO(_jwtService.GenerateToken(user)));
    }

    [HttpPost("logout")]
    [Authorize]
    [SwaggerOperation(Summary = "Logout and revoke current JWT token")]
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
}