using FinShark.DTOs.Stock;
using FinShark.Models;

namespace FinShark.Mappers
{
    public static class StockMapper
    {
        public static StockDTO ToStockDTO(this Stock stockModel)
        {
            return new StockDTO
            {
                StockId = stockModel.StockId,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                StockCost = stockModel.StockCost,
                LastDiv = stockModel.LastDividend,
                Industry = stockModel.Industry,
                MarketCap = stockModel.MarketCap,
                Comments = stockModel.Comments.Select(c => c.ToCommentDTO()).ToList(),
            };
        }
        public static Stock ToStockFromCreateDto(this CreateStockRequestDTO stockDto)
        {
            return new Stock
            {
                Symbol = stockDto.Symbol,
                CompanyName = stockDto.CompanyName,
                StockCost = stockDto.StockCost,
                LastDividend = stockDto.LastDiv,
                Industry = stockDto.Industry,
                MarketCap = stockDto.MarketCap
            };
        }
    }
}
