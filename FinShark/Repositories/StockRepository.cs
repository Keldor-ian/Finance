using FinShark.Database;
using FinShark.DTOs.Stock;
using FinShark.Interfaces;
using FinShark.Models;
using Microsoft.EntityFrameworkCore;

namespace FinShark.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;

        public StockRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Stock?> GetStockById(int stockId)
        {
            var getStockById = await _context.Stocks.FirstOrDefaultAsync(s => s.StockId == stockId);

            if (getStockById == null) return null;

            return getStockById;
        }

        public async Task<List<Stock>> GetAllStocksAsync()
        {
            var getAllStocks = await _context.Stocks.ToListAsync();

            if (getAllStocks == null) return null;

            return getAllStocks;
        }

        public async Task<Stock> CreateStockAsync(Stock stockModel)
        {
            await _context.AddAsync(stockModel);

            await _context.SaveChangesAsync();

            return stockModel;
        }

        public async Task<Stock?> DeleteStockAsync(int stockId)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(s => s.StockId == stockId);

            if (stockModel == null) return null;
            
            _context.Stocks.Remove(stockModel);
            
            await _context.SaveChangesAsync();

            return stockModel;
        }

        public async Task<Stock?> GetStockToUpdate(int stockId, UpdateStockFromCreateDto updateStockDto)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(s => s.StockId == stockId);

            if (stockModel == null) return null;

            stockModel.Symbol = updateStockDto.Symbol;
            stockModel.CompanyName = updateStockDto.CompanyName;
            stockModel.StockCost = updateStockDto.StockCost;
            stockModel.LastDividend = updateStockDto.LastDividend;
            stockModel.Industry = updateStockDto.Industry;
            stockModel.MarketCap = updateStockDto.MarketCap;

            await _context.SaveChangesAsync();

            return stockModel;
        }
    }
}
