using System.ComponentModel.DataAnnotations;

namespace QM.Models.DTOs
{
    public class QuantityInputDTO
    {
        [Required]
        public QuantityDTO ThisQuantityDTO { get; set; } = new();

        [Required]
        public QuantityDTO ThatQuantityDTO { get; set; } = new();
    }
}