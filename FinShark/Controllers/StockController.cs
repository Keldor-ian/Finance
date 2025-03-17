using FinShark.DTOs.Stock;
using FinShark.Interfaces;
using FinShark.Mappers;
using FinShark.Queries.Stocks;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;

namespace FinShark.Controllers
{
    [ApiController]
    [Route("api/stock")] // Base URL of the Controller
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _stockRepo;
        public StockController(IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
        }

        // Grab all stocks and convert them all to a StockDTO to display in API
        [HttpGet]
        public async Task<IActionResult> GetAllStocks([FromQuery] StockQueryObject stockQuery)
        {
            var getAllStocks = await _stockRepo.GetAllStocksAsync(stockQuery);

            var convertAllStocks = getAllStocks.Select(s => s.ToStockDTO()).ToList();

            return Ok(convertAllStocks);
        }

        // Grab a stock by a passed stock Id and convert to a StockDTO to display in API
        [HttpGet("{stockId:int}")]
        public async Task<IActionResult> GetStockById([FromRoute] int stockId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var getStockById = await _stockRepo.GetStockById(stockId);

            if (getStockById == null) return BadRequest("The stock being retrieved doesn't exist!");

            return Ok(getStockById.ToStockDTO());
        }

        // Create a stock using a StockDTO and converting to a Stock database model to insert it
        [HttpPost]
        public async Task<IActionResult> CreateStock([FromBody] CreateStockRequestDTO stockDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stockModel = stockDto.ToStockFromCreateDto();

            await _stockRepo.CreateStockAsync(stockModel);
            // What's the point of having stockId on the new object? Because it explicitly maps it to GetStockById(stockId) -> Which is the parameter
            return CreatedAtAction(nameof(GetStockById), new { stockId = stockModel.StockId }, stockModel.ToStockDTO());
        }

        [HttpDelete]
        [Route("{stockId:int}")]
        public async Task<IActionResult> DeleteStockAsync([FromRoute] int stockId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var getStockToDelete = await _stockRepo.DeleteStockAsync(stockId);

            if (getStockToDelete == null) return BadRequest("The stock to be deleted doesn't exist!");

            return Ok(getStockToDelete);
        }

        [HttpPut]
        [Route("{stockId:int}")]
        public async Task<IActionResult> GetStockToUpdate([FromRoute] int stockId, [FromBody] UpdateStockFromCreateDto updateStockDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stockToUpdate = await _stockRepo.GetStockToUpdate(stockId, updateStockDto);

            if (stockToUpdate == null) return BadRequest("The stock to be edited doesn't exist!");

            return Ok(stockToUpdate.ToStockDTO());
        }
    }
}
