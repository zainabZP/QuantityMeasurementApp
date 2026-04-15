using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QM.Models.Entities;

[Table("Measurements")]
public class QuantityMeasurementEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(100)]
    public string OperationType { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Operand1 { get; set; }

    [MaxLength(500)]
    public string? Operand2 { get; set; }

    [MaxLength(500)]
    public string? Result { get; set; }

    public double? ScalarResult { get; set; }

    [Required]
    public bool HasError { get; set; }

    [MaxLength(1000)]
    public string? ErrorMessage { get; set; }

    [Required]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [MaxLength(450)]
    public string? UserId { get; set; }
}