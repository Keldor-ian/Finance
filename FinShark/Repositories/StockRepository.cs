using FinShark.Database;
using FinShark.DTOs.Stock;
using FinShark.Interfaces;
using FinShark.Models;
using FinShark.Queries.Stocks;
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
        
        // Returns a stock by a supplied stockId
        public async Task<Stock?> GetStockById(int stockId)
        {
            var getStockById = await _context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(s => s.StockId == stockId);

            if (getStockById == null) return null;

            return getStockById;
        }

        // Returns a stock by a supplied symbol
        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            return await _context.Stocks.FirstOrDefaultAsync(s => s.Symbol == symbol);
        }

        // Returns all stocks (and returns stock queries if supplied)
        public async Task<List<Stock>> GetAllStocksAsync(StockQueryObject stockQuery)
        {

            var stocks = _context.Stocks.Include(c => c.Comments).ThenInclude(a => a.AppUser).AsQueryable();

            // Search Queries
            
            if (!string.IsNullOrWhiteSpace(stockQuery.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(stockQuery.Symbol.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(stockQuery.CompanyName))
            {
                stocks = stocks.Where(s => s.CompanyName.Contains(stockQuery.CompanyName.Trim()));
            }

            // Sort By Queries

            if (!string.IsNullOrEmpty(stockQuery.SortBy))
            {
                if (stockQuery.SortBy.Equals("Symbol"))
                {
                    stocks = stockQuery.IsDescending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
                }

                if (stockQuery.SortBy.Equals("CompanyName"))
                {
                    stocks = stockQuery.IsDescending ? stocks.OrderByDescending(s => s.CompanyName) : stocks.OrderBy(s => s.CompanyName);
                }
            }

            // Displays all stock's market caps that are LESS than the supplied value

           if (stockQuery.MarketCap.HasValue)
           {
                stocks = stocks.Where(s => s.MarketCap <= stockQuery.MarketCap.Value);
           }

            // Displays all stock's cost that are LESS than the supplied value

           if (stockQuery.StockCost.HasValue)
           {
                stocks = stocks.Where(s => s.StockCost <= stockQuery.StockCost.Value);
           }

            // Gets the number of stocks to skip if a page size and number is applied

            var getStocksToSkip = (stockQuery.PageNumber - 1) * stockQuery.PageSize;

            return await stocks.Skip(getStocksToSkip).Take(stockQuery.PageSize).ToListAsync();
        }

        // Creates a stock
        public async Task<Stock> CreateStockAsync(Stock stockModel)
        {
            await _context.Stocks.AddAsync(stockModel);

            await _context.SaveChangesAsync();

            return stockModel;
        }

        // Deletes a stock by a supplied stockId
        public async Task<Stock?> DeleteStockAsync(int stockId)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(s => s.StockId == stockId);

            if (stockModel == null) return null;
            
            _context.Stocks.Remove(stockModel);
            
            await _context.SaveChangesAsync();

            return stockModel;
        }

        // Updates a stock by a supplied stockId
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
        public Task<bool> StockExists(int stockId)
        {
            return _context.Stocks.AnyAsync(s => s.StockId == stockId);
        }
    }
}
