using api.Data;
using api.DTOs.Stock;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories;

public class StockRepository : IStockRepository
{
    private readonly ApplicationDbContext _context;

    public StockRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Stock>> GetAllAsync(QueryObject query)
    {
        var stocks = _context.Stocks
            .Include(c => c.Comments)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.CompanyName)) 
            stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));

        if (!string.IsNullOrWhiteSpace(query.Symbol))
            stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));

        if (!string.IsNullOrWhiteSpace(query.SortBy))
        {
            if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
            {
                stocks = query.IsDescending
                    ? stocks.OrderByDescending(s => s.Symbol)
                    : stocks.OrderBy(s => s.Symbol);
            }
        }

        return await stocks.ToListAsync();
    }  

    public async Task<Stock?> GetByIdAsync(int id)
    {
        return await _context.Stocks
            .Include(c=>c.Comments)
            .FirstOrDefaultAsync(s=>s.StockId == id);
    }

    public async Task<Stock> CreateAsync(Stock stockModel)
    {
        // var stockModel = stockRequestDto.ToStockFromCreateDto();
        await _context.Stocks.AddAsync(stockModel);
        await _context.SaveChangesAsync();

        return stockModel;
    }

    public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto updateStockRequestDto)
    {
        var existingStockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.StockId == id);
        if (existingStockModel == null)
            return null;

        existingStockModel.Symbol = updateStockRequestDto.Symbol;
        existingStockModel.CompanyName = updateStockRequestDto.CompanyName;
        existingStockModel.Purchase = updateStockRequestDto.Purchase;
        existingStockModel.LastDiv = updateStockRequestDto.LastDiv;
        existingStockModel.Industry = updateStockRequestDto.Industry;
        existingStockModel.MarketCap = updateStockRequestDto.MarketCap;
        
        await _context.SaveChangesAsync();

        return existingStockModel;
    }

    public async Task<Stock?> DeleteAsync(int id)
    {
        var existingStockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.StockId == id);
        if (existingStockModel == null)
            return null;

        _context.Stocks.Remove(existingStockModel);
        await _context.SaveChangesAsync();

        return existingStockModel;
    }

    public async Task<bool> StockExists(int id)
    {
        return await _context.Stocks.AnyAsync(s => s.StockId == id);
    }
} 