using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QM.Models.Entities;

[Table("AspNetUsers")]
public class ApplicationUser : IdentityUser
{

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}