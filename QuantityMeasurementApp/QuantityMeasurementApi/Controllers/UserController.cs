using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QM.Models.DTOs;
using QM.Models.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace QuantityMeasurementApi.Controllers;

[ApiController]
[Route("api/v1/users")]
[Authorize]
[Tags("User Management")]
public class UserController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserController(UserManager<ApplicationUser> userManager)
        => _userManager = userManager;

    /// <summary>Get the profile of the currently authenticated user</summary>
    [HttpGet("me")]
    [SwaggerOperation(Summary = "Get current user profile")]
    [ProducesResponseType(typeof(UserProfileDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);

        var user = await _userManager.FindByIdAsync(userId!);
        if (user == null) return NotFound("User not found");

        return Ok(new UserProfileDTO(user.Id, user.UserName!, user.Email!, user.CreatedAt));
    }

    /// <summary>Update username or password for the currently authenticated user</summary>
    [HttpPut("me")]
    [SwaggerOperation(Summary = "Update current user profile")]
    [ProducesResponseType(typeof(UserProfileDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDTO dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);

        var user = await _userManager.FindByIdAsync(userId!);
        if (user == null) return NotFound("User not found");

        if (!string.IsNullOrWhiteSpace(dto.UserName))
            user.UserName = dto.UserName;

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
            return BadRequest(updateResult.Errors);

        if (!string.IsNullOrWhiteSpace(dto.CurrentPassword) && !string.IsNullOrWhiteSpace(dto.NewPassword))
        {
            var pwResult = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (!pwResult.Succeeded)
                return BadRequest(pwResult.Errors);
        }

        return Ok(new UserProfileDTO(user.Id, user.UserName!, user.Email!, user.CreatedAt));
    }

    /// <summary>Delete the currently authenticated user's account</summary>
    [HttpDelete("me")]
    [SwaggerOperation(Summary = "Delete current user account")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteAccount()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);

        var user = await _userManager.FindByIdAsync(userId!);
        if (user == null) return NotFound();

        await _userManager.DeleteAsync(user);
        return Ok(new { message = "Account deleted successfully." });
    }
}