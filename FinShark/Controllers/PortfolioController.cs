using FinShark.ClaimExtensions;
using FinShark.Interfaces;
using FinShark.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinShark.Controllers
{
    [ApiController]
    [Route("api/portfolio")]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepo;
        private readonly IPortfolioRepository _portfolioRepo;
        private readonly IFMPService _fmpService;
        public PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepo, IPortfolioRepository portfolioRepo, IFMPService fmpService)
        {
            _userManager = userManager;
            _stockRepo = stockRepo;
            _portfolioRepo = portfolioRepo;
            _fmpService = fmpService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            // Here we grab the claims via our extension method
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            // Grab all the stocks associated to an AppUser
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);
            return Ok(userPortfolio);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(string symbol)
        {
            // Here we grab the claims via our extension method
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            // Grab the stock by the symbol
            var stock = await _stockRepo.GetBySymbolAsync(symbol);

            // Null check to see if the stock symbol exists
            if (stock == null)
            {
                // Next, check to see if it exists within' the external API
                stock = await _fmpService.FindStockBySymbolAsync(symbol);

                // Secondary null check, this time on the external API
                if (stock == null)
                {
                    return BadRequest("Stock does not exist!");
                }
                else
                {
                    // Creates the stock from the external API to the Stock Table
                    // And with the FindByStockSymbolAsync() method, it creates a correct database model for our case
                    await _stockRepo.CreateStockAsync(stock);
                }
            }

            // Stock symbol exists, grab the user's portfolio next
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);

            // Ensure the same stock symbol doesn't already exist inside the user's portfolio
            if (userPortfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower()))
            {
                return BadRequest("Cannot add the same stock to the portfolio!");
            }

            // Stock symbol doesn't already exist, so add it to the user's portfolio and create it
            var portfolioModel = new Portfolio
            {
                StockId = stock.StockId,
                AppUserId = appUser.Id,
            };

            await _portfolioRepo.CreatePortfolioAsync(portfolioModel);

            // In case of any portfolioModel issues
            if (portfolioModel == null)
            {
                return StatusCode(500, "Could not create portfolio!");
            } 
            else
            {
                return Created();
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(string symbol)
        {
            // Here we grab the claims via our extension method
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);

            // Grab all the stocks associated to an AppUser
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);

            // Filter all stocks to find the one we're trying to delete
            var filterStock = userPortfolio.Where(s => s.Symbol.ToLower() ==  symbol.ToLower()).ToList();

            // Stock symbol found, now delete that stock
            if (filterStock.Count() == 1)
            {
                await _portfolioRepo.DeletePortfolio(appUser, symbol);
            }
            // Otherwise, stock not found so return a BadRequest.
            else
            {
                return BadRequest("Stock is not found in your portfolio");
            }

            return Ok();
        }
    }
}
