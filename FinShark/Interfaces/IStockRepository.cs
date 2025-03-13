using FinShark.DTOs.Stock;
using FinShark.Models;

namespace FinShark.Interfaces
{
    public interface IStockRepository
    {
        Task<Stock?> GetStockById(int stockId);
        Task<List<Stock>> GetAllStocksAsync();
        Task<Stock> CreateStockAsync(Stock stockModel);
        Task<Stock?> DeleteStockAsync(int stockId);
        Task<Stock?> GetStockToUpdate(int stockId, UpdateStockFromCreateDto updateStockDto);
    }
}
