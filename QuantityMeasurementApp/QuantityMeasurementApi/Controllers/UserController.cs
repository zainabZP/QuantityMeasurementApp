using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QM.BusinessLogic.Service;
using QM.Models.DTOs;
using QM.Repository.Data;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace QuantityMeasurementApi.Controllers;

[ApiController]
[Route("api/v1/users")]
[Authorize]
[Tags("User Management")]
public class UserController : ControllerBase
{
    private readonly QuantityMeasurementDbContext _db;
    private readonly IHashService                 _hash;

    public UserController(QuantityMeasurementDbContext db, IHashService hash)
    {
        _db   = db;
        _hash = hash;
    }

    [HttpGet("me")]
    [SwaggerOperation(Summary = "Get current user profile")]
    public async Task<IActionResult> GetProfile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);

        var user = await _db.Users.FindAsync(userId);
        if (user == null) return NotFound("User not found.");

        return Ok(new UserProfileDTO(user.Id, user.UserName, user.Email, user.CreatedAt));
    }

    [HttpPut("me")]
    [SwaggerOperation(Summary = "Update current user profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDTO dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);

        var user = await _db.Users.FindAsync(userId);
        if (user == null) return NotFound("User not found.");

        if (!string.IsNullOrWhiteSpace(dto.UserName))
            user.UserName = dto.UserName;

        if (!string.IsNullOrWhiteSpace(dto.CurrentPassword) && !string.IsNullOrWhiteSpace(dto.NewPassword))
        {
            if (!_hash.VerifyBcrypt(dto.CurrentPassword, user.PasswordHash))
                return BadRequest("Current password is incorrect.");

            user.PasswordHash = _hash.HashBcrypt(dto.NewPassword);
        }

        await _db.SaveChangesAsync();
        return Ok(new UserProfileDTO(user.Id, user.UserName, user.Email, user.CreatedAt));
    }

    [HttpDelete("me")]
    [SwaggerOperation(Summary = "Delete current user account")]
    public async Task<IActionResult> DeleteAccount()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);

        var user = await _db.Users.FindAsync(userId);
        if (user == null) return NotFound();

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
        return Ok(new { message = "Account deleted successfully." });
    }
}