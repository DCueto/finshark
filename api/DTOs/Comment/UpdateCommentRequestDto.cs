using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Comment;

public class UpdateCommentRequestDto
{
    [StringLength(100)] 
    public string Title { get; set; } = string.Empty;
    
    public string Content { get; set; } = string.Empty;
    
    public int? StockId { get; set; }
}