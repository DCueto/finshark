using System.ComponentModel.DataAnnotations.Schema;
using api.DTOs.Comment;

namespace api.DTOs.Stock;

public class StockDto
{
    public int StockId { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public decimal Purchase { get; set; }
    public decimal LastDiv { get; set; }
    public string Industry { get; set; } = string.Empty;
    public long MarketCap { get; set; }
    public List<CommentDto> Comments { get; set; } = new List<CommentDto>();
}