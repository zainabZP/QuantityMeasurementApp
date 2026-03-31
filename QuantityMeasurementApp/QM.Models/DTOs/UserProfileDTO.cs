namespace QM.Models.DTOs;

public record UserProfileDTO(
    string Id,
    string UserName,
    string Email,
    DateTime CreatedAt
);

public record UpdateProfileDTO(
    string? UserName,
    string? CurrentPassword,
    string? NewPassword
);