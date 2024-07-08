using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories;

public class PortfolioRepository : IPortfolioRepository
{
    private readonly ApplicationDbContext _context;

    public PortfolioRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Stock>> GetUserPortfolio(AppUser user)
    {
        return await _context.Portfolios.Where(p => p.AppUserId == user.Id)
            .Select(portfolio => new Stock
            {
                StockId = portfolio.StockId,
                Symbol = portfolio.Stock.Symbol,
                CompanyName = portfolio.Stock.CompanyName,
                Purchase = portfolio.Stock.Purchase,
                LastDiv = portfolio.Stock.LastDiv,
                Industry = portfolio.Stock.Industry,
                MarketCap = portfolio.Stock.MarketCap
            })
            .ToListAsync();
    }

    public async Task<Portfolio> CreateAsync(int stockId, string userId)
    {
        var portfolioModel = new Portfolio
        {
            StockId = stockId,
            AppUserId = userId
        };

        await _context.Portfolios.AddAsync(portfolioModel);
        await _context.SaveChangesAsync();

        return portfolioModel;
    }
}