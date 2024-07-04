using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Comment;

public class CreateStockRequestDto
{
    [StringLength(100)] 
    public string Title { get; set; } = string.Empty;
    
    public string Content { get; set; } = string.Empty;

    public DateTime CreatedOn { get; set; } = DateTime.Now;
    
    public int? StockId { get; set; }
}