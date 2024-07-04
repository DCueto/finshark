using api.DTOs.Stock;
using api.Models;

namespace api.Mappers;

public static class StockMappers
{
    public static StockDto ToStockDto(this Stock stockModel)
    {
        return new StockDto()
        {
            StockId = stockModel.StockId,
            Symbol = stockModel.Symbol,
            CompanyName = stockModel.CompanyName,
            Purchase = stockModel.Purchase,
            LastDiv = stockModel.LastDiv,
            Industry = stockModel.Industry,
            MarketCap = stockModel.MarketCap,
            Comments = stockModel.Comments.Select(c => c.ToCommentDto()).ToList()
        };
    }

    public static Stock ToStockFromCreateDto(this CreateStockRequestDto createStockDto)
    {
        return new Stock()
        {
            Symbol = createStockDto.Symbol,
            CompanyName = createStockDto.CompanyName,
            Purchase = createStockDto.Purchase,
            LastDiv = createStockDto.LastDiv,
            Industry = createStockDto.Industry,
            MarketCap = createStockDto.MarketCap
        };
    }

    public static Stock ToStockFromUpdateDto(this UpdateStockRequestDto updateStockDto)
    {
        return new Stock()
        {
            Symbol = updateStockDto.Symbol,
            CompanyName = updateStockDto.CompanyName,
            Purchase = updateStockDto.Purchase,
            LastDiv = updateStockDto.LastDiv,
            Industry = updateStockDto.Industry,
            MarketCap = updateStockDto.MarketCap
        };
    }
}