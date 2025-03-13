using System.ComponentModel.DataAnnotations;

namespace FinShark.DTOs.Stock
{
    public class UpdateStockFromCreateDto
    {
        [Required]
        [MaxLength(10, ErrorMessage = "Symbol cannot exceed 10 characters!")]
        public string Symbol { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(25, ErrorMessage = "Company Name cannot exceed 25 characters!")]
        public string CompanyName { get; set; } = string.Empty;
        
        [Required]
        [Range(1, 100000000000)]
        public decimal StockCost { get; set; }
        
        [Required]
        [Range(0.001, 100)]
        public decimal LastDividend { get; set; }
        
        [Required]
        [MaxLength(10, ErrorMessage = "Industry cannot exceed 10 characters!")]
        public string Industry { get; set; } = string.Empty;

        [Required]
        [Range(0, 5000000000000)]
        public long MarketCap { get; set; }
    }
}
