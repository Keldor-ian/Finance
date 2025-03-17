using FinShark.DTOs.Stock;
using FinShark.Models;
using FinShark.Queries.Stocks;

namespace FinShark.Interfaces
{
    public interface IStockRepository
    {
        Task<Stock?> GetStockById(int stockId);
        Task<List<Stock>> GetAllStocksAsync(StockQueryObject stockQuery);
        Task<Stock> CreateStockAsync(Stock stockModel);
        Task<Stock?> DeleteStockAsync(int stockId);
        Task<Stock?> GetStockToUpdate(int stockId, UpdateStockFromCreateDto updateStockDto);
        Task<bool> StockExists(int stockId);

    }
}
