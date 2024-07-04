using api.Data;
using api.DTOs.Stock;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

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
    public IActionResult GetAll()
    {
        var stocks = _context.Stocks.ToList()
            .Select(s => s.ToStockDto());

        return Ok(stocks);
    }

    [HttpGet("{id}")]
    public IActionResult GetById([FromRoute] int id)
    {
        var stock = _context.Stocks.Find(id);

        if (stock == null)
            NotFound();

        return Ok(stock!.ToStockDto());
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateStockRequestDto stockDto)
    {
        var stockModel = stockDto.ToStockFromCreateDto();
        _context.Stocks.Add(stockModel);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetById), new { id = stockModel.StockId }, stockModel.ToStockDto());
    }

    [HttpPut("{id}")]
    public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
    {
        var existingStockModel = _context.Stocks.FirstOrDefault(x => x.StockId == id);
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

        _context.SaveChanges();

        return Ok(existingStockModel.ToStockDto()); 
    }
}