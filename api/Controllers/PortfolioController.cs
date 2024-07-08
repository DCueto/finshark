using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepository;
        private readonly IPortfolioRepository _portfolioRepository;

        public PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepository,
            IPortfolioRepository portfolioRepository)
        {
            _userManager = userManager;
            _stockRepository = stockRepository;
            _portfolioRepository = portfolioRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username = User.GetUsername();
            if (string.IsNullOrEmpty(username))
                return BadRequest("Claim username not found");

            foreach (var claim in User.Claims)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            }
            
            var appUser = await _userManager.FindByNameAsync(username);
            if (appUser == null)
                return BadRequest("User doesn't exists");
            
            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);

            return Ok(userPortfolio);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(string symbol)
        {
            var username = User.GetUsername();
            if (string.IsNullOrEmpty(username))
                return BadRequest("Claim username not found");

            var appUser = await _userManager.FindByNameAsync(username);
            if (appUser == null)
                return NotFound("User doesn't exists");
            
            var stock = await _stockRepository.GetBySymbolAsync(symbol);
            if (stock == null)
                return NotFound($"Stock with symbol {symbol} doesn't exists");

            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);
            if (userPortfolio.Any(s => s.Symbol.ToLower() == symbol.ToLower()))
                return BadRequest("Can't add same stock to portfolio");
            
            
            var createdPortfolio = await _portfolioRepository.CreateAsync(stock.StockId, appUser.Id);
            
            // Portfolio returns data from user --> Shouldn't be visible to api customer
            // return CreatedAtAction(nameof(GetUserPortfolio), createdPortfolio);
            
            return Created();
        }
    }
}
