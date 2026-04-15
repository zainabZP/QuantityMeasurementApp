// using Microsoft.AspNetCore.Identity;
// using System.ComponentModel.DataAnnotations;
// using System.ComponentModel.DataAnnotations.Schema;

// namespace QM.Models.Entities;

// [Table("AspNetUsers")]
// public class ApplicationUser : IdentityUser
// {

//     [Required]
//     public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
// }

//-------------------------------------------------------
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QM.Models.Entities;

[Table("Users")]
public class ApplicationUser
{
    [Key]
    [MaxLength(450)]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    [MaxLength(256)]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [MaxLength(256)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;  // BCrypt hash stored here

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}