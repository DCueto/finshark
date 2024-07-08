using api.Models;

namespace api.Interfaces;

public interface IPortfolioRepository
{
    Task<List<Stock>> GetUserPortfolio(AppUser user);
    Task<Portfolio> CreateAsync(int stockId, string userId);
    Task<Portfolio?> DeletePortfolio(string userId, string symbol);
}