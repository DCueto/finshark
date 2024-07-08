using System.Runtime.Intrinsics.X86;
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
        var portfolios = await _context.Portfolios.Where(p => p.AppUserId == user.Id)
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

        return portfolios;
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

    public async Task<Portfolio?> DeletePortfolio(string userId, string symbol)
    {
        var portfolioModel = await _context.Portfolios
            .FirstOrDefaultAsync(x => x.AppUserId == userId && x.Stock.Symbol.ToLower() == symbol.ToLower());
        if (portfolioModel == null)
            return null;

        _context.Portfolios.Remove(portfolioModel);
        await _context.SaveChangesAsync();

        return portfolioModel;
    }
}