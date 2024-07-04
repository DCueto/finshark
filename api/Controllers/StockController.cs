using api.Data;
using api.DTOs.Stock;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[Route("api/stock")]
[ApiController]
public class StockController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public StockController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var stocks = await _context.Stocks.ToListAsync();
        var stocksDto = stocks.Select(s => s.ToStockDto());

        return Ok(stocksDto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var stock = await _context.Stocks.FindAsync(id);

        if (stock == null)
            NotFound();

        return Ok(stock!.ToStockDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
    {
        var stockModel = stockDto.ToStockFromCreateDto();
        await _context.Stocks.AddAsync(stockModel);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = stockModel.StockId }, stockModel.ToStockDto());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
    {
        var existingStockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.StockId == id);
        if (existingStockModel == null)
            return BadRequest($"This stock with id {id} doesn't exists.");

        existingStockModel.Symbol = updateDto.Symbol;
        existingStockModel.CompanyName = updateDto.CompanyName;
        existingStockModel.Purchase = updateDto.Purchase;
        existingStockModel.LastDiv = updateDto.LastDiv;
        existingStockModel.Industry = updateDto.Industry;
        existingStockModel.MarketCap = updateDto.MarketCap;

        // var stockModel = updateDto.ToStockFromUpdateDto();
        // _context.Stocks.Update(stockModel);

        await _context.SaveChangesAsync();

        return Ok(existingStockModel.ToStockDto()); 
    } 

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var existingStockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.StockId == id);

        if (existingStockModel == null)
            return NotFound();

        _context.Stocks.Remove(existingStockModel);
        await _context.SaveChangesAsync();

        return NoContent(); 
    }
}