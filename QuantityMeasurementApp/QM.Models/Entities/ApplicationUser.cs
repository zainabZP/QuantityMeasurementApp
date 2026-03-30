using Microsoft.AspNetCore.Identity;

namespace QM.Models.Entities;

public class ApplicationUser : IdentityUser
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}