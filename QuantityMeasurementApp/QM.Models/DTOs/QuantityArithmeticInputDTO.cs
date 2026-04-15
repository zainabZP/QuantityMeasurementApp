// QM.Models/DTOs/QuantityArithmeticInputDTO.cs
using System.ComponentModel.DataAnnotations;

namespace QM.Models.DTOs
{
    public class QuantityArithmeticInputDTO : QuantityInputDTO
    {
        [Required]
        public string ResultUnit { get; set; } = string.Empty;
    }
}