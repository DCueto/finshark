using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Comment;

public class CommentDto
{
    public int Id { get; set; }

    [StringLength(100)] 
    public string Title { get; set; } = string.Empty;
    
    public string Content { get; set; } = string.Empty;

    public DateTime CreatedOn { get; set; } = DateTime.Now;
    
    public int? StockId { get; set; }
}